using Core.Domain.Response;
using Suporte.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suporte.Domain.Services
{
    public interface IUserService
    {
        IResponseEnvelope<string> RequestPasswordChangeToken(string email);
        

        Task<IResponseEnvelope<User>> SearchUserById(int userId);

        Task<IResponseEnvelope<User>> SearchUserByLogin(string login);
    }
}
