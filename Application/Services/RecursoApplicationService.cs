using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.Inventory;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class RecursoApplicationService : IRecursoApplicationService
    {
        private readonly IRecursoRepository _recursosRepository;
        private readonly ObtenerTodosRecursosUseCase _obtenerTodosRecursosUseCase;
        private readonly ObtenerRecursoUseCase _obtenerRecursoUseCase;
        private readonly CrearRecursoUseCase _crearRecursoUseCase;
        private readonly ActualizarRecursoUseCase _actualizarRecursoUseCase;
        private readonly EliminarRecursoUseCase _eliminarRecursoUseCase;
        private readonly DescontarStockUseCase _descontarStockUseCase;
        private readonly ILogger<RecursoApplicationService> _logger;

        public RecursoApplicationService(
            IRecursoRepository recursoRepository,
            ILogger<RecursoApplicationService> logger)
        {
            _recursosRepository = recursoRepository;
            _obtenerTodosRecursosUseCase = new ObtenerTodosRecursosUseCase(recursoRepository);
            _obtenerRecursoUseCase = new ObtenerRecursoUseCase(recursoRepository);
            _crearRecursoUseCase = new CrearRecursoUseCase(recursoRepository);
            _actualizarRecursoUseCase = new ActualizarRecursoUseCase(recursoRepository);
            _eliminarRecursoUseCase = new EliminarRecursoUseCase(recursoRepository);
            _descontarStockUseCase = new DescontarStockUseCase(recursoRepository);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<RecursoDTO>> ObtenerTodosAsync()
        {
            try
            {
                return await _obtenerTodosRecursosUseCase.ExecuteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los recursos");
                throw;
            }
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

        public async Task<RecursoDTO> CrearRecursoAsync(CreateRecursoRequest request)
        {
            try
            {
                return await _crearRecursoUseCase.ExecuteAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear recurso");
                throw;
            }
        }

        public async Task<RecursoDTO> ActualizarRecursoAsync(int id, CreateRecursoRequest request)
        {
            try
            {
                return await _actualizarRecursoUseCase.ExecuteAsync(id, request);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar recurso {Id}", id);
                throw;
            }
        }

        public async Task EliminarRecursoAsync(int id)
        {
            try
            {
                await _eliminarRecursoUseCase.ExecuteAsync(id);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar recurso {Id}", id);
                throw;
            }
        }

        public async Task<RecursoDTO> DescontarStockAsync(int recursoId, int cantidad)
        {
            try
            {
                var result = await _descontarStockUseCase.ExecuteAsync(recursoId, cantidad);

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

        public async Task<RecursoDTO> AgregarStockAsync(int recursoId, int cantidad)
        {
            try
            {
                var recurso = await _obtenerRecursoUseCase.ExecuteAsync(recursoId);
                if (recurso == null)
                    throw new KeyNotFoundException($"Recurso con ID {recursoId} no encontrado");

                var recursoEntity = await _recursosRepository.GetByIdAsync(recursoId);
                if (recursoEntity == null)
                    throw new KeyNotFoundException($"Recurso con ID {recursoId} no encontrado");

                recursoEntity.AgregarStock(cantidad);
                await _recursosRepository.UpdateAsync(recursoEntity);

                return new RecursoDTO
                {
                    Id = recursoEntity.Id,
                    Nombre = recursoEntity.Nombre,
                    Stock = recursoEntity.Stock,
                    UmbralMinimo = recursoEntity.UmbralMinimo,
                    UrlOriginal = recursoEntity.UrlOriginal,
                    CodigoCorto = recursoEntity.CodigoCorto,
                    Clicks = recursoEntity.Clicks,
                    EstaEnEstadoCritico = recursoEntity.Stock <= recursoEntity.UmbralMinimo
                };
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar stock al recurso {RecursoId}, cantidad {Cantidad}",
                    recursoId, cantidad);
                throw;
            }
        }
    }
}