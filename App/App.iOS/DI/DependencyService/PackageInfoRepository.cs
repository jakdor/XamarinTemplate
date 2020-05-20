using App.XF.DI.DependencyService;
using Foundation;

namespace App.iOS.DI.DependencyService
{
    internal class PackageInfoRepository : IPackageInfo
    {
        string IPackageInfo.PackageName => PackageName();

        public string Version => AppVersion();

        public string BuildNumber => BuildNumberStr();

        public string PackageName()
        {
            return NSBundle.MainBundle.BundleIdentifier;
        }

        public string AppVersion()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString") as NSString;
        }

        public string BuildNumberStr()
        {
            return NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion") as NSString;
        }
    }
}
