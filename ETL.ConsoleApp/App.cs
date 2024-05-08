using ETL.ConsoleApp.Models;
using ETL.ConsoleApp.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ETL.ConsoleApp;

public class App
{
    private readonly IDataService _dataService;
    private readonly ILogger<App> _logger;
    private readonly AppSettings _config;

    public App(
        IDataService dataService, 
        ILogger<App> logger, 
        IOptions<AppSettings> config)
    {
        _dataService = dataService;
        _logger = logger;
        _config = config.Value;
    }
    
    public void Run()
    {
        _logger.LogInformation($"Starting console application...");
        _dataService.Run();
        _logger.LogInformation("Press any key to exit the application.");
        Console.ReadKey();
    }
}