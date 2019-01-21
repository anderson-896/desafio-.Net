using Core.Domain.Response;
using Suporte.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suporte.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> SearchUserById(int userId);

        Task<User> SearchUserByLogin(string login);
    }
}
