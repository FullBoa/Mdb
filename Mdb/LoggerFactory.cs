using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;

namespace Mdb
{
    /// <summary>
    /// Factory for Log4net logger
    /// </summary>
    public class LoggerFactory
    {
        /// <summary>
        /// Log4net config filename
        /// </summary>
        public const string Log4netConfigurationFileName = "log4net.config";

        /// <summary>
        /// Default logger name
        /// </summary>
        public const string LoggerName = "Logger";

        /// <summary>
        /// Gets instance of logger
        /// </summary>
        /// <returns><see cref="ILog"/></returns>
        public static ILog GetLogger()
        {
            var loggerRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(loggerRepository, new FileInfo(Log4netConfigurationFileName));
            return LogManager.GetLogger(Assembly.GetEntryAssembly(), LoggerName);
        }
    }
}