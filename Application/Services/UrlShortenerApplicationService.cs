using Application.DTOs;
using Application.Interfaces;
using Application.UseCases.UrlShortener;
using Domain.Interfaces;
using Infraestructura.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UrlShortenerApplicationService : IUrlShortenerService
    {
        private readonly CrearUrlCortaUseCase _crearUrlCortaUseCase;
        private readonly RegistrarClickUseCase _registrarClickUseCase;
        private readonly ObtenerEstadisticasUseCase _obtenerEstadisticasUseCase;
        private readonly ILogger<UrlShortenerApplicationService> _logger;

        public UrlShortenerApplicationService(
            IRecursoRepository recursoRepository,
            RecursoDbContext dbContext,
            IConfiguration configuration,
            ILogger<UrlShortenerApplicationService> logger)
        {
            _crearUrlCortaUseCase = new CrearUrlCortaUseCase(recursoRepository);
            _registrarClickUseCase = new RegistrarClickUseCase(recursoRepository, dbContext);
            _obtenerEstadisticasUseCase = new ObtenerEstadisticasUseCase(recursoRepository, configuration);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RecursoDTO> CrearUrlCortaAsync(CreateUrlRequest request)
        {
            try
            {
                var result = await _crearUrlCortaUseCase.ExecuteAsync(request);
                _logger.LogInformation("URL corta creada: {CodigoCorto} -> {UrlOriginal}",
                    result.CodigoCorto, result.UrlOriginal);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear URL corta para {UrlOriginal}", request.UrlOriginal);
                throw;
            }
        }

        public async Task<UrlRedirectDTO> RegistrarClickAsync(string codigoCorto, string? ipOrigen = null)
        {
            try
            {
                var result = await _registrarClickUseCase.ExecuteAsync(codigoCorto, ipOrigen);
                _logger.LogInformation("Click registrado para {CodigoCorto}. Total clicks: {Total}",
                    codigoCorto, result.ClicksTotales);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar click para código {CodigoCorto}", codigoCorto);
                throw;
            }
        }

        public async Task<UrlStatisticsDTO> ObtenerEstadisticasAsync(string codigoCorto)
        {
            try
            {
                return await _obtenerEstadisticasUseCase.ExecuteAsync(codigoCorto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas para código {CodigoCorto}", codigoCorto);
                throw;
            }
        }
    }
}