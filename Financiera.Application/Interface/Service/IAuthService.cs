
using Financiera.Application.DTO.Usuarios;
using Financiera.Application.Response;

namespace Financiera.Application.Interface.Service
{
    public interface IAuthService
    {
        Task<RespuestaLoginDto> loginAsync(UsuarioLoginDto dto);
        Task<UsuarioDto> ReegistrarUsuarioAsync(UsuarioRegistroDto dto);
        Task<RespuestaLoginDto> RefreshTokenAsync(string refreshToken);
    }
}
