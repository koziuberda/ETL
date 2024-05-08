using ETL.ConsoleApp.Models;

namespace ETL.ConsoleApp.Repositories.Contracts;

public interface ITripRepository
{
    Task BulkInsert(IEnumerable<Trip> trips);
}