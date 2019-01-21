using Actions.Domain.Entities;
using Core.Domain.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Actions.Domain.Services
{
    public interface IPhoneActionsService
    {
        Task<IResponseEnvelope<IEnumerable<Phone>>> SearchByUserIdAsync(int userId);
        Task<IResponseEnvelope> InsertPhonesAsync(Phone[] phone, int userId);
    }
}
