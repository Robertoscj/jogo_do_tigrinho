-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TigrinhoGame')
BEGIN
    CREATE DATABASE TigrinhoGame;
END
GO

USE TigrinhoGame;
GO

-- Create Players Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Players]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Players] (
        [Id] UNIQUEIDENTIFIER PRIMARY KEY,
        [Username] NVARCHAR(50) NOT NULL UNIQUE,
        [Email] NVARCHAR(100) NOT NULL UNIQUE,
        [PasswordHash] NVARCHAR(255) NOT NULL,
        [Balance] DECIMAL(18, 2) NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME2 NOT NULL,
        [LastLoginAt] DATETIME2 NULL,
        [IsActive] BIT NOT NULL DEFAULT 1
    );
END
GO

-- Create Symbols Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Symbols]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Symbols] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(50) NOT NULL,
        [Code] NVARCHAR(10) NOT NULL UNIQUE,
        [PayoutMultiplier3X] DECIMAL(18, 2) NOT NULL,
        [PayoutMultiplier4X] DECIMAL(18, 2) NOT NULL,
        [PayoutMultiplier5X] DECIMAL(18, 2) NOT NULL,
        [Weight] INT NOT NULL,
        [IsWild] BIT NOT NULL DEFAULT 0,
        [IsScatter] BIT NOT NULL DEFAULT 0,
        [IsActive] BIT NOT NULL DEFAULT 1
    );
END
GO

-- Create PayLines Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayLines]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[PayLines] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [Name] NVARCHAR(50) NOT NULL,
        [Positions] NVARCHAR(50) NOT NULL, 
        [IsActive] BIT NOT NULL DEFAULT 1
    );
END
GO

-- Create Spins Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Spins]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Spins] (
        [Id] UNIQUEIDENTIFIER PRIMARY KEY,
        [PlayerId] UNIQUEIDENTIFIER NOT NULL,
        [BetAmount] DECIMAL(18, 2) NOT NULL,
        [WinAmount] DECIMAL(18, 2) NOT NULL,
        [Matrix] NVARCHAR(MAX) NOT NULL, 
        [CreatedAt] DATETIME2 NOT NULL,
        [IsFreeSpin] BIT NOT NULL DEFAULT 0,
        [PlayerBalanceAfter] DECIMAL(18, 2) NOT NULL,
        [RTPContribution] DECIMAL(18, 4) NOT NULL,
        FOREIGN KEY ([PlayerId]) REFERENCES [Players]([Id])
    );
END
GO

-- Create WinningLines Table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WinningLines]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[WinningLines] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [SpinId] UNIQUEIDENTIFIER NOT NULL,
        [LineNumber] INT NOT NULL,
        [SymbolCode] NVARCHAR(10) NOT NULL,
        [SymbolCount] INT NOT NULL,
        [Multiplier] DECIMAL(18, 2) NOT NULL,
        [WinAmount] DECIMAL(18, 2) NOT NULL,
        FOREIGN KEY ([SpinId]) REFERENCES [Spins]([Id])
    );
END
GO

-- Create indexes for better performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Players_Email' AND object_id = OBJECT_ID('Players'))
BEGIN
    CREATE INDEX [IX_Players_Email] ON [Players]([Email]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Players_Username' AND object_id = OBJECT_ID('Players'))
BEGIN
    CREATE INDEX [IX_Players_Username] ON [Players]([Username]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Spins_PlayerId' AND object_id = OBJECT_ID('Spins'))
BEGIN
    CREATE INDEX [IX_Spins_PlayerId] ON [Spins]([PlayerId]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Spins_CreatedAt' AND object_id = OBJECT_ID('Spins'))
BEGIN
    CREATE INDEX [IX_Spins_CreatedAt] ON [Spins]([CreatedAt]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_WinningLines_SpinId' AND object_id = OBJECT_ID('WinningLines'))
BEGIN
    CREATE INDEX [IX_WinningLines_SpinId] ON [WinningLines]([SpinId]);
END
GO 