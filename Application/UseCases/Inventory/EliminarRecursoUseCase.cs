using Domain.Interfaces;

namespace Application.UseCases.Inventory
{
    public class EliminarRecursoUseCase
    {
        private readonly IRecursoRepository _repository;

        public EliminarRecursoUseCase(IRecursoRepository repository)
        {
            _repository = repository;
        }

        public async Task ExecuteAsync(int id)
        {
            var recurso = await _repository.GetByIdAsync(id);
            if (recurso == null)
                throw new KeyNotFoundException($"Recurso con ID {id} no encontrado");

            await _repository.DeleteAsync(id);
        }
    }
}