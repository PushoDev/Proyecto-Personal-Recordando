using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiProyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecursosController : ControllerBase
    {
        private readonly IRecursoApplicationService _recursoService;

        public RecursosController(IRecursoApplicationService recursoService)
        {
            _recursoService = recursoService ?? throw new ArgumentNullException(nameof(recursoService));
        }

        /// <summary>
        /// Obtiene un recurso por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RecursoDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRecurso(int id)
        {
            var recurso = await _recursoService.ObtenerRecursoAsync(id);
            if (recurso == null)
                return NotFound($"No se encontró el recurso con ID {id}");

            return Ok(recurso);
        }

        /// <summary>
        /// Descuenta stock de un recurso
        /// </summary>
        [HttpPut("{id}/stock")]
        [ProducesResponseType(typeof(RecursoDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> DescontarStock(int id, [FromBody] DescontarStockRequest request)
        {
            if (request == null)
                return BadRequest("El cuerpo de la solicitud es requerido");

            if (request.Cantidad <= 0)
                return BadRequest("La cantidad debe ser mayor a cero");

            try
            {
                var result = await _recursoService.DescontarStockAsync(id, request.Cantidad);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se encontró el recurso con ID {id}");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message); // Stock insuficiente
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}