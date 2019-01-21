using Actions.Domain.Entities;
using Actions.Domain.Repositories;
using Actions.Domain.Services;
using Core.Domain.Response;
using CrossCutting.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Actions.Services
{
    public class PhoneActionsService : IPhoneActionsService
    {
        IPhoneRepository phoneRepository;
        public PhoneActionsService(IPhoneRepository phoneRepository)
        {
            this.phoneRepository = phoneRepository;
        }

        public async Task<IResponseEnvelope> InsertPhonesAsync(Phone[] phones, int userId)
        {
            await phoneRepository.InsertPhonesAsync(phones, userId);
            return ResponseEnvelope.CreateResponseEnvelope();
        }

        public async Task<IResponseEnvelope<IEnumerable<Phone>>> SearchByUserIdAsync(int userId)
        {
            var phones = await phoneRepository.SearchByUserIdAsync(userId);
            return ResponseEnvelope.CreateResponseEnvelope(phones);
        }
    }
}
