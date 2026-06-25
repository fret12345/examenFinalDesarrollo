using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Domain.Entities
{
    public class Rol
    {
        public int Id { get; set; }
        public string NombreRol { get; set; } = null!;

        public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();

    }
}
