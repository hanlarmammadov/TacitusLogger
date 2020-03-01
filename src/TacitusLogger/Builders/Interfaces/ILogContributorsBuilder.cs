using System.ComponentModel;
using TacitusLogger.Contributors;

namespace TacitusLogger.Builders
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ILogContributorsBuilder
    {
        ILogContributorsBuilder Custom(LogContributorBase logContributor, Setting<bool> isActive);
        ILoggerBuilder BuildContributors();
    }
}
