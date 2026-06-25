using Financiera.Application.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Financiera.Application.DTO.Prestamo
{
    public class PrestamoActualizarEstadoDto
    {

        [Required(ErrorMessage = "El campo RstadoPrestamo es obligatorio.")]
        public EstadoPrestamoEnum EstadoPrestamo { get; set; }
    }
}
