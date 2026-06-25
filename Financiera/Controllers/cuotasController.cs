using Financiera.Application.DTO.Cuota;
using Financiera.Application.Interface.Service;
using Financiera.Application.Response;
using Financiera.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Financiera.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class cuotasController : ControllerBase
    {
        private readonly ICuotaService _service;

        public cuotasController(ICuotaService service)
        {
           _service = service;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]

        public async Task<ActionResult<IEnumerable<CuotaDto>>> ObtenerTodas([FromQuery] int pagina = 1, [FromQuery] int tamano = 10)
        {
            var registros = await _service.ObtenerTodasAsync(pagina, tamano);
            var total = await _service.ContarAsync();
            return Ok(new RespuestaPaginada<CuotaDto>(registros, total, pagina, tamano));
        }

        [HttpGet("{Id:int}/ObtenerPorId")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<ICollection<CuotaDto>>> ObtenerPorId(int Id)
        {
            var ruta = await _service.obtenerPorIdAsync(Id);
            if (ruta == null)
                return NotFound("No hay cuotas registradas para este prestamo, verifique si el rpestamo esta aprobado.");
            return Ok(ruta);
        }

        [HttpGet("{idprestamos:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult<IEnumerable<CuotaDto>>> ObtenerPorIdPrestamo(int idprestamos,[FromQuery] int pagina = 1, [FromQuery] int tamanio  = 10)
        {
            var registros = await _service.ObtenerPorIdPrestamoAsync(idprestamos, pagina, tamanio);
            var total = await _service.ContarBusquedaAsync(idprestamos);
            return Ok(new RespuestaPaginada<CuotaDto>(registros, total, pagina, tamanio));
        }
    }
}
