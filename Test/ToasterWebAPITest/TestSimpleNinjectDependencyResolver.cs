using System;
using System.Linq;
using System.Threading.Tasks;
using Ninject;
using Ninject.Modules;
using NUnit.Framework;

namespace Toaster.WebAPI.Test
{
    [TestFixture]
    public class TestSimpleNinjectDependencyResolver
    {
        [Test]
        public void TestResolver()
        {
            var module = new DummyModule();
            using (var kernel = new StandardKernel(module))
            {
                using (var resolver = new SimpleNinjectDependencyResolver(kernel))
                {
                    Assert.That(resolver.BeginScope(), Is.SameAs(resolver));
                    Assert.That(resolver.GetService(typeof(IToasterStatusMonitor)), Is.TypeOf<MockToasterStatusMonitor>());
                    Assert.That(resolver.GetServices(typeof(IToasterStatusMonitor)).ToArray(), Is.EqualTo(new IToasterStatusMonitor[0]));
                }
            }
        }

        class DummyModule : NinjectModule
        {
            public override void Load()
            {
                Bind<IToasterStatusMonitor>().To<MockToasterStatusMonitor>().InSingletonScope();
            }
        }

        class MockToasterStatusMonitor : IToasterStatusMonitor
        {
            public Task<ToasterStatus> GetToasterStatusAsync(int timeout)
            {
                throw new NotImplementedException();
            }
        }
    }
}
