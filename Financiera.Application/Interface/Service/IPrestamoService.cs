using Financiera.Application.DTO.Prestamo;
using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Interface.Service
{
    public interface IPrestamoService
    {
        Task<PrestamoDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<PrestamoDto>> ObtenerTodasAsync();
        Task<IEnumerable<PrestamoDto>> ObtenerPorNoPrestamoAsync(string Numero);

        Task<PrestamoDto> CrearAsync(PrestamoCrearDto prestamos);
        Task<PrestamoDto> ActualizarAsync(int id,PrestamoActualizarDto prestamos);
        Task<PrestamoDto> ActualizarEstado(int id, PrestamoActualizarEstadoDto dto); 
    }
}
