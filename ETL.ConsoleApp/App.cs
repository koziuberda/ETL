using ETL.ConsoleApp.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace ETL.ConsoleApp;

public class App
{
    private readonly IEtlService _etlService;
    private readonly ILogger<App> _logger;

    public App(
        IEtlService etlService, 
        ILogger<App> logger)
    {
        _etlService = etlService;
        _logger = logger;
    }
    
    public async Task Run()
    {
        _logger.LogInformation($"Starting console application...");
        await _etlService.Run();
        _logger.LogInformation("Press any key to exit the application.");
        Console.ReadKey();
    }
}