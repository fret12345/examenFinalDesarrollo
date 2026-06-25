   using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Domain.Entities
{
    public class Clientes
    {
        public int Id { get; set; }
        public  int idRuta { get; set; }
        public Rutas Rutas { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Cedula {  get; set; } = null!;
        public string DireccionDomiciliar { get; set; }=null!;
        public string DireccionNegocio { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public Boolean Estado {  get; set; }

        public virtual ICollection<Prestamos> Prestamos { get; set; } = new List<Prestamos>();

    }
}
