using System.ComponentModel;

namespace TacitusLogger.Builders
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IFileDestinationBuilder : IDestinationBuilder, IBuilderWithLogTextSerialization<IFileDestinationBuilder>, IBuilderWithFilePath<IFileDestinationBuilder>
    {

    }
}
