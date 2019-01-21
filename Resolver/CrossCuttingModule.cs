using CrossCutting;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resolver
{
    public class CrossCuttingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISecurityHelper>().To<SecurityHelper>(); 
        }
    }
}
