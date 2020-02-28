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
        public static ILogContributorsBuilder StackTrace(this ILogContributorsBuilder self, string name = "Stack trace")
        {
            return self.Custom(new StackTraceContributor(name), true);
        }

        public static ILogContributorsBuilder Custom(this ILogContributorsBuilder self, LogContributorBase logContributor)
        {
            return self.Custom(logContributor, true);
        }
    }
}
