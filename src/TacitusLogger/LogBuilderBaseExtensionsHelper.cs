using System;
using TacitusLogger.Contributors;

namespace TacitusLogger
{
    internal static class LogBuilderBaseExtensionsHelper
    {
        public static TReturn WithEx<TReturn>(LogBuilderBase<TReturn> self, Exception ex)
        {
            return self.With(new LogItem("Exception", ex));
        }
        public static TReturn WithStackTrace<TReturn>(LogBuilderBase<TReturn> self)
        {
            return self.With(new LogItem("Stack trace", new StackTraceContributor().ProduceLogItem()));
        }
    }
}
