using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ETL.ConsoleApp.Models;

namespace ETL.ConsoleApp.Services;

public static class CsvHelper
{
    public static List<Trip> ReadTripsCsv(string path)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null
        };
        
        using var reader = new StreamReader(path);
        using var csvReader = new CsvReader(reader, config);
        csvReader.Context.RegisterClassMap<TripMap>();
        
        return csvReader.GetRecords<Trip>().ToList();
    }
    
    public static void WriteTripsToCsv(string filePath, IEnumerable<Trip> trips)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        };

        using var writer = new StreamWriter(filePath);
        using var csvWriter = new CsvWriter(writer, config);
        csvWriter.Context.RegisterClassMap<TripMap>();

        csvWriter.WriteRecords(trips);
    }
}

public sealed class TripMap : ClassMap<Trip>
{
    public TripMap()
    {
        Map(t => t.PickupDateTime).Name("tpep_pickup_datetime");
        Map(t => t.DropoffDateTime).Name("tpep_dropoff_datetime");
        Map(t => t.PassengerCount).Name("passenger_count");
        Map(t => t.TripDistance).Name("trip_distance");
        Map(t => t.StoreAndFwdFlag).Name("store_and_fwd_flag");
        Map(t => t.PULocationID).Name("PULocationID");
        Map(t => t.DOLocationID).Name("DOLocationID");
        Map(t => t.FareAmount).Name("fare_amount");
        Map(t => t.TipAmount).Name("tip_amount");
    }
}