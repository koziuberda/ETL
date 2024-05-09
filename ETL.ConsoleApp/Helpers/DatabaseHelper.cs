using System.Data.SqlClient;

namespace ETL.ConsoleApp.Helpers;

public static class DatabaseHelper
{
    public static string GetDatabaseNameFromConnectionString(string connectionString)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        return builder.InitialCatalog;
    }
    
    public static bool DatabaseExists(string connectionString, string databaseName)
    {
        // Connect to the master database to check if the specified database exists
        var masterConnectionString = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = "master" }.ConnectionString;
        using SqlConnection connection = new SqlConnection(masterConnectionString);
        connection.Open();
        var command = new SqlCommand($"SELECT COUNT(*) FROM sys.databases WHERE name = '{databaseName}'", connection);
        var databaseCount = (int)command.ExecuteScalar();
        return databaseCount > 0;
    }
    
    public static void CreateDatabase(string connectionString, string databaseName)
    {
        // Connect to the master database to create a new database
        var masterConnectionString = new SqlConnectionStringBuilder(connectionString) { InitialCatalog = "master" }.ConnectionString;
        using SqlConnection connection = new SqlConnection(masterConnectionString);
        connection.Open();
        var command = new SqlCommand($"CREATE DATABASE {databaseName}", connection);
        command.ExecuteNonQuery();
    }
}