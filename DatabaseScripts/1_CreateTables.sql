-- Create Users table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(255) NOT NULL,
        Email NVARCHAR(255) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(255) NOT NULL,
        Role NVARCHAR(50) NOT NULL,
        Location NVARCHAR(255) NOT NULL
    );
    PRINT 'Users table created successfully.';
END
ELSE
BEGIN
    PRINT 'Users table already exists.';
END

-- Create Devices table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Devices')
BEGIN
    CREATE TABLE Devices (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(255) NOT NULL,
        Manufacturer NVARCHAR(255) NOT NULL,
        Type NVARCHAR(255) NOT NULL,
        OperatingSystem NVARCHAR(255) NOT NULL,
        OsVersion NVARCHAR(255) NOT NULL,
        Processor NVARCHAR(100) NOT NULL,
        RamAmountGB INT NOT NULL CHECK (RamAmountGB > 0),
        Description NVARCHAR(MAX) NULL,
        AssignedUserID INT NULL CONSTRAINT FK_Devices_Users FOREIGN KEY (AssignedUserID) REFERENCES Users(Id) ON DELETE SET NULL
    );
    PRINT 'Devices table created successfully.';
END
ELSE
BEGIN
    PRINT 'Devices table already exists.';
END

-- Index on AssignedUserId
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Devices_AssignedUserID' AND object_id = OBJECT_ID('Devices'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Devices_AssignedUserID ON Devices(AssignedUserID);
    PRINT 'Index IX_Devices_AssignedUserID created.';
END

-- Index on Manufacturer
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Devices_Manufacturer' AND object_id = OBJECT_ID('Devices'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Devices_Manufacturer ON Devices(Manufacturer);
    PRINT 'Index IX_Devices_Manufacturer created.';
END

-- Index on Type
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Devices_Type' AND object_id = OBJECT_ID('Devices'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Devices_Type ON Devices(Type);
    PRINT 'Index IX_Devices_Type created.';
END