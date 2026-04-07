using Domain.Entidades;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRecursoRepository
    {
        Task<Recurso?> GetByIdAsync(int id);
        Task<Recurso?> GetByCodigoCortoAsync(string codigo);
        Task AddAsync(Recurso recurso);
        Task UpdateAsync(Recurso recurso);
    }
}
