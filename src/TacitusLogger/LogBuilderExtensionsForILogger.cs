using System.ComponentModel;

namespace TacitusLogger
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class LogBuilderExtensionsForILogger
    {
        public static LogBuilder Success(this ILogger self, string description)
        {
            return new LogBuilder(self, LogType.Success, description);
        }
        public static LogBuilder Info(this ILogger self, string description)
        {
            return new LogBuilder(self, LogType.Info, description);
        }
        public static LogBuilder Event(this ILogger self, string description)
        {
            return new LogBuilder(self, LogType.Event, description);
        }
        public static LogBuilder Warning(this ILogger self, string description)
        {
            return new LogBuilder(self, LogType.Warning, description);
        }
        public static LogBuilder Failure(this ILogger self, string description)
        {
            return new LogBuilder(self, LogType.Failure, description);
        }
        public static LogBuilder Error(this ILogger self, string description)
        {
            return new LogBuilder(self, LogType.Error, description);
        }
        public static LogBuilder Critical(this ILogger self, string description)
        {
            return new LogBuilder(self, LogType.Critical, description);
        }
    }
}
