namespace ETL.ConsoleApp.Models;

public class AppSettings
{
    public string ConnectionString { get; set; } = string.Empty;

    public string DestinationTable { get; set; } = string.Empty;
    
    public string PathToCreateTablesScript { get; set; } = string.Empty;

    public string InputCsvFilePath { get; set; } = string.Empty;

    public string DuplicatesCsvFilePath { get; set; } = string.Empty;

    public bool DropNullOrEmpty { get; set; }
}