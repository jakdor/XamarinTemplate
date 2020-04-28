using App.XF.DI.Modules;
using Autofac;

namespace App.XF.DI
{
    /// <inheritdoc />
    /// <summary>
    /// Base shared Dependency Container - to be extended on each platform
    /// </summary>
    public abstract class BaseDependencyContainer : IDependencyContainer
    {
        protected ContainerBuilder Builder;
        protected IContainer Container;

        public void Init()
        {
            Builder = new ContainerBuilder();

            Builder.RegisterModule(ProvideCoreModule());
            Builder.RegisterModule(ProvideI18Module());
            Builder.RegisterModule(ProvideWebModule());
            Builder.RegisterModule(ProvideDeviceDependencyServiceModule());
        }

        public void Build()
        {
            Container = Builder.Build();
        }

        public TService Resolve<TService>()
        {
            return Container.Resolve<TService>();
        }

        public void Dispose()
        {
            Container.Dispose();
        }

        public Module ProvideWebModule()
        {
            return new WebDependencyModule { PrintDebugNetworkCalls = true };
        }

        public Module ProvideI18Module()
        {
            return new I18NDependencyModule();
        }

        public Module ProvideCoreModule()
        {
            return new CoreDependencyModule();
        }

        public abstract Module ProvideDeviceDependencyServiceModule();
    }
}
