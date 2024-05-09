namespace ETL.ConsoleApp.Models;

public class DbSettings
{
    public string ConnectionString { get; set; } = string.Empty;

    public string DestinationTable { get; set; } = string.Empty;
    
    public string PathToCreateTablesScript { get; set; } = string.Empty;
    
    public bool DropNullOrEmpty { get; set; }
}