using System;

namespace TacitusLogger
{
    /// <summary>
    /// Represents log model.
    /// </summary>
    public class LogModel
    {
        /// <summary>
        /// Log id
        /// </summary>
        public string LogId;
        /// <summary>
        /// Information about context of logging event (etc. method or class name)
        /// </summary>
        public string Context;
        /// <summary>
        /// 
        /// </summary>
        public string[] Tags;
        /// <summary>
        /// Information about source of the log.
        /// </summary>
        public string Source;
        /// <summary>
        /// Type of logging event.
        /// </summary>
        public LogType LogType;
        /// <summary>
        /// Detailed information about logging event.
        /// </summary>
        public string Description;
        /// <summary>
        /// List of log items which can be used to incorporate some additional information about the logging event.
        /// </summary>
        public LogItem[] LogItems;
        /// <summary>
        /// Date and time of logging event.
        /// </summary>
        public DateTime LogDate;

        /// <summary>
        /// String representation of type of logging event.
        /// </summary>
        public string LogTypeName => Enum.GetName(typeof(LogType), LogType);
        public bool IsInfo => LogType == LogType.Info;
        public bool IsSuccess => LogType == LogType.Success;
        public bool IsEvent => LogType == LogType.Event;
        public bool IsWarning => LogType == LogType.Warning;
        public bool IsFailure => LogType == LogType.Failure;
        public bool IsError => LogType == LogType.Error;
        public bool IsCritical => LogType == LogType.Critical;
        public bool HasTags => Tags != null && Tags.Length != 0;
        public bool HasItems => LogItems != null && LogItems.Length != 0;

        public bool IsWithinLogLevel(LogLevel logLevel)
        {
            return (int)LogType >= (int)logLevel;
        }
        public bool LogTypeIsIn(params LogType[] types)
        {
            if (types != null)
                for (int i = 0; i < types.Length; i++)
                    if (LogType == types[i])
                        return true;
            return false;
        }
        public bool HasTag(string tag)
        {
            if (Tags != null)
                for (int i = 0; i < Tags.Length; i++)
                    if (Tags[i] == tag)
                        return true;
            return false;
        }
        public bool HasTagsAll(params string[] tags)
        {
            bool allTagsFound = false;
            if (Tags != null && tags != null)
                for (int i = 0; i < tags.Length; i++)
                {
                    bool containsThisTag = false;
                    for (var j = 0; j < Tags.Length; j++)
                        if (Tags[j] == tags[i])
                        {
                            containsThisTag = true;
                            break;
                        }
                    allTagsFound = containsThisTag;
                    if (!containsThisTag)
                        break;
                }
            return allTagsFound;
        }
        public bool HasTagsAny(params string[] tags)
        {
            if (Tags != null && tags != null)
                for (int i = 0; i < tags.Length; i++)
                    for (var j = 0; j < Tags.Length; j++)
                        if (Tags[j] == tags[i])
                            return true;
            return false;
        }
        public bool HasItem(string itemName)
        {
            if (LogItems != null)
                for (int i = 0; i < LogItems.Length; i++)
                    if (LogItems[i].Name == itemName)
                        return true;
            return false;
        }
    }
}
