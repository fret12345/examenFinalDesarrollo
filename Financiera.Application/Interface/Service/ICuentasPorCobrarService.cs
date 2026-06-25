using Financiera.Application.DTO.CuentasPorCobrar;
using Financiera.Application.DTO.Cuota;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Interface.Service
{
    public interface ICuentasPorCobrarService
    {
        Task<CuentasPorCobrarDto?> obtenerPorIdAsync(int id);
        Task<IEnumerable<CuentasPorCobrarDto>> ObtenerTodasAsync(int pagina, int tamano);
        Task<ICollection<CuentasPorCobrarDto>> ObtenerPorIdPrestamoAsync(int IdPrestamos, int pagina, int tamano);
        Task<int> ContarAsync();
        Task<int> ContarBusquedaAsync(int valor);
        Task<CuentasPorCobrarDto> CrearAsync(CuentasPorCobrarCrearDto Dto);
        Task<CuentasPorCobrarDto> ActualizarAsync(int id, CuentasPorCobrarActualizarDto Dto);
    }
}
