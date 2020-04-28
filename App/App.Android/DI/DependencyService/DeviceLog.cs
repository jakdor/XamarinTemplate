using System;
using Android.Util;
using App.XF.DI.DependencyService;
using Java.IO;
using Java.Lang;
using Exception = System.Exception;

namespace App.Android.DI.DependencyService
{
    /// <inheritdoc />
    /// <summary>
    /// IDeviceLog implementation forwarding logs to Android.Util.Log
    /// </summary>
    internal class DeviceLog : IDeviceLog
    {
        private readonly IDeviceStorageRepository _deviceStorageRepository;

        private const string LogClearDateKey = "LogClearDateKey";

        public DeviceLog(IDeviceStorageRepository deviceStorageRepository)
        {
            _deviceStorageRepository = deviceStorageRepository;
        }

        public void Info(string tag, string msg)
        {
            Log.Info(tag, msg);
        }

        public void Debug(string tag, string msg)
        {
            Log.Debug(tag, msg);
        }

        public void Error(string tag, string msg)
        {
            Log.Error(tag, msg);
        }

        public void Error(string tag, string msg, Exception exception)
        {
            Log.Error(tag, msg + ": " + exception);
        }

        public string GetLogs()
        {
            try
            {
                var process = Runtime.GetRuntime().Exec("logcat -d");
                var bufferedReader = new BufferedReader(
                    new InputStreamReader(process.InputStream));

                var log = new StringBuilder();
                string line;
                while ((line = bufferedReader.ReadLine()) != null)
                {
                    log.Append(line + "\n");
                }

                return log.ToString();
            }
            catch (IOException e)
            {
                Log.Error("DeviceLog", "Unable to read logs", e);
                return "";
            }
        }

        public void ClearLogsMonthly()
        {
            var today = DateTime.Today;

            if (_deviceStorageRepository.ContainsKey(LogClearDateKey))
            {
                var savedClearDate = _deviceStorageRepository.GetObj<DateTime>(LogClearDateKey);

                if (today - savedClearDate > TimeSpan.FromDays(7))
                {
                    ClearLogs();
                    _deviceStorageRepository.SaveObj(LogClearDateKey, today);
                }
            }
            else
            {
                ClearLogs();
                _deviceStorageRepository.SaveObj(LogClearDateKey, today);
            }
        }

        private static void ClearLogs()
        {
            new ProcessBuilder()
                .Command("logcat", "-c")
                .RedirectErrorStream(true)
                .Start();
        }
    }
}