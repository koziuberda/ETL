using ETL.ConsoleApp.Models;
using ETL.ConsoleApp.Repositories.Contracts;
using ETL.ConsoleApp.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ETL.ConsoleApp.Services;

public class EtlService : IEtlService
{
    private readonly ILogger<EtlService> _logger;
    private readonly AppSettings _config;
    private readonly IDatabaseService _databaseService;

    public EtlService(
        ILogger<EtlService> logger,
        IOptions<AppSettings> config, 
        IDatabaseService databaseService)
    {
        _logger = logger;
        _databaseService = databaseService;
        _config = config.Value;
    }

    public async Task Run()
    {
        _logger.LogInformation($"EtlService is running");

        _databaseService.CreateDatabaseIfNotExist();
        _databaseService.CreateTablesAndIndexesIfNotExist();
        _logger.LogInformation("Database is set up");
        
        var allRecords = Helpers.CsvHelper.ReadTripsCsv(_config.InputCsvFilePath);
        _logger.LogInformation("Read {recordsNum} records from CSV.", allRecords.Count);

        var processedRecords = ProcessRecords(allRecords);
        _logger.LogInformation("Records were processed.");
        
        await _databaseService.InsertTrips(processedRecords);
        _logger.LogInformation("Inserted {processedRecordsNum} items into database.", processedRecords.Count);
        
        _logger.LogInformation("Stopping EtlService...");
    }

    private List<Trip> ProcessRecords(IReadOnlyCollection<Trip> records)
    {
        var uniqueRecords = GetUniqueRecords(records, out var duplicates);
        Helpers.CsvHelper.WriteTripsToCsv(_config.DuplicatesCsvFilePath, duplicates);
        _logger.LogInformation("Unique records: {uniqueRecordsNum}; N. of records removed: {duplicatesNum}", 
            uniqueRecords.Count, duplicates.Count);

        TrimTextFields(uniqueRecords);
        _logger.LogInformation("Trimmed text fields.");

        if (_config.DropNullOrEmpty)
        {
            uniqueRecords = uniqueRecords
                .Where(x => !string.IsNullOrEmpty(x.StoreAndFwdFlag) && x.PassengerCount is not null)
                .ToList();
            _logger.LogInformation("Records with empty fields were excluded.");
            _logger.LogInformation("Now there are {recordsNum} records", uniqueRecords.Count);
        }
        
        ConvertFlags(uniqueRecords);
        _logger.LogInformation("Successfully converted flags from Y or N to Yes or No.");

        return uniqueRecords;
    }
    
    private static List<Trip> GetUniqueRecords(IReadOnlyCollection<Trip> trips, out List<Trip> duplicates)
    {
        duplicates = trips
            .GroupBy(trip => (trip.PickupDateTime, trip.DropoffDateTime, trip.PassengerCount))
            .SelectMany(group => group.Skip(1))
            .ToList();

        var uniqueRecords = trips
            .Except(duplicates)
            .ToList();

        return uniqueRecords;
    }

    private static void ConvertFlags(List<Trip> trips)
    {
        trips.ForEach(t =>
        {
            var newFlag = t.StoreAndFwdFlag switch
            {
                "N" => "No",
                "Y" => "Yes",
                _ => t.StoreAndFwdFlag
            };

            t.StoreAndFwdFlag = newFlag;
        });
    }

    private static void TrimTextFields(List<Trip> trips)
    {
        trips.ForEach(t =>
        {
            // It seems it's the only one text-based field so far
            t.StoreAndFwdFlag = t.StoreAndFwdFlag?.Trim();
        });
    }
}