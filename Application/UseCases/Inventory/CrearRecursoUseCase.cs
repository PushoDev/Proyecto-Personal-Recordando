using Application.DTOs;
using Domain.Entidades;
using Domain.Interfaces;

namespace Application.UseCases.Inventory
{
    public class CrearRecursoUseCase
    {
        private readonly IRecursoRepository _repository;

        public CrearRecursoUseCase(IRecursoRepository repository)
        {
            _repository = repository;
        }

        public async Task<RecursoDTO> ExecuteAsync(CreateRecursoRequest request)
        {
            Recurso recurso;
            
            // Si tiene stockInicial > 0 o UmbralMinimo > 0, es inventario
            if (request.StockInicial > 0 || request.UmbralMinimo > 0)
            {
                recurso = new Recurso(request.Nombre, request.StockInicial, request.UmbralMinimo);
            }
            else
            {
                // Es una tarea
                recurso = new Recurso(request.Nombre, request.Descripcion, request.FechaVencimiento, request.Prioridad);
            }
            
            await _repository.AddAsync(recurso);

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