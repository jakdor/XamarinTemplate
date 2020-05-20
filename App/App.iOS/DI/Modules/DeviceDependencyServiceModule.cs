using App.iOS.DI.DependencyService;
using App.XF.DI.DependencyService;
using Autofac;

namespace App.iOS.DI.Modules
{
    public class DeviceDependencyServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionStatusRepository>().As<IConnectionStatusRepository>();
            builder.RegisterType<DeviceLog>().As<IDeviceLog>().SingleInstance();
            builder.RegisterType<DeviceStorageRepository>().As<IDeviceStorageRepository>();
            builder.RegisterType<PackageInfoRepository>().As<IPackageInfo>();
            builder.RegisterType<SaveFileRepository>().As<ISaveFileRepository>();
        }
    }
}
