CREATE DATABASE CurrencyTradingDB;
GO

USE CurrencyTradingDB;
GO

CREATE TABLE Currency (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Country NVARCHAR(50) NOT NULL,
    CurrencyName NVARCHAR(50) NOT NULL,
    Abbreviation NVARCHAR(10) NOT NULL,
    CurrentValue DECIMAL(10, 4),
    CONSTRAINT UQ_Currency_Name_Country UNIQUE (CurrencyName, Abbreviation)
);

CREATE TABLE CurrencyPair (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FromCurrencyId INT FOREIGN KEY REFERENCES Currency(Id),
    ToCurrencyId INT FOREIGN KEY REFERENCES Currency(Id),
    MinValue DECIMAL(10, 4),
    MaxValue DECIMAL(10, 4)
);

INSERT INTO Currency (Country, CurrencyName, Abbreviation, CurrentValue) VALUES
('United States', 'Dollar', 'USD', 3.67),
('European Union', 'Euro', 'EUR', 3.95),
('United Kingdom', 'Pound', 'GBP', 4.65),
('Israel', 'Shekel', 'ILS', 1.00);

INSERT INTO CurrencyPair (FromCurrencyId, ToCurrencyId, MinValue, MaxValue)
SELECT 
    c1.Id AS FromCurrencyId,
    c2.Id AS ToCurrencyId,
    ROUND(c2.CurrentValue / c1.CurrentValue, 4) AS MinValue,
    ROUND(c2.CurrentValue / c1.CurrentValue, 4) AS MaxValue
FROM Currency c1
JOIN Currency c2 ON c1.Id < c2.Id;


