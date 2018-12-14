using System;
using FWTL.Core.Services.Logging;

namespace Auth.FWT.Infrastructure.Logging
{
    public class NLogLogger : ILogger
    {
        private static readonly Lazy<NLogLogger> LazyLogger = new Lazy<NLogLogger>(() => new NLogLogger());

        private static readonly Lazy<NLog.Logger> LazyNLogger = new Lazy<NLog.Logger>(() => NLog.LogManager.GetLogger("AppLogger"));

        private NLogLogger()
        {
        }

        public static ILogger Instance
        {
            get
            {
                return LazyLogger.Value;
            }
        }

        public void Debug(Exception exception, string message = null, params object[] args)
        {
            LazyNLogger.Value.Debug(exception, message, args);
        }

        public void Debug(string message, params object[] args)
        {
            LazyNLogger.Value.Debug(message, args);
        }

        public void Error(Exception exception, string message = null, params object[] args)
        {
            LazyNLogger.Value.Error(exception, message, args);
        }

        public void Error(string message, params object[] args)
        {
            LazyNLogger.Value.Error(message, args);
        }

        public void Fatal(Exception exception, string message = null, params object[] args)
        {
            LazyNLogger.Value.Fatal(exception, message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            LazyNLogger.Value.Fatal(message, args);
        }

        public void Info(Exception exception, string message = null, params object[] args)
        {
            LazyNLogger.Value.Info(exception, message, args);
        }

        public void Info(string message, params object[] args)
        {
            LazyNLogger.Value.Info(message, args);
        }

        public void Trace(Exception exception, string message = null, params object[] args)
        {
            LazyNLogger.Value.Trace(message, args);
        }

        public void Trace(string message, params object[] args)
        {
            LazyNLogger.Value.Trace(message, args);
        }

        public void Warn(Exception exception, string message = null, params object[] args)
        {
            LazyNLogger.Value.Warn(exception, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            LazyNLogger.Value.Warn(message, args);
        }
    }
}
