using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Financiera.Application.DTO.Cuota
{
    public class CuotaCrearDto
    {
        [Required(ErrorMessage = "El Id del prestamo es requerido.")]
        public int IdPrestamos { get; set; }

        [Required(ErrorMessage ="El Numero de cuota es requerido")]
        public int NumCuota { get; set; }

        [Required(ErrorMessage ="El Monto de la cuota esrequerido")]
        public decimal MontoCuota { get; set; }

        [Required(ErrorMessage = "La fecha de vensimiento de la cuota esrequerido")]

        public DateOnly FechaVensimiento { get; set; }
        public decimal CobroMora { get; set; }
        public int DiasAtraso { get; set; }
        public decimal MontoMora { get; set; }
    }
}
