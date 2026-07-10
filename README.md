# Finance Management

ASP.NET Web Forms application for managing personal finances with income and expense tracking, budgets, reminders, notes, and reporting.

## Features

- User registration and login with secure password handling
- Income and expense tracking with add, edit, and delete support
- Monthly budget limits per expense category
- Dashboard with summary cards, charts, budget progress, and recent transactions
- Income report, expense report, and combined profit/loss report with charts
- Reminders and notes

## Technology stack

- ASP.NET Web Forms (.NET Framework 4.8)
- SQL Server
- C#
- HTML, CSS, and Bootstrap

## Requirements

- Windows with Visual Studio 2019+ and the ASP.NET web development workload
- .NET Framework 4.8
- SQL Server or SQL Server Express
- IIS Express

## Database setup

1. Open SQL Server Management Studio (SSMS) or use `sqlcmd`.
2. Run the script at `Database/schema.sql`.
3. This creates the `finance_mgt` database and the main tables for registration, income, expense, reminders, notes, and budgets.

## Connection string

Edit `Finance_management/Web.config` and set your SQL Server instance:

```xml
<add name="connstr"
     connectionString="Data Source=YOUR_PC\SQLEXPRESS;Initial Catalog=finance_mgt;Integrated Security=True"/>
```

Replace `YOUR_PC\SQLEXPRESS` with your server name.

## Run the project

1. Open `Finance_management.sln` in Visual Studio.
2. Restore NuGet packages if prompted.
3. Press F5 to run with IIS Express.
4. Register a new account and sign in.

## Existing users and passwords

Passwords are stored using PBKDF2 hashing. Existing plain-text passwords continue to work on first login and are upgraded automatically.

## Project structure

- `Home.aspx` — dashboard and charts
- `income.aspx` / `expense.aspx` — transaction entry and management
- `budget.aspx` — monthly category budgets
- `Report.aspx` — profit/loss reporting
- `incomereport.aspx` / `expensereport.aspx` — detailed reports
- `reminder.aspx` / `note.aspx` — reminders and notes
- `Database/schema.sql` — database setup script
- `PasswordHelper.cs` — password hashing logic
- `ValidationHelper.cs` — server-side validation

## Security notes

- Change `debug="false"` in `Web.config` before production deployment.
- Do not commit real connection strings with production credentials.
- Session timeout is set to 30 minutes.

## Troubleshooting

- If the database cannot be reached, verify that SQL Server is running and the connection string is correct.
- If you see build error MSB4019, install the ASP.NET and web development workload in Visual Studio.
- If the budget page errors, run `schema.sql` again to create the `budgets` table.
- If login fails after an upgrade, re-register or reset the password in the database.

## Author

Finance Management — academic and personal finance project.
