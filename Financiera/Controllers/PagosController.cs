using Financiera.Application.DTO.Clientes;
using Financiera.Application.DTO.Cuota;
using Financiera.Application.DTO.Pagos;
using Financiera.Application.Interface.Service;
using Financiera.Application.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financiera.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly IPagosService _Service;
        
        public PagosController(IPagosService service)
        {
            _Service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<IEnumerable<PagosDto>>> ObtenerTodas([FromQuery] int pagina = 1, [FromQuery] int tamano = 10)
        {
            var registros = await _Service.ObtenerTodasAsync(pagina, tamano);
            var total = await _Service.ContarAsync();
            return Ok(new RespuestaPaginada<PagosDto>(registros, total, pagina, tamano));
        }

        [HttpGet("{Id:int}/ObtenerPorId")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ICollection<PagosDto>>> ObtenerPorId(int Id)
        {
            var ruta = await _Service.obtenerPorIdAsync(Id);
            if (ruta == null)
                return NotFound("No hay cuotas registradas para este prestamo, verifique si el rpestamo esta aprobado.");
            return Ok(ruta);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<PagosDto>> Crear([FromBody] PagosCrearDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var nuevaRuta = await _Service.CrearAsync(dto);
            return Ok(nuevaRuta);
        }

    }
}
