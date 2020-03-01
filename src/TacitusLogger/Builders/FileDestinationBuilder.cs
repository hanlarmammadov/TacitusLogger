using System;
using System.ComponentModel;
using TacitusLogger.Destinations.File;
using TacitusLogger.Serializers;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Builds the file log destination and adds it to own log group destination builder.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class FileDestinationBuilder : LogDestinationBuilderBase, IFileDestinationBuilder
    {
        private ILogSerializer _logSerializer;
        private ILogSerializer _filePathGenerator;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Builders.FileDestinationBuilder</c> using its parent 
        /// log group destination builder.
        /// </summary>
        /// <param name="logGroupDestinationsBuilder">Parent log group destinations builder that will be used to complete build process.</param>
        internal FileDestinationBuilder(ILogGroupDestinationsBuilder logGroupDestinationsBuilder)
            : base(logGroupDestinationsBuilder)
        {

        }

        /// <summary>
        /// Gets the log serializer specified during the build.
        /// </summary>
        public ILogSerializer LogSerializer => _logSerializer;
        /// <summary>
        /// Gets the log file path generator specified during the build.
        /// </summary>
        public ILogSerializer LogFilePathGenerator => _filePathGenerator;

        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.ILogSerializer</c>
        /// that will be used to generate log model text representation. If omitted, the default log serializer
        /// of type <c>TacitusLogger.Serializers.ExtendedTemplateLogSerializer</c> with the default template will be added.
        /// </summary>
        /// <param name="logSerializer">Log serializer.</param>
        /// <returns>Self.</returns>
        public IFileDestinationBuilder WithLogSerializer(ILogSerializer logSerializer)
        {
            if (_logSerializer != null)
                throw new InvalidOperationException("Log serializer has already been set");
            _logSerializer = logSerializer ?? throw new ArgumentNullException("logSerializer");
            return this;
        }
        /// <summary>
        /// Adds a custom log file path generator of type <c>TacitusLogger.Serializers.ILogSerializer</c>
        /// that will be used to generate log file path using log model. If omitted, the default log serializer
        /// of type <c>TacitusLogger.Serializers.FilePathTemplateLogSerializer</c> with the default template will be added.
        /// </summary>
        /// <param name="filePathGenerator">Log file path generator.</param>
        /// <returns>Self.</returns>
        public IFileDestinationBuilder WithPath(ILogSerializer filePathGenerator)
        {
            if (_filePathGenerator != null)
                throw new InvalidOperationException("Log file path generator has already been set");
            _filePathGenerator = filePathGenerator ?? throw new ArgumentNullException("filePathGenerator");
            return this;
        }
        /// <summary>
        /// Completes log destination build process by adding it to the parent log group destination builder.
        /// </summary>
        /// <returns>The parent log group destination builder.</returns>
        public ILogGroupDestinationsBuilder Add()
        {
            //Set defaults if nulls
            if (_logSerializer == null)
                _logSerializer = new ExtendedTemplateLogSerializer();
            if (_filePathGenerator == null)
                _filePathGenerator = new FilePathTemplateLogSerializer();

            return AddToLogGroup(new FileDestination(_logSerializer, _filePathGenerator));
        }
    }
}
