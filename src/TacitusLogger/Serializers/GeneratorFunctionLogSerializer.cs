using System;
using TacitusLogger.Exceptions;

namespace TacitusLogger.Serializers
{
    /// <summary>
    /// Implements log model serialization by means of a custom <c>LogModelFunc<string></c> delegate. 
    /// This delegate is expected to take LogModel model and to return serialized string. Its logic is defined by user. 
    /// </summary>
    public class GeneratorFunctionLogSerializer : ILogSerializer
    {
        private readonly LogModelFunc<string> _generatorFunction;
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Serializers.GeneratorFunctionLogSerializer
        /// class using the specified <c>LogModelFunc<string></c> delegate.
        /// </summary>
        /// <exception cref="ArgumentNullException">If specified <paramref name="generatorFunction"/> is null</exception>
        /// <param name="generatorFunction">Delegate of type <c>LogModelFunc<string></c></param>
        public GeneratorFunctionLogSerializer(LogModelFunc<string> generatorFunction)
        {
            _generatorFunction = generatorFunction ?? throw new ArgumentNullException("generatorFunction");
        }


        /// <summary>
        /// Gets <c>LogModelFunc<string></c> specified on <c>GeneratorFunctionLogSerializer</c> initialization
        /// </summary>
        public LogModelFunc<string> GeneratorFunction => _generatorFunction;


        /// <summary>
        /// Serializes provided <paramref name="logModel"/> to string using specified template.
        /// </summary>
        /// <param name="logModel">Log model to be serialized.</param>
        /// <returns>Resulting string.</returns>
        public string Serialize(LogModel logModel)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("StandardLogCreationStrategy");

            try
            {
                return _generatorFunction(logModel);
            }
            catch (Exception ex)
            {
                throw new LogSerializerException("Error in provided generator function. See the inner exception.", ex);
            }
        }
        public void Dispose()
        {
            _isDisposed = true;
        }
    }
}
