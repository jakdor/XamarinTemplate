using App.iOS.DI.Modules;
using App.XF.DI;
using Autofac;

namespace App.iOS.DI
{
    public class DependencyContainer : BaseDependencyContainer
    {
        public override Module ProvideDeviceDependencyServiceModule()
        {
            return new DeviceDependencyServiceModule();
        }
    }
}
