using Application.DTOs;

namespace Application.Interfaces
{
    public interface IRecursoApplicationService
    {
        Task<RecursoDTO?> ObtenerRecursoAsync(int id);
        Task<RecursoDTO> DescontarStockAsync(int recursoId, int cantidad);
    }
}