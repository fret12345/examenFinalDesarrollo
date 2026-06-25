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
    public class PagosRepository: IPagosRepository
    {
        private readonly ApplicationDbContext _context;

        public PagosRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Pagos pagos)
        {
            _context.Pagos.Update(pagos);
            await _context.SaveChangesAsync();
        }

        public async Task<int> ContarAsync()
        {
            return await _context.Pagos
                .CountAsync();
        }

        public async Task<int> ContarBusquedaAsync(int valor)
        {
            var query = _context.Pagos
               .AsNoTracking();
               //.Where(p => p.Estado == true);

            if (valor > 0)
            {
                // Como 'valor' ya es un int, lo usas directamente con '=='
                query = query.Where(p => p.Id == valor ||
                                          p.IdCuota == valor);
            }

            return await query.CountAsync();
        }

        public async Task CrearAsync(Pagos pagos)
        {
            _context.Pagos.Add(pagos);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExisteCuotaAsync(int Numero)
        {
            var numeroCuota = Numero;

            return await _context.Pagos
                .AnyAsync(c => c.IdCuota == numeroCuota);
        }

        public async Task<Pagos?> obtenerPorIdAsync(int id)
        {
            return await _context.Pagos.FirstOrDefaultAsync(c =>c.Id == id);
        }

        public async Task<IEnumerable<Pagos>> ObtenerPorIdCuotaAsync(int IdCuota, int pagina, int tamano)
        {
            var query = _context.Pagos
              .AsNoTracking()
              .AsQueryable();

            if (IdCuota > 0)
            {
                var busqueda = IdCuota;

                query = query.Where(p =>
                    p.IdCuota == busqueda);
            }

            return await query
                .OrderBy(p => p.IdCuota)
                .Skip((pagina - 1) * tamano)
                .Take(tamano)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pagos>> ObtenerTodasAsync(int pagina, int tamano)
        {
            return await _context.Pagos
               .AsNoTracking()
               .OrderBy(p => p.IdCuota)
               .Skip((pagina - 1) * tamano)
               .Take(tamano)
               .ToListAsync();
        }
    }
}
