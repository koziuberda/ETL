namespace ETL.ConsoleApp.Models;

public class AppSettings
{
    public DbSettings DbSettings { get; set; } = new ();

    public CsvSettings CsvSettings { get; set; } = new ();
}