using System;
using System.IO;
using System.Text;
using App.XF.DI.DependencyService;
using Serilog;

namespace App.iOS.DI.DependencyService
{
    internal class DeviceLog : IDeviceLog
    {
        private readonly Serilog.Core.Logger _logger;

        public DeviceLog()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.Personal), "logs/logs.txt"),
                    rollingInterval: RollingInterval.Month,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 31,
                    encoding: Encoding.UTF8)
                .WriteTo.NSLog()
                .CreateLogger();
        }

        ~DeviceLog()
        {
            _logger.Dispose();
        }

        public void Info(string tag, string msg)
        {
            _logger.Information(tag + " - " + msg);
        }

        public void Debug(string tag, string msg)
        {
            _logger.Debug(tag + " - " + msg);
        }

        public void Error(string tag, string msg)
        {
            _logger.Error(tag + " - " + msg);
        }

        public void Error(string tag, string msg, Exception exception)
        {
            _logger.Error(exception, tag + " - " + msg);
        }

        public string GetLogs()
        {
            var logs = Directory.GetFiles(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal), "logs/"));
            var logsStr = "";

            foreach (var logfile in logs)
            {
                try
                {
                    using (var stream = new FileStream(logfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            logsStr += reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return logsStr;
        }

        public void ClearLogsMonthly()
        {
        }
    }
}
