using Suporte.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Response;
using Suporte.Domain.Entities;
using Suporte.Domain.Repositories;
using CrossCutting.Response;

namespace Suporte.Services
{
    public class UserService : IUserService
    {
        protected IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<IResponseEnvelope<User>> SearchUserById(int userId)
        {
            return ResponseEnvelope.CreateResponseEnvelope(await userRepository.SearchUserById(userId));
        }

        public async Task<IResponseEnvelope<User>> SearchUserByLogin(string login)
        {
            return ResponseEnvelope.CreateResponseEnvelope(await userRepository.SearchUserByLogin(login));
        }

        public IResponseEnvelope<string> RequestPasswordChangeToken(string email)
        {
            throw new NotImplementedException();
        }

        public IResponseEnvelope ResetPassword(string forgottenToken, string newPassword)
        {
            throw new NotImplementedException();
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                return new System.Net.Mail.MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
