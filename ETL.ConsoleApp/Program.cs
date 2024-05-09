using ETL.ConsoleApp.Models;
using ETL.ConsoleApp.Repositories;
using ETL.ConsoleApp.Repositories.Contracts;
using ETL.ConsoleApp.Services;
using ETL.ConsoleApp.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ETL.ConsoleApp;

public class Program
{
    public static async Task Main(string[] args)
    {
        // create service collection
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        // create service provider
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // run app
        var app = serviceProvider.GetService<App>();
        await app.Run();
    }
    
    private static void ConfigureServices(IServiceCollection serviceCollection)
    {
        // add logging
        serviceCollection.AddSingleton(new LoggerFactory()
            .AddConsole()
            .AddDebug());
        serviceCollection.AddLogging(); 

        // build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .Build();

        serviceCollection.AddOptions();
        serviceCollection.Configure<AppSettings>(configuration.GetSection("Configuration"));

        // add services
        serviceCollection.AddTransient<IEtlService, EtlService>();
        serviceCollection.AddScoped<ITripRepository, TripRepository>();
        serviceCollection.AddScoped<IDatabaseService, DatabaseService>();

        // add app
        serviceCollection.AddTransient<App>();
    }
}