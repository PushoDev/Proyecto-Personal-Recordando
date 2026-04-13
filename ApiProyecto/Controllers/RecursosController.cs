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
        /// Obtiene todos los recursos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<RecursoDTO>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var recursos = await _recursoService.ObtenerTodosAsync();
            return Ok(recursos);
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
        /// Crea un nuevo recurso
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(RecursoDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateRecurso([FromBody] CreateRecursoRequest request)
        {
            if (request == null)
                return BadRequest("El cuerpo de la solicitud es requerido");

            if (string.IsNullOrWhiteSpace(request.Nombre))
                return BadRequest("El nombre es requerido");

            try
            {
                var recurso = await _recursoService.CrearRecursoAsync(request);
                return CreatedAtAction(nameof(GetRecurso), new { id = recurso.Id }, recurso);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Actualiza un recurso existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(RecursoDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateRecurso(int id, [FromBody] CreateRecursoRequest request)
        {
            if (request == null)
                return BadRequest("El cuerpo de la solicitud es requerido");

            try
            {
                var recurso = await _recursoService.ActualizarRecursoAsync(id, request);
                return Ok(recurso);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se encontró el recurso con ID {id}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Elimina un recurso
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRecurso(int id)
        {
            try
            {
                await _recursoService.EliminarRecursoAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se encontró el recurso con ID {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Descuenta stock de un recurso
        /// </summary>
        [HttpPut("{id}/stock/descontar")]
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
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// <summary>
        /// Agrega stock a un recurso
        /// </summary>
        [HttpPut("{id}/stock/agregar")]
        [ProducesResponseType(typeof(RecursoDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> AgregarStock(int id, [FromBody] DescontarStockRequest request)
        {
            if (request == null)
                return BadRequest("El cuerpo de la solicitud es requerido");

            if (request.Cantidad <= 0)
                return BadRequest("La cantidad debe ser mayor a cero");

            try
            {
                var result = await _recursoService.AgregarStockAsync(id, request.Cantidad);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se encontró el recurso con ID {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}