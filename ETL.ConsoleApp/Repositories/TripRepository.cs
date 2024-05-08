using System.Data;
using System.Data.SqlClient;
using ETL.ConsoleApp.Models;
using ETL.ConsoleApp.Repositories.Contracts;
using Microsoft.Extensions.Options;

namespace ETL.ConsoleApp.Repositories;

public class TripRepository : ITripRepository
{
    private readonly AppSettings _config;
    
    public TripRepository(IOptions<AppSettings> config)
    {
        _config = config.Value;
    }

    public async Task BulkInsert(IEnumerable<Trip> trips)
    {
        var dataTable = MapToDataTable(trips);
        using var bulkCopy = new SqlBulkCopy(_config.ConnectionString);
        
        bulkCopy.DestinationTableName = _config.DestinationTable;

        bulkCopy.ColumnMappings.Add(nameof(Trip.PickupDateTime), "PickupDateTime");
        bulkCopy.ColumnMappings.Add(nameof(Trip.DropoffDateTime), "DropoffDateTime");
        bulkCopy.ColumnMappings.Add(nameof(Trip.PassengerCount), "PassengerCount");
        bulkCopy.ColumnMappings.Add(nameof(Trip.TripDistance), "TripDistance");
        bulkCopy.ColumnMappings.Add(nameof(Trip.StoreAndFwdFlag), "StoreAndFwdFlag");
        bulkCopy.ColumnMappings.Add(nameof(Trip.PULocationID), "PULocationID");
        bulkCopy.ColumnMappings.Add(nameof(Trip.DOLocationID), "DOLocationID");
        bulkCopy.ColumnMappings.Add(nameof(Trip.FareAmount), "FareAmount");
        bulkCopy.ColumnMappings.Add(nameof(Trip.TipAmount), "TipAmount");

        await bulkCopy.WriteToServerAsync(dataTable);
    }
    
    private static DataTable MapToDataTable(IEnumerable<Trip> trips)
    {
        var dataTable = new DataTable();
        
        dataTable.Columns.Add(nameof(Trip.PickupDateTime), typeof(DateTime));
        dataTable.Columns.Add(nameof(Trip.DropoffDateTime), typeof(DateTime));
        dataTable.Columns.Add(nameof(Trip.PassengerCount), typeof(int));
        dataTable.Columns.Add(nameof(Trip.TripDistance), typeof(double));
        dataTable.Columns.Add(nameof(Trip.StoreAndFwdFlag), typeof(string));
        dataTable.Columns.Add(nameof(Trip.PULocationID), typeof(int));
        dataTable.Columns.Add(nameof(Trip.DOLocationID), typeof(int));
        dataTable.Columns.Add(nameof(Trip.FareAmount), typeof(decimal));
        dataTable.Columns.Add(nameof(Trip.TipAmount), typeof(decimal));
        
        foreach (var trip in trips)
        {
            dataTable.Rows.Add(
                trip.PickupDateTime, 
                trip.DropoffDateTime, 
                trip.PassengerCount, 
                trip.TripDistance,
                trip.StoreAndFwdFlag,
                trip.PULocationID,
                trip.DOLocationID,
                trip.FareAmount,
                trip.TipAmount);
        }
        
        return dataTable;
    }
}