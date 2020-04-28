using Android.Content;
using Android.Content.PM;
using App.XF.DI.DependencyService;

namespace App.Android.DI.DependencyService
{
    internal class PackageInfoRepository : IPackageInfo
    {
        public string PackageName => GetPackageName();
        public string Version => AppVersion();

        private readonly Context _appContext;

        public PackageInfoRepository(Context appContext)
        {
            _appContext = appContext;
        }

        private string GetPackageName()
        {
            return _appContext.PackageName;
        }

        private string AppVersion()
        {
            try
            {
                var packageInfo = _appContext.PackageManager.GetPackageInfo(GetPackageName(), 0);
                return packageInfo.VersionName;
            }
            catch (PackageManager.NameNotFoundException)
            {
                return "*.*.*";
            }
        }
    }
}