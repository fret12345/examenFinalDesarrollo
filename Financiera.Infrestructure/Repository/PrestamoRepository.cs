using Financiera.Application.Interface.Repository;
using Financiera.Domain.Entities;
using Financiera.Infrestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Infrestructure.Repository
{
    public class PrestamoRepository : IPrestamoRepository
    {
        private readonly ApplicationDbContext _context;

        public PrestamoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Prestamos prestamos)
        {
            _context.Prestamos.Update(prestamos);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarEstado(Prestamos dto)
        {

            _context.Prestamos.Update(dto);
            await _context.SaveChangesAsync();
        }

        public async Task CrearAsync(Prestamos prestamos)
        {
            _context.Prestamos.Add(prestamos);
            await _context.SaveChangesAsync();
        }

        public Task<bool> ExistePrestamo(string Numero)
        {
            var NumeroNormmalizado = Numero.Trim().ToLower();

            return _context.Prestamos
                .AnyAsync(p => p.NumPrestamos.Trim().ToLower() == NumeroNormmalizado);
        }

        public Task<Prestamos?> ObtenerPorIdAsync(int id)
        {
           return _context.Prestamos.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Prestamos>> ObtenerPorNoPrestamoAsync(string Numero)
        {
            var query = _context.Prestamos
             .AsNoTracking()
             .AsQueryable();

            if (!string.IsNullOrWhiteSpace(Numero))
            {
                var busqueda = Numero.Trim().ToLower();

                query = query.Where(p => 
                p.NumPrestamos.ToLower().Contains(busqueda));
            }

            return await _context.Prestamos
                .OrderBy(c => c.NumPrestamos)
                .ToListAsync();
        }

        public async Task<IEnumerable<Prestamos>> ObtenerTodasAsync()
        {
            return await _context.Prestamos.ToListAsync();
        }
    }
}
