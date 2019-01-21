using Auth.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auth.Domain.Repositories
{
    public interface IUserTokenRepository
    {
        Task<UserCredentials> SearchAsync(string login);

        Task DeleteByEmailAsync(string email);

        Task<IEnumerable<UserToken>> SearchTokensByLoginAsync(string login);

        Task<UserToken> SearchByTokenAsync(string token);

        Task InsertToken(UserToken token);

        Task UpdateTokenAsync(UserToken token);
    }
}
