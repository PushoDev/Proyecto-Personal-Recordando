using Domain.Entidades;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRecursoRepository
    {
        Task<List<Recurso>> GetAllAsync();
        Task<Recurso?> GetByIdAsync(int id);
        Task<Recurso?> GetByCodigoCortoAsync(string codigo);
        Task AddAsync(Recurso recurso);
        Task UpdateAsync(Recurso recurso);
        Task DeleteAsync(int id);
    }
}
