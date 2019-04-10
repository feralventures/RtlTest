using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RtlTestScraper
{
    public static class RtlTestScraper
    {
        static readonly TvMazeClient.IService _tvMazeClient = null;

        static RtlTestScraper()
        {
            _tvMazeClient = ServiceProvider.GetService<TvMazeClient.IService>();
        }

        [FunctionName("RtlTestScraper")]
        public static async Task Run([TimerTrigger("0 0/5 * * * *")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            var workload = await DetermineWorkload(cancellationToken);
            ProcessWorkload(workload, cancellationToken);

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        private static async Task<IEnumerable<long>> DetermineWorkload(CancellationToken cancellationToken)
        {
            List<long> workload = new List<long>();

            IDictionary<long, long> tvMazeUpdates = await _tvMazeClient.GetUpdates(cancellationToken);
            IDictionary<long, long> rtlTestUpdates = await ServiceProvider.GetService<RtlTestRepository.IService>().GetUpdates(cancellationToken);

            for (var updatesToScan = tvMazeUpdates.AsEnumerable(); (workload.Count() < 100) && updatesToScan.Any(); updatesToScan = updatesToScan.Skip(1))
            {
                cancellationToken.ThrowIfCancellationRequested();

                var tvMazeUpdate = updatesToScan.First();

                if (rtlTestUpdates.TryGetValue(tvMazeUpdate.Key, out var updated))
                {
                    if (updated < tvMazeUpdate.Value)
                    {
                        workload.Add(tvMazeUpdate.Key);
                    }
                }
                else
                {
                    workload.Add(tvMazeUpdate.Key);
                }
            }

            return workload;
        }

        private static void ProcessWorkload(IEnumerable<long> workload, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Parallel.ForEach(workload, new ParallelOptions() { CancellationToken = cancellationToken }, async (work) =>
                {
                    var s = await _tvMazeClient.GetShow(work, cancellationToken);

                    var observedShow = new RtlTestRepository.Models.Show
                    {
                        TvMazeId = s.Id,
                        Updated = s.Updated,
                        Name = s.Name,
                        Cast = s.Embedded.Cast.GroupBy(a => a.Person.Id)
                        .Select(ga => ga.First())
                        .Select(a => new RtlTestRepository.Models.Person
                        {
                            TvMazeId = a.Person.Id,
                            Name = a.Person.Name,
                            Birthday = a.Person.Birthday
                        }).ToList(),
                    };

                    var rtlTestRepository = ServiceProvider.GetService<RtlTestRepository.IService>();

                    await rtlTestRepository.PersistShow(observedShow, cancellationToken);
                });
            };
        }

        private static ServiceProvider _serviceProvider = null;

        private static ServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    var services = new ServiceCollection();

                    services.AddSingleton<TvMazeClient.IService, TvMazeClient.Service>(sp => new TvMazeClient.Service("http://api.tvmaze.com"));
                    services.AddTransient<RtlTestRepository.IService, RtlTestRepository.Service>();
                    services.AddDbContext<RtlTestRepository.Context>(options => options.UseSqlServer("Server=tcp:rtltestrepository.database.windows.net,1433;Initial Catalog=RTLTestStorage;Persist Security Info=False;User ID=paultak;Password=Pd3GbCe#=2Img+SHX86Z;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"), ServiceLifetime.Transient);

                    _serviceProvider = services.BuildServiceProvider();
                }

                return _serviceProvider;
            }
        }
    }
}
