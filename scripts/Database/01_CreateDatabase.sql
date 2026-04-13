-- ============================================================================
-- SCRIPT SQL: Creación de Base de Datos - ProyectoPersonal
-- ============================================================================
-- Este script crea la base de datos y todas las tablas necesarias para el 
-- sistema de gestión de inventario y tareas con autenticación JWT.
--
-- USAGE: Ejecutar en SQL Server Management Studio (SSMS) o Azure Data Studio
-- ============================================================================

USE master;
GO

-- Crear la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'ProyectoPersonalDb')
BEGIN
    CREATE DATABASE ProyectoPersonalDb;
    PRINT 'Base de datos ProyectoPersonalDb creada correctamente.';
END
ELSE
BEGIN
    PRINT 'La base de datos ProyectoPersonalDb ya existe.';
END
GO

USE ProyectoPersonalDb;
GO

-- ============================================================================
-- PARTE 1: TABLAS DE ASP.NET CORE IDENTITY
-- ============================================================================
-- Identity maneja la autenticación y autorización de usuarios.
-- Incluye tablas para usuarios, roles, claims, logins y tokens.

-- Tabla de usuarios (extiende IdentityUser con campos propios)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUsers')
BEGIN
    CREATE TABLE AspNetUsers (
        Id NVARCHAR(450) NOT NULL PRIMARY KEY,           -- Identity usa NVARCHAR(450)
        NombreCompleto NVARCHAR(200) NOT NULL,
        FechaRegistro DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        Activo BIT NOT NULL DEFAULT 1,
        
        -- Campos heredados de IdentityUser:
        UserName NVARCHAR(256) NULL,
        NormalizedUserName NVARCHAR(256) NULL,
        Email NVARCHAR(256) NULL,
        NormalizedEmail NVARCHAR(256) NULL,
        EmailConfirmed BIT NOT NULL DEFAULT 0,
        PasswordHash NVARCHAR(MAX) NULL,
        SecurityStamp NVARCHAR(MAX) NULL,
        ConcurrencyStamp NVARCHAR(MAX) NULL,
        PhoneNumber NVARCHAR(MAX) NULL,
        PhoneNumberConfirmed BIT NOT NULL DEFAULT 0,
        TwoFactorEnabled BIT NOT NULL DEFAULT 0,
        LockoutEnd DATETIMEOFFSET NULL,
        LockoutEnabled BIT NOT NULL DEFAULT 1,
        AccessFailedCount INT NOT NULL DEFAULT 0
    );
    
    -- Índice único para emails normalizados (mejor búsqueda)
    CREATE UNIQUE INDEX IX_AspNetUsers_NormalizedEmail ON AspNetUsers(NormalizedEmail);
    CREATE UNIQUE INDEX IX_AspNetUsers_NormalizedUserName ON AspNetUsers(NormalizedUserName);
    
    PRINT 'Tabla AspNetUsers creada correctamente.';
END
GO

-- Tabla de roles
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetRoles')
BEGIN
    CREATE TABLE AspNetRoles (
        Id NVARCHAR(450) NOT NULL PRIMARY KEY,
        Name NVARCHAR(256) NULL,
        NormalizedName NVARCHAR(256) NULL,
        ConcurrencyStamp NVARCHAR(MAX) NULL
    );
    
    CREATE UNIQUE INDEX IX_AspNetRoles_NormalizedName ON AspNetRoles(NormalizedName);
    PRINT 'Tabla AspNetRoles creada correctamente.';
END
GO

-- Tabla de relación Usuario-Rol (muchos a muchos)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserRoles')
BEGIN
    CREATE TABLE AspNetUserRoles (
        UserId NVARCHAR(450) NOT NULL,
        RoleId NVARCHAR(450) NOT NULL,
        PRIMARY KEY (UserId, RoleId),
        FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
        FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
    );
    PRINT 'Tabla AspNetUserRoles creada correctamente.';
END
GO

-- Tabla de claims (reclamaciones/permisos de usuario)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserClaims')
BEGIN
    CREATE TABLE AspNetUserClaims (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        UserId NVARCHAR(450) NOT NULL,
        ClaimType NVARCHAR(MAX) NULL,
        ClaimValue NVARCHAR(MAX) NULL,
        FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_AspNetUserClaims_UserId ON AspNetUserClaims(UserId);
    PRINT 'Tabla AspNetUserClaims creada correctamente.';
END
GO

-- Tabla de logins externos (Google, Facebook, etc.)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserLogins')
BEGIN
    CREATE TABLE AspNetUserLogins (
        LoginProvider NVARCHAR(128) NOT NULL,
        ProviderKey NVARCHAR(128) NOT NULL,
        ProviderDisplayName NVARCHAR(MAX) NULL,
        UserId NVARCHAR(450) NOT NULL,
        PRIMARY KEY (LoginProvider, ProviderKey),
        FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
    );
    PRINT 'Tabla AspNetUserLogins creada correctamente.';
END
GO

-- Tabla de tokens de usuario
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserTokens')
BEGIN
    CREATE TABLE AspNetUserTokens (
        UserId NVARCHAR(450) NOT NULL,
        LoginProvider NVARCHAR(128) NOT NULL,
        Name NVARCHAR(128) NOT NULL,
        Value NVARCHAR(MAX) NULL,
        PRIMARY KEY (UserId, LoginProvider, Name),
        FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
    );
    PRINT 'Tabla AspNetUserTokens creada correctamente.';
END
GO

-- ============================================================================
-- PARTE 2: TABLAS DE NEGOCIO
-- ============================================================================

-- Tabla de Recursos (Inventario y Tareas)
-- Una tabla polimórfica que sirve tanto para inventario como para tareas
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Recursos')
BEGIN
    CREATE TABLE Recursos (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Nombre NVARCHAR(200) NOT NULL,
        Descripcion NVARCHAR(1000) NULL,
        
        -- Campos de inventario
        Stock INT NOT NULL DEFAULT 0,
        UmbralMinimo INT NOT NULL DEFAULT 0,
        UrlOriginal NVARCHAR(2000) NULL,
        CodigoCorto NVARCHAR(50) NULL,
        Clicks INT NOT NULL DEFAULT 0,
        
        -- Campos de tarea
        FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
        FechaVencimiento DATETIME2 NULL,
        Prioridad INT NOT NULL DEFAULT 1,      -- 0=Baja, 1=Media, 2=Alta
        Estado INT NOT NULL DEFAULT 0,         -- 0=Pendiente, 1=EnProgreso, 2=Completada
        
        -- Relación con usuario (opcional - recurso puede no tener usuario asignado)
        UserId NVARCHAR(450) NULL,
        FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE SET NULL
    );
    
    -- Índices para mejorar rendimiento en consultas comunes
    CREATE INDEX IX_Recursos_CodigoCorto ON Recursos(CodigoCorto) WHERE CodigoCorto IS NOT NULL;
    CREATE INDEX IX_Recursos_Nombre ON Recursos(Nombre);
    CREATE INDEX IX_Recursos_Estado ON Recursos(Estado);
    CREATE INDEX IX_Recursos_Prioridad ON Recursos(Prioridad);
    CREATE INDEX IX_Recursos_FechaVencimiento ON Recursos(FechaVencimiento);
    CREATE INDEX IX_Recursos_UserId ON Recursos(UserId);
    
    PRINT 'Tabla Recursos creada correctamente.';
END
GO

-- Tabla de Logs de Clics (para estadísticas de URL shortener)
-- Registra cada click en una URL acortada
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ClickLogs')
BEGIN
    CREATE TABLE ClickLogs (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        RecursoId INT NOT NULL,
        FechaHora DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IpOrigen NVARCHAR(45) NULL,  -- IPv6 máximo: 45 caracteres
        
        -- Relación con recurso (cada click pertenece a un recurso)
        FOREIGN KEY (RecursoId) REFERENCES Recursos(Id) ON DELETE CASCADE
    );
    
    -- Índices para consultas analíticas
    CREATE INDEX IX_ClickLogs_RecursoId ON ClickLogs(RecursoId);
    CREATE INDEX IX_ClickLogs_FechaHora ON ClickLogs(FechaHora);
    CREATE INDEX IX_ClickLogs_RecursoId_FechaHora ON ClickLogs(RecursoId, FechaHora);
    
    PRINT 'Tabla ClickLogs creada correctamente.';
END
GO

-- Tabla de Refresh Tokens (para autenticación JWT con refresh)
-- Permite renovar tokens sin hacer login de nuevo
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RefreshTokens')
BEGIN
    CREATE TABLE RefreshTokens (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Token NVARCHAR(500) NOT NULL,
        UserId NVARCHAR(450) NOT NULL,
        ExpiryDate DATETIME2 NOT NULL,
        CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsRevoked BIT NOT NULL DEFAULT 0,
        
        -- Relación con usuario
        FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
    );
    
    -- Índices
    CREATE UNIQUE INDEX IX_RefreshTokens_Token ON RefreshTokens(Token);
    CREATE INDEX IX_RefreshTokens_UserId_IsRevoked ON RefreshTokens(UserId, IsRevoked);
    
    PRINT 'Tabla RefreshTokens creada correctamente.';
END
GO

-- ============================================================================
-- PARTE 3: DATOS DE EJEMPLO (OPCIONAL - DESCOMENTA SI LO NECESITAS)
-- ============================================================================

-- Insertar rol de administrador
--IF NOT EXISTS (SELECT * FROM AspNetRoles WHERE Name = 'Admin')
--BEGIN
--    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
--    VALUES (NEWID(), 'Admin', 'ADMIN', NEWID());
--    
--    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
--    VALUES (NEWID(), 'User', 'USER', NEWID());
--END
--GO

-- Insertar usuario de prueba (contraseña: Admin123!)
--IF NOT EXISTS (SELECT * FROM AspNetUsers WHERE UserName = 'admin@ejemplo.com')
--BEGIN
--    INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, 
--                             EmailConfirmed, PasswordHash, SecurityStamp, NombreCompleto, Activo)
--    VALUES (
--        NEWID(),
--        'admin@ejemplo.com',
--        'ADMIN@EJEMPLO.COM',
--        'admin@ejemplo.com',
--        'ADMIN@EJEMPLO.COM',
--        1,
--        'AQAAAAIAAYagAAAAELN+Fk+5T8k3L5K5T8k3L5K5T8k3L5K5T8k3L5K5T8k3L5K5T8k3L5K5T8k3L5K5T8k3L5K5Q==',  -- Hash placeholder
--        NEWID(),
--        'Administrador del Sistema',
--        1
--    );
--END
--GO

-- Insertar recursos de ejemplo (Inventario)
--IF NOT EXISTS (SELECT * FROM Recursos WHERE Nombre = 'Laptop HP ProBook')
--BEGIN
--    INSERT INTO Recursos (Nombre, Descripcion, Stock, UmbralMinimo, Clicks, FechaCreacion, Prioridad, Estado)
--    VALUES 
--        ('Laptop HP ProBook', 'Laptop de trabajo para desarrollo', 5, 2, 0, GETDATE(), 1, 0),
--        ('Monitor Dell 24"', 'Monitor Full HD para estación de trabajo', 8, 3, 0, GETDATE(), 1, 0),
--        ('Teclado Mecánico', 'Teclado gaming con switches azules', 12, 5, 0, GETDATE(), 0, 0),
--        ('Ratón Inalámbrico', 'Ratón ergonómico recargable', 3, 5, 0, GETDATE(), 2, 0),  -- Stock bajo!
--        ('Cable USB-C 1m', 'Cable de carga y datos', 25, 10, 0, GETDATE(), 0, 0);
--END
--GO

-- Insertar recursos de ejemplo (Tareas)
--IF NOT EXISTS (SELECT * FROM Recursos WHERE FechaVencimiento IS NOT NULL)
--BEGIN
--    INSERT INTO Recursos (Nombre, Descripcion, Stock, UmbralMinimo, FechaVencimiento, Prioridad, Estado)
--    VALUES 
--        ('Revisar código pull request #42', 'Hacer code review del PR del módulo de usuarios', 0, 0, DATEADD(day, 3, GETDATE()), 2, 0),
--        ('Actualizar documentación API', 'Documentar nuevos endpoints de inventario', 0, 0, DATEADD(day, 7, GETDATE()), 1, 0),
--        ('Reunión semanal de equipo', 'Sprint planning del próximo sprint', 0, 0, DATEADD(day, 1, GETDATE()), 1, 1),  -- En progreso
--        ('Tarea vencida de ejemplo', 'Esta tarea ya debería estar completada', 0, 0, DATEADD(day, -5, GETDATE()), 0, 0);  -- Vencida
--END
--GO

-- ============================================================================
-- VERIFICACIÓN: Mostrar resumen de lo creado
-- ============================================================================
PRINT '';
PRINT '========================================';
PRINT 'RESUMEN DE ESTRUCTURA CREADA';
PRINT '========================================';
PRINT '';

SELECT 'AspNetUsers' AS Tabla, COUNT(*) AS Registros FROM AspNetUsers
UNION ALL
SELECT 'AspNetRoles', COUNT(*) FROM AspNetRoles
UNION ALL
SELECT 'Recursos', COUNT(*) FROM Recursos
UNION ALL
SELECT 'ClickLogs', COUNT(*) FROM ClickLogs
UNION ALL
SELECT 'RefreshTokens', COUNT(*) FROM RefreshTokens;

PRINT '';
PRINT 'Tablas de Identity: AspNetUsers, AspNetRoles, AspNetUserRoles, AspNetUserClaims, AspNetUserLogins, AspNetUserTokens';
PRINT 'Tablas de Negocio: Recursos, ClickLogs, RefreshTokens';
PRINT '';
PRINT 'Script completado exitosamente!';
GO
