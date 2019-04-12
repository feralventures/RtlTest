using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RtlTestFunctionApp
{
    public static class RtlTestScraper
    {
        [FunctionName("RtlTestScraper")]
        public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log, Microsoft.Azure.WebJobs.ExecutionContext context, CancellationToken cancellationToken)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = configurationBuilder.Build();

            var services = new ServiceCollection();

            services.AddSingleton<TvMazeClient.IService, TvMazeClient.Service>(sp => new TvMazeClient.Service()
            {
                BaseUri = configuration["TvMazeClient_BaseUri"],
                TooManyRequestsDelay = int.Parse(configuration["TvMazeClient_TooManyRequestsDelay"])
            });
            services.AddTransient<RtlTestRepository.IService, RtlTestRepository.Service>();
            services.AddTransient<RtlTestLogic.IService, RtlTestLogic.Service>(sp => new RtlTestLogic.Service(sp.GetService<TvMazeClient.IService>(), () => sp.GetService<RtlTestRepository.IService>()));
            services.AddDbContext<RtlTestRepository.Context>(options => options.UseSqlServer(configuration.GetConnectionString("RtlTestRepository_ConnectionString")), ServiceLifetime.Transient);

            var serviceProvider = services.BuildServiceProvider();

            var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var rtlTestLogic = serviceScope.ServiceProvider.GetService<RtlTestLogic.IService>();

                await rtlTestLogic.Execute(int.Parse(configuration["RtlTestLogic_WorkloadSize"]), cancellationToken);
            }

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
