using Financiera.Application.DTO.Rutas;
using Financiera.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financiera.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RutasController : ControllerBase
    {
        private readonly IRutasService _rutasService;

        public RutasController(IRutasService rutasService)
        {
            _rutasService = rutasService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ICollection<RutasDto>>> ObtenerTodas()
        {
            var rutas = await _rutasService.ObtenerTodasAsync();
            if (rutas == null || !rutas.Any())
                return NotFound("No hay categorías registradas.");

            return Ok(rutas);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<RutasDto>> ObtenerPorId(int id)
        {
            var rutas = await _rutasService.ObtenerPorIdAsync(id);
            return Ok(rutas);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<RutasDto>> Crear([FromBody] RutasCrearDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevaRuta = await _rutasService.CrearAsync(dto);
            return Ok(nuevaRuta);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> Actualizar(int id, [FromBody] RutasActualizarDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rutaActualizada = await _rutasService.ActualizarAsync(id, dto);
            return Ok(rutaActualizada);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> Eliminar(int id)
        {
            await _rutasService.EliminarAsync(id);
            return NoContent();
        }
    }
}
