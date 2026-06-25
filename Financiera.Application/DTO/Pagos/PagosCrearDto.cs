using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Financiera.Application.DTO.Pagos
{
    public class PagosCrearDto
    {
        [Required(ErrorMessage = "El Id de la cuota esrequerido")]
        public int IdCuota { get; set; }

        [Required(ErrorMessage = "El Id del usuario es esrequerido")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El MontoRecibido es esrequerido")]
        public decimal MontoRecibido { get; set; }

        //[Required(ErrorMessage = "El SaldoPendiente esrequerido")]
        //public decimal SaldoPendiente { get; set; }

        //[Required(ErrorMessage = "El SaldoPendiente esrequerido")]

        //public decimal MontoCuota { get; set; }
        //public string ImpuestoExtra { get; set; } = null!;
        //public DateOnly FechaPago { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
