using Actions.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actions.Domain.Entities;
using Core.Domain.Response;
using Actions.Domain.Repositories;
using CrossCutting.Response;
using Auth.Domain.Repositories;
using Auth.Domain.Entities;
using CrossCutting;
using System.ComponentModel.DataAnnotations;

namespace Actions.Services
{
    public class UserActionsService : IUserActionsService
    {
        IUserRepository userRepository;
        IPhoneRepository phoneRepository;
        IUserTokenRepository userTokenRepository;
        ISecurityHelper securityHelper;

        public UserActionsService(IUserRepository userRepository, IPhoneRepository phoneRepository, IUserTokenRepository userTokenRepository, ISecurityHelper securityHelper)
        {
            this.userRepository = userRepository;
            this.phoneRepository = phoneRepository;
            this.userTokenRepository = userTokenRepository;
            this.securityHelper = securityHelper;
        }

        public async Task<IResponseEnvelope> InsertAsync(User user)
        {
            //Verify if already exists
            var u = await userRepository.SearchByEmailAsync(user.Email);
            if (u != null)
            {
                return ResponseEnvelope.CreateErrorResponseEnvelope(ValidationMessageHelper.Create(ValidationMessages.EMAIL_EXISTS));
            }
            if (!IsUserFieldsValid(user)) {
                return ResponseEnvelope.CreateErrorResponseEnvelope(ValidationMessageHelper.Create(ValidationMessages.INVALID_FIELDS));
            };
            if (!IsUserFieldsFilled(user))
            {
                return ResponseEnvelope.CreateErrorResponseEnvelope(ValidationMessageHelper.Create(ValidationMessages.MISSING_FIELDS));
            }
            user.Password = securityHelper.SHA256(user.Password);
            await userRepository.InsertAsync(user);
            return ResponseEnvelope.CreateResponseEnvelope();
        }

        public async Task<IResponseEnvelope<User>> SearchByIdAsync(int usuarioId)
        {
            User usuario = await userRepository.SearchByIdAsync(usuarioId);
            List<Phone> phones = (await phoneRepository.SearchByUserIdAsync(usuarioId)).ToList();
            usuario.Phones = phones;
            UserToken userToken = (await userTokenRepository.SearchTokensByLoginAsync(usuario.Email)).FirstOrDefault();
            usuario.Last_login = userToken.ModifyDate;
            return ResponseEnvelope.CreateResponseEnvelope(usuario);
        }

        private bool IsUserFieldsValid(User user)
        {           
            // Validate E-mail
            var emailValidator = new EmailAddressAttribute();
            return emailValidator.IsValid(user.Email);
        }

        private bool IsUserFieldsFilled(User user)
        {
            // Validate Fields
            var hasAnyEmptyPhone = user.Phones.Any(p =>
            {
               return  p.HasAnyEmptyProperties();
            });            
            return !user.HasAnyEmptyProperties() && !hasAnyEmptyPhone;
        }

    }
}
