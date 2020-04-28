using App.XF.i18n;
using Autofac;
using ToyaMobileNative.XF.i18n;

namespace App.XF.DI.Modules
{
    internal class I18NDependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppRes>().As<IAppRes>().SingleInstance();
        }
    }
}
