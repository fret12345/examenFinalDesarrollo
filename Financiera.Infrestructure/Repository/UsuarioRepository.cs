
using Financiera.Application.Interface.Repository;
using Financiera.Domain.Entities;
using Financiera.Infrestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Financiera.Infrestructure.Repository
{
    public class UsuarioRepository : IUsuarioRepositoty
    {
        private readonly ApplicationDbContext _context;
        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> ContarAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<Usuarios?> ObtenerPorIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<Usuarios>> ObtenerTodosAsync(int Pagina, int Tamano)
         {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(p => p.NombresCompleto)
                .Skip((Pagina - 1) * Tamano)
                .Take(Tamano)
                .ToListAsync();
        }
    }
}
