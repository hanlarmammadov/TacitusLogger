using System; 

namespace TacitusLogger.Destinations.TextWriter
{
    /// <summary>
    /// Implements the <c>TacitusLogger.Providers.ITextWriterProvider</c> interface by providing <c>System.IO.TextWriter</c>
    /// specified during the initialization.
    /// </summary>
    public class TextWriterProvider : ITextWriterProvider
    {
        private readonly System.IO.TextWriter _textWriter;
        private bool _isDisposed;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Providers.TextWriterProvider</c> using 
        /// instance of <c>System.IO.TextWriter</c> class.
        /// </summary>
        /// <param name="textWriter">Specified instance of <c>System.IO.TextWriter</c> class.</param>
        public TextWriterProvider(System.IO.TextWriter textWriter)
        {
            _textWriter = textWriter ?? throw new ArgumentNullException("textWriter");
        }

        /// <summary>
        /// Gets TextWriter instance provided during the initialization.
        /// </summary>
        internal System.IO.TextWriter TextWriter => _textWriter;

        /// <summary>
        /// Gets the <c>System.IO.TextWriter</c> instance using log model.
        /// </summary>
        /// <param name="logModel">Log model.</param>
        /// <returns>An instance of <c>System.IO.TextWriter</c> class.</returns>
        public System.IO.TextWriter GetTextWriter(LogModel logModel)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("TextWriterProvider");

            return _textWriter;
        }
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _textWriter.Dispose();

            _isDisposed = true;
        }
    }
}
