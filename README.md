<div align="center">

# 🌟 GleamVault

<!-- Short description -->
<p align="center">
  <strong>A secure and efficient desktop vault application built with .NET MAUI and ASP.NET Core</strong>
</p>

<!-- Badges -->
<p align="center">
  <img src="https://img.shields.io/badge/.NET-MAUI%20Desktop-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/ASP.NET-6.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white" />
  <img src="https://img.shields.io/badge/C%23-10.0-239120?style=for-the-badge&logo=c-sharp&logoColor=white" />
</p>

<p align="center">
  <img src="https://img.shields.io/github/repo-size/Bhbored/GleamVault?style=for-the-badge" />
  <img src="https://img.shields.io/github/languages/top/Bhbored/GleamVault?style=for-the-badge" />
  <img src="https://img.shields.io/github/last-commit/Bhbored/GleamVault?style=for-the-badge" />
</p>

</div>

## 📋 Table of Contents
- [✨ Features](#-features)
- [🛠️ Technologies Used](#️-technologies-used)
- [🏗️ Architecture](#️-architecture)
- [📸 Screenshots](#-screenshots)
- [🚀 Getting Started](#-getting-started)
- [⚙️ Configuration](#️-configuration)
- [🤝 Contributing](#-contributing)
- [📄 License](#-license)

## ✨ Features
- 💼 **Secure Desktop Application**: Built with .NET MAUI for native desktop experience on Windows, macOS, and Linux
- 🖥️ **Robust Backend API**: ASP.NET Core Web API for business logic and data processing
- 🗄️ **Reliable Database**: SQL Server for secure and scalable data storage
- 🔐 **Authentication & Authorization**: Secure user management and access control
- 📊 **Data Management**: Efficient CRUD operations with optimized queries
- 🎨 **Modern UI/UX**: Intuitive and responsive desktop user interface

## 🛠️ Technologies Used

### Frontend (MAUI Desktop)
| Technology | Description |
|------------|-------------|
| [.NET MAUI](https://docs.microsoft.com/en-us/dotnet/maui/) | 🖥️ Cross-platform application framework (Desktop) |
| [C#](https://docs.microsoft.com/en-us/dotnet/csharp/) | 💻 Primary programming language |
| [XAML](https://docs.microsoft.com/en-us/dotnet/desktop/xaml/) | 🎨 User interface markup language |

### Backend
| Technology | Description |
|------------|-------------|
| [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) | ⚙️ Web API framework |
| [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) | 🗃️ ORM for database operations |
| [SQL Server](https://www.microsoft.com/en-us/sql-server/) | 🗄️ Relational database management |

### Additional Tools
| Tool | Purpose |
|------|---------|
| [Visual Studio](https://visualstudio.microsoft.com/) | 🛠️ Development environment |
| [NuGet](https://www.nuget.org/) | 📦 Package management |

## 🏗️ Architecture
┌─────────────────┐ ┌──────────────────┐ ┌─────────────────┐
│ .NET MAUI │ │ ASP.NET Core │ │ SQL Server │
│ Desktop App │◄──►│ API │◄──►│ Database │
│ │ │ │ │ │
└─────────────────┘ └──────────────────┘ └─────────────────┘
## 🚀 Getting Started

### Prerequisites
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Bhbored/GleamVault.git
   cd GleamVault
2. **Set up the database**
bash
# Navigate to the API projectcd GleamVault.API# Update the database connection string in appsettings.json# Then run migrationsdotnet ef database update

3. **Configure environment variables**
Update connection strings in appsettings.json
Configure any API keys or secrets
4. **Run the applications**
bash
# For MAUI desktop clientcd GleamVault.Clientdotnet builddotnet run# For API server (in a separate terminal)cd GleamVault.APIdotnet run

## API Endpoints
GET /api/users - Get all users
POST /api/users - Create a new user
GET /api/users/{id} - Get user by ID
PUT /api/users/{id} - Update user
DELETE /api/users/{id} - Delete user
## ⚙️ Configuration
Database Configuration
Update the connection string in appsettings.json:

## json
```
{  "ConnectionStrings": {    "DefaultConnection": "Server=.;Database=GleamVault;Trusted_Connection=true;TrustServerCertificate=true;"  }}
```

## Environment Variables
ASPNETCORE_ENVIRONMENT - Development, Staging, or Production
ConnectionStrings__DefaultConnection - Database connection string

### 📞 Contact
Bourhan Hassoun - [Your Email] | [LinkedIn Profile]

Project Link: 
```
https://github.com/Bhbored/GleamVault
```
