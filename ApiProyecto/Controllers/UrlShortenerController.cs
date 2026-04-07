using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IUrlShortenerService _urlShortenerService;

        public UrlShortenerController(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService ?? throw new ArgumentNullException(nameof(urlShortenerService));
        }

        /// <summary>
        /// Crea una URL corta a partir de una URL original
        /// </summary>
        [HttpPost("shorten")]
        [ProducesResponseType(typeof(RecursoDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateShortUrl([FromBody] CreateUrlRequest request)
        {
            if (request == null)
                return BadRequest("El cuerpo de la solicitud es requerido");

            if (string.IsNullOrWhiteSpace(request.UrlOriginal))
                return BadRequest("La URL original es requerida");

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest("El nombre del recurso es requerido");

            try
            {
                var result = await _urlShortenerService.CrearUrlCortaAsync(request);
                return CreatedAtAction(nameof(GetStatistics), new { codigoCorto = result.CodigoCorto }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Redirige a la URL original y registra el click
        /// </summary>
        [HttpGet("{codigoCorto}")]
        [ProducesResponseType(302)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RedirectToOriginal(string codigoCorto)
        {
            if (string.IsNullOrWhiteSpace(codigoCorto))
                return BadRequest("El código corto es requerido");

            try
            {
                var result = await _urlShortenerService.RegistrarClickAsync(codigoCorto, GetClientIpAddress());

                // Redirigir a la URL original
                return Redirect(result.UrlOriginal);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se encontró una URL con el código '{codigoCorto}'");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene estadísticas de una URL corta
        /// </summary>
        [HttpGet("{codigoCorto}/stats")]
        [ProducesResponseType(typeof(UrlStatisticsDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetStatistics(string codigoCorto)
        {
            if (string.IsNullOrWhiteSpace(codigoCorto))
                return BadRequest("El código corto es requerido");

            try
            {
                var stats = await _urlShortenerService.ObtenerEstadisticasAsync(codigoCorto);
                return Ok(stats);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se encontraron estadísticas para el código '{codigoCorto}'");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        private string? GetClientIpAddress()
        {
            // Obtener IP del cliente desde headers o connection
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Si está detrás de un proxy, buscar en headers
            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
            {
                ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
            {
                ipAddress = HttpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            }

            return ipAddress;
        }
    }
}