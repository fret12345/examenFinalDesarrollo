
using Financiera.Domain.Entities;

namespace Financiera.Application.Interface.Repository
{
    public interface IRefreshTokenRepository
    {
        Task GuardarAsync(RefreshToken token);
        Task<RefreshToken?> ObtenerAsync(string token);
        Task ActualizarAsync(RefreshToken token);



    }
}
