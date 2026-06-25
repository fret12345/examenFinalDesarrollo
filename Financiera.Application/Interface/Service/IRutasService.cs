using Financiera.Application.DTO.Rutas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Interface.Service
{
    public interface IRutasService
    {
        Task<RutasDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<RutasDto>> ObtenerTodasAsync();
        Task<IEnumerable<RutasDto>> BuscarRutasAsync(string nombre);


        Task<RutasDto> CrearAsync(RutasCrearDto dto);
        Task<RutasDto> ActualizarAsync(int id, RutasActualizarDto dto);
        Task EliminarAsync(int id);
    }
}
