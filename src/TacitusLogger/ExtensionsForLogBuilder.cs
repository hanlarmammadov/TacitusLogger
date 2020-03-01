using System;
using System.ComponentModel;

namespace TacitusLogger
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ExtensionsForLogBuilder
    {
        public static LogBuilder WithEx(this LogBuilderBase<LogBuilder> self, Exception ex)
        {
            return LogBuilderBaseExtensionsHelper.WithEx(self, ex);
        }
        public static LogBuilder WithStackTrace(this LogBuilderBase<LogBuilder> self)
        {
            return LogBuilderBaseExtensionsHelper.WithStackTrace(self);
        }
    }
}
