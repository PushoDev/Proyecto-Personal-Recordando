using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Inventory;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class RecursoApplicationService : IRecursoApplicationService
    {
        private readonly ObtenerRecursoUseCase _obtenerRecursoUseCase;
        private readonly DescontarStockUseCase _descontarStockUseCase;
        private readonly ILogger<RecursoApplicationService> _logger;

        public RecursoApplicationService(
            IRecursoRepository recursoRepository,
            ILogger<RecursoApplicationService> logger)
        {
            _obtenerRecursoUseCase = new ObtenerRecursoUseCase(recursoRepository);
            _descontarStockUseCase = new DescontarStockUseCase(recursoRepository);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RecursoDTO?> ObtenerRecursoAsync(int id)
        {
            try
            {
                return await _obtenerRecursoUseCase.ExecuteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener recurso con ID {Id}", id);
                throw;
            }
        }

        public async Task<RecursoDTO> DescontarStockAsync(int recursoId, int cantidad)
        {
            try
            {
                var result = await _descontarStockUseCase.ExecuteAsync(recursoId, cantidad);

                // Log de alerta si está en estado crítico
                if (result.EstaEnEstadoCritico)
                {
                    _logger.LogWarning(
                        "ALERTA: Recurso '{Nombre}' (ID: {Id}) ha alcanzado stock crítico. Stock actual: {Stock}, Umbral: {Umbral}",
                        result.Nombre, result.Id, result.Stock, result.UmbralMinimo);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al descontar stock del recurso {RecursoId}, cantidad {Cantidad}",
                    recursoId, cantidad);
                throw;
            }
        }
    }
}