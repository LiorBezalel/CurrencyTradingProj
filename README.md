# Currency Trading Simulator

A real-time currency trading simulation project built with C# and WPF using .NET 9.

---

## Project Structure

- **BusinessLayer** – Simulation logic and currency rate updates.
- **DataLayer** – Data access layer for SQL Server database.
- **UILayer (WPF)** – User interface displaying currency pairs and real-time data.

---

## Requirements

- SQL Server (Express or full version)
- .NET 9
- Visual Studio 2022 or later (recommended)

---

## Database Setup

1. Open the `create_tables.sql` file using SQL Server Management Studio (SSMS).
2. Execute the script to create the database, tables, and seed initial data.

> **Note:** The SQL script is intended to be run only within SSMS.

---

## Running the Project

1. Open the solution in Visual Studio.
2. Make sure the database connection string (`connectionString`) is configured correctly in the code (configure it at UI_Layer
/MainWindow.xaml.cs).
3. Run the project – a WPF window will open showing a data grid updating in real time.
4. The simulator updates currency values every 2 seconds and calculates min/max values.

---

Good luck!
