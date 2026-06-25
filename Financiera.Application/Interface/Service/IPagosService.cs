using Financiera.Application.DTO.Cuota;
using Financiera.Application.DTO.Pagos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Interface.Service
{
    public interface IPagosService
    {
        Task<PagosDto?> obtenerPorIdAsync(int id);
        Task<IEnumerable<PagosDto>> ObtenerTodasAsync(int pagina, int tamano);
        Task<ICollection<PagosDto>> ObtenerPorIdCuotaAsync(int IdCuota, int pagina, int tamano);
        Task<int> ContarAsync();
        Task<int> ContarBusquedaAsync(int valor);
        Task<PagosDto> CrearAsync(PagosCrearDto dto);
        Task<PagosDto> ActualizarAsync(int id, PagosActualizarDto dto);
    }
}
