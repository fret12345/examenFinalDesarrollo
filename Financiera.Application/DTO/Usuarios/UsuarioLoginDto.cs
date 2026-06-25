
using System.ComponentModel.DataAnnotations;

namespace Financiera.Application.DTO.Usuarios
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "El Email del usuarioe es requerido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña del usuarioe es requerido")]
        public string Password { get; set; } = string.Empty;
    }
}
