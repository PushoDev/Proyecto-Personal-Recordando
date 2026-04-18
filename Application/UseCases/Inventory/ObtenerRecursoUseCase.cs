using Application.DTOs;
using Domain.Entidades;
using Domain.Interfaces;

namespace Application.UseCases.Inventory
{
    public class ObtenerRecursoUseCase
    {
        private readonly IRecursoRepository _recursoRepository;

        public ObtenerRecursoUseCase(IRecursoRepository recursoRepository)
        {
            _recursoRepository = recursoRepository ?? throw new ArgumentNullException(nameof(recursoRepository));
        }

        public async Task<RecursoDTO?> ExecuteAsync(int recursoId)
        {
            if (recursoId <= 0)
                throw new ArgumentException("El ID del recurso debe ser positivo.", nameof(recursoId));

            var recurso = await _recursoRepository.GetByIdAsync(recursoId);
            if (recurso == null)
                return null;

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
                EstaEnEstadoCritico = recurso.EstaEnEstadoCritico(),
                FechaCreacion = recurso.FechaCreacion,
                FechaVencimiento = recurso.FechaVencimiento,
                Prioridad = recurso.Prioridad,
                Estado = recurso.Estado
            };
        }
    }
}