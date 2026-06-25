using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Domain.Entities
{
    public class Cuota
    {
        public int Id { get; set; }
        public int IdPrestamos { get; set; }
        public Prestamos Prestamos { get; set; } = null!;
        public int NumCuota { get; set; }
        public decimal MontoCuota { get; set; }
        public DateOnly FechaVensimiento { get; set; }
        public decimal CobroMora { get; set; }
        public int DiasAtraso { get; set; }
        public decimal MontoMora { get; set; }
        public bool Estado {  get; set; } = true;
        public virtual ICollection<Pagos> Pagos { get; set; } = new List<Pagos>();

    }
}
