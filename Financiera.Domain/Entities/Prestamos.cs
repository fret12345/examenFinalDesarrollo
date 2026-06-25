using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Domain.Entities
{
    public class Prestamos
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public Clientes Clientes { get; set; } = null!;
        public int IdUsuarios { get; set; }
        public Usuarios Usuarios { get; set; } = null!;
        public string NumPrestamos { get; set; } = null!;
        public decimal MontoCapital { get; set; }
        public string Moneda {  get; set; } = null!;
        public decimal TipoCambio { get; set; }
        public string TasaInteres {  get; set; } = null !;
        public decimal TotalAPAgar {  get; set; }
        public int PladoMeses { get; set; }
        public string FrecuenciaPago { get; set; } = null!;
        public string  EstadoPrestamo { get; set; } = null!;
        public DateOnly FechaDesembolso { get; set; }
        public DateOnly FechaVensimiento { get; set; }
        public DateOnly FechaRegistro { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
        public bool Estado {  get; set; }
        public virtual ICollection<Cuota> Cuotas { get; set; } = new List<Cuota>();
        public virtual ICollection<CuentasPorCobrar> CuentasPorCobrars { get; set; } = new List<CuentasPorCobrar>();

    }
}
