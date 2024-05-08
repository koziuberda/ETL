using ETL.ConsoleApp.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace ETL.ConsoleApp;

public class App
{
    private readonly IDataService _dataService;
    private readonly ILogger<App> _logger;

    public App(
        IDataService dataService, 
        ILogger<App> logger)
    {
        _dataService = dataService;
        _logger = logger;
    }
    
    public async Task Run()
    {
        _logger.LogInformation($"Starting console application...");
        await _dataService.Run();
        _logger.LogInformation("Press any key to exit the application.");
        Console.ReadKey();
    }
}