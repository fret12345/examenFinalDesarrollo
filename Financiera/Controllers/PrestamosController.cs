using Financiera.Application.DTO.Clientes;
using Financiera.Application.DTO.Prestamo;
using Financiera.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financiera.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosController : ControllerBase
    {
        private readonly IPrestamoService _service;

        public PrestamosController(IPrestamoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ICollection<PrestamoDto>>> ObtenerTodas()
        {
            var rutas = await _service.ObtenerTodasAsync();
            if (rutas == null || !rutas.Any())
                return NotFound("No hay Prestamos registrados.");

            return Ok(rutas);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ICollection<PrestamoDto>>> ObtenerPorId(int id)
        {
            var rutas = await _service.ObtenerPorIdAsync(id);
            return Ok(rutas);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<PrestamoDto>> Crear([FromBody] PrestamoCrearDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevaRuta = await _service.CrearAsync(dto);
            return Ok(nuevaRuta);
        }

        [HttpPut("{id:int}", Name = "ActualizarRegistro")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> Actualizar(int id,[FromBody] PrestamoActualizarDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevaRuta = await _service.ActualizarAsync  (id, dto);
            return Ok(nuevaRuta);
        }


        [HttpPut("{id:int}/estado", Name = "ActualizarEstado"),]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> Actualizarestado(int id, [FromBody] PrestamoActualizarEstadoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var PrestamoActualizado = await _service.ActualizarEstado(id, dto);
            return Ok(PrestamoActualizado);
        }


        [HttpGet("{Numero}/BuscarPorNumero")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ICollection<PrestamoDto>>> BuscarPorNumeroPrestamo(string Numero)
        {
            var registro = await _service.ObtenerPorNoPrestamoAsync (Numero);
            return Ok(registro);
        }
    }
}
