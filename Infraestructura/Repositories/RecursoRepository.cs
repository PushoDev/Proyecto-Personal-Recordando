using Domain.Entidades;
using Domain.Interfaces;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repositories
{
    public class RecursoRepository : IRecursoRepository
    {
        private readonly RecursoDbContext _context;

        public RecursoRepository(RecursoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Recurso>> GetAllAsync()
        {
            return await _context.Recursos.ToListAsync();
        }

        public async Task<Recurso?> GetByIdAsync(int id)
        {
            return await _context.Recursos
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Recurso?> GetByCodigoCortoAsync(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                return null;

            return await _context.Recursos
                .FirstOrDefaultAsync(r => r.CodigoCorto == codigo.Trim());
        }

        public async Task AddAsync(Recurso recurso)
        {
            if (recurso == null)
                throw new ArgumentNullException(nameof(recurso));

            await _context.Recursos.AddAsync(recurso);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Recurso recurso)
        {
            if (recurso == null)
                throw new ArgumentNullException(nameof(recurso));

            _context.Recursos.Update(recurso);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var recurso = await _context.Recursos.FindAsync(id);
            if (recurso != null)
            {
                _context.Recursos.Remove(recurso);
                await _context.SaveChangesAsync();
            }
        }
    }
}