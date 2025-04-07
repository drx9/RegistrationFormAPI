# Registration Form API

A complete ASP.NET Core 8 Web API project that performs CRUD operations for user registration using PostgreSQL and Npgsql.

## 🏗️ Architecture

The project follows a three-tier architecture:
- **Presentation Layer**: API Controllers
- **Business Layer**: Business logic and validation
- **Data Access Layer**: Database interaction via Npgsql

## 🛠️ Tech Stack

- ASP.NET Core 8 Web API
- PostgreSQL
- Npgsql (for direct database access)
- Swagger UI for API testing

## 📋 Prerequisites

- .NET 8 SDK
- PostgreSQL 12 or higher
- Visual Studio 2022 or VS Code with C# extensions

## 🚀 Setup Instructions

1. Clone the repository
2. Create a PostgreSQL database named `RegistrationDB`
3. Run the database scripts from `Database/Scripts` folder
4. Update the connection string in `appsettings.json`
5. Run the application

## 📦 Database Setup

1. Create a new PostgreSQL database:
```sql
CREATE DATABASE RegistrationDB;
```

2. Run the schema and stored procedures scripts from `Database/Scripts` folder

## 🔍 API Testing

1. Run the application
2. Navigate to `https://localhost:5041/swagger`
3. Use Swagger UI to test the API endpoints

## 📁 Project Structure

```
RegistrationAPI/
├── Presentation/
│   └── Controllers/
├── Business/
│   ├── Models/
│   └── Services/
├── DataAccess/
│   ├── Repositories/
│   └── Database/
└── Common/
    └── Utilities/
```

## 📝 API Endpoints

- POST /api/users - Create a new user
- GET /api/users - Get all users
- GET /api/users/{id} - Get user by ID
- PUT /api/users/{id} - Update user
- DELETE /api/users/{id} - Delete user

## 🔒 Security

- Passwords are hashed using SHA256
- No plain-text passwords are stored
- Input validation is performed at both API and database levels
