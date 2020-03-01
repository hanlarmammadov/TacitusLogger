using System.ComponentModel; 
using TacitusLogger.Destinations.TextWriter;

namespace TacitusLogger.Builders
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ITextWriterDestinationBuilder : IDestinationBuilder, IBuilderWithLogTextSerialization<ITextWriterDestinationBuilder>
    {
        ITextWriterDestinationBuilder WithWriter(ITextWriterProvider textWriterProvider); 
    }
}
