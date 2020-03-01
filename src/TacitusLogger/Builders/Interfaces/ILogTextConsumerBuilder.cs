using System.ComponentModel;
using TacitusLogger.Serializers;

namespace TacitusLogger.Builders
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IBuilderWithLogTextSerialization<TBuilder>
    {
        TBuilder WithLogSerializer(ILogSerializer logSerializer);
    }
}