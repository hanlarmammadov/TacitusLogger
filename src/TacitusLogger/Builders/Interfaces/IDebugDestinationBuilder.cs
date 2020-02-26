using System.ComponentModel;

namespace TacitusLogger.Builders
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IDebugDestinationBuilder : IDestinationBuilder, IBuilderWithLogTextSerialization<IDebugDestinationBuilder>
    {

    }
}
