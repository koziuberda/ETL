namespace ETL.ConsoleApp.Models;

public class AppSettings
{
    public string ConnectionString { get; set; } = string.Empty;

    public string DestinationTable { get; set; } = string.Empty;

    public string CsvFilePath { get; set; } = string.Empty;
}