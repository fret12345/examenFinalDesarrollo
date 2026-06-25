using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Interface.Repository
{
    public interface IPagosRepository
    {
        Task<Pagos?> obtenerPorIdAsync(int id);
        Task<IEnumerable<Pagos>> ObtenerTodasAsync(int pagina, int tamano);
        Task<IEnumerable<Pagos>> ObtenerPorIdCuotaAsync(int IdCuota, int pagina, int tamano);
        Task<int> ContarAsync();
        Task<int> ContarBusquedaAsync(int valor);

        Task<bool> ExisteCuotaAsync(int Numero);

        Task CrearAsync(Pagos pagos);
        Task ActualizarAsync(Pagos pagos);
    }
}
