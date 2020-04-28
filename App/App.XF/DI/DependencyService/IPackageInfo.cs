namespace App.XF.DI.DependencyService
{
    /// <summary>
    /// APP version info interface - to be implemented on each platform
    /// </summary>
    public interface IPackageInfo
    {
        string PackageName { get; }
        string Version { get; }
    }
}
