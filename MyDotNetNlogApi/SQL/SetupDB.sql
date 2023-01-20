USE master
GO

--USE master; DROP DATABASE NLog;
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'NLog')
BEGIN
  CREATE DATABASE NLog;
END;
GO

USE NLog;
IF OBJECT_ID('AppLogs', 'U') IS NULL
BEGIN
CREATE TABLE dbo.AppLogs(
    Id bigint IDENTITY(1,1) NOT NULL,
    Added_Date nvarchar(40) NOT NULL,
    Level nvarchar(max) NULL,
	Filter int NULL,
	AppUser nvarchar(20) NULL,
    Message nvarchar(max) NULL,
    StackTrace nvarchar(max) NULL,
    Exception nvarchar(max) NULL,
    Logger nvarchar(max) NULL,
    RequestUrl nvarchar(255) NULL,
    RequestType nvarchar(10) NULL,
	);
END;
GO

--SELECT * FROM dbo.AppLogs ORDER BY Id DESC;
SELECT TOP 15 * FROM dbo.AppLogs WHERE Filter >= 100 ORDER BY Id DESC;