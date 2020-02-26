using System.ComponentModel;

namespace TacitusLogger.Builders
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IDestinationBuilder
    {
        ILogGroupDestinationsBuilder Add();
    }
}
