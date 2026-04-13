using Application.DTOs;

namespace Application.Interfaces
{
    public interface IRecursoApplicationService
    {
        Task<List<RecursoDTO>> ObtenerTodosAsync();
        Task<RecursoDTO?> ObtenerRecursoAsync(int id);
        Task<RecursoDTO> CrearRecursoAsync(CreateRecursoRequest request);
        Task<RecursoDTO> ActualizarRecursoAsync(int id, CreateRecursoRequest request);
        Task EliminarRecursoAsync(int id);
        Task<RecursoDTO> DescontarStockAsync(int recursoId, int cantidad);
        Task<RecursoDTO> AgregarStockAsync(int recursoId, int cantidad);
    }
}