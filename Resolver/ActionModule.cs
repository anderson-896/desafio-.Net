using Acoes.Services;
using Actions.Domain.Repositories;
using Actions.Domain.Services;
using Actions.Repositories;
using Actions.Services;
using Ninject.Modules;

namespace Resolver
{
    public class ActionModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IUserActionsService>().To<UserActionsService>();
            Bind<IPhoneRepository>().To<PhoneRepository>();
            Bind<IPhoneActionsService>().To<PhoneActionsService>();
        }
    }
}
