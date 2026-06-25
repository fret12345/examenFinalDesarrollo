
using System.ComponentModel.DataAnnotations;

namespace Financiera.Application.DTO.Rutas
{
    public class RutasActualizarDto
    {
        [Required(ErrorMessage = "El nombre de la Ruta es requerido.")]
        [MaxLength(40, ErrorMessage = "El nombre de la Ruta no puede exceder los 40 caracteres.")]
        public string NombreRuta { get; set; } = null!;
    }
}
