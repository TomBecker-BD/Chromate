using System;
using System.Linq;
using System.Web.Http.Dependencies;
using System.Web.Http.ExceptionHandling;
using Chromate;
using Ninject;
using NUnit.Framework;

namespace Toaster.WebAPI.Test
{
    [TestFixture]
    public class TestToasterWebAPIModule
    {
        [Test]
        public void TestLoad()
        {
            var module = new ToasterWebAPIModule();
            using (var kernel = new StandardKernel(module))
            {
                var bindings = module.Bindings.Select(binding => binding.Service).ToArray();
                Assert.That(bindings, Is.EquivalentTo(new Type[]
                {
                    typeof(IDependencyResolver),
                    typeof(IWebAPI),
                    typeof(IExceptionLogger),
                    typeof(IToasterStatusMonitor)
                }));
            }
        }
    }
}
