using App.XF.Repository;
using App.XF.Utils;
using App.XF.Utils.Rx;
using Autofac;

namespace App.XF.DI.Modules
{
    internal class CoreDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Register ViewModels
            builder.RegisterAssemblyTypes(GetType().Assembly)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .AsImplementedInterfaces();

            //Register Repositories
            builder.RegisterAssemblyTypes(GetType().Assembly)
                .Where(type => type.Name.EndsWith("Repository"))
                .Except<IAuthRepository>()
                .AsImplementedInterfaces();

            builder.RegisterType<AuthRepository>().As<IAuthRepository>().SingleInstance();
           
            //Register Services
            builder.RegisterAssemblyTypes(GetType().Assembly)
                .Where(type => type.Name.EndsWith("Service"))
                .AsImplementedInterfaces();

            //Register DisplayModelFactories
            builder.RegisterAssemblyTypes(GetType().Assembly)
                .Where(type => type.Name.EndsWith("DisplayModelFactory"));

            //Register Other
            builder.RegisterType<RxSchedulersFacade>().As<IRxSchedulersFacade>();
            builder.RegisterType<ObjCloneFactory>().As<IObjCloneFactory>();
        }
    }
}
