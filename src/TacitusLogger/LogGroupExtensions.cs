using System.ComponentModel;
using TacitusLogger.Caching;

namespace TacitusLogger
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class LogGroupExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheSize"></param>
        /// <param name="cacheTime"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public static LogGroup SetLogCache(this LogGroup self, int cacheSize, int cacheTime = -1, bool isActive = true)
        {
            return self.SetLogCache(new InMemoryLogCache(cacheSize, cacheTime), isActive);
        }
        /// <summary> 
        /// Sets the active status of the log group. If omitted the group is active by default.
        /// </summary> 
        /// <param name="status">Log group status</param>
        /// <returns></returns>
        public static LogGroup SetStatus(this LogGroup self, LogGroupStatus status)
        {
            return self.SetStatus(new MutableSetting<LogGroupStatus>(status));
        }
    }
}
