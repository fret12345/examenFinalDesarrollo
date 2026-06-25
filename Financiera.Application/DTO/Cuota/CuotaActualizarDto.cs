using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Financiera.Application.DTO.Cuota
{
    public class CuotaActualizarDto
    {
        [Required(ErrorMessage = "El Cobro Mora de la cuota esrequerido")]

        public decimal CobroMora { get; set; }

        [Required(ErrorMessage = "Los dias de atraso de la cuota esrequerido")]
        public int DiasAtraso { get; set; }

        [Required(ErrorMessage = "El monto extra de la cuota esrequerido")]
        public decimal MontoMora { get; set; }
    }
}
