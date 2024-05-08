using ETL.ConsoleApp.Models;
using ETL.ConsoleApp.Repositories.Contracts;
using ETL.ConsoleApp.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ETL.ConsoleApp.Services;

public class DataService : IDataService
{
    private readonly ILogger<DataService> _logger;
    private readonly AppSettings _config;
    private readonly ITripRepository _tripRepository;

    public DataService(
        ILogger<DataService> logger,
        IOptions<AppSettings> config, 
        ITripRepository tripRepository)
    {
        _logger = logger;
        _tripRepository = tripRepository;
        _config = config.Value;
    }

    public async Task Run()
    {
        _logger.LogInformation($"DataService is running");

        var testList = GetTestTrips();
        await _tripRepository.BulkInsert(testList);
        
        _logger.LogInformation("Stopping DataService");
    }

    private IEnumerable<Trip> GetTestTrips()
    {
        return new List<Trip>()
        {
            new Trip()
            {
                PickupDateTime = new DateTime(2023, 12, 11),
                DropoffDateTime = new DateTime(2023, 12, 13),
                PassengerCount = 2,
                TripDistance = 13.4,
                StoreAndFwdFlag = "N",
                PULocationID = 4,
                DOLocationID = 7,
                FareAmount = 12.4m,
                TipAmount = 3.6m
            },
            new Trip()
            {
                PickupDateTime = new DateTime(2022, 5, 11),
                DropoffDateTime = new DateTime(2023, 6, 12),
                PassengerCount = 3,
                TripDistance = 5.5,
                StoreAndFwdFlag = "Y",
                PULocationID = 2,
                DOLocationID = 3,
                FareAmount = 2.7m,
                TipAmount = 2.1m
            }
        };
    }
}