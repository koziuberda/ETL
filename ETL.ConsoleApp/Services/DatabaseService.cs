using System.Data.SqlClient;
using ETL.ConsoleApp.Helpers;
using ETL.ConsoleApp.Models;
using ETL.ConsoleApp.Repositories.Contracts;
using ETL.ConsoleApp.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ETL.ConsoleApp.Services;

public class DatabaseService : IDatabaseService
{
    private readonly DbSettings _config;
    private readonly ITripRepository _tripRepository;
    private readonly ILogger<DatabaseService> _logger;
    
    public DatabaseService(
        IOptions<AppSettings> config,
        ITripRepository tripRepository,
        ILogger<DatabaseService> logger)
    {
        _tripRepository = tripRepository;
        _config = config.Value.DbSettings;
        _logger = logger;
    }

    public void CreateDatabaseIfNotExist()
    {
        _logger.LogInformation("Setting up your database...");
        var connectionString = _config.ConnectionString;
        var dbName = DatabaseHelper.GetDatabaseNameFromConnectionString(connectionString);
        if (!DatabaseHelper.DatabaseExists(connectionString, dbName))
        {
            DatabaseHelper.CreateDatabase(connectionString, dbName);
        }
    }
    
    public void CreateTablesAndIndexesIfNotExist()
    {
        _logger.LogInformation("Creating tables and indexes...");
        var sqlQuery = File.ReadAllText(_config.PathToCreateTablesScript);
        
        using SqlConnection connection = new SqlConnection(_config.ConnectionString);
        connection.Open();
        using SqlCommand command = new SqlCommand(sqlQuery, connection);
        command.ExecuteNonQuery();
    }

    public async Task InsertTrips(IEnumerable<Trip> trips)
    {
        await _tripRepository.BulkInsert(trips);
    }
}