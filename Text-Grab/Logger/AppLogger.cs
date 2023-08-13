using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.Web;
using System.Security.Cryptography;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Layouts;
using System.Net;
using System.IO;

namespace Text_Grab.Logger
{
    public partial class AppLogger
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Log error with keyword
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="message">Log Message</param>
        /// <param name="ex">Exception, if this parameter present, that means this log is error</param>
        /// <param name="keyword">key word</param>
        /// <remarks></remarks>
        public static void LogError(string logger, object message, Exception ex, string keyword)
        {
            //ILogger log = LogManager.GetLogger("ExternalDispatchingFailure");
            //log.ErrorException(message.IfNullTrim(), ex);
            ILogger log = LogManager.GetLogger(logger);
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Error, logger, message.ToString());
            theEvent.Exception = ex;
            theEvent.Properties["EvtKeyword"] = keyword;
            log.Log(theEvent);
        }

        /// <summary>
        /// Log Info with keyword
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="message">Log Message</param>
        /// <param name="ex">Exception, if this parameter present, that means this log is error</param>
        /// <param name="keyword">key word</param>
        /// <remarks></remarks>
        public static void LogInfo(string logger, object message, string keyword)
        {
            //ILogger log = LogManager.GetLogger("ExternalDispatchingFailure");
            //log.ErrorException(message.IfNullTrim(), ex);
            ILogger log = LogManager.GetLogger(logger);
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Info, logger, message.ToString());
            theEvent.Properties["EvtKeyword"] = keyword;
            log.Log(theEvent);
        }

        /// <summary>
        /// Log debug with keyword
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="message">Log Message</param>
        /// <param name="ex">Exception, if this parameter present, that means this log is error</param>
        /// <param name="keyword">key word</param>
        /// <remarks></remarks>
        public static void LogDebug(string logger, object message, string keyword)
        {
            //ILogger log = LogManager.GetLogger("ExternalDispatchingFailure");
            //log.ErrorException(message.IfNullTrim(), ex);
            ILogger log = LogManager.GetLogger(logger);
            LogEventInfo theEvent = new LogEventInfo(LogLevel.Info, logger, message.ToString());
            theEvent.Properties["EvtKeyword"] = keyword;
            log.Log(theEvent);
        }

        /// <summary>
        /// Write Log through Log4Net Component
        /// </summary>
        /// <param name="message">Log Message</param>
        /// <remarks></remarks>
        public static void LogInfo(object message)
        {
            //var log = LogManager.GetLogger("Info");
            //log.Info(message);
            LogInfo(message, String.Empty);
        }

        public static void LogInfo(object message, string keyword)
        {
            //var log = LogManager.GetLogger("Info");
            //log.Info(message);
            LogInfo("Info", message, keyword);
        }


        /// <summary>
        /// Write EDI Log through Log4Net Component
        /// </summary>
        /// <param name="message">Log Message</param>
        /// <remarks></remarks>
        public static void LogError(object message, Exception ex)
        {
            LogError("Error", message, ex, String.Empty);
        }

        public static void Init(string logDir)
        {
            LogManager.Configuration = new LoggingConfiguration();
            var configuration = LogManager.Configuration;
            var target = new FileTarget("file")
            {
                Layout = "${longdate} [${uppercase:${level}}] ${message}",
                FileName = Path.Combine(logDir, "log.txt"),
                ArchiveFileName = Path.Combine(logDir, "archive", "log.${date:format=yyyy-MM-dd HHmm}-{#}.txt"),
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveAboveSize = 1024000,
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                MaxArchiveFiles = 31,
                ConcurrentWrites = true,
                KeepFileOpen = false,
                Encoding = Encoding.UTF8
            };
            configuration.AddTarget(target);
            LogManager.Configuration.LoggingRules.Add(new LoggingRule("Info", LogLevel.Info, target));

            target = new FileTarget("errfile")
            {
#if DEBUG
                Layout = "${longdate} [${uppercase:${level}}] ${message} ${newline} ${exception:format=Message,StackTrace}",
#else
                Layout = "${longdate} [${uppercase:${level}}] ${message} ${newline} ${exception:format=Message}",
#endif
                FileName = Path.Combine(logDir, "log.error.txt"),
                ArchiveFileName = Path.Combine(logDir, "archive", "log.error.${date:format=yyyy-MM-dd HHmm}-{#}.txt"),
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveAboveSize = 1024000,
                ArchiveNumbering = ArchiveNumberingMode.Rolling,
                MaxArchiveFiles = 31,
                ConcurrentWrites = true,
                KeepFileOpen = false,
                Encoding = Encoding.UTF8
            };
            configuration.AddTarget(target);
            LogManager.Configuration.LoggingRules.Add(new LoggingRule("Error", LogLevel.Error, target));

            LogManager.ReconfigExistingLoggers(); // Magic method
        }
    }
}
