using Financiera.Application.DTO.Clientes;
using Financiera.Application.DTO.Rutas;
using Financiera.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financiera.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _Service;

        public ClientesController(IClienteService Service)
        {
            _Service = Service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ICollection<ClienteDto>>> ObtenerTodas()
        {
            var rutas = await _Service.ObtenerTodasAsync();
            if (rutas == null || !rutas.Any())
                return NotFound("No hay categorías registradas.");

            return Ok(rutas);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ClienteDto>> ObtenerPorId(int id)
        {
            var rutas = await _Service.ObtenerPorIdAsync(id);
            return Ok(rutas);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ClienteDto>> Crear([FromBody] ClienteCrearDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevaRuta = await _Service.CrearAsync(dto);
            return Ok(nuevaRuta);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> Actualizar(int id, [FromBody] ClienteActualizarDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var rutaActualizada = await _Service.ActualizarAsync(id, dto);
            return Ok(rutaActualizada);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> Eliminar(int id)
        {
            await _Service.EliminarAsync(id);
            return NoContent();
        }
    }
}
