# Proyecto Personal - Backend API

[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-10.0-green.svg)](https://dotnet.microsoft.com/apps/aspnet)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-10.0-purple.svg)](https://docs.microsoft.com/ef/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Express-red.svg)](https://www.microsoft.com/sql-server)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-yellow.svg)](https://swagger.io/)

Un backend ASP.NET Core Web API que implementa un sistema de gestión de inventario y un acortador de URLs, desarrollado siguiendo los principios de Clean Architecture.

## 📋 Tabla de Contenidos

- [Características](#-características)
- [Tecnologías](#-tecnologías)
- [Prerrequisitos](#-prerrequisitos)
- [Instalación](#-instalación)
- [Configuración](#-configuración)
- [Uso](#-uso)
- [Arquitectura](#-arquitectura)
- [API Reference](#-api-reference)
- [Base de Datos](#-base-de-datos)
- [Testing](#-testing)
- [Contribución](#-contribución)
- [Licencia](#-licencia)

## ✨ Características

### Gestión de Inventario
- ✅ Control de stock de recursos
- ✅ Alertas automáticas cuando el stock alcanza el umbral mínimo
- ✅ Validación de operaciones de descuento de stock
- ✅ Prevención de stock negativo

### Acortador de URLs
- ✅ Generación de códigos cortos únicos usando Base58
- ✅ Seguimiento de clicks por URL
- ✅ Estadísticas detalladas (total clicks, clicks por hora, historial diario)
- ✅ Redireccionamiento automático a URLs originales

### Características Técnicas
- ✅ Arquitectura limpia (Clean Architecture)
- ✅ API RESTful con documentación Swagger/OpenAPI
- ✅ Persistencia con Entity Framework Core
- ✅ Consultas de alto rendimiento con Dapper
- ✅ Inyección de dependencias
- ✅ Logging estructurado
- ✅ Validación de datos
- ✅ Manejo de errores consistente

## 🛠️ Tecnologías

### Framework y Runtime
- **.NET 10.0** - Framework de desarrollo
- **ASP.NET Core 10.0** - Framework web
- **C# 12** - Lenguaje de programación

### Base de Datos y ORM
- **SQL Server Express** - Motor de base de datos
- **Entity Framework Core 10.0** - ORM para operaciones CRUD
- **Dapper 2.1.72** - Micro-ORM para consultas de alto rendimiento

### Documentación y Testing
- **Swashbuckle.AspNetCore 10.1.7** - Generación de documentación Swagger/OpenAPI
- **Microsoft.AspNetCore.OpenApi 10.0.5** - Soporte OpenAPI

### Herramientas de Desarrollo
- **Visual Studio 2022** - IDE principal
- **SQL Server Management Studio** - Administración de base de datos
- **Postman** - Testing de APIs
- **Git** - Control de versiones

## 📋 Prerrequisitos

Antes de comenzar, asegúrate de tener instalados los siguientes componentes:

### Requisitos del Sistema
- **Sistema Operativo**: Windows 10/11, macOS, o Linux
- **Memoria RAM**: Mínimo 4 GB (recomendado 8 GB)
- **Espacio en Disco**: 2 GB libres

### Software Requerido
- **.NET 10.0 SDK** - [Descargar](https://dotnet.microsoft.com/download/dotnet/10.0)
- **SQL Server Express** - [Descargar](https://www.microsoft.com/sql-server/sql-server-downloads)
- **Visual Studio 2022** (opcional) - [Descargar](https://visualstudio.microsoft.com/)
- **Git** - [Descargar](https://git-scm.com/)

### Verificación de Instalación
```bash
# Verificar .NET SDK
dotnet --version

# Verificar SQL Server (desde PowerShell como administrador)
Get-Service -Name "MSSQL$SQLEXPRESS"
```

## 🚀 Instalación

### 1. Clonar el Repositorio
```bash
git clone https://github.com/PushoDev/Proyecto-Personal-Recordando.git
cd Proyecto-Personal-Recordando
```

### 2. Restaurar Paquetes NuGet
```bash
dotnet restore ProyectoPersonal.slnx
```

### 3. Compilar la Solución
```bash
dotnet build ProyectoPersonal.slnx
```

### 4. Configurar la Base de Datos
```bash
# Crear la base de datos y aplicar migraciones
dotnet ef database update --project Infraestructura --startup-project ApiProyecto
```

## ⚙️ Configuración

### Variables de Entorno
El proyecto utiliza configuración basada en archivos `appsettings.json`. Los archivos de configuración principales son:

- `ApiProyecto/appsettings.json` - Configuración base
- `ApiProyecto/appsettings.Development.json` - Configuración de desarrollo

### Connection String
La cadena de conexión a SQL Server se configura en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=ProyectoPersonalDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Configuración de Puerto
La aplicación se ejecuta por defecto en `http://localhost:5241`. Para cambiar el puerto, modifica `Properties/launchSettings.json`.

## 🎯 Uso

### Ejecutar la Aplicación
```bash
# Desde el directorio raíz del proyecto
cd ApiProyecto
dotnet run
```

### Acceder a la API
- **API Base URL**: `http://localhost:5241`
- **Documentación Swagger**: `http://localhost:5241/swagger`
- **Health Check**: `http://localhost:5241/weatherforecast`

### Probar la API
```bash
# Ejemplo: Crear una URL corta
curl -X POST "http://localhost:5241/api/urlshortener/shorten" \
  -H "Content-Type: application/json" \
  -d '{
    "urlOriginal": "https://www.ejemplo.com",
    "nombre": "Página de Ejemplo",
    "stockInicial": 100,
    "umbralMinimo": 10
  }'
```

## 🏗️ Arquitectura

El proyecto sigue los principios de **Clean Architecture** con separación clara de responsabilidades:

```
ProyectoPersonal/
├── ApiProyecto/              # Capa de Presentación (API)
│   ├── Controllers/          # Controladores REST
│   ├── DTOs/                 # Objetos de Transferencia de Datos
│   └── Program.cs           # Punto de entrada de la aplicación
├── Application/              # Capa de Aplicación
│   ├── UseCases/            # Casos de uso
│   ├── Services/            # Servicios de aplicación
│   ├── DTOs/                # DTOs de aplicación
│   └── Interfaces/          # Interfaces de aplicación
├── Domain/                   # Capa de Dominio
│   ├── Entidades/           # Entidades de negocio
│   └── Interfaces/          # Interfaces de dominio
└── Infraestructura/          # Capa de Infraestructura
    ├── Data/                # Contexto de EF Core
    ├── Repositories/        # Implementaciones de repositorios
    └── Migrations/          # Migraciones de base de datos
```

### Principios Arquitectónicos
- **Separación de Responsabilidades**: Cada capa tiene una responsabilidad específica
- **Inversión de Dependencias**: Las capas superiores no dependen de las inferiores
- **Inyección de Dependencias**: Todas las dependencias se resuelven en tiempo de ejecución
- **Principio de Responsabilidad Única**: Cada clase tiene una única razón para cambiar

## 📚 API Reference

### Gestión de Inventario

#### Obtener Recurso
```http
GET /api/recursos/{id}
```

**Respuesta (200 OK):**
```json
{
  "id": 1,
  "nombre": "Producto A",
  "stock": 95,
  "umbralMinimo": 10,
  "urlOriginal": null,
  "codigoCorto": null,
  "clicks": 0,
  "estaEnEstadoCritico": false
}
```

#### Descontar Stock
```http
PUT /api/recursos/{id}/stock
Content-Type: application/json

{
  "cantidad": 5
}
```

**Respuesta (200 OK):**
```json
{
  "id": 1,
  "stock": 90,
  "estaEnEstadoCritico": false
}
```

### Acortador de URLs

#### Crear URL Corta
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

**Respuesta (201 Created):**
```json
{
  "id": 2,
  "codigoCorto": "Ab3Xy9",
  "urlOriginal": "https://www.ejemplo.com/pagina-larga"
}
```

#### Redirigir a URL Original
```http
GET /api/urlshortener/{codigoCorto}
```
**Respuesta:** Redirección 302 a la URL original

#### Obtener Estadísticas
```http
GET /api/urlshortener/{codigoCorto}/stats
```

**Respuesta (200 OK):**
```json
{
  "codigoCorto": "Ab3Xy9",
  "totalClicks": 42,
  "clicksUltimaHora": 3,
  "recursoId": 2,
  "clicksPorDia": [
    {
      "fecha": "2026-04-07",
      "cantidadClicks": 15
    }
  ]
}
```

### Códigos de Estado HTTP
- `200 OK` - Operación exitosa
- `201 Created` - Recurso creado
- `302 Found` - Redirección
- `400 Bad Request` - Datos inválidos
- `404 Not Found` - Recurso no encontrado
- `409 Conflict` - Stock insuficiente
- `500 Internal Server Error` - Error del servidor

## 🗄️ Base de Datos

### Esquema
El proyecto utiliza Entity Framework Core con un enfoque Code-First. Las entidades principales son:

#### Recurso
```sql
CREATE TABLE Recursos (
    Id INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(255) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    UmbralMinimo INT NOT NULL DEFAULT 10,
    UrlOriginal NVARCHAR(MAX),
    CodigoCorto NVARCHAR(10) UNIQUE,
    Clicks INT NOT NULL DEFAULT 0,
    EstaEnEstadoCritico BIT NOT NULL DEFAULT 0
);
```

#### ClickLog
```sql
CREATE TABLE ClickLogs (
    Id INT PRIMARY KEY IDENTITY,
    RecursoId INT NOT NULL,
    FechaClick DATETIME2 NOT NULL DEFAULT GETDATE(),
    IpAddress NVARCHAR(45),
    UserAgent NVARCHAR(MAX),
    FOREIGN KEY (RecursoId) REFERENCES Recursos(Id)
);
```

### Migraciones
```bash
# Crear nueva migración
dotnet ef migrations add NombreMigracion --project Infraestructura --startup-project ApiProyecto

# Aplicar migraciones
dotnet ef database update --project Infraestructura --startup-project ApiProyecto

# Revertir migración
dotnet ef database update NombreMigracionAnterior --project Infraestructura --startup-project ApiProyecto
```

## 🧪 Testing

### Ejecutar Tests
```bash
# Ejecutar todos los tests
dotnet test

# Ejecutar tests con cobertura
dotnet test --collect:"XPlat Code Coverage"
```

### Testing Manual
1. **Usar Swagger UI**: `http://localhost:5241/swagger`
2. **Usar Postman**: Importar colección desde `ApiProyecto/ApiProyecto.http`
3. **Usar curl**: Ver ejemplos en la documentación

### Casos de Prueba Recomendados
- ✅ Crear URL corta
- ✅ Obtener recurso existente
- ✅ Descontar stock normal
- ✅ Descontar stock por debajo del umbral
- ✅ Intentar descontar stock insuficiente
- ✅ Redirigir URL corta
- ✅ Obtener estadísticas
- ✅ Acceder a recurso inexistente

## 🤝 Contribución

### Guías de Contribución
1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

### Estándares de Código
- Seguir las convenciones de nomenclatura de C#
- Usar async/await para operaciones I/O
- Implementar logging apropiado
- Escribir tests para nueva funcionalidad
- Actualizar documentación según sea necesario

### Reportar Issues
Usar las plantillas de issue disponibles en el repositorio para:
- Reportar bugs
- Solicitar nuevas características
- Preguntas generales

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo `LICENSE` para más detalles.

## 📞 Soporte

Para soporte técnico o preguntas:
- Crear un issue en GitHub
- Revisar la documentación en Swagger UI
- Consultar los logs de la aplicación

---

**Desarrollado con ❤️ usando .NET 10.0 y ASP.NET Core**</content>
<parameter name="filePath"> README.md