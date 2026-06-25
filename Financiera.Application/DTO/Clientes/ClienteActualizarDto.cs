using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Financiera.Application.DTO.Clientes
{
    public class ClienteActualizarDto
    {
        [Required(ErrorMessage = "El id de la ruta es requerido.")]
        public int idRuta { get; set; }

        [Required(ErrorMessage = "Los Nombres es requerido.")]
        [MaxLength(50, ErrorMessage = "los nombres no puede exceder los 50 caracteres.")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "los Apellidos es requerido.")]
        [MaxLength(50, ErrorMessage = "El Apellidos no puede exceder los 50 caracteres.")]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "La cedula es requerido.")]
        [MaxLength(50, ErrorMessage = "La cedula no puede exceder los 50 caracteres.")]
        public string Cedula { get; set; } = null!;

        [Required(ErrorMessage = "la Direccion domiciliar es requerido.")]
        [MaxLength(100, ErrorMessage = "La Direccion domiciliar exceder los 50 caracteres.")]
        public string DireccionDomiciliar { get; set; } = null!;

        [Required(ErrorMessage = "la Direccion de negocio es requerido.")]
        [MaxLength(100, ErrorMessage = "La Direccion de negocio exceder los 50 caracteres.")]
        public string DireccionNegocio { get; set; } = null!;

        [Required(ErrorMessage = "el Telefono domiciliar es requerido.")]
        [MaxLength(20, ErrorMessage = "El telefono domiciliar exceder los 50 caracteres.")]
        public string Telefono { get; set; } = null!;
    }
}
