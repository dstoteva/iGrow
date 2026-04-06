# 🚀 iGrow

> iGrow is a task & habit management web application built with ASP.NET Core and Razor views. It helps users create, schedule, and track tasks and habits with categories, recurring types, and measurable goals.

![.NET Version](https://img.shields.io/badge/.NET-10.0-purple)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-10.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)
![Tests](https://img.shields.io/badge/tests-NUnit%20%2B%20Moq-yellow)

---

## 📋 Table of Contents

- [About the Project](#about-the-project)
- [Technologies Used](#technologies-used)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Features](#features)
- [Admin Area](#admin-area)
- [Category Icons](#category-icons)
- [Testing](#testing)
- [Usage](#usage)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

## 📖 About the Project

iGrow is a web application for managing tasks and habits. It supports per-user tasks and habits with categories, recurring types, amounts, and measurable metrics. It integrates ASP.NET Identity for authentication and role-based authorization, uses Entity Framework Core for persistence, and includes an admin area for managing users, categories, recurring types, and amounts.

---

## 🛠️ Technologies Used

| Technology            | Version  | Purpose                          |
|-----------------------|----------|----------------------------------|
| ASP.NET Core MVC      | 10.0.2   | Web framework                    |
| Entity Framework Core | 10.0.2   | ORM / Database access            |
| SQL Server / SQLite   | -        | Database                         |
| Bootstrap (Bootswatch Cosmo) | 5.x | Frontend styling              |
| Razor Pages / Views   | -        | Server-side HTML rendering       |
| ASP.NET Identity      | -        | Authentication & role management |
| NUnit                 | -        | Unit testing framework           |
| Moq                   | -        | Mocking library for tests        |
| Coverlet              | -        | Code coverage collection         |

---

## ✅ Prerequisites

Make sure you have the following installed before running the project:

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- [Visual Studio 2026](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [Git](https://git-scm.com/)

---

## 🚀 Getting Started

Follow these steps to get the project running locally.

### 1. Clone the repository

```sh
git clone https://github.com/dstoteva/iGrow.git
cd iGrow
```

### 2. Restore dependencies

```sh
dotnet restore
```

### 3. Apply database migrations

```sh
dotnet ef database update
```

### 4. Run the application

```sh
dotnet run
```

The app will be available at `https://localhost:7149`.

---

## 📁 Project Structure

```
iGrow/
│
├── iGrow/                        # Main web project
│   ├── Controllers/              # MVC Controllers (Home, MyTasks, Habits)
│   ├── Areas/Admin/              # Admin area (Categories, Amounts, RecurringTypes, Users)
│   │   ├── Controllers/
│   │   └── Views/
│   ├── Views/                    # Razor Views (.cshtml)
│   ├── wwwroot/                  # Static files (CSS, JS, images)
│   │   ├── css/theme.css         # Custom Bootswatch overrides
│   │   └── images/categories/    # Category icon SVGs
│   └── Program.cs                # App entry point and middleware setup
│
├── iGrow.Data/                   # DbContext, migrations, and repositories
│   ├── Migrations/
│   ├── Repository/
│   └── ApplicationDbContext.cs
│
├── iGrow.Data.Models/            # Domain entity models
├── iGrow.Web.ViewModels/         # View models for MVC views
├── iGrow.Web.Infrastructure/     # Service/repository DI registration extensions
├── iGrow.Services/               # Business logic / service layer
│   └── Contracts/                # Service interfaces
├── iGrow.GCommon/                # Shared constants, validation, custom exceptions
└── iGrow.Services.Tests/         # Unit tests (NUnit + Moq)
```

---

## ✨ Features

- [x] User registration and login (ASP.NET Identity)
- [x] Role-based authorization (Admin, User)
- [x] CRUD operations for MyTask
- [x] CRUD operations for Habits (with amounts, metrics, units)
- [x] Soft-delete and hard-delete for tasks and habits
- [x] Categories with uploadable icon images (SVG/PNG/JPG/GIF)
- [x] Seeded category icons via EF migration
- [x] Recurring types (Daily, Weekly, etc.)
- [x] Amounts (Less than, More than, etc.)
- [x] Search and pagination on list views
- [x] Admin area for managing categories, amounts, recurring types, and users
- [x] Custom error pages (400, 404, 500) via `UseStatusCodePagesWithReExecute`
- [x] Input validation (server-side & client-side)
- [x] Responsive UI with Bootswatch (Cosmo) + custom theme
- [x] Unit tests for all service methods (NUnit + Moq)
- [x] Code coverage with Coverlet

---

## 🔐 Admin Area

The admin area is available at `/Admin` and requires the **Admin** role. It provides management for:

- **Categories** – Create / delete categories with icon upload (max 2 MB; `.png`, `.jpg`, `.jpeg`, `.gif`)
- **Amounts** – Create / delete amount types
- **Recurring Types** – Create / delete recurring schedule types
- **Users** – View users, assign/remove roles, delete users

Roles are seeded on application startup via `UseRolesSeeder` and `UseAdminSeeder` middleware.

---

## 🖼️ Category Icons

Each category can have an associated icon image. Icons are:

- Uploaded via the admin Create Category form (`enctype="multipart/form-data"`)
- Stored under `wwwroot/images/categories/`
- Displayed as small thumbnails (40×40) in list and detail views
- Seeded for default categories via the `SeedCategoriesImages` migration

A `default.png` fallback is used when no icon is set.

---

## 🧪 Testing

The project includes unit tests for all service-layer methods using **NUnit** and **Moq**.

### Test coverage

| Service               | Methods tested |
|-----------------------|---------------|
| `AmountService`       | ItemExistsByName, Add, GetAll, GetById, Delete |
| `CategoryService`     | ItemExistsByName, Add, GetAll, GetById, Delete |
| `RecurringTypeService`| ItemExistsByName, Add, GetAll, GetById, Delete |
| `HabitService`        | Add, GetById, GetDetails, Edit, GetToBeDeleted, SoftDelete, HardDelete, IsUserCreator, GetCount, GetAll |
| `MyTaskService`       | Add, GetById, GetDetails, Edit, GetToBeDeleted, SoftDelete, HardDelete, IsUserCreator, GetCount, GetAll |
| `UserService`         | GetAllManageableUsers, AssignRole, RemoveRole, DeleteUser |

### Run tests

```sh
dotnet test
```

### Collect coverage

```sh
dotnet test -c Debug --collect:"XPlat Code Coverage"
```

### Generate HTML report (optional)

```sh
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html
```

---

## 💻 Usage

```
1. Navigate to /Register to create an account.
2. Log in at /Login.
3. Use the dashboard to manage your Tasks and Habits.
4. Admin users can access /Admin to manage categories, amounts, recurring types, and users.
```

## 🗄️ Database Setup

The project uses **Entity Framework Core** with a Code-First approach.

Connection string is configured in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=iGrow;Trusted_Connection=True;"
}
```

To create and seed the database:

```sh
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Seed data includes default categories (with icons), recurring types, and amounts.

---

## ⚙️ Configuration

Key settings in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=iGrow;Trusted_Connection=True;"
  },
  "Identity": {
    "SignIn": {
      "RequireConfirmedAccount": false,
      "RequireConfirmedEmail": false,
      "RequireConfirmedPhoneNumber": false
    },
    "User": { "RequireUniqueEmail": true },
    "Password": {
      "RequireDigit": true,
      "RequireLowercase": true,
      "RequireUppercase": true,
      "RequireNonAlphanumeric": false,
      "RequiredLength": 6,
      "RequiredUniqueChars": 1
    },
    "Lockout": {
      "MaxFailedAccessAttempts": 5,
      "DefaultLockoutTimeSpanMin": 5
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

---

## 📬 Contact

**Denitsa Toteva** – [@dstoteva](https://github.com/dstoteva)

Project Link: [https://github.com/dstoteva/iGrow](https://github.com/dstoteva/iGrow)

---

*Built as part of the **ASP.NET Advanced** course.*