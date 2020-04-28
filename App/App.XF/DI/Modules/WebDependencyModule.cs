using App.Web;
using App.Web.Rest.Network;
using Autofac;

namespace App.XF.DI.Modules
{
    internal class WebDependencyModule : Module
    {
        public bool PrintDebugNetworkCalls { get; set; }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RestServices>().As<IRestServices>();

#if DEBUG
            if (PrintDebugNetworkCalls) builder.RegisterType<DebugRefitFactory>().As<IRefitFactory>();
            else builder.RegisterType<RefitFactory>().As<IRefitFactory>();
#else
            builder.RegisterType<RefitFactory>().As<IRefitFactory>();
#endif
        }
    }
}
