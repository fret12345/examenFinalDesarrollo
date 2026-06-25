using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Domain.Entities
{
    public class Usuarios : IdentityUser<int>
    {
        //public int Id { get; set; } 
        //public int IdRol {  get; set; }
        //public Rol Rol { get; set; } = null!;
        public string NombresCompleto { get; set; } = null!;
        //public string Apellidos { get; set; } = null!;
        //public string Usuario { get; set; } = null!;
        //public string Clave { get; set; } = null!;
          
        //public  Boolean Estado { get; set; }

        public virtual ICollection<Prestamos> Prestamos { get; set; } = new List<Prestamos>();
        public virtual ICollection<Pagos> Pagos { get; set; } = new List<Pagos>();



    }
}
