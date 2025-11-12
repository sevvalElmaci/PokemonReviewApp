using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonWorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

        // Worker class'ını kaydet
        services.AddHostedService<LogCleanupWorker>();
        services.AddHttpContextAccessor();

    })
    .Build();

await host.RunAsync();
