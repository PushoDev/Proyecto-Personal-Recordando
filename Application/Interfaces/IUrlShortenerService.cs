using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUrlShortenerService
    {
        Task<RecursoDTO> CrearUrlCortaAsync(CreateUrlRequest request);
        Task<UrlRedirectDTO> RegistrarClickAsync(string codigoCorto, string? ipOrigen = null);
        Task<UrlStatisticsDTO> ObtenerEstadisticasAsync(string codigoCorto);
    }
}