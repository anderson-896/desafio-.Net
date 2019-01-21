using Actions.Domain.Entities;
using Core.Domain.Response;
using System.Threading.Tasks;

namespace Actions.Domain.Services
{
    public interface IUserActionsService
    {
        Task<IResponseEnvelope<User>> SearchByIdAsync(int usuarioId);
        Task<IResponseEnvelope> InsertAsync(User user);
    }
}
