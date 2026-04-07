# Instrucciones para Copilot

## Propósito del repositorio
Este es un backend ASP.NET Core Web API dividido en cuatro proyectos:
- `ApiProyecto`: API mínima con controladores y configuración de servicios.
- `Application`: capa de aplicación / casos de uso.
- `Domain`: lógica de dominio, entidades y contratos.
- `Infraestructura`: persistencia / acceso a datos.

El reto principal es implementar un sistema de gestión de inventario y un acortador de URLs dentro de un solo backend.

## Objetivos clave
1. Implementar un endpoint que descuente stock en un recurso.
   - El dominio debe manejar la validación y la lógica de inventario.
   - Si el stock alcanza o cruza el umbral mínimo, se debe disparar una alerta o registrarla en un log.
2. Implementar un acortador de URLs con métricas de clicks.
   - Crear URL corta y almacenar la URL original.
   - Registrar cada click con fecha y hora.
   - Exponer estadísticas de uso por URL.
3. Mantener la separación `Domain` vs `Infraestructura`.
   - `Domain`: entidades, invariantes y contratos.
   - `Infraestructura`: EF Core / Dapper / SQL Server y consultas de rendimiento.
   - `Application`: casos de uso, servicios orquestadores y operaciones de negocio.

## Convenciones de desarrollo
- No hay documentación adicional en el repositorio; infiere la intención del proyecto desde la estructura de los proyectos y los nombres.
- Usa `Domain` para reglas de negocio y validaciones fuertes.
- Usa `Infraestructura` para el acceso a datos y la compatibilidad con SQL Server.
- Usa `ApiProyecto` solo para exponer controladores y configurar DI.
- Si implementas logs de alerta, basta con un logger estructurado o una interfaz de notificación.
- Si hay necesidad de pruebas, preferiré proyectos de prueba o archivos nuevos dentro del mismo repositorio.

## Stack y comandos útiles
- Framework: `.NET 10.0`
- Compilar desde la raíz: `msbuild /property:GenerateFullPaths=true /t:build`
- Tarea disponible: `build`
- Solución principal: `ProyectoPersonal.slnx`

## Qué busca el reviewer
- Separación clara de dominio y persistencia.
- Endpoints REST bien definidos y con manejo de errores adecuado.
- Implementación de inventario con lógica de umbral/minimo en `Domain`.
- Uso de EF Core para persistencia y Dapper para consultas de estadísticas si se necesita un micro-ORM.
- Estructura simple pero mantenible dentro de un solo proyecto de backend.

## Notas específicas del repositorio
- El proyecto está prácticamente en blanco: `Application` e `Infraestructura` no contienen implementación real.
- `Domain` contiene una entidad `Recurso` y una interfaz `IRecursoRepository` que aún necesita corrección.
- No hay archivos README ni documentación de estilo existentes.

## Cómo usar estas instrucciones
- Trabaja en la implementación del backend sin cambiar la estructura de carpetas salvo que sea necesario.
- Mantén las respuestas enfocadas en la tarea de negocio: inventario y acortador de URL.
- Si el reto requiere decidir entre usar EF Core o Dapper, prioriza EF Core para CRUD y usa Dapper en consultas de estadísticas de alto rendimiento.
