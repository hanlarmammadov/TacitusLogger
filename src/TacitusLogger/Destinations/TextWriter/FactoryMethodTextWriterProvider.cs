using System;
using System.IO;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.TextWriter
{ 
    /// <summary>
    /// Implements the <c>TacitusLogger.Providers.ITextWriterProvider</c> interface by providing <c>System.IO.TextWriter</c>
    /// using special factory method specified during the initialization.
    /// </summary>
    public class FactoryMethodTextWriterProvider : ITextWriterProvider
    {
        private readonly LogModelFunc<System.IO.TextWriter> _factoryMethod;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Providers.FactoryMethodTextWriterProvider</c> using a factory method of 
        /// type <c>TacitusLogger.LogModelFunc<TextWriter></c>
        /// </summary>
        /// <param name="factoryMethod">The factory method</param>
        public FactoryMethodTextWriterProvider(LogModelFunc<System.IO.TextWriter> factoryMethod)
        {
            _factoryMethod = factoryMethod ?? throw new ArgumentNullException("factoryMethod");
        }

        /// <summary>
        /// Gets the factory method specified during the initialization.
        /// </summary>
        public LogModelFunc<System.IO.TextWriter> FactoryMethod => _factoryMethod;

        /// <summary>
        /// Gets the <c>System.IO.TextWriter</c> instance using log model.
        /// </summary>
        /// <param name="logModel">Log model.</param>
        /// <returns>An instance of <c>System.IO.TextWriter</c> class.</returns>
        public System.IO.TextWriter GetTextWriter(LogModel logModel)
        {
            return _factoryMethod(logModel);
        }
        public void Dispose()
        {

        } 
    }
}
