
using Financiera.Domain.Entities;

namespace Financiera.Application.Interface.Repository
{
    public interface IUsuarioRepositoty
    {
        Task<Usuarios?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Usuarios>> ObtenerTodosAsync(int Pagina, int Tamano);
        Task<int> ContarAsync();

    }
}
