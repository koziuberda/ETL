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

-- Add a persisted computed column for the time spent traveling
ALTER TABLE Trips
    ADD TimeSpentTraveling AS DATEDIFF(MINUTE, PickupDateTime, DropoffDateTime) PERSISTED;

PRINT 'New table created successfully.';

-- Index creation
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_PULocationID_TipAmount' AND object_id = OBJECT_ID('Trips'))
BEGIN
CREATE INDEX idx_PULocationID_TipAmount ON Trips (PULocationID, TipAmount);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_TripDistance' AND object_id = OBJECT_ID('Trips'))
BEGIN
CREATE INDEX idx_TripDistance ON Trips (TripDistance);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_TravelTime' AND object_id = OBJECT_ID('Trips'))
BEGIN
CREATE INDEX idx_TravelTime ON Trips (TimeSpentTraveling);
END
