using Auth.Domain.Entities;
using Auth.Domain.Services;
using Core.Domain.Response;
using CrossCutting.Response;
using Ninject;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApi
{
    /// <summary>
    /// AuthorizationFilter para basic authentication
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SecretAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public bool Ativo { get; set; }
        [Inject]
        public IAuthService AuthService { get; set; }

        public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            if (Ativo)
            {
                var identity = ParseAuthorizationHeader(actionContext);
                var responseTokenValidation = (await OnAuthorizeUser(identity, actionContext));
                if (identity == null || !responseTokenValidation.Success)
                {
                    IssueHttpAuthorizationChallenge(
                        actionContext, 
                        (responseTokenValidation as ResponseEnvelope<UserToken>).Error.Message,
                        (responseTokenValidation as ResponseEnvelope<UserToken>).Error.ErrorCode);
                    return;
                }

                var principal = new GenericPrincipal(identity, null);

                Thread.CurrentPrincipal = principal;
                System.Web.HttpContext.Current.User = principal;

                base.OnAuthorization(actionContext);
            }
        }

        protected async Task<IResponseEnvelope> OnAuthorizeUser(SecretAuthenticationIdentity identity, HttpActionContext actionContext)
        {
            var authResponse = await AuthService.SearchByTokenAsync(identity?.Name);
            if (authResponse.Success)
                identity.UserInfo = authResponse.Content;

            return authResponse;
        }

        protected SecretAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext)
        {
            var auth = actionContext.Request.Headers.Authorization;
            if (auth != null && !string.IsNullOrWhiteSpace(auth.Parameter))
            {
                var authHeader = auth.Parameter;
                return new SecretAuthenticationIdentity(authHeader);
            }
            return null;
        }

        void IssueHttpAuthorizationChallenge(HttpActionContext actionContext, string msg, int? code)
        {
            var host = actionContext.Request.RequestUri.DnsSafeHost;

            //var message = ValidationMessages.UNAUTHORIZED_USER;
            var message = msg;

            ValidationMessage vmsg = new ValidationMessage(msg, code);

            var response = ResponseEnvelope
                .CreateErrorResponseEnvelope(vmsg);

            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, response);

            actionContext.Response.Headers.Add("WWW-Authenticate", string.Format("secret realm=\"{0}\"", host));
        }
    }

    public class SecretAuthenticationIdentity : GenericIdentity
    {
        public SecretAuthenticationIdentity(string accessToken)
            : base(accessToken, "Secret")
        { }

        public UserToken UserInfo { get; set; }
    }
}
