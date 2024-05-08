using ETL.ConsoleApp.Models;
using ETL.ConsoleApp.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ETL.ConsoleApp.Services;

public class DataService : IDataService
{
    private readonly ILogger<DataService> _logger;
    private readonly AppSettings _config;

    public DataService(
        ILogger<DataService> logger,
        IOptions<AppSettings> config)
    {
        _logger = logger;
        _config = config.Value;
    }

    public void Run()
    {
        _logger.LogInformation($"DataService is running");
        _logger.LogInformation("Stopping DataService");
    }
}