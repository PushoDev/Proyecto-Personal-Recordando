using Application.DTOs;
using Domain.Entidades;
using Domain.Interfaces;

namespace Application.UseCases.UrlShortener
{
    public class CrearUrlCortaUseCase
    {
        private readonly IRecursoRepository _recursoRepository;

        public CrearUrlCortaUseCase(IRecursoRepository recursoRepository)
        {
            _recursoRepository = recursoRepository ?? throw new ArgumentNullException(nameof(recursoRepository));
        }

        public async Task<RecursoDTO> ExecuteAsync(CreateUrlRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.UrlOriginal))
                throw new ArgumentException("La URL original es requerida.", nameof(request.UrlOriginal));

            if (string.IsNullOrWhiteSpace(request.Nombre))
                throw new ArgumentException("El nombre del recurso es requerido.", nameof(request.Nombre));

            // Generar código corto único (Base58 para evitar caracteres problemáticos)
            string codigoCorto;
            do
            {
                codigoCorto = GenerarCodigoCorto();
            }
            while (await _recursoRepository.GetByCodigoCortoAsync(codigoCorto) != null);

            // Crear recurso con URL
            var recurso = new Recurso(request.Nombre, request.StockInicial, request.UmbralMinimo);
            recurso.ConfigurarUrlCorta(request.UrlOriginal, codigoCorto);

            await _recursoRepository.AddAsync(recurso);

            return new RecursoDTO
            {
                Id = recurso.Id,
                Nombre = recurso.Nombre,
                Stock = recurso.Stock,
                UmbralMinimo = recurso.UmbralMinimo,
                UrlOriginal = recurso.UrlOriginal,
                CodigoCorto = recurso.CodigoCorto,
                Clicks = recurso.Clicks,
                EstaEnEstadoCritico = recurso.EstaEnEstadoCritico()
            };
        }

        private string GenerarCodigoCorto(int longitud = 6)
        {
            const string caracteres = "23456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz";
            var random = new Random();
            var resultado = new char[longitud];

            for (int i = 0; i < longitud; i++)
            {
                resultado[i] = caracteres[random.Next(caracteres.Length)];
            }

            return new string(resultado);
        }
    }
}