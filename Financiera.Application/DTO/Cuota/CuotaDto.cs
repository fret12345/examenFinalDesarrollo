using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.DTO.Cuota
{
    public class CuotaDto
    {
        public int Id { get; set; }
        public int IdPrestamos { get; set; }
        public int NumCuota { get; set; }
        public decimal MontoCuota { get; set; }
        public DateOnly FechaVensimiento { get; set; }
        public decimal CobroMora { get; set; }
        public int DiasAtraso { get; set; }
        public decimal MontoMora { get; set; }
        public bool Estado { get; set; }
    }
}
