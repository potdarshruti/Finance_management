-- Finance Management - Database Setup
-- Run on SQL Server (SQL Express or full)

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'finance_mgt')
BEGIN
    CREATE DATABASE finance_mgt;
END
GO

USE finance_mgt;
GO

IF OBJECT_ID(N'dbo.registration', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.registration (
        srno INT IDENTITY(1,1) PRIMARY KEY,
        name NVARCHAR(100) NOT NULL,
        email NVARCHAR(150) NOT NULL UNIQUE,
        contact NVARCHAR(15) NOT NULL,
        Category NVARCHAR(50) NOT NULL,
        password NVARCHAR(256) NOT NULL
    );
END
GO

IF OBJECT_ID(N'dbo.income', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.income (
        srno INT IDENTITY(1,1) PRIMARY KEY,
        in_date DATE NOT NULL,
        in_category NVARCHAR(100) NOT NULL,
        in_amount NVARCHAR(50) NOT NULL,
        in_remark NVARCHAR(500) NULL,
        loginid INT NOT NULL,
        CONSTRAINT FK_income_registration FOREIGN KEY (loginid) REFERENCES dbo.registration(srno)
    );
END
GO

IF OBJECT_ID(N'dbo.expense', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.expense (
        srno INT IDENTITY(1,1) PRIMARY KEY,
        ex_date DATE NOT NULL,
        ex_category NVARCHAR(100) NOT NULL,
        ex_amount NVARCHAR(50) NOT NULL,
        ex_remark NVARCHAR(500) NULL,
        loginid INT NOT NULL,
        CONSTRAINT FK_expense_registration FOREIGN KEY (loginid) REFERENCES dbo.registration(srno)
    );
END
GO

IF OBJECT_ID(N'dbo.reminders', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.reminders (
        srno INT IDENTITY(1,1) PRIMARY KEY,
        re_date DATE NOT NULL,
        re_tittle NVARCHAR(200) NOT NULL,
        re_remark NVARCHAR(500) NULL,
        loginid INT NOT NULL,
        CONSTRAINT FK_reminders_registration FOREIGN KEY (loginid) REFERENCES dbo.registration(srno)
    );
END
GO

IF OBJECT_ID(N'dbo.notes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.notes (
        srno INT IDENTITY(1,1) PRIMARY KEY,
        note_tittle NVARCHAR(200) NOT NULL,
        note_remark NVARCHAR(1000) NULL,
        loginid INT NOT NULL,
        CONSTRAINT FK_notes_registration FOREIGN KEY (loginid) REFERENCES dbo.registration(srno)
    );
END
GO

IF OBJECT_ID(N'dbo.budgets', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.budgets (
        srno INT IDENTITY(1,1) PRIMARY KEY,
        loginid INT NOT NULL,
        budget_month DATE NOT NULL,
        ex_category NVARCHAR(100) NOT NULL,
        budget_amount FLOAT NOT NULL,
        CONSTRAINT FK_budgets_registration FOREIGN KEY (loginid) REFERENCES dbo.registration(srno),
        CONSTRAINT UQ_budget_user_month_category UNIQUE (loginid, budget_month, ex_category)
    );
END
GO

PRINT 'Finance Management database schema is ready.';
