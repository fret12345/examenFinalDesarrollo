using Financiera.Application.Interface.Repository;
using Financiera.Domain.Entities;
using Financiera.Infrestructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Financiera.Infrestructure.Repository
{
    public class CuentaPorCobrarRepository :ICuetnasPorCobrarRepository
    {
        private readonly ApplicationDbContext _context;

        public CuentaPorCobrarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(CuentasPorCobrar cuentasPorCobrar)
        {
            _context.CuentasPorCobrar.Update(cuentasPorCobrar);
            await _context.SaveChangesAsync();
        }

        public async Task<int> ContarAsync()
        {
           return await _context.CuentasPorCobrar
                .CountAsync(c => c.Estado == true);
        }

        public Task<int> ContarBusquedaAsync(int Valor)
        {
            var query = _context.CuentasPorCobrar
                .AsNoTracking()
                .Where(c => c.Estado == true);

            if(Valor != 0)
            {
                query = query.Where(c => c.IdPrestammo == Valor);
            }
            return query.CountAsync();
        }

        public async Task CrearAsync(CuentasPorCobrar cuentasPorCobrar)
        {
            _context.CuentasPorCobrar.Add(cuentasPorCobrar);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistePrestamo(int idPrestamo)
        {
            var NumeroPrestamo = idPrestamo;
            return await _context.Prestamos
               .AnyAsync(p => p.Id == idPrestamo);
        }

        public async Task<CuentasPorCobrar?> ObtenerPorIdAsync(int id)
        {
            return await _context.CuentasPorCobrar.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CuentasPorCobrar?> ObtenerPorIdPrestamoAsync(int id)
        {
            return await _context.CuentasPorCobrar.FirstOrDefaultAsync(c => c.IdPrestammo == id);
        }

        public async Task<IEnumerable<CuentasPorCobrar>> ObtenerPorNumPrestamoAsync(int Idprestamo, int pagina, int tamanio)
        {
            var query = _context.CuentasPorCobrar
                 .AsNoTracking()
                 .Where(p => p.Estado == true)
                 .AsQueryable();

            if (Idprestamo > 0)
            {
                var busqueda = Idprestamo;

                query = query.Where(p =>
                    p.IdPrestammo == busqueda);
            }

            return await query
                .OrderBy(p => p.IdPrestammo)
                .Skip((pagina - 1) * tamanio)
                .Take(tamanio)
                .ToListAsync();
        }

        public async Task<IEnumerable<CuentasPorCobrar>> ObtenerTodasAsync(int pagina, int tamanio)
        {
            return await _context.CuentasPorCobrar
              .AsNoTracking()
              .Where(p => p.Estado == true)
              .OrderBy(p => p.IdPrestammo)
              .Skip((pagina - 1) * tamanio)
              .Take(tamanio)
              .ToListAsync();
        }
    }
}
