
using System.ComponentModel.DataAnnotations;

namespace Financiera.Application.DTO.Usuarios
{
    public class UsuarioRegistroDto
    {
        [Required(ErrorMessage ="El nombre del usuarioe es requerido")]
        public string NombreCompleto { get; set; } = null!;
        

        [Required(ErrorMessage = "El Email del usuarioe es requerido")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña del usuarioe es requerido")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "El Rol del usuarioe es requerido")]
        public string Rol { get; set; } = null!;

        [Required(ErrorMessage = "El Telefono del usuarioe es requerido")]
        public string PhonNumber { get; set; } = null!;

    }
}
