using Android.Content;
using App.Android.DI.Modules;
using App.XF.DI;
using Autofac;

namespace App.Android.DI
{
    /// <inheritdoc />
    /// <summary>
    /// Device dependency container
    /// </summary>
    internal class DependencyContainer : BaseDependencyContainer
    {
        public override Module ProvideDeviceDependencyServiceModule()
        {
            return new DeviceDependencyServiceModule();
        }

        public void RegisterAppContext(Context context)
        {
            Builder.RegisterInstance(context);
        }
    }
}