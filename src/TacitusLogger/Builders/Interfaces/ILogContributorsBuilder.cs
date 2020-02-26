using System.ComponentModel;
using TacitusLogger.Contributors;

namespace TacitusLogger.Builders
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ILogContributorsBuilder
    {
        ILogContributorsBuilder Custom(LogContributorBase logContributor, Setting<bool> isActive);
        ILoggerBuilder BuildContributors();
    }
}
