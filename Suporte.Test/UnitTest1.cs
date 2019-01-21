using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Suporte.Domain.Services;
using Ninject;
using Resolver;
using Suporte.Domain.Entities;

namespace Suporte.Test
{
    [TestClass]
    public class UnitTest1
    {
        private StandardKernel iKernel;

        protected IUserService userService;

        public UnitTest1()
        {
            iKernel = new StandardKernel(new SuporteModule(), new DatabaseHelperModule(), new CrossCuttingModule());

            this.userService = iKernel.Get<IUserService>();
        
        }

        [TestMethod]
        public void TestMethod1()
        {
           
        }
    }
}
