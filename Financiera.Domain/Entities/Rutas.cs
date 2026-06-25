using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Domain.Entities
{
    public class Rutas
    {
        public int id { get; set; }
        public string NombreRuta { get; set; } = null!;

        public virtual ICollection<Clientes> Clientes { get; set; } = new List<Clientes>();


    }
}
