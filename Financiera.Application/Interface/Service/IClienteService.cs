using Financiera.Application.DTO.Clientes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Interface.Service
{
    public interface IClienteService
    {
        Task<ClienteDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<ClienteDto>> ObtenerTodasAsync();
        Task<IEnumerable<ClienteDto>> BuscarClienteAsync(string nombre);

        Task<ClienteDto> CrearAsync(ClienteCrearDto dto);
        Task<ClienteDto> ActualizarAsync(int id, ClienteActualizarDto dto);
        Task EliminarAsync(int id);
    }
}
