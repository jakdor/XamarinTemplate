using System;

namespace App.XF.DI.DependencyService
{
    /// <summary>
    /// Device Logger interface - to be implemented on each platform
    /// </summary>
    public interface IDeviceLog
    {
        void Info(string tag, string msg);
        void Debug(string tag, string msg);
        void Error(string tag, string msg);
        void Error(string tag, string msg, Exception exception);

        string GetLogs();
        void ClearLogsMonthly();
    }
}
