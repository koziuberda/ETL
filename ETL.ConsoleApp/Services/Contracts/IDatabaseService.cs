using ETL.ConsoleApp.Models;

namespace ETL.ConsoleApp.Services.Contracts;

public interface IDatabaseService
{
    void CreateDatabaseIfNotExist();
    void CreateTablesAndIndexesIfNotExist();
    Task InsertTrips(IEnumerable<Trip> trips);
}