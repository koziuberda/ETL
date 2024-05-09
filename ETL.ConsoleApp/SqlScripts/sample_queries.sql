-- Query 1: Find the PULocationId with the highest average TipAmount
SELECT TOP 1 PULocationID, AVG(TipAmount) AS AvgTipAmount
FROM Trips
GROUP BY PULocationID
ORDER BY AVG(TipAmount) DESC;

-- Query 2: Find the top 100 longest fares in terms of TripDistance
SELECT TOP 100 *
FROM Trips
ORDER BY TripDistance DESC;

-- Query 3: Find the top 100 longest fares in terms of time spent traveling
SELECT TOP 100 *
FROM Trips
ORDER BY TimeSpentTraveling DESC;

-- Query 4: Search where part of the conditions is PULocationId
DECLARE @PULocationId INT = 5; -- Replace @PULocationID with the desired value
SELECT *
FROM Trips
WHERE PULocationID = @PULocationID; 