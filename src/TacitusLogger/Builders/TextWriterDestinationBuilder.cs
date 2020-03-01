using System;
using System.ComponentModel; 
using TacitusLogger.Destinations.TextWriter; 
using TacitusLogger.Serializers;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Builds and adds an instance of <c>TacitusLogger.Destinations.TextWriterDestination</c> class to the <c>TacitusLogger.Builders.ILogGroupDestinationsBuilder</c>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class TextWriterDestinationBuilder : LogDestinationBuilderBase, ITextWriterDestinationBuilder
    {
        private ILogSerializer _logSerializer;
        private ITextWriterProvider _textWriterProvider;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Builders.TextWriterDestination</c> using parent <c>ILogGroupDestinationsBuilder</c> instance.
        /// </summary>
        /// <param name="logGroupDestinationsBuilder">Parent log group destinations builder that will be used to complete build process.</param>
        public TextWriterDestinationBuilder(ILogGroupDestinationsBuilder logGroupDestinationsBuilder)
            : base(logGroupDestinationsBuilder)
        {

        }

        /// <summary>
        /// Gets the log serializer that was specified during the build process.
        /// </summary>
        public ILogSerializer LogSerializer => _logSerializer;
        /// <summary>
        /// Gets the TextWriter provider that was specified during the build process.
        /// </summary>
        public ITextWriterProvider TextWriterProvider => _textWriterProvider;

        /// <summary>
        /// Adds a custom log serializer of type <c>TacitusLogger.Serializers.ILogSerializer</c>
        /// that will be used to generate log model text representation. If omitted, the default log serializer
        /// of type <c>TacitusLogger.Serializers.SimpleTemplateLogSerializer</c> with the default template will be added.
        /// </summary>
        /// <param name="logSerializer"></param>
        /// <returns>Self.</returns>
        public ITextWriterDestinationBuilder WithLogSerializer(ILogSerializer logSerializer)
        {
            if (_logSerializer != null)
                throw new InvalidOperationException("Log serializer has already been set");
            _logSerializer = logSerializer ?? throw new ArgumentNullException("logSerializer");
            return this;
        }
        /// <summary>
        /// Adds a custom TextWriter provider of type <c>TacitusLogger.Providers.ITextWriterProvider</c>
        /// that will be used to get the right TextWriter to write to. This dependency is mandatory, building without providing
        /// it will result in <c>InvalidOperationException</c>.
        /// </summary>
        /// <param name="textWriterProvider"></param>
        /// <returns>Self.</returns>
        public ITextWriterDestinationBuilder WithWriter(ITextWriterProvider textWriterProvider)
        {
            if (_textWriterProvider != null)
                throw new InvalidOperationException("TextWriter provider has already been set");
            _textWriterProvider = textWriterProvider ?? throw new ArgumentNullException("textWriterProvider");
            return this;
        }
        /// <summary>
        /// Completes log destination build process by adding it to the parent log group destination builder.
        /// </summary>
        /// <returns>The parent log group destination builder.</returns>
        public ILogGroupDestinationsBuilder Add()
        {
            // Mandatory dependencies.
            if (_textWriterProvider == null)
                throw new InvalidOperationException("TextWriter provider was not specified during the build process");

            // Set default if null.
            if (_logSerializer == null)
                _logSerializer = new SimpleTemplateLogSerializer();

            return AddToLogGroup(new TextWriterDestination(_logSerializer, _textWriterProvider));
        }
    }
}
