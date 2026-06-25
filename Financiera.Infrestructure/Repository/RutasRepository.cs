using Financiera.Application.Interface.Repository;
using Financiera.Domain.Entities;
using Financiera.Infrestructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Financiera.Infrestructure.Repository
{
    public class RutasRepository : IRutasRepository
    {
        private readonly ApplicationDbContext _context;

        public RutasRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarAsync(Rutas rutas)
        {
            _context.Rutas.Update(rutas);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Rutas>> BuscarRutasAsync(string nombre)
        {
            var query = _context.Rutas
               .AsNoTracking()
               .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var busqueda = nombre.Trim().ToLower();

                query = query.Where(c =>
                    c.NombreRuta.ToLower().Contains(busqueda));
            }

            return await _context.Rutas
                .OrderBy(c => c.NombreRuta)
                .ToListAsync();
        }

        public async Task CrearAsync(Rutas rutas)
        {
            _context.Rutas.Add(rutas);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            await _context.Rutas.Where(c => c.id == id).ExecuteDeleteAsync();

        }

        public Task<bool> ExisteNombreAsync(string nombre)
        {
            var nombreNormalizado = nombre.Trim().ToLower();

            return _context.Rutas
                .AnyAsync(c => c.NombreRuta.Trim().ToLower() == nombreNormalizado);
        }

        public async Task<Rutas?> ObtenerPorIdAsync(int id)
        {
            return await _context.Rutas.FirstOrDefaultAsync(c => c.id == id);

        }

        public async Task<IEnumerable<Rutas>> ObtenerTodasAsync()
        {
            return await _context.Rutas.ToListAsync();
        }
    }
}
