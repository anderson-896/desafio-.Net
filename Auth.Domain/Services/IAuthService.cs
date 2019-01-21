using Auth.Domain.Entities;
using Core.Domain.Response;
using System.Threading.Tasks;

namespace Auth.Domain.Services
{
    public interface IAuthService
    {
        Task<IResponseEnvelope<UserToken>> AuthenticateAsync(string login, string senha);
        Task<IResponseEnvelope<UserToken>> SearchByTokenAsync(string token);
        Task<IResponseEnvelope> InvalidateToken(string token);
    }
}
