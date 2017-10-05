using Ninject.Modules;
using Toaster.Interfaces;

namespace Toaster.Device
{
    public class ToasterDeviceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IToaster>().To<Toaster>().InSingletonScope();
        }
    }
}
