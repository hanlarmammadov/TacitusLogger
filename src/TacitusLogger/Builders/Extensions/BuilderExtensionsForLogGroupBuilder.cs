using System.ComponentModel;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Adds extension methods to <c>TacitusLogger.Builders.ILogGroupDestinationsBuilder</c> interface and its implementations.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class BuilderExtensionsForLogGroupBuilder
    {
        /// <summary>
        /// Initiate the adding a file destination to the log group builder.
        /// </summary>
        /// <param name="obj">Log group destination builder.</param>
        /// <returns>File destination builder.</returns>
        public static IFileDestinationBuilder File(this ILogGroupDestinationsBuilder obj)
        {
            return new FileDestinationBuilder(obj);
        }
        /// <summary>
        /// Initiate the adding a console destination to the log group builder.
        /// </summary>
        /// <param name="obj">Log group destination builder.</param>
        /// <returns>Console destination builder.</returns>
        public static IConsoleDestinationBuilder Console(this ILogGroupDestinationsBuilder obj)
        {
            return new ConsoleDestinationBuilder(obj);
        }
        /// <summary>
        /// Initiate the adding a debug destination to the log group builder.
        /// </summary>
        /// <param name="obj">Log group destination builder.</param>
        /// <returns>Debug destination builder.</returns>
        public static IDebugDestinationBuilder Debug(this ILogGroupDestinationsBuilder obj)
        {
            return new DebugDestinationBuilder(obj);
        } 
        /// <summary>
        /// Initiate the adding a TextWriter destination to the log group builder.
        /// </summary>
        /// <param name="obj">Log group destination builder.</param>
        /// <returns>TextWriter destination builder.</returns>
        public static ITextWriterDestinationBuilder TextWriter(this ILogGroupDestinationsBuilder obj)
        {
            return new TextWriterDestinationBuilder(obj);
        }
        /// <summary>
        /// Finishes the build process by building the current log group and the logger.
        /// </summary>
        /// <param name="self">Log group destination builder.</param>
        /// <returns>The ready to use instance of the <c>TacitusLogger.ILogger</c></returns>
        public static Logger BuildLogger(this ILogGroupDestinationsBuilder self)
        {
            return self.BuildLogGroup().BuildLogger();
        }
    }
}
