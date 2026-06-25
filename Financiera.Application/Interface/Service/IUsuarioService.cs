

using Financiera.Application.DTO.Usuarios;

namespace Financiera.Application.Interface.Service
{
    public interface IUsuarioService
    {
        Task<UsuarioDto?> ObtenerPorIdAsync(int id);
        Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync(int Pagina, int Tamano);
        Task<int> ContarAsync();
    }
}
