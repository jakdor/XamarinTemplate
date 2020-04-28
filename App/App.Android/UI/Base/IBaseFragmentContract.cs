using App.XF.DI.DependencyService;
using ToyaMobileNative.XF.i18n;

namespace App.Android.UI.Base
{
    interface IBaseFragmentContract
    {
        IDeviceLog GetDeviceLog();
        IAppRes GetAppRes();
    }
}