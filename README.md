# Proyecto Personal - Backend API

[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-green.svg)](https://dotnet.microsoft.com/apps/aspnet)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-10.0-purple.svg)](https://learn.microsoft.com/ef/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Express-red.svg)](https://www.microsoft.com/sql-server)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-yellow.svg)](https://swagger.io/)

## Resumen Ejecutivo

Proyecto Personal es un backend ASP.NET Core Web API construido para demostrar una solución empresarial moderna con:

- Autenticación JWT profesional con ASP.NET Core Identity.
- Gestión de inventario con control de stock y alertas de umbral mínimo.
- Acortador de URLs con registro de clicks y métricas de uso.
- Arquitectura limpia escalable para integración con clientes web o móviles.

Esta presentación está orientada a mostrar una implementación práctica de API segura, documentada y mantenible, lista para evaluación técnica.

## Descripción

Backend ASP.NET Core Web API diseñado para presentar una solución empresarial con:

- Autenticación y autorización profesional basada en JWT.
- Gestión de inventario con control de stock y umbral mínimo.
- Acortador de URLs con seguimiento de clicks y métricas.
- Arquitectura limpia con separación de capas.

Esta solución está construida para ser una prueba de concepto sólida y documentada, adecuada para una presentación de estilo Microsoft.

## Tabla de Contenidos

- [Características](#-características)
- [Tecnologías](#-tecnologías)
- [Prerrequisitos](#-prerrequisitos)
- [Instalación](#-instalación)
- [Configuración](#-configuración)
- [Uso](#-uso)
- [Autenticación](#-autenticación)
- [Arquitectura](#-arquitectura)
- [Base de Datos](#-base-de-datos)
- [Testing](#-testing)
- [Contribuir](#-contribuir)
- [Licencia](#-licencia)

## ✨ Características

### Gestión de Inventario
- Control de stock de recursos.
- Validación de descuentos y transacciones de stock.
- Alertas de stock crítico cuando se alcanza el umbral mínimo.
- Prevención de stock negativo.

### Acortador de URLs
- Generación de enlaces cortos únicos.
- Redirección automática a la URL original.
- Registro de clicks y seguimiento de uso.
- Consultas de métricas de alto rendimiento.

### Autenticación y Seguridad
- Implementación de ASP.NET Core Identity.
- Tokens JWT firmados y verificados.
- Refresh tokens para renovar sesión de manera segura.
- Rutas protegidas con políticas de autorización.

### Calidad Técnica
- Arquitectura limpia (Clean Architecture).
- API RESTful con Swagger/OpenAPI.
- Persistencia con Entity Framework Core.
- Consultas de alto rendimiento con Dapper.
- Inyección de dependencias y servicios desacoplados.
- Manejo de errores y validación estructurada.

## 🛠️ Tecnologías

- **.NET 10.0**
- **ASP.NET Core 10.0**
- **C# 12**
- **Entity Framework Core 10.0**
- **SQL Server Express**
- **Dapper 2.1.72**
- **Swashbuckle.AspNetCore 10.1.7**
- **Microsoft.IdentityModel.Tokens**

## 📋 Prerrequisitos

### Software requerido
- **.NET 10.0 SDK**
- **SQL Server Express**
- **Git**

### Verificación

```powershell
dotnet --version
Get-Service -Name "MSSQL$SQLEXPRESS"
```

## 🚀 Instalación

```powershell
git clone https://github.com/PushoDev/Proyecto-Personal-Recordando.git
cd Proyecto-Personal-Recordando
dotnet restore ProyectoPersonal.slnx
dotnet build ProyectoPersonal.slnx
```

## ⚙️ Configuración

### Connection String

En `ApiProyecto/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=ProyectoPersonalDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Key": "SuperSecretKey12345678901234567890123456789012345678901234567890",
    "Issuer": "ProyectoPersonal",
    "Audience": "ProyectoPersonalUsers",
    "ExpiryInMinutes": 60,
    "RefreshTokenExpiryInDays": 7
  }
}
```

### Puertos y entorno

Por defecto la API se inicia en `http://localhost:5241`.

Para cambiar el puerto o el entorno, edite `ApiProyecto/Properties/launchSettings.json`.

## 🎯 Uso

### Ejecutar la API

```powershell
cd ApiProyecto
dotnet run
```

### Acceder a la documentación

- **Swagger**: `http://localhost:5241/swagger`
- **Health check**: `http://localhost:5241/weatherforecast`

## 🔐 Autenticación

Esta API usa JWT para autenticación y ASP.NET Core Identity para la gestión de usuarios.

### Endpoints principales

- `POST /api/auth/register`
  - Registra un usuario nuevo.
  - Campos: `email`, `password`, `confirmPassword`, `nombreCompleto`.

- `POST /api/auth/login`
  - Autentica con correo y contraseña.
  - Devuelve token JWT y refresh token.

- `POST /api/auth/refresh`
  - Renueva el access token con un refresh token válido.

- `POST /api/auth/revoke`
  - Revoca un refresh token.

### Ejemplo: registro de usuario

```bash
curl -X POST "http://localhost:5241/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test123!",
    "confirmPassword": "Test123!",
    "nombreCompleto": "Usuario Test"
  }'
```

### Ejemplo: login

```bash
curl -X POST "http://localhost:5241/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test123!"
  }'
```

### Ejemplo: refresh token

```bash
curl -X POST "http://localhost:5241/api/auth/refresh" \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "<refresh_token_aqui>"
  }'
```

### Ejemplo: revocar token

```bash
curl -X POST "http://localhost:5241/api/auth/revoke" \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "<refresh_token_aqui>"
  }'
```

### Respuesta de ejemplo al iniciar sesión

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "f47ac10b-58cc-4372-a567-0e02b2c3d479",
  "expiration": "2026-04-07T15:30:00Z",
  "user": {
    "id": "12345",
    "email": "test@example.com",
    "nombreCompleto": "Usuario Test",
    "activo": true,
    "fechaRegistro": "2026-04-07T14:00:00Z"
  }
}
```

## 📦 API Reference

### Gestión de Inventario

#### Obtener recurso

```http
GET /api/recursos/{id}
```

#### Descontar stock

```http
PUT /api/recursos/{id}/stock
Content-Type: application/json

{
  "cantidad": 5
}
```

### Acortador de URLs

#### Crear URL corta

```http
POST /api/urlshortener/shorten
Content-Type: application/json

{
  "urlOriginal": "https://www.ejemplo.com/pagina-larga",
  "nombre": "Página de Ejemplo",
  "stockInicial": 100,
  "umbralMinimo": 10
}
```

#### Redirigir a URL original

```http
GET /api/urlshortener/{codigoCorto}
```

## 🏗️ Arquitectura

Este repositorio está estructurado con las siguientes capas:

```
ProyectoPersonal/
├─ ApiProyecto/          # Capa de presentación y API
├─ Application/          # Lógica de aplicación y servicios
├─ Domain/               # Entidades de dominio y contratos
└─ Infraestructura/      # Persistencia y repositorios
```

### Ventajas de esta arquitectura

- Separación de responsabilidades.
- Mejor mantenibilidad.
- Más facilidad para pruebas.
- Menor acoplamiento entre capas.

## 🧱 Base de Datos

Generación de migraciones y actualización de DB:

```powershell
dotnet ef migrations add InitialIdentity --project Infraestructura --startup-project ApiProyecto
dotnet ef database update --project Infraestructura --startup-project ApiProyecto
```

## 🧪 Testing

Actualmente no existe un proyecto de pruebas independiente, pero la solución está diseñada para admitir:

- pruebas unitarias de servicios en `Application`
- pruebas de integración en `ApiProyecto`
- pruebas de repositorios en `Infraestructura`

## 🤝 Contribuir

- Abrir issues para errores o mejoras.
- Enviar pull requests con cambios claros.
- Mantener la separación de capas.

## 📄 Licencia

Este repositorio no incluye una licencia explícita. Agrega un archivo `LICENSE` si deseas publicar bajo términos oficiales.
