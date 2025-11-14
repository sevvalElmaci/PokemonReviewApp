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

                    // 1️⃣ Count Pokémon Logs
                    var pokemonLogsCount = await context.PokemonLogs.CountAsync();

                    // 2️⃣ Count User Logs
                    var userLogsCount = await context.UserLogs.CountAsync();

                    _logger.LogInformation(
                        "Current PokemonLogs: {pokemonCount} | Current UserLogs: {userCount}",
                        pokemonLogsCount,
                        userLogsCount
                    );
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
