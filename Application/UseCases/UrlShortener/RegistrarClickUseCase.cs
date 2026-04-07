using Application.DTOs;
using Domain.Entidades;
using Domain.Interfaces;
using Infraestructura.Data;

namespace Application.UseCases.UrlShortener
{
    public class RegistrarClickUseCase
    {
        private readonly IRecursoRepository _recursoRepository;
        private readonly RecursoDbContext _context;

        public RegistrarClickUseCase(IRecursoRepository recursoRepository, RecursoDbContext context)
        {
            _recursoRepository = recursoRepository ?? throw new ArgumentNullException(nameof(recursoRepository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UrlRedirectDTO> ExecuteAsync(string codigoCorto, string? ipOrigen = null)
        {
            if (string.IsNullOrWhiteSpace(codigoCorto))
                throw new ArgumentException("El código corto es requerido.", nameof(codigoCorto));

            // Obtener recurso
            var recurso = await _recursoRepository.GetByCodigoCortoAsync(codigoCorto);
            if (recurso == null)
                throw new KeyNotFoundException($"No se encontró una URL con el código '{codigoCorto}'.");

            // Registrar click en ClickLog
            var clickLog = new ClickLog(recurso.Id, ipOrigen);
            await _context.ClickLogs.AddAsync(clickLog);

            // Incrementar contador en el recurso
            recurso.RegistrarClick();

            // Guardar cambios
            await _context.SaveChangesAsync();

            return new UrlRedirectDTO
            {
                UrlOriginal = recurso.UrlOriginal!,
                CodigoCorto = recurso.CodigoCorto!,
                ClicksTotales = recurso.Clicks
            };
        }
    }
}