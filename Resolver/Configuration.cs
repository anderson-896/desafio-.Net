using Ninject.Modules;

namespace Resolver
{
    public static class Configuration
    {
        public static NinjectModule[] LoadWebContext()
        {
            return new NinjectModule[]
            {
                new AuthModule(),
                new CrossCuttingModule(),
                new ActionModule()

            };
        }
    }
}
