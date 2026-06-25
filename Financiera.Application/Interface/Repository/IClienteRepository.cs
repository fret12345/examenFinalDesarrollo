using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Interface.Repository
{
    public interface IClienteRepository
    {
        Task<Clientes?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Clientes>> ObtenerTodasAsync();
        Task<IEnumerable<Clientes>> BuscarClienteAsync(string nombre);
        Task<bool> ExisteNombreAsync(string nombre);


        Task CrearAsync(Clientes clientes);
        Task ActualizarAsync(Clientes clientes);
        Task EliminarAsync(int id);
    }
}
