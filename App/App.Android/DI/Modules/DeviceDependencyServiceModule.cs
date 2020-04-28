using App.Android.DI.DependencyService;
using App.XF.DI.DependencyService;
using Autofac;

namespace App.Android.DI.Modules
{
    internal class DeviceDependencyServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionStatusRepository>().As<IConnectionStatusRepository>();
            builder.RegisterType<DeviceLog>().As<IDeviceLog>();
            builder.RegisterType<DeviceStorageRepository>().As<IDeviceStorageRepository>();
            builder.RegisterType<PackageInfoRepository>().As<IPackageInfo>();
        }
    }
}