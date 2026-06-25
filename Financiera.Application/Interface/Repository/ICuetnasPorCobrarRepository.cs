using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Interface.Repository
{
    public interface ICuetnasPorCobrarRepository
    {
        public Task<CuentasPorCobrar?> ObtenerPorIdAsync(int id);
        public Task<CuentasPorCobrar?> ObtenerPorIdPrestamoAsync(int idPrestamo);
        public Task<IEnumerable<CuentasPorCobrar>> ObtenerTodasAsync(int pagina, int tamanio);
        public Task<IEnumerable<CuentasPorCobrar>> ObtenerPorNumPrestamoAsync(int idPrestamo, int pagina, int tamanio);
        public Task<int> ContarAsync();
        public Task<int> ContarBusquedaAsync(int Valor);

        public Task<bool> ExistePrestamo(int idPrestamo);
        public Task CrearAsync(CuentasPorCobrar cuentasPorCobrar);
        public Task ActualizarAsync(CuentasPorCobrar cuentasPorCobrar);
    }
}
