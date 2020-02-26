using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ExtensionMethodsForILogger
    {
        #region Log overloads

        /// <summary>
        /// Writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string Log(this ILogger self, string context, LogType type, string description, string[] tags, LogItem[] logItems)
        {
            return self.Log(new Log(context, type, description, tags, logItems));
        }
        /// <summary>
        /// Writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string Log(this ILogger self, string context, LogType type, string description, string[] tags, LogItem logItem)
        {
            return self.Log(new Log(context, type, description, tags, new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string Log(this ILogger self, string context, LogType type, string description, string[] tags, object logItem)
        {
            return self.Log(new Log(context, type, description, tags, new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <returns>Generated log id.</returns>
        public static string Log(this ILogger self, string context, LogType type, string description, string[] tags)
        {
            return self.Log(new Log(context, type, description, tags, null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string Log(this ILogger self, string context, LogType type, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context, type, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string Log(this ILogger self, string context, LogType type, string description, LogItem logItem)
        {
            return self.Log(new Log(context, type, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string Log(this ILogger self, string context, LogType type, string description, object logItem)
        {
            return self.Log(new Log(context, type, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string Log(this ILogger self, string context, LogType type, string description)
        {
            return self.Log(new Log(context, type, description, null as string[], null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string Log(this ILogger self, LogType type, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context: null, type, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string Log(this ILogger self, LogType type, string description, LogItem logItem)
        {
            return self.Log(new Log(context: null, type, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string Log(this ILogger self, LogType type, string description, object logItem)
        {
            return self.Log(new Log(context: null, type, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string Log(this ILogger self, LogType type, string description)
        {
            return self.Log(new Log(context: null, type, description, null as string[], null as LogItem[]));
        }

        #endregion

        #region LogInfo overloads

        /// <summary>
        /// Writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogInfo(this ILogger self, string context, string description, string[] tags, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Info, description, tags, logItems));
        }
        /// <summary>
        /// Writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogInfo(this ILogger self, string context, string description, string[] tags, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Info, description, tags, new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogInfo(this ILogger self, string context, string description, string[] tags, object logItem)
        {
            return self.Log(new Log(context, LogType.Info, description, tags, new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <returns>Generated log id.</returns>
        public static string LogInfo(this ILogger self, string context, string description, string[] tags)
        {
            return self.Log(new Log(context, LogType.Info, description, tags, null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogInfo(this ILogger self, string context, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Info, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogInfo(this ILogger self, string context, string description, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Info, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogInfo(this ILogger self, string context, string description, object logItem)
        {
            return self.Log(new Log(context, LogType.Info, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogInfo(this ILogger self, string context, string description)
        {
            return self.Log(new Log(context, LogType.Info, description, null as string[], null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Info.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogInfo(this ILogger self, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context: null, LogType.Info, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Info.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogInfo(this ILogger self, string description, LogItem logItem)
        {
            return self.Log(new Log(context: null, LogType.Info, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Info.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogInfo(this ILogger self, string description, object logItem)
        {
            return self.Log(new Log(context: null, LogType.Info, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Info.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogInfo(this ILogger self, string description)
        {
            return self.Log(new Log(context: null, LogType.Info, description, null as string[], null as LogItem[]));
        }

        #endregion

        #region LogSuccess overloads

        /// <summary>
        /// Writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogSuccess(this ILogger self, string context, string description, string[] tags, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Success, description, tags, logItems));
        }
        /// <summary>
        /// Writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogSuccess(this ILogger self, string context, string description, string[] tags, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Success, description, tags, new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogSuccess(this ILogger self, string context, string description, string[] tags, object logItem)
        {
            return self.Log(new Log(context, LogType.Success, description, tags, new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <returns>Generated log id.</returns>
        public static string LogSuccess(this ILogger self, string context, string description, string[] tags)
        {
            return self.Log(new Log(context, LogType.Success, description, tags, null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogSuccess(this ILogger self, string context, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Success, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogSuccess(this ILogger self, string context, string description, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Success, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogSuccess(this ILogger self, string context, string description, object logItem)
        {
            return self.Log(new Log(context, LogType.Success, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogSuccess(this ILogger self, string context, string description)
        {
            return self.Log(new Log(context, LogType.Success, description, null as string[], null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Success.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogSuccess(this ILogger self, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context: null, LogType.Success, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Success.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogSuccess(this ILogger self, string description, LogItem logItem)
        {
            return self.Log(new Log(context: null, LogType.Success, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Success.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogSuccess(this ILogger self, string description, object logItem)
        {
            return self.Log(new Log(context: null, LogType.Success, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Success.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogSuccess(this ILogger self, string description)
        {
            return self.Log(new Log(context: null, LogType.Success, description, null as string[], null as LogItem[]));
        }

        #endregion

        #region LogEvent overloads

        /// <summary>
        /// Writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogEvent(this ILogger self, string context, string description, string[] tags, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Event, description, tags, logItems));
        }
        /// <summary>
        /// Writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogEvent(this ILogger self, string context, string description, string[] tags, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Event, description, tags, new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogEvent(this ILogger self, string context, string description, string[] tags, object logItem)
        {
            return self.Log(new Log(context, LogType.Event, description, tags, new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <returns>Generated log id.</returns>
        public static string LogEvent(this ILogger self, string context, string description, string[] tags)
        {
            return self.Log(new Log(context, LogType.Event, description, tags, null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogEvent(this ILogger self, string context, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Event, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogEvent(this ILogger self, string context, string description, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Event, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogEvent(this ILogger self, string context, string description, object logItem)
        {
            return self.Log(new Log(context, LogType.Event, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogEvent(this ILogger self, string context, string description)
        {
            return self.Log(new Log(context, LogType.Event, description, null as string[], null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Event.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogEvent(this ILogger self, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context: null, LogType.Event, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Event.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogEvent(this ILogger self, string description, LogItem logItem)
        {
            return self.Log(new Log(context: null, LogType.Event, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Event.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogEvent(this ILogger self, string description, object logItem)
        {
            return self.Log(new Log(context: null, LogType.Event, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Event.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogEvent(this ILogger self, string description)
        {
            return self.Log(new Log(context: null, LogType.Event, description, null as string[], null as LogItem[]));
        }

        #endregion

        #region LogWarning overloads

        /// <summary>
        /// Writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogWarning(this ILogger self, string context, string description, string[] tags, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Warning, description, tags, logItems));
        }
        /// <summary>
        /// Writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogWarning(this ILogger self, string context, string description, string[] tags, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Warning, description, tags, new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogWarning(this ILogger self, string context, string description, string[] tags, object logItem)
        {
            return self.Log(new Log(context, LogType.Warning, description, tags, new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <returns>Generated log id.</returns>
        public static string LogWarning(this ILogger self, string context, string description, string[] tags)
        {
            return self.Log(new Log(context, LogType.Warning, description, tags, null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogWarning(this ILogger self, string context, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Warning, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogWarning(this ILogger self, string context, string description, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Warning, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogWarning(this ILogger self, string context, string description, object logItem)
        {
            return self.Log(new Log(context, LogType.Warning, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogWarning(this ILogger self, string context, string description)
        {
            return self.Log(new Log(context, LogType.Warning, description, null as string[], null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Warning.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogWarning(this ILogger self, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context: null, LogType.Warning, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Warning.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogWarning(this ILogger self, string description, LogItem logItem)
        {
            return self.Log(new Log(context: null, LogType.Warning, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Warning.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogWarning(this ILogger self, string description, object logItem)
        {
            return self.Log(new Log(context: null, LogType.Warning, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Warning.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogWarning(this ILogger self, string description)
        {
            return self.Log(new Log(context: null, LogType.Warning, description, null as string[], null as LogItem[]));
        }

        #endregion

        #region LogError overloads

        /// <summary>
        /// Writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogError(this ILogger self, string context, string description, string[] tags, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Error, description, tags, logItems));
        }
        /// <summary>
        /// Writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogError(this ILogger self, string context, string description, string[] tags, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Error, description, tags, new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogError(this ILogger self, string context, string description, string[] tags, object logItem)
        {
            return self.Log(new Log(context, LogType.Error, description, tags, new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <returns>Generated log id.</returns>
        public static string LogError(this ILogger self, string context, string description, string[] tags)
        {
            return self.Log(new Log(context, LogType.Error, description, tags, null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogError(this ILogger self, string context, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Error, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogError(this ILogger self, string context, string description, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Error, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogError(this ILogger self, string context, string description, object logItem)
        {
            return self.Log(new Log(context, LogType.Error, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogError(this ILogger self, string context, string description)
        {
            return self.Log(new Log(context, LogType.Error, description, null as string[], null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Error.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogError(this ILogger self, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context: null, LogType.Error, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Error.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogError(this ILogger self, string description, LogItem logItem)
        {
            return self.Log(new Log(context: null, LogType.Error, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Error.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogError(this ILogger self, string description, object logItem)
        {
            return self.Log(new Log(context: null, LogType.Error, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Error.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogError(this ILogger self, string description)
        {
            return self.Log(new Log(context: null, LogType.Error, description, null as string[], null as LogItem[]));
        }

        #endregion

        #region LogFailure overloads

        /// <summary>
        /// Writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogFailure(this ILogger self, string context, string description, string[] tags, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Failure, description, tags, logItems));
        }
        /// <summary>
        /// Writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogFailure(this ILogger self, string context, string description, string[] tags, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Failure, description, tags, new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogFailure(this ILogger self, string context, string description, string[] tags, object logItem)
        {
            return self.Log(new Log(context, LogType.Failure, description, tags, new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <returns>Generated log id.</returns>
        public static string LogFailure(this ILogger self, string context, string description, string[] tags)
        {
            return self.Log(new Log(context, LogType.Failure, description, tags, null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogFailure(this ILogger self, string context, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Failure, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogFailure(this ILogger self, string context, string description, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Failure, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogFailure(this ILogger self, string context, string description, object logItem)
        {
            return self.Log(new Log(context, LogType.Failure, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogFailure(this ILogger self, string context, string description)
        {
            return self.Log(new Log(context, LogType.Failure, description, null as string[], null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Failure.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogFailure(this ILogger self, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context: null, LogType.Failure, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Failure.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogFailure(this ILogger self, string description, LogItem logItem)
        {
            return self.Log(new Log(context: null, LogType.Failure, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Failure.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogFailure(this ILogger self, string description, object logItem)
        {
            return self.Log(new Log(context: null, LogType.Failure, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Failure.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogFailure(this ILogger self, string description)
        {
            return self.Log(new Log(context: null, LogType.Failure, description, null as string[], null as LogItem[]));
        }

        #endregion

        #region LogCritical overloads

        /// <summary>
        /// Writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogCritical(this ILogger self, string context, string description, string[] tags, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Critical, description, tags, logItems));
        }
        /// <summary>
        /// Writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogCritical(this ILogger self, string context, string description, string[] tags, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Critical, description, tags, new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogCritical(this ILogger self, string context, string description, string[] tags, object logItem)
        {
            return self.Log(new Log(context, LogType.Critical, description, tags, new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <returns>Generated log id.</returns>
        public static string LogCritical(this ILogger self, string context, string description, string[] tags)
        {
            return self.Log(new Log(context, LogType.Critical, description, tags, null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogCritical(this ILogger self, string context, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context, LogType.Critical, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogCritical(this ILogger self, string context, string description, LogItem logItem)
        {
            return self.Log(new Log(context, LogType.Critical, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogCritical(this ILogger self, string context, string description, object logItem)
        {
            return self.Log(new Log(context, LogType.Critical, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogCritical(this ILogger self, string context, string description)
        {
            return self.Log(new Log(context, LogType.Critical, description, null as string[], null as LogItem[]));
        }
        /// <summary>
        /// Writes a new log of type Critical.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogCritical(this ILogger self, string description, LogItem[] logItems)
        {
            return self.Log(new Log(context: null, LogType.Critical, description, null as string[], logItems));
        }
        /// <summary>
        /// Writes a new log of type Critical.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogCritical(this ILogger self, string description, LogItem logItem)
        {
            return self.Log(new Log(context: null, LogType.Critical, description, null as string[], new LogItem[] { logItem }));
        }
        /// <summary>
        /// Writes a new log of type Critical.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <returns>Generated log id.</returns>
        public static string LogCritical(this ILogger self, string description, object logItem)
        {
            return self.Log(new Log(context: null, LogType.Critical, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }));
        }
        /// <summary>
        /// Writes a new log of type Critical.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <returns>Generated log id.</returns>
        public static string LogCritical(this ILogger self, string description)
        {
            return self.Log(new Log(context: null, LogType.Critical, description, null as string[], null as LogItem[]));
        }

        #endregion

        #region LogAsync overloads

        /// <summary>
        /// Asynchronously writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogAsync(this ILogger self, string context, LogType type, string description, string[] tags, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, type, description, tags, logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogAsync(this ILogger self, string context, LogType type, string description, string[] tags, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, type, description, tags, new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogAsync(this ILogger self, string context, LogType type, string description, string[] tags, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, type, description, tags, new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogAsync(this ILogger self, string context, LogType type, string description, string[] tags, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, type, description, tags, null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogAsync(this ILogger self, string context, LogType type, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, type, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogAsync(this ILogger self, string context, LogType type, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, type, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogAsync(this ILogger self, string context, LogType type, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, type, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogAsync(this ILogger self, string context, LogType type, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, type, description, null as string[], null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogAsync(this ILogger self, LogType type, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, type, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogAsync(this ILogger self, LogType type, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, type, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogAsync(this ILogger self, LogType type, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, type, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log.
        /// </summary>
        /// <param name="type">Log type.</param>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogAsync(this ILogger self, LogType type, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, type, description, null as string[], null as LogItem[]), cancellationToken);
        }

        #endregion

        #region LogInfoAsync overloads

        /// <summary>
        /// Asynchronously writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogInfoAsync(this ILogger self, string context, string description, string[] tags, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Info, description, tags, logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogInfoAsync(this ILogger self, string context, string description, string[] tags, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Info, description, tags, new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogInfoAsync(this ILogger self, string context, string description, string[] tags, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Info, description, tags, new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogInfoAsync(this ILogger self, string context, string description, string[] tags, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Info, description, tags, null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogInfoAsync(this ILogger self, string context, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Info, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogInfoAsync(this ILogger self, string context, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Info, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogInfoAsync(this ILogger self, string context, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Info, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Info.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogInfoAsync(this ILogger self, string context, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Info, description, null as string[], null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Info.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogInfoAsync(this ILogger self, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Info, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Info.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogInfoAsync(this ILogger self, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Info, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Info.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogInfoAsync(this ILogger self, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Info, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Info.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogInfoAsync(this ILogger self, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Info, description, null as string[], null as LogItem[]), cancellationToken);
        }

        #endregion

        #region LogSuccessAsync overloads

        /// <summary>
        /// Asynchronously writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogSuccessAsync(this ILogger self, string context, string description, string[] tags, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Success, description, tags, logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogSuccessAsync(this ILogger self, string context, string description, string[] tags, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Success, description, tags, new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogSuccessAsync(this ILogger self, string context, string description, string[] tags, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Success, description, tags, new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogSuccessAsync(this ILogger self, string context, string description, string[] tags, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Success, description, tags, null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogSuccessAsync(this ILogger self, string context, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Success, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogSuccessAsync(this ILogger self, string context, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Success, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogSuccessAsync(this ILogger self, string context, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Success, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Success.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogSuccessAsync(this ILogger self, string context, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Success, description, null as string[], null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Success.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogSuccessAsync(this ILogger self, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Success, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Success.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogSuccessAsync(this ILogger self, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Success, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Success.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogSuccessAsync(this ILogger self, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Success, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Success.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogSuccessAsync(this ILogger self, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Success, description, null as string[], null as LogItem[]), cancellationToken);
        }

        #endregion

        #region LogEventAsync overloads

        /// <summary>
        /// Asynchronously writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogEventAsync(this ILogger self, string context, string description, string[] tags, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Event, description, tags, logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogEventAsync(this ILogger self, string context, string description, string[] tags, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Event, description, tags, new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogEventAsync(this ILogger self, string context, string description, string[] tags, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Event, description, tags, new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogEventAsync(this ILogger self, string context, string description, string[] tags, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Event, description, tags, null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogEventAsync(this ILogger self, string context, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Event, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogEventAsync(this ILogger self, string context, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Event, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogEventAsync(this ILogger self, string context, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Event, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Event.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogEventAsync(this ILogger self, string context, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Event, description, null as string[], null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Event.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogEventAsync(this ILogger self, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Event, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Event.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogEventAsync(this ILogger self, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Event, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Event.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogEventAsync(this ILogger self, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Event, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Event.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogEventAsync(this ILogger self, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Event, description, null as string[], null as LogItem[]), cancellationToken);
        }

        #endregion

        #region LogWarningAsync overloads

        /// <summary>
        /// Asynchronously writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogWarningAsync(this ILogger self, string context, string description, string[] tags, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Warning, description, tags, logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogWarningAsync(this ILogger self, string context, string description, string[] tags, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Warning, description, tags, new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogWarningAsync(this ILogger self, string context, string description, string[] tags, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Warning, description, tags, new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogWarningAsync(this ILogger self, string context, string description, string[] tags, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Warning, description, tags, null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogWarningAsync(this ILogger self, string context, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Warning, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogWarningAsync(this ILogger self, string context, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Warning, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogWarningAsync(this ILogger self, string context, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Warning, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Warning.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogWarningAsync(this ILogger self, string context, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Warning, description, null as string[], null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Warning.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogWarningAsync(this ILogger self, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Warning, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Warning.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogWarningAsync(this ILogger self, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Warning, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Warning.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogWarningAsync(this ILogger self, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Warning, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Warning.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogWarningAsync(this ILogger self, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Warning, description, null as string[], null as LogItem[]), cancellationToken);
        }

        #endregion

        #region LogErrorAsync overloads

        /// <summary>
        /// Asynchronously writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogErrorAsync(this ILogger self, string context, string description, string[] tags, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Error, description, tags, logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogErrorAsync(this ILogger self, string context, string description, string[] tags, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Error, description, tags, new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogErrorAsync(this ILogger self, string context, string description, string[] tags, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Error, description, tags, new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogErrorAsync(this ILogger self, string context, string description, string[] tags, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Error, description, tags, null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogErrorAsync(this ILogger self, string context, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Error, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogErrorAsync(this ILogger self, string context, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Error, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogErrorAsync(this ILogger self, string context, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Error, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Error.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogErrorAsync(this ILogger self, string context, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Error, description, null as string[], null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Error.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogErrorAsync(this ILogger self, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Error, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Error.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogErrorAsync(this ILogger self, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Error, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Error.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogErrorAsync(this ILogger self, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Error, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Error.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogErrorAsync(this ILogger self, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Error, description, null as string[], null as LogItem[]), cancellationToken);
        }

        #endregion

        #region LogFailureAsync overloads

        /// <summary>
        /// Asynchronously writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogFailureAsync(this ILogger self, string context, string description, string[] tags, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Failure, description, tags, logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogFailureAsync(this ILogger self, string context, string description, string[] tags, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Failure, description, tags, new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogFailureAsync(this ILogger self, string context, string description, string[] tags, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Failure, description, tags, new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogFailureAsync(this ILogger self, string context, string description, string[] tags, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Failure, description, tags, null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogFailureAsync(this ILogger self, string context, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Failure, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogFailureAsync(this ILogger self, string context, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Failure, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogFailureAsync(this ILogger self, string context, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Failure, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Failure.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogFailureAsync(this ILogger self, string context, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Failure, description, null as string[], null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Failure.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogFailureAsync(this ILogger self, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Failure, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Failure.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogFailureAsync(this ILogger self, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Failure, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Failure.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogFailureAsync(this ILogger self, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Failure, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Failure.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogFailureAsync(this ILogger self, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Failure, description, null as string[], null as LogItem[]), cancellationToken);
        }

        #endregion

        #region LogCriticalAsync overloads

        /// <summary>
        /// Asynchronously writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogCriticalAsync(this ILogger self, string context, string description, string[] tags, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Critical, description, tags, logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogCriticalAsync(this ILogger self, string context, string description, string[] tags, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Critical, description, tags, new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogCriticalAsync(this ILogger self, string context, string description, string[] tags, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Critical, description, tags, new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="tags">Collection of tags.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogCriticalAsync(this ILogger self, string context, string description, string[] tags, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Critical, description, tags, null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogCriticalAsync(this ILogger self, string context, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Critical, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogCriticalAsync(this ILogger self, string context, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Critical, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogCriticalAsync(this ILogger self, string context, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Critical, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Critical.
        /// </summary>
        /// <param name="context">Log context.</param>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogCriticalAsync(this ILogger self, string context, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context, LogType.Critical, description, null as string[], null as LogItem[]), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Critical.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItems">Collection of log items that contain additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogCriticalAsync(this ILogger self, string description, LogItem[] logItems, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Critical, description, null as string[], logItems), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Critical.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Log item that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogCriticalAsync(this ILogger self, string description, LogItem logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Critical, description, null as string[], new LogItem[] { logItem }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Critical.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="logItem">Object that contains additional data to be saved in the log.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogCriticalAsync(this ILogger self, string description, object logItem, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Critical, description, null as string[], new LogItem[] { LogItem.FromObj(logItem) }), cancellationToken);
        }
        /// <summary>
        /// Asynchronously writes a new log of type Critical.
        /// </summary>
        /// <param name="description">Log description.</param>
        /// <param name="cancellationToken">Operation cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation. The value of the TResult represents generated log id.</returns>
        public static Task<string> LogCriticalAsync(this ILogger self, string description, CancellationToken cancellationToken = default(CancellationToken))
        {
            return self.LogAsync(new Log(context: null, LogType.Critical, description, null as string[], null as LogItem[]), cancellationToken);
        }

        #endregion
    }
}
