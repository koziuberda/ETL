# ETL Project in C#/.NET

## Steps to run the project

1. **Configure App Settings:**
    - Open `ETL.ConsoleApp/appsettings.json` file.
    - Update `ConnectionString` field with the actual value specific to your SQL Server. 
    - In case database doesn't exist, application should create a database on its own, so you can write any database name you like.

2. **Run the Program:**
    - Build ETL solution
    - Run ETL.ConsoleApp project
    - If everything is all right, the database will be created and updated automatically

## Comments
1. **Querying efficiency**. 
The database is optimized for queries as per requirements. Request implementations can be found in the file`ETL.ConsoleApp/SqlScripts/sample_queries.sql`
In order to optimize the database, I used indexes and a computed persistent field. 
To check schema and indexes, please take a look at `ETL.ConsoleApp/SqlScripts/create_tables_and_indexes.sql`
2. **Inserting efficiency and security**. 
The `SqlBulkCopy` is used to achieve good speed of inserting huge amount of data into the table.
It's a good option not only for efficiency, but also for security, since it's resistant to SQL injections.
3. **Data cleaning**
It wasn't obvious for me if we should keep the rows with null or empty values in some columns, so I made a switch in appsettings.
By default, all rows with null or empty values inside `StoreAndFwdFlag` or `PassengerCount` are being removed.
If you want to change this behaviour, please make `DropNullOrEmpty` in `appsettings.json` equal to false.

## Results
After running the program, there are 29840 rows in the database. 
111 duplicates and 49 rows with missing values were removed from initial dataset (of 30,000 rows).

## Discussions
It's possible to make the program way more efficient and be able to handle large files (10GB or even more). 
Here is what I would try to do:
1. **Streaming Reading**. 
Instead of loading the entire CSV file into memory at once, I would use a streaming approach to read and process the file in chunks. 
This would prevent memory overflow issues and allow the program to handle files of any size.
2. **Batch processing**.
Rather than processing all records in one go, I would implement batch processing to handle a certain number of records at a time. 
This would reduce memory usage and improve performance, especially during database insertion.
3. **Parallel Processing**.
Consider leveraging parallel processing techniques to distribute the workload across multiple CPU cores. 
It's possible to parallelize tasks such as reading CSV records and duplicate removal. 
It's also might be a good idea to use PLINQ instead of plain LINQ.