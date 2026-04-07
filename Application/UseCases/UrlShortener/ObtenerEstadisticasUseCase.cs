using Application.DTOs;
using Domain.Interfaces;
using Infraestructura.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.UrlShortener
{
    public class ObtenerEstadisticasUseCase
    {
        private readonly IRecursoRepository _recursoRepository;
        private readonly string _connectionString;

        public ObtenerEstadisticasUseCase(IRecursoRepository recursoRepository, IConfiguration configuration)
        {
            _recursoRepository = recursoRepository ?? throw new ArgumentNullException(nameof(recursoRepository));
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("DefaultConnection not found");
        }

        public async Task<UrlStatisticsDTO> ExecuteAsync(string codigoCorto)
        {
            if (string.IsNullOrWhiteSpace(codigoCorto))
                throw new ArgumentException("El código corto es requerido.", nameof(codigoCorto));

            // Obtener recurso
            var recurso = await _recursoRepository.GetByCodigoCortoAsync(codigoCorto);
            if (recurso == null)
                throw new KeyNotFoundException($"No se encontró una URL con el código '{codigoCorto}'.");

            // Usar Dapper para query de alto rendimiento en ClickLogs
            using var connection = new SqlConnection(_connectionString);

            var stats = await connection.QueryFirstOrDefaultAsync<UrlStatisticsDTO>(
                @"SELECT
                    r.CodigoCorto,
                    r.Clicks as TotalClicks,
                    COUNT(cl.Id) as ClicksUltimaHora,
                    r.Id as RecursoId,
                    r.Nombre as NombreRecurso
                FROM Recursos r
                LEFT JOIN ClickLogs cl ON r.Id = cl.RecursoId AND cl.FechaHora >= DATEADD(HOUR, -1, GETUTCDATE())
                WHERE r.CodigoCorto = @CodigoCorto
                GROUP BY r.Id, r.CodigoCorto, r.Clicks, r.Nombre",
                new { CodigoCorto = codigoCorto });

            if (stats == null)
                throw new KeyNotFoundException($"No se encontraron estadísticas para el código '{codigoCorto}'.");

            // Obtener clicks por día (últimos 30 días)
            var clicksPorDia = await connection.QueryAsync<ClickPorDiaDTO>(
                @"SELECT
                    CAST(cl.FechaHora AS DATE) as Fecha,
                    COUNT(*) as CantidadClicks
                FROM ClickLogs cl
                INNER JOIN Recursos r ON cl.RecursoId = r.Id
                WHERE r.CodigoCorto = @CodigoCorto
                AND cl.FechaHora >= DATEADD(DAY, -30, GETUTCDATE())
                GROUP BY CAST(cl.FechaHora AS DATE)
                ORDER BY Fecha DESC",
                new { CodigoCorto = codigoCorto });

            stats.ClicksPorDia = clicksPorDia.ToList();

            return stats;
        }
    }
}