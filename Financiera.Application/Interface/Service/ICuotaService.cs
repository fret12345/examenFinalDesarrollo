using Financiera.Application.DTO.Cuota;
using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Interface.Service
{
    public interface ICuotaService
    {
        Task<CuotaDto?> obtenerPorIdAsync(int id);
        Task<IEnumerable<CuotaDto>> ObtenerTodasAsync(int pagina, int tamano);
        Task<ICollection<CuotaDto>> ObtenerPorIdPrestamoAsync(int IdPrestamos, int pagina, int tamano);
        Task<int> ContarAsync();
        Task<int> ContarBusquedaAsync(int valor);
        Task<CuotaDto> CrearAsync(CuotaCrearDto cuota);
        Task<CuotaDto> ActualizarAsync(int id,CuotaActualizarDto cuota);
    }
}
