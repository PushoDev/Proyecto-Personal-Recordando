# Proyecto Personal

> Full-stack application with .NET 10.0 backend and React + MUI frontend.

[![.NET](https://img.shields.io/badge/.NET-10.0-blue?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-green?style=flat-square&logo=microsoft)](https://dotnet.microsoft.com/apps/aspnet)
[![React](https://img.shields.io/badge/React-19-61dafb?style=flat-square&logo=react)](https://react.dev/)
[![TypeScript](https://img.shields.io/badge/TypeScript-6.0-3178c6?style=flat-square&logo=typescript)](https://www.typescriptlang.org/)
[![MUI](https://img.shields.io/badge/MUI-9.0-007fff?style=flat-square&logo=mui)](https://mui.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Express-red?style=flat-square&logo=microsoft)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/license-MIT-green?style=flat-square)](LICENSE)

## Overview

Full-stack application with Clean Architecture demonstrating:
- **Task Manager (TODO)**: Task management with priorities, due dates, and status tracking
- **URL Shortener**: Short URL generation with click tracking and analytics
- **Authentication**: JWT-based auth with refresh tokens

## Features

### Task Manager
- Create, edit, and delete tasks
- Priority levels (Low, Medium, High)
- Due date tracking
- Status management (Pending, In Progress, Completed)
- Overdue and critical task detection

### URL Shortener
- Custom short code generation
- Click tracking and analytics
- High-performance statistics queries using Dapper
- Automatic redirection with click registration

### Authentication
- JWT-based authentication with access and refresh tokens
- Secure password hashing
- Token refresh and revocation

## Technology Stack

### Backend
| Component | Technology |
|-----------|------------|
| Framework | ASP.NET Core 10.0 |
| Language | C# 12 |
| ORM | Entity Framework Core 10.0 |
| Query Performance | Dapper 2.1.72 |
| Authentication | JWT Bearer Tokens |
| Database | SQL Server Express |
| API Documentation | Swagger/OpenAPI |

### Frontend
| Component | Technology |
|-----------|------------|
| Framework | React 19 |
| Language | TypeScript 6.0 |
| Build Tool | Vite 8.0 |
| UI Library | Material-UI 9.0 |
| Routing | React Router 7 |

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads)
- [Node.js 18+](https://nodejs.org/) (for frontend)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository**

```powershell
git clone https://github.com/PushoDev/Proyecto-Personal-Recordando.git
cd Proyecto-Personal-Recordando
```

2. **Restore and build**

```powershell
dotnet restore ProyectoPersonal.slnx
dotnet build ProyectoPersonal.slnx
```

3. **Configure database**

Update `ApiProyecto/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=ProyectoPersonalDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

4. **Run migrations**

```powershell
dotnet ef database update --project Infraestructura --startup-project ApiProyecto
```

5. **Start backend**

```powershell
cd ApiProyecto
dotnet run
```

API: `http://localhost:5241`

6. **Start frontend**

```powershell
cd frontend
npm install
npm run dev
```

Frontend: `http://localhost:5173`

## Architecture

Clean Architecture with 4 layers:

```
ProyectoPersonal/
├── ApiProyecto/           # Controllers, Program.cs, Configuration
├── Application/           # Use cases, Services, DTOs
├── Domain/                 # Entities, Interfaces
└── Infraestructura/        # EF Core, Repositories, Migrations
```

## API Endpoints

### Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register user |
| POST | `/api/auth/login` | Login |
| POST | `/api/auth/refresh` | Refresh token |
| POST | `/api/auth/revoke` | Revoke token |

### Tasks
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/recursos` | List tasks |
| GET | `/api/recursos/{id}` | Get task |
| POST | `/api/recursos` | Create task |
| PUT | `/api/recursos/{id}` | Update task |
| DELETE | `/api/recursos/{id}` | Delete task |
| PUT | `/api/recursos/{id}/stock` | Update stock |

### URL Shortener
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/urlshortener/shorten` | Create short URL |
| GET | `/api/urlshortener/{code}` | Redirect |
| GET | `/api/urlshortener/{code}/stats` | Statistics |

Swagger: `http://localhost:5241/swagger`

## License

MIT License