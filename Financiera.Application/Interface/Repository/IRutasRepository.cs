using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Interface.Repository
{
    public interface IRutasRepository
    {
        Task<Rutas?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Rutas>> ObtenerTodasAsync();
        Task<IEnumerable<Rutas>> BuscarRutasAsync(string nombre);
        Task<bool> ExisteNombreAsync(string nombre);


        Task CrearAsync(Rutas rutas);
        Task ActualizarAsync(Rutas rutas);
        Task EliminarAsync(int id);
    }
}
