# Authora
Authora is a Blazor Server application built on .NET 9 that provides user management, group-based access control, and a permission system.
It was developed as part of a coding assessment and emphasizes:

✅ Clean Architecture

✅ Entity Framework Core (Code-First)

✅ Modular Design & Separation of Concerns

✅ SQLite Persistence

✅ Dynamic Service/UI Composition

✅ API-First Interoperability

# 🏗️ Architecture
This project follows Clean Architecture principles with clear separation between:

Authora.Domain – Core entities and interfaces

Authora.Application – Service interfaces and DTOs

Authora.Infrastructure – EF Core DbContext and service implementations

Authora.API – RESTful API endpoints

Authora – Blazor Server UI (RCL)

Authora.Tests.Unit and Authora.Tests.Integration – Full test coverage support

# ⚙️ Tech Stack
.NET 9

Blazor Server

Entity Framework Core (SQLite)

Bootstrap 5 (for styling)

xUnit & Moq for testing

#📦 Features
Feature	Status
User CRUD	✅ Done
Group CRUD	✅ Done
Permission CRUD	✅ Done
Assign groups to users	✅ Done
Assign permissions to groups	✅ Done
View user group + permission tree	✅ Done
Dashboard with all user relations	✅ Done
Code-first EF Core migrations	✅ Done
SQLite database	✅ Done
Auto-migration at startup	✅ Done
API endpoints for all operations	✅ Done
Structured error handling	✅ Done
Interactive Blazor UI	✅ Done
Success alerts with timeout	✅ Done

# 📡 API Endpoints
All endpoints are exposed under:

bash
Copy
Edit
https://localhost:7215/api/
Users
GET /user – Get all users

GET /user/{id} – Get a single user

POST /user – Create a user

PUT /user/{id} – Update a user

DELETE /user/{id} – Delete a user

Groups
GET /group

GET /group/{id}

POST /group

DELETE /group/{id}

Permissions
GET /permission

GET /permission/by-group/{groupId}

POST /permission

DELETE /permission/{id}

# 🧪 Testing
Authora.Tests.Unit includes tests for service logic, e.g., UserService

Authora.Tests.Integration (planned) for DB + API integration scenarios

✅ Assessment Goals Met
 Implement clean architecture

 Model user/group/permission relationships

 Persist all data via SQLite

 Build interactive and maintainable Blazor UI

 Expose full REST API

 Build unit test coverage for services

# 🚀 Getting Started
Prerequisites
.NET 9 SDK

Visual Studio 2022 (Preview) or VS Code

Commands
bash
Copy
Edit
git clone https://github.com/your-username/Authora.git
cd Authora
dotnet build
dotnet ef database update --project Authora.Infrastructure --startup-project Authora
dotnet run --project Authora

# 📄 License
All rights reserved.
This code was developed solely for assessment purposes.
Do not reuse or distribute without explicit permission.

