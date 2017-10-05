using System;
using System.Linq;
using Ninject;
using NUnit.Framework;
using Toaster.Interfaces;

namespace Toaster.Device.Test
{
    [TestFixture]
    public class TestToasterDeviceModule
    {
        [Test]
        public void TestLoad()
        {
            var module = new ToasterDeviceModule();
            using (var kernel = new StandardKernel(module))
            {
                var bindings = module.Bindings.Select(binding => binding.Service).ToArray();
                Assert.That(bindings, Is.EquivalentTo(new Type[] { typeof(IToaster) }));
            }
        }
    }
}
