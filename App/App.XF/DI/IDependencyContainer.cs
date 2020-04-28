using Autofac;

namespace App.XF.DI
{
    /// <summary>
    /// Core dependency container interface
    /// </summary>
    public interface IDependencyContainer
    {
        void Init();
        void Build();
        TService Resolve<TService>();
        void Dispose();

        Module ProvideWebModule();
        Module ProvideI18Module();
        Module ProvideCoreModule();
        Module ProvideDeviceDependencyServiceModule();
    }
}
