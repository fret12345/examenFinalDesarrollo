using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Domain.Entities
{
    public class Pagos
    {
        public int Id { get; set; }
        public int IdCuota { get; set; }
        public Cuota cuota { get; set; } = null!;
        public int IdUsuario { get; set; }
        public Usuarios usuarios { get; set; } = null!;
        public decimal MontoRecibido { get; set; }
        public decimal SaldoPendiente { get; set; }
        public decimal MontoCuota { get; set; }
        public string ImpuestoExtra { get; set; }=null!;
        public DateOnly FechaPago { get; set; }
        public DateOnly FechaRegistro { get; private set; } = DateOnly.FromDateTime(DateTime.Now);

        
    }
}
