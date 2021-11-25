using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Codesanook.EFNote
{
    public class WarmupService : IHostedService, IDisposable
    {
        private readonly ILogger logger;
        private readonly HttpClient httpClient;
        private static Regex hostPattern = new Regex(@"(0\.0\.0\.0|\[::\]|\*)", RegexOptions.Compiled);

        public WarmupService(ILogger<WarmupService> logger)
        {
            this.logger = logger;
            httpClient = new HttpClient();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting IHostedService...");

            try
            {
                var url = GetValidUrl();
                return httpClient.GetAsync(url);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error when starting IHostedService");
                return Task.CompletedTask;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopping IHostedService...");
            return Task.CompletedTask;
        }

        public void Dispose() => httpClient?.Dispose();

        private static string GetValidUrl()
        {
            var url = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new InvalidOperationException("No ASPNETCORE_URLS environment value specified");
            }

            return hostPattern.Replace(url, "localhost");
        }
    }
}
