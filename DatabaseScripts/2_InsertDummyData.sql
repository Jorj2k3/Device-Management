USE DeviceManagementDb;
GO

-- Insert dummy Users
-- Password for all dummy users is: DummyHash123!
-- Hashed with BCrypt
IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'admin@company.com')
BEGIN
    INSERT INTO Users (Name, Email, PasswordHash, Role, Location)
    VALUES ('Alice Admin', 'admin@company.com', '$2a$11$n0CXny2iWLWrQoZ6FNyHgO2Q8.hJMqLicQKIz./v5QJYCcfJvri8O', 'Admin', 'Cluj-Napoca');
END
ELSE
BEGIN
    UPDATE Users SET PasswordHash = '$2a$11$n0CXny2iWLWrQoZ6FNyHgO2Q8.hJMqLicQKIz./v5QJYCcfJvri8O' WHERE Email = 'admin@company.com';
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'bob@company.com')
BEGIN
    INSERT INTO Users (Name, Email, PasswordHash, Role, Location)
    VALUES ('Bob Builder', 'bob@company.com', '$2a$11$n0CXny2iWLWrQoZ6FNyHgO2Q8.hJMqLicQKIz./v5QJYCcfJvri8O', 'User', 'London');
END
ELSE
BEGIN
    UPDATE Users SET PasswordHash = '$2a$11$n0CXny2iWLWrQoZ6FNyHgO2Q8.hJMqLicQKIz./v5QJYCcfJvri8O' WHERE Email = 'bob@company.com';
END

IF NOT EXISTS (SELECT 1 FROM Users WHERE Email = 'charlie@company.com')
BEGIN
    INSERT INTO Users (Name, Email, PasswordHash, Role, Location)
    VALUES ('Charlie Chaplin', 'charlie@company.com', '$2a$11$n0CXny2iWLWrQoZ6FNyHgO2Q8.hJMqLicQKIz./v5QJYCcfJvri8O', 'User', 'New York');
END
ELSE
BEGIN
    UPDATE Users SET PasswordHash = '$2a$11$n0CXny2iWLWrQoZ6FNyHgO2Q8.hJMqLicQKIz./v5QJYCcfJvri8O' WHERE Email = 'charlie@company.com';
END

PRINT 'User checks and inserts completed.';
GO

-- Insert dummy Devices
DECLARE @AliceId INT = (SELECT Id FROM Users WHERE Email = 'admin@company.com');
DECLARE @BobId INT = (SELECT Id FROM Users WHERE Email = 'bob@company.com');

-- Device 1: Alice's MacBook
IF NOT EXISTS (SELECT 1 FROM Devices WHERE Name = 'MacBook Pro 16' AND AssignedUserID = @AliceId)
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OperatingSystem, OsVersion, Processor, RamAmountGB, Description, Status, AssignedUserID)
    VALUES ('MacBook Pro 16', 'Apple', 'Laptop', 'macOS', 'Sonoma 14.4', 'M3 Max', 32, 'High-end developer machine', 'In Use', @AliceId);
END

-- Device 2: Bob's ThinkPad
IF NOT EXISTS (SELECT 1 FROM Devices WHERE Name = 'ThinkPad X1 Carbon' AND AssignedUserID = @BobId)
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OperatingSystem, OsVersion, Processor, RamAmountGB, Description, Status, AssignedUserID)
    VALUES ('ThinkPad X1 Carbon', 'Lenovo', 'Laptop', 'Windows', '11 Pro', 'Intel Core i7', 16, 'Standard issue corporate laptop', 'In Use', @BobId);
END

-- Device 3: Bob's iPhone
IF NOT EXISTS (SELECT 1 FROM Devices WHERE Name = 'iPhone 15 Pro' AND AssignedUserID = @BobId)
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OperatingSystem, OsVersion, Processor, RamAmountGB, Description, Status, AssignedUserID)
    VALUES ('iPhone 15 Pro', 'Apple', 'Smartphone', 'iOS', '17.4', 'A17 Pro', 8, 'Corporate mobile device', 'In Use', @BobId);
END

-- Device 4: Unassigned Samsung Tablet (Status uses default 'Available')
IF NOT EXISTS (SELECT 1 FROM Devices WHERE Name = 'Galaxy Tab S9' AND AssignedUserID IS NULL)
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OperatingSystem, OsVersion, Processor, RamAmountGB, Description, AssignedUserID)
    VALUES ('Galaxy Tab S9', 'Samsung', 'Tablet', 'Android', '14.0', 'Snapdragon 8 Gen 2', 12, 'For field testing', NULL);
END

PRINT 'Device checks and inserts completed.';
GO