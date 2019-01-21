using Auth.Domain.Repositories;
using Auth.Domain.Services;
using Auth.Repositories;
using Auth.Services;
using Ninject.Modules;

namespace Resolver
{
    public class AuthModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserTokenRepository>().To<UserTokenRepository>();
            Bind<IAuthService>().To<AuthService>();
        }
    }
}
