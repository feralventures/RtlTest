using System;
using System.Linq;
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
        public static async Task Run([TimerTrigger("0 0 */12 * * *")]TimerInfo myTimer, ILogger log)
        {
            var services = new ServiceCollection();

            services.AddTransient<TvMazeClient.IService, TvMazeClient.Service>(sp => new TvMazeClient.Service("http://api.tvmaze.com"));
            services.AddTransient<RtlTestRepository.IService, RtlTestRepository.Service>();
            services.AddDbContext<RtlTestRepository.Context>(options => options.UseSqlServer("Server=tcp:rtltestrepository.database.windows.net,1433;Initial Catalog=RTLTestStorage;Persist Security Info=False;User ID=paultak;Password=Pd3GbCe#=2Img+SHX86Z;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

            var serviceProvider = services.BuildServiceProvider();

            var tvMazeClient = serviceProvider.GetService<TvMazeClient.IService>();
            var rtlTestRepository = serviceProvider.GetService<RtlTestRepository.IService>();

            var pageIndex = 0;

            while (true)
            {
                var shows = await tvMazeClient.GetShows(pageIndex++, default);

                if (shows == null)
                {
                    break;
                }

                var observedShows = await Task.WhenAll(shows.AsParallel().Select(async s =>
                {
                    var cast = await tvMazeClient.GetCast(s.Id, default);

                    return new RtlTestRepository.Models.Show
                    {
                        TvMazeId = s.Id,
                        Name = s.Name,
                        Cast = cast.Select(a => new RtlTestRepository.Models.Person
                        {
                            TvMazeId = a.Person.Id,
                            Name = a.Person.Name,
                            Birthday = a.Person.Birthday
                        }).ToList(),
                    };
                }));

                await rtlTestRepository.CreateOrUpdateShowsIncludingCast(observedShows, default);
            };

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
