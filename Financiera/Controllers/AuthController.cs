using Financiera.Application.DTO.Usuarios;
using Financiera.Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Financiera.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("registro")]
        public async Task<ActionResult<UsuarioDto>> Crear([FromBody]UsuarioRegistroDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var registroCreado = await _service.ReegistrarUsuarioAsync(dto);
            return Ok(registroCreado);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login([FromBody] UsuarioLoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var respuesta = await _service.loginAsync(dto);
            return Ok(respuesta);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<UsuarioDto>> Refresh([FromBody] RefreshTokenDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.RefreshToken))
                return BadRequest("Refresh token is required");

            var respuesta = await _service.RefreshTokenAsync(dto.RefreshToken);
            return Ok(respuesta);
        }
    }
}
