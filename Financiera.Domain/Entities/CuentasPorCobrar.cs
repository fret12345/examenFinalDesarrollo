using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Financiera.Domain.Entities
{
    public class CuentasPorCobrar
    {
        public int Id { get; set; }
        public int IdPrestammo { get; set; }

        [ForeignKey("IdPrestammo")]
        public Prestamos prestamos { get; set; } = null!;
        public decimal MontoTotal { get; set; }
        public decimal SaldoPendiente { get; set; }
        public decimal TotalMoraAcumuada { get; set; }
        public decimal  TotalImpuestoExtra { get; set; }
        public bool Estado {  get; set; }
    }
}
