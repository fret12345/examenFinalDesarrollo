using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.DTO.Clientes
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public int idRuta { get; set; }
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public string DireccionDomiciliar { get; set; } = null!;
        public string DireccionNegocio { get; set; } = null!;
        public string Telefono { get; set; } = null!;
    }
}
