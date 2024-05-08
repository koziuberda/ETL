-- Check if the database exists, if not, create it
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TripsDatabase')
BEGIN
    CREATE DATABASE TripsDatabase;
    PRINT 'Database created successfully.';
END
ELSE
BEGIN
    PRINT 'Database already exists.';
END
GO

-- Use the database
USE TripsDatabase;
GO

-- Drop the table if it exists
IF OBJECT_ID('Trips', 'U') IS NOT NULL
BEGIN
DROP TABLE Trips;
PRINT 'Existing table dropped successfully.';
END
ELSE
BEGIN
    PRINT 'Table does not exist.';
END
GO

-- Create the table to store the taxi trip data
CREATE TABLE Trips (
    TripID INT IDENTITY(1,1) PRIMARY KEY,
    PickupDateTime DATETIME,
    DropoffDateTime DATETIME,
    PassengerCount INT,
    TripDistance FLOAT,
    StoreAndFwdFlag VARCHAR(3),
    PULocationID INT,
    DOLocationID INT,
    FareAmount DECIMAL(10,2),
    TipAmount DECIMAL(10,2)
);
PRINT 'New table created successfully.';
GO