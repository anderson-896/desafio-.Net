using Auth.Domain.Entities;
using Auth.Domain.Repositories;
using Auth.Domain.Services;
using Core.Domain.Response;
using CrossCutting;
using CrossCutting.Response;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Services
{
    public class AuthService : IAuthService
    {
        IUserTokenRepository userTokenRepository;
        ISecurityHelper securityHelper;
        public AuthService(IUserTokenRepository userTokenRepository, ISecurityHelper securityHelper)
        {
            this.userTokenRepository = userTokenRepository;
            this.securityHelper = securityHelper;
        }

        public async Task<IResponseEnvelope<UserToken>> AuthenticateAsync(string login, string senha)
        {
            if (login == null || senha == null)
            {
                return ResponseEnvelope.CreateErrorResponseEnvelope<UserToken>(ValidationMessageHelper.CreateErrorMessage(ValidationMessages.MISSING_FIELDS));
            }
            var userCredentials = await userTokenRepository.SearchAsync(login);

            if (userCredentials == null || userCredentials.Password != securityHelper.SHA256(senha))
                return ResponseEnvelope.CreateErrorResponseEnvelope<UserToken>(ValidationMessageHelper.CreateErrorMessage(ValidationMessages.INVALID_LOGIN));

            var userToken = new UserToken
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                IsValid = true,
                UserId = userCredentials.Id,
                Token = securityHelper.GenerateUniqueToken(),
                Email = userCredentials.Email
            };

            await userTokenRepository.DeleteByEmailAsync(login);
            await userTokenRepository.InsertToken(userToken);

            return ResponseEnvelope.CreateResponseEnvelope(userToken);
        }

        public async Task<IResponseEnvelope<UserToken>> SearchByTokenAsync(string token)
        {
            //Recover user by token
            var userToken = await userTokenRepository.SearchByTokenAsync(token);
            // Verify if the token is valid
            if (userToken == null || !userToken.IsValid)
                return ResponseEnvelope.CreateErrorResponseEnvelope<UserToken>(ValidationMessageHelper.CreateErrorMessage(ValidationMessages.UNAUTHORIZED_USER));
            // Get all tokens from the current user
            var userTokens = await userTokenRepository.SearchTokensByLoginAsync(userToken.Email);
            // Get the last token from the current user
            var lastUserToken = userTokens.OrderByDescending(e => e.CreateDate).FirstOrDefault();

            if (lastUserToken == null ||
                lastUserToken.Token != token)
                return ResponseEnvelope.CreateErrorResponseEnvelope<UserToken>(ValidationMessageHelper.CreateErrorMessage(ValidationMessages.UNAUTHORIZED_USER));

            if (DateTime.Now.Subtract(lastUserToken.ModifyDate).TotalMinutes > 3)
                return ResponseEnvelope.CreateErrorResponseEnvelope<UserToken>(ValidationMessageHelper.CreateErrorMessage(ValidationMessages.INVALID_TOKEN));
            
            lastUserToken.ModifyDate = DateTime.Now;
            await userTokenRepository.UpdateTokenAsync(lastUserToken);

            return ResponseEnvelope.CreateResponseEnvelope(userToken);
        }

        public async Task<IResponseEnvelope> InvalidateToken(string token)
        {
            var userToken = await userTokenRepository.SearchByTokenAsync(token);

            if (userToken == null || !userToken.IsValid)
                return ResponseEnvelope.CreateErrorResponseEnvelope<UserToken>(ValidationMessageHelper.CreateErrorMessage(ValidationMessages.INVALID_TOKEN));

            userToken.IsValid = false;
            userToken.ModifyDate = DateTime.Now;

            await userTokenRepository.UpdateTokenAsync(userToken);

            return ResponseEnvelope.CreateResponseEnvelope();
        }
    }
}
