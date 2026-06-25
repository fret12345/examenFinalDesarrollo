using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Financiera.Application.DTO.Prestamo
{
    public class PrestamoCrearDto
    {
        [Required(ErrorMessage = "El campo IdCliente es obligatorio.")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El campo IdUsuarios es obligatorio.")]
        public int IdUsuarios { get; set; }

        [Required(ErrorMessage = "El campo NumPrestamos es obligatorio.")]
        public string NumPrestamos { get; set; } = null!;

        [Required(ErrorMessage = "El campo MontoCapital es obligatorio.")]
        public decimal MontoCapital { get; set; }

        [Required(ErrorMessage = "El campo Moneda es obligatorio.")]
        public string Moneda { get; set; } = null!;

        [Required(ErrorMessage = "El campo TipoCambio es obligatorio.")]
        public decimal TipoCambio { get; set; }

        [Required(ErrorMessage = "El campo TasaInteres es obligatorio.")]
        public string TasaInteres { get; set; } = null!;

        [Required(ErrorMessage = "El campo TotalAPAgar es obligatorio.")]
        public decimal TotalAPAgar { get; set; }

        [Required(ErrorMessage = "El campo PladoMeses es obligatorio.")]
        public int PladoMeses { get; set; }

        [Required(ErrorMessage = "El campo FrecuenciaPago es obligatorio.")]
        [MaxLength(15, ErrorMessage = "los nombres no puede exceder los 15 caracteres.")]
        public string FrecuenciaPago { get; set; } = null!;

        [Required(ErrorMessage = "El campo FechaDesembolso es obligatorio.")]
        public DateOnly FechaDesembolso { get; set; }

        [Required(ErrorMessage = "El campo FechaVensimiento es obligatorio.")]
        public DateOnly FechaVensimiento { get; set; }
    }
}
