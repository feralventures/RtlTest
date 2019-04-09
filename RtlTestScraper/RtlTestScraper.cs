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
        [FunctionName("RtlTestScraper")]
        public static async Task Run([TimerTrigger("0 0/5 * * * *")]TimerInfo myTimer, ILogger log, CancellationToken cancellationToken)
        {
            var tvMazeClient = ServiceProvider.GetService<TvMazeClient.IService>();

            IEnumerable<(long ShowId, long Updated)> tvMazeUpdates = await tvMazeClient.GetUpdates(cancellationToken);
            IEnumerable<(long TvMazeId, long Updated)> rtlTestUpdates;

            {
                var rtlTestRepository = ServiceProvider.GetService<RtlTestRepository.IService>();
                rtlTestUpdates = await rtlTestRepository.GetUpdates(cancellationToken);
            }

            var workload = tvMazeUpdates.GroupJoin(rtlTestUpdates,
                outerItem => outerItem.ShowId,
                innerItem => innerItem.TvMazeId,
                (outerItem, innerSet) => new { outerItem, innerItem = innerSet.SingleOrDefault() })
                .Where(g => (g.innerItem == default((long TvMazeId, long Updated))) || (g.innerItem.Updated < g.outerItem.Updated))
                .Take(100)
                .ToList();

            while (!cancellationToken.IsCancellationRequested)
            {
                Parallel.ForEach(workload, new ParallelOptions() { CancellationToken = cancellationToken }, async (workloadItem) =>
                {
                    var s = await tvMazeClient.GetShow(workloadItem.outerItem.ShowId, cancellationToken);

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

                    if (workloadItem.innerItem == default)
                    {
                        await rtlTestRepository.CreateShow(observedShow, cancellationToken);
                    }
                    else
                    {
                        await rtlTestRepository.UpdateShow(observedShow, cancellationToken);
                    }
                });
            };

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        private static ServiceProvider _serviceProvider = null;

        private static ServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    var services = new ServiceCollection();

                    services.AddTransient<TvMazeClient.IService, TvMazeClient.Service>(sp => new TvMazeClient.Service("http://api.tvmaze.com"));
                    services.AddTransient<RtlTestRepository.IService, RtlTestRepository.Service>();
                    services.AddDbContext<RtlTestRepository.Context>(options => options.UseSqlServer("Server=tcp:rtltestrepository.database.windows.net,1433;Initial Catalog=RTLTestStorage;Persist Security Info=False;User ID=paultak;Password=Pd3GbCe#=2Img+SHX86Z;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"), ServiceLifetime.Transient);

                    _serviceProvider = services.BuildServiceProvider();
                }

                return _serviceProvider;
            }
        }
    }
}
