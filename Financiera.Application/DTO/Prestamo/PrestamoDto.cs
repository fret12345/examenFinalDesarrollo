
namespace Financiera.Application.DTO.Prestamo
{
    public class PrestamoDto
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public int IdUsuarios { get; set; }
        public string NumPrestamos { get; set; } = null!;
        public decimal MontoCapital { get; set; }
        public string Moneda { get; set; } = null!;
        public decimal TipoCambio { get; set; }
        public string TasaInteres { get; set; } = null!;
        public decimal TotalAPAgar { get; set; }
        public int PladoMeses { get; set; }
        public string FrecuenciaPago { get; set; } = null!;
        public string EstadoPrestamo { get; set; } = null!;
        public DateOnly FechaDesembolso { get; set; }
        public DateOnly FechaVensimiento { get; set; }
        public DateOnly FechaRegistro { get; private set; } = DateOnly.FromDateTime(DateTime.Now);
        public bool Estado { get; set; }
    }
}
