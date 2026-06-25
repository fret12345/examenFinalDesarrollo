using Financiera.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Application.Interface.Repository
{
    public interface IPrestamoRepository
    {
        Task<Prestamos?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Prestamos>> ObtenerTodasAsync();
        Task<IEnumerable<Prestamos>> ObtenerPorNoPrestamoAsync(string Numero);
        Task<bool> ExistePrestamo(string Numero);

        Task CrearAsync(Prestamos prestamos);
        Task ActualizarAsync(Prestamos prestamos);
        Task ActualizarEstado(Prestamos dto);
    }
}
