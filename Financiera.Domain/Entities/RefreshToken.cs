using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Financiera.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;

        [ForeignKey("Usuario")] // <-- ¡Añade esta línea justo aquí!
        public int UsuarioId { get; set; }

        public Usuarios Usuario { get; set; } = null!;
        public DateTime Expiracion { get; set; }

    }
}
