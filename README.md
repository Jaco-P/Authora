# Authora

Authora is a Blazor Server application built with .NET 9 that supports user management, group assignment, and permission modeling.  
It was developed as part of a coding assessment with an emphasis on:

- ✅ Clean architecture
- ✅ Code-first Entity Framework Core
- ✅ Scalable and maintainable design
- ✅ SQLite-based persistence
- ✅ Modular service and UI layers

---

## 🏗️ Architecture

This project follows Clean Architecture principles:

---

## ⚙️ Tech Stack

- .NET 9 Core
- Blazor Server
- Entity Framework Core (SQLite)
- Bootstrap 5 (UI styling)

---

## 📦 Features

| Feature                        | Status |
|-------------------------------|--------|
| User CRUD                     | ✅ Done |
| Group CRUD                    | ✅ Done |
| Assign groups to users        | ✅ Done |
| View user group assignments   | ✅ Done |
| Edit group assignments        | ✅ Done |
| Code-first EF Core setup      | ✅ Done |
| SQLite persistence            | ✅ Done |
| Success messages with timeout | ✅ Done |
| Clean service layering        | ✅ Done |
| Fully interactive UI          | ✅ Done |

---

## 🧪 Bonus

- Group assignment is interactive (checkboxes/dropdowns)
- Success alerts auto-dismiss after 4 seconds
- Designed for extensibility (permissions per group, RBAC ready)
- No third-party dependencies required

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- Visual Studio 2022 (Preview) or VS Code

### Steps

```bash
git clone https://github.com/your-username/Authora.git
cd Authora
dotnet build
dotnet ef database update --project Authora.Infrastructure --startup-project Authora
dotnet run --project Authora

## 📄 License

All rights reserved. This code is proprietary and was submitted solely for assessment purposes.  
No part of this codebase may be reused, copied, or redistributed without explicit permission.

![image](https://github.com/user-attachments/assets/d06567a6-cec0-4c2d-8073-f45fb9b727c9)


