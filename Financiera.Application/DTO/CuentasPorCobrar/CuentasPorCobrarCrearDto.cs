using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Financiera.Application.DTO.CuentasPorCobrar
{
    public class CuentasPorCobrarCrearDto
    {
        [Required(ErrorMessage = "El Id del prestamo es requerido.")]
        public int IdPrestammo { get; set; }

        // Quitamos el [Required] porque el servicio se encargará de llenarlos
        public decimal MontoTotal { get; set; }
        public decimal SaldoPendiente { get; set; }

        public decimal TotalMoraAcumulada { get; set; }
        public decimal TotalImpuestoExtra { get; set; }
    }
}
