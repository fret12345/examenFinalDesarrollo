using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.DTO.Pagos
{
    public class PagosDto
    {
        public int Id { get; set; }
        public int IdCuota { get; set; }
        public int IdUsuario { get; set; }
        public decimal MontoRecibido { get; set; }
        public decimal SaldoPendiente { get; set; }
        public decimal MontoCuota { get; set; }
        public string ImpuestoExtra { get; set; } = null!;
        public DateOnly FechaPago { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }
}
