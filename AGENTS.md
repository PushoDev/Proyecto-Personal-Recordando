# AGENTS.md - ProyectoPersonal

## Project Overview

This repository contains a full-stack application:
- **Backend**: ASP.NET Core Web API (.NET 10.0) with Clean Architecture
- **Frontend**: React 19 + TypeScript + Vite + MUI

### Backend Structure (4 projects)
- `ApiProyecto`: API controllers, endpoints, DI configuration
- `Application`: Use cases, business orchestration
- `Domain`: Entities, business rules, interfaces
- `Infraestructura`: EF Core + SQL Server persistence

### Key Features to Implement
1. Inventory management with stock threshold alerts
2. URL shortener with click metrics and statistics

---

## Build / Lint / Test Commands

### Backend (.NET)
```bash
# Build entire solution
dotnet build

# Build specific project
dotnet build ApiProyecto/ApiProyecto.csproj

# Run API (from ApiProyecto folder)
dotnet run

# No test framework configured yet
```

### Frontend (React/TypeScript)
```bash
cd frontend

# Install dependencies
npm install

# Development server
npm run dev

# Build for production
npm run build

# Lint code
npm run lint

# Preview production build
npm run preview
```

---

## Code Style Guidelines

### C# Backend Conventions

**Project Layering**
- `Domain`: Entities, interfaces (repositories), domain logic, invariants
- `Infraestructura`: EF Core DbContext, repository implementations, Dapper queries
- `Application`: Use cases, DTOs, service orchestration
- `ApiProyecto`: Controllers, Program.cs, DI setup

**Naming**
- Classes: PascalCase (`Recurso`, `UrlShortenerService`)
- Interfaces: `I` prefix (`IRecursoRepository`)
- Methods: PascalCase
- Private fields: `_camelCase` or `camelCase`

**Types**
- Use nullable reference types (`string?`, `List<T>?`)
- Prefer `record` for immutable DTOs
- Use `var` for local type inference

**Error Handling**
- Return proper HTTP status codes (400 for validation, 404 for not found, 500 for errors)
- Use try-catch in controllers with problem details response
- Log exceptions via `ILogger<T>`

**Database**
- EF Core for CRUD operations
- Dapper for high-performance statistical queries
- Use migrations for schema changes

---

### Frontend Conventions (React + TypeScript)

**File Structure**
```
frontend/src/
├── components/    # Reusable UI components
├── pages/         # Route pages
├── api/           # API calls
├── types/         # TypeScript interfaces
└── App.tsx        # Main app
```

**Component Style**
- Use functional components with hooks
- Prefer `const` over function declarations
- Type props explicitly

**TypeScript**
- Use explicit types for props, state, and API responses
- Avoid `any`
- Use `interface` for object shapes

**Imports**
- Use absolute imports from `@/` (configured in tsconfig)
- Order: external libs → internal modules → relative

**MUI Usage**
- Use `sx` prop for custom styling
- Follow theme conventions
- Use `Stack` and `Box` for layout

**State Management**
- Local state with `useState` for simple cases
- Consider context for shared state

---

## Existing Documentation

- `.github/copilot-instructions.md` - Project-specific guidelines (included above)
- `README.md` and `FRONTEND.md` - General project info
- `ApiProyecto/API_Endpoints.md` - API documentation

---

## Notes for Agents

1. **Do not modify** the project folder structure without good reason
2. **Prioritize** Domain layer for business rules (especially inventory validation)
3. **Use EF Core** for CRUD; Dapper only for high-performance statistics queries
4. **Follow REST conventions** for API endpoints
5. **Handle errors** with proper HTTP status codes and structured responses
6. **Alerts/logging** for stock threshold crossing can use `ILogger` or notification interface

The backend is mostly blank - `Application` and `Infraestructura` need implementation.