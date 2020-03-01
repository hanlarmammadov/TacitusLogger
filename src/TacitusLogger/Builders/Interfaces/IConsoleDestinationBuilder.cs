using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TacitusLogger.Builders
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IConsoleDestinationBuilder: IDestinationBuilder, IBuilderWithLogTextSerialization<IConsoleDestinationBuilder>
    {  
        IConsoleDestinationBuilder WithCustomColors(IDictionary<LogType, ConsoleColor> colorScheme);
    }
}

