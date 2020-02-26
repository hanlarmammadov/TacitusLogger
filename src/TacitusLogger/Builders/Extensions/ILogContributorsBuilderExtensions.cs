using System.ComponentModel;
using TacitusLogger.Contributors;

namespace TacitusLogger.Builders
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ILogContributorsBuilderExtensions
    { 
        public static ILogContributorsBuilder StackTrace(this ILogContributorsBuilder self, Setting<bool> isActive, string name = "Stack trace")
        {
            return self.Custom(new StackTraceContributor(name), isActive);
        }
        public static ILogContributorsBuilder StackTrace(this ILogContributorsBuilder self, bool isActive = true, string name = "Stack trace")
        {
            return self.Custom(new StackTraceContributor(name), isActive);
        }

        public static ILogContributorsBuilder Custom(this ILogContributorsBuilder self, LogContributorBase logContributor, bool isActive = true)
        {
            return self.Custom(logContributor, isActive);
        }
    }
}
