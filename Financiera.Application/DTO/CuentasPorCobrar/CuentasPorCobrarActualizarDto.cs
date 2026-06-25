using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Financiera.Application.DTO.CuentasPorCobrar
{
    public class CuentasPorCobrarActualizarDto
    {
        [Required(ErrorMessage = "El Id del prestamo es requerido.")]
        public int IdPrestammo { get; set; }

        [Required(ErrorMessage = "El MontoTotal del prestamo es requerido.")]
        public decimal MontoTotal { get; set; }

        [Required(ErrorMessage = "El Saldopendiente del prestamo es requerido.")]
        public decimal SaldoPendiente { get; set; }

        public decimal TotalMoraAcumuada { get; set; }
        public decimal TotalImpuestoExtra { get; set; }
    }
}
