using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TacitusLogger.Builders
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IConsoleDestinationBuilder: IDestinationBuilder, IBuilderWithLogTextSerialization<IConsoleDestinationBuilder>
    {  
        IConsoleDestinationBuilder WithCustomColors(IDictionary<LogType, ConsoleColor> colorScheme);
    }
}

