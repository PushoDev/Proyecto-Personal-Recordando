using Application.DTOs;
using Domain.Interfaces;

namespace Application.UseCases.Inventory
{
    public class ObtenerTodosRecursosUseCase
    {
        private readonly IRecursoRepository _repository;

        public ObtenerTodosRecursosUseCase(IRecursoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<RecursoDTO>> ExecuteAsync()
        {
            var recursos = await _repository.GetAllAsync();
            return recursos.Select(MapToDTO).ToList();
        }

        private static RecursoDTO MapToDTO(Domain.Entidades.Recurso recurso)
        {
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