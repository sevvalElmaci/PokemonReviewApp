using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;

namespace PokemonWorkerService
{
    public class LogCleanupWorker : BackgroundService
    {
        private readonly ILogger<LogCleanupWorker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public LogCleanupWorker(ILogger<LogCleanupWorker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                    var logCount = context.PokemonLogs.Count();
                    _logger.LogInformation("Current PokemonLogs count: {count}", logCount);
                }

                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                        var logCount = context.UserLogs.Count();
                        _logger.LogInformation("Current PokemonLogs count: {count}", logCount);
                    }


                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }

            }
        }
    }
}
