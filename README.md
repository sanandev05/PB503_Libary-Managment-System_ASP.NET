# PB503 Library Management System - ASP.NET Core MVC

## Overview
PB503 Library Management System is a web application developed using **ASP.NET Core MVC** designed to automate and streamline library operations. It enables library staff to manage books, users, borrowing, and returning processes efficiently through a user-friendly interface.

## Features
- Manage library books: add, update, delete, and list books
- Borrow and return books with status tracking
- User and admin roles with appropriate access controls
- View borrowing history and current issued books
- Validation and security measures to ensure data integrity
- Responsive UI using HTML, CSS, and JavaScript
- Entity Framework Core for database interaction with migrations support

## Technologies Used
- ASP.NET Core MVC
- Entity Framework Core (Code First)
- SQL Server (or compatible relational database)
- HTML, CSS, JavaScript
- C#

## Project Structure
- **Controllers**: Handle HTTP requests and application logic
- **Models**: Represent data entities such as Books, Users, BorrowRecords
- **View Models**: Data transfer objects for views
- **Views**: Razor views for UI rendering
- **Data**: Database context and migration files
- **Migrations**: EF Core migration scripts
- **wwwroot**: Static assets like CSS and JS
- **Program.cs**: Application startup and configuration

## Getting Started

### Prerequisites
- Visual Studio 2022 or later / .NET 6 SDK or newer
- SQL Server or SQL Server Express instance
- Git

### Installation and Setup
1. Clone the repository:
git clone https://github.com/sanandev05/PB503_Libary-Managment-System_ASP.NET.git

text
2. Open the solution file `PB503_Libary Managment System_ASP.NET.sln` in Visual Studio.
3. Configure your database connection string in `appsettings.json` or `appsettings.Development.json`.
4. Open the Package Manager Console and run:
Update-Database

text
This will apply migrations and create the database schema.
5. Build and run the application (F5 or `dotnet run`).

## Usage
- Access the web app via the local server URL.
- Use the interface to add new books, manage borrowing and returns.
- Admin users can manage users and library inventory.
- The system tracks book availability and borrowing status.

## Contributing
Contributions are welcome. Please fork the repository and submit pull requests with improvements or bug fixes.

## License
No license specified. Please contact the repository owner for details.

---

*This README is created based on the repository structure and common practices for ASP.NET Core MVC Library Management Systems.*
