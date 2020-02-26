using System;
using System.Collections.Generic;
using System.ComponentModel;
using TacitusLogger.Transformers;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Builds a list of log transformers for the given logger builder.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class LogTransformersBuilder : ILogTransformersBuilder
    {
        private readonly Func<IList<LogTransformerBase>, ILoggerBuilder> _buildCallback;
        private readonly IList<LogTransformerBase> _logTransformers;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Builders.LogTransformersBuilder</c> using build callback.
        /// </summary>
        /// <param name="buildCallback">The build callback delegate used to complete the log transformers building process.</param>
        public LogTransformersBuilder(Func<IList<LogTransformerBase>, ILoggerBuilder> buildCallback)
        {
            _buildCallback = buildCallback;
            _logTransformers = new List<LogTransformerBase>();
        }

        /// <summary>
        /// Gets build callback that was specified during the initialization.
        /// </summary>
        public Func<IList<LogTransformerBase>, ILoggerBuilder> BuildCallback => _buildCallback;
        /// <summary>
        /// The list of log transformers that was created during the build process.
        /// </summary>
        public IList<LogTransformerBase> LogTransformers => _logTransformers;

        /// <summary>
        /// Adds a new log transformer of type <c>TacitusLogger.Transformers.ILogTransformer</c> to the builder.
        /// </summary>
        /// <param name="logTransformer">Log transformer.</param>
        /// <param name="isActive"></param>
        /// <returns>Self.</returns>
        public ILogTransformersBuilder Custom(LogTransformerBase logTransformer, Setting<bool> isActive)
        {
            if (logTransformer == null)
                throw new ArgumentNullException("logTransformer");
            // Set the status.
            logTransformer.SetActive(isActive);
            // Add to list
            _logTransformers.Add(logTransformer);
            return this;
        }
        /// <summary>
        /// Completes the log transformers build process.
        /// </summary>
        /// <returns>Logger builder.</returns>
        public ILoggerBuilder BuildTransformers()
        {
            return _buildCallback(_logTransformers);
        }
    }
}
