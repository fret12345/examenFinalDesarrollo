using Financiera.Application.Interface.Repository;
using Financiera.Domain.Entities;
using Financiera.Infrestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Financiera.Infrestructure.Repository
{
    public class CuotaRepository : ICuotasRepository
    {
        private readonly ApplicationDbContext _context;

        public CuotaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Cuota cuota)
        {
            _context.Cuota.Update(cuota);
            await _context.SaveChangesAsync();
        }
        public async Task CrearAsync(Cuota cuota)
        {
            _context.Cuota.Add(cuota);
            await _context.SaveChangesAsync();
        }

        public Task<bool> ExisteCuotaAsync(int Numero)
        {
            var numeroCuota = Numero;

            return _context.Cuota
                .AnyAsync(c => c.NumCuota == numeroCuota);
        }

        public async Task<Cuota?> obtenerPorIdAsync(int id)
        {
            return await _context.Cuota.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> ContarAsync()
        {
            return await _context.Cuota
                .CountAsync(p => p.Estado == true);
        }

        public async Task<int> ContarBusquedaAsync(int valor)
        {
            var query = _context.Cuota
                .AsNoTracking()
                .Where(p => p.Estado == true);

            if (valor > 0)
            {
                // Como 'valor' ya es un int, lo usas directamente con '=='
                query = query.Where(p => p.NumCuota == valor ||
                                          p.IdPrestamos == valor);
            }

            return await query.CountAsync();
        }

        public async Task<IEnumerable<Cuota>> ObtenerPorIdPrestamoAsync(int IdPrestamo, int pagina, int tamano)
        {
            var query = _context.Cuota
                 .AsNoTracking()
                 .Where(p => p.Estado == true)
                 .AsQueryable();

            if (IdPrestamo > 0)
            {
                var busqueda = IdPrestamo;

                query = query.Where(p =>
                    p.IdPrestamos == busqueda);
            }

            return await query
                .OrderBy(p => p.NumCuota)
                .Skip((pagina - 1) * tamano)
                .Take(tamano)
                .ToListAsync();
        }

        public async Task<IEnumerable<Cuota>> ObtenerTodasAsync(int pagina, int tamano)
        {
            return await _context.Cuota
               .AsNoTracking()
               .Where(p => p.Estado == true)
               .OrderBy(p => p.NumCuota)
               .Skip((pagina - 1) * tamano)
               .Take(tamano)
               .ToListAsync();
        }
    }
}
