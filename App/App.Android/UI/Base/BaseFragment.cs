using System;
using Android.OS;
using App.XF.DI.DependencyService;
using CrossPlatformLiveData.Android;
using ToyaMobileNative.XF.i18n;

namespace App.Android.UI.Base
{
    /// <summary>
    /// Base Fragment class
    /// </summary>
    public abstract class BaseFragment : LiveDataSupportFragment
    {
        protected IDeviceLog Log;
        protected IAppRes AppRes;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            if (Activity is IBaseFragmentContract contract)
            {
                Log = contract.GetDeviceLog();
                AppRes = contract.GetAppRes();
            }
            else
            {
                throw new Exception("Context not implementing IBaseFragmentContract");
            }

            ApplyI18N();
        }

        protected virtual void ApplyI18N() { }
    }
}