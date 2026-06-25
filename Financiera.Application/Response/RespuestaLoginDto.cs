
using Financiera.Application.DTO.Usuarios;

namespace Financiera.Application.Response
{
    public class RespuestaLoginDto
    {
        public UsuarioDto Usuario { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime ExpiraEn { get; set; }
    }
}
