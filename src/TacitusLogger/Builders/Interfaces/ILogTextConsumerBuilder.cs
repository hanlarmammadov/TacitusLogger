using System.ComponentModel;
using TacitusLogger.Serializers;

namespace TacitusLogger.Builders
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBuilderWithLogTextSerialization<TBuilder>
    {
        TBuilder WithLogSerializer(ILogSerializer logSerializer);
    }
}