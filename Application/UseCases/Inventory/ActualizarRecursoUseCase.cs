using Application.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.Inventory
{
    public class ActualizarRecursoUseCase
    {
        private readonly IRecursoRepository _repository;

        public ActualizarRecursoUseCase(IRecursoRepository repository)
        {
            _repository = repository;
        }

        public async Task<RecursoDTO> ExecuteAsync(int id, CreateRecursoRequest request)
        {
            var recurso = await _repository.GetByIdAsync(id);
            if (recurso == null)
                throw new KeyNotFoundException($"Recurso con ID {id} no encontrado");

            // Si tiene stock significativo, es inventario
            if (recurso.Stock > 0 || recurso.CodigoCorto != null)
            {
                recurso.ActualizarNombre(request.Nombre);
                if (request.StockInicial > 0)
                {
                    recurso.AgregarStock(request.StockInicial - recurso.Stock);
                }
                if (request.UmbralMinimo >= 0)
                {
                    recurso.AjustarUmbralMinimo(request.UmbralMinimo);
                }
            }
            else
            {
                // Es una tarea
                recurso.ActualizarTarea(request.Nombre, request.Descripcion, request.FechaVencimiento, request.Prioridad);
            }

            await _repository.UpdateAsync(recurso);

            return new RecursoDTO
            {
                Id = recurso.Id,
                Nombre = recurso.Nombre,
                Descripcion = recurso.Descripcion,
                Stock = recurso.Stock,
                UmbralMinimo = recurso.UmbralMinimo,
                UrlOriginal = recurso.UrlOriginal,
                CodigoCorto = recurso.CodigoCorto,
                Clicks = recurso.Clicks,
                EstaEnEstadoCritico = recurso.Stock <= recurso.UmbralMinimo,
                FechaCreacion = recurso.FechaCreacion,
                FechaVencimiento = recurso.FechaVencimiento,
                Prioridad = recurso.Prioridad,
                Estado = recurso.Estado
            };
        }
    }
}