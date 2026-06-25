using Financiera.Application.Interface.Repository;
using Financiera.Domain.Entities;
using Financiera.Infrestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Infrestructure.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Clientes clientes)
        {
            _context.Clientes.Update(clientes);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Clientes>> BuscarClienteAsync(string nombre)
        {
            var query = _context.Clientes
              .AsNoTracking()
              .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var busqueda = nombre.Trim().ToLower();

                query = query.Where(c =>
                    c.Nombres.ToLower().Contains(busqueda));
            }

            return await _context.Clientes
                .OrderBy(c => c.Nombres)
                .ToListAsync();
        }

        public async Task CrearAsync(Clientes clientes)
        {
            _context.Clientes.Add(clientes);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            await _context.Clientes.Where(p => p.Id == id).ExecuteDeleteAsync();
        }

        public Task<bool> ExisteNombreAsync(string nombre)
        {
            var nombreNormalizado = nombre.Trim().ToLower();

            return _context.Clientes
                .AnyAsync(p => p.Nombres.Trim().ToLower() == nombreNormalizado);
        }

        public async Task<Clientes?> ObtenerPorIdAsync(int id)
        {
            return await _context.Clientes.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Clientes>> ObtenerTodasAsync()
        {
            return await _context.Clientes.ToListAsync();

        }
    }
}
