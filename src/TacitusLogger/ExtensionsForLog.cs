using System;
using System.ComponentModel;

namespace TacitusLogger
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ExtensionsForLog
    {
        public static Log WithEx(this LogBuilderBase<Log> self, Exception ex)
        {
            return LogBuilderBaseExtensionsHelper.WithEx(self, ex);
        }
        public static Log WithStackTrace(this LogBuilderBase<Log> self)
        {
            return LogBuilderBaseExtensionsHelper.WithStackTrace(self);
        }
    }
}
