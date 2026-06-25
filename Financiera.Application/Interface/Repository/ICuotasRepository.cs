using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Interface.Repository
{
    public interface ICuotasRepository
    {
        Task<Cuota?> obtenerPorIdAsync(int id);
        Task<IEnumerable<Cuota>> ObtenerTodasAsync(int pagina, int tamano);
        Task<IEnumerable<Cuota>> ObtenerPorIdPrestamoAsync(int IdPrestamos, int pagina, int tamano);
        Task<int> ContarAsync();
        Task<int> ContarBusquedaAsync(int valor);

        Task<bool> ExisteCuotaAsync(int Numero);  

        Task CrearAsync(Cuota cuota);
        Task ActualizarAsync(Cuota cuota);
    }
}
