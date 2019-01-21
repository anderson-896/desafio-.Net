using Actions.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Actions.Domain.Repositories
{
    public interface IPhoneRepository
    {
        Task<IEnumerable<Phone>> SearchByUserIdAsync(int userId);

        Task InsertPhonesAsync(Phone[] phone, int userId);
    }
}
