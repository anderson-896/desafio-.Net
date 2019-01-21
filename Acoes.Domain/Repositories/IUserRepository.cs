using Actions.Domain.Entities;
using System.Threading.Tasks;

namespace Actions.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> SearchByIdAsync(int userId);
        Task<User> SearchByEmailAsync(string userEmail);
        Task InsertAsync(User user);
    }
}
