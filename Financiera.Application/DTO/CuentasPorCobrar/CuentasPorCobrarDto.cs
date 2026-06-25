using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.DTO.CuentasPorCobrar
{
    public class CuentasPorCobrarDto
    {
        public int Id { get; set; }
        public int IdPrestammo { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal SaldoPendiente { get; set; }
        public decimal TotalMoraAcumuada { get; set; }
        public decimal TotalImpuestoExtra { get; set; }
        public bool Estado { get; set; }
    }
}
