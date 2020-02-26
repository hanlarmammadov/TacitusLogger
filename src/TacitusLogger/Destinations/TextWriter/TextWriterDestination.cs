using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Components.Helpers;
using TacitusLogger.Components.Strings;
using TacitusLogger.Exceptions;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.TextWriter
{
    /// <summary>
    /// Destination that writes log model to the <c>System.IO.TextWriter</c> object.
    /// </summary>
    public class TextWriterDestination : ILogDestination
    {
        private readonly ILogSerializer _logSerializer;
        private readonly ITextWriterProvider _textWriterProvider;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.TextWriterDestination</c> using specified log serializer and 
        /// TextWriter provider.
        /// </summary>
        /// <param name="logSerializer">Log serializer</param>
        /// <param name="textWriterProvider">TextWriter provider</param>
        public TextWriterDestination(ILogSerializer logSerializer, ITextWriterProvider textWriterProvider)
        {
            _logSerializer = logSerializer ?? throw new ArgumentNullException("logSerializer");
            _textWriterProvider = textWriterProvider ?? throw new ArgumentNullException("textWriterProvider");
        }
        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Destinations.TextWriterDestination</c> using SimpleTemplateLogSerializer
        /// serializer with the specified template and the specified TestWriter. 
        /// TextWriter provider.
        /// </summary>
        /// <param name="singleLinedTemplate">Template the will be used by SimpleTemplateLogSerializer to serialize log models.</param>
        /// <param name="textWriter">TextWriter that will be written to.</param>
        public TextWriterDestination(string singleLinedTemplate, System.IO.TextWriter textWriter)
            : this(new SimpleTemplateLogSerializer(singleLinedTemplate), new TextWriterProvider(textWriter))
        {

        }

        /// <summary>
        /// Gets the instance of <c>TacitusLogger.Serializers.ILogSerializer</c> that was specified during the initialization.
        /// </summary>
        public ILogSerializer LogSerializer => _logSerializer;
        /// <summary>
        /// Gets the instance of <c>TacitusLogger.Providers.ITextWriterProvider</c> that was specified during the initialization.
        /// </summary>
        public ITextWriterProvider TextWriterProvider => _textWriterProvider;

        /// <summary>
        /// Writes log model to the destination.
        /// </summary>
        /// <param name="logs">Log models collection.</param> 
        public void Send(LogModel[] logs)
        {
            try
            {
                if (logs.Length == 1)
                {
                    // Get TextWriter instance.
                    var textWriter = _textWriterProvider.GetTextWriter(logs[0]) ?? throw new LogDestinationException("TextWriter provider returned null");
                    // Generate and send log text to TextWriter.
                    textWriter.Write(_logSerializer.Serialize(logs[0]) + Environment.NewLine);
                    // Flush the text writer but does not close.
                    textWriter.Flush();
                }
                else
                {
                    // Use caching.
                    var dict = new Dictionary<System.IO.TextWriter, StringBuilder>();
                    for (int i = 0; i < logs.Length; i++)
                    {
                        // Get TextWriter instance.
                        System.IO.TextWriter textWriter = _textWriterProvider.GetTextWriter(logs[i]) ?? throw new LogDestinationException("TextWriter provider returned null");
                        // If dictionary does not contain this text writer, add it with an empty string builder attached to it.
                        dict.TryAdd(textWriter, new StringBuilder());
                        // Add log text to the string builder attached to that writer.
                        dict[textWriter].AppendLine(_logSerializer.Serialize(logs[i]));
                    }
                    // Write logs to TextWriters.
                    foreach (var keyValuePair in dict)
                    {
                        keyValuePair.Key.Write(keyValuePair.Value.ToString());
                        // Flush the text writer but does not close.
                        keyValuePair.Key.Flush();
                    }
                }
            }
            catch (LogDestinationException)
            {
                throw;
            }
            catch (LogSerializerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LogDestinationException("Error when writing logs. See the inner exception", ex);
            }
        }
        /// <summary>
        /// Asynchronously writes log model to the destination.
        /// </summary>
        /// <param name="logs">Log models collection.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendAsync(LogModel[] logs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Check if operation has been canceled.
                if (cancellationToken.IsCancellationRequested)
                    await Task.FromCanceled(cancellationToken);

                if (logs.Length == 1)
                {
                    // Get the TextWriter instance.
                    var textWriter = _textWriterProvider.GetTextWriter(logs[0]) ?? throw new LogDestinationException("TextWriter provider returned null");
                    // Generate and send log text to TextWriter.
                    await textWriter.WriteAsync(_logSerializer.Serialize(logs[0]) + Environment.NewLine);
                    // Flush the text writer but does not close.
                    await textWriter.FlushAsync();
                }
                else
                {
                    // Use caching.
                    var dict = new Dictionary<System.IO.TextWriter, StringBuilder>();
                    for (int i = 0; i < logs.Length; i++)
                    {
                        // Get the TextWriter instance.
                        System.IO.TextWriter textWriter = _textWriterProvider.GetTextWriter(logs[i]) ?? throw new LogDestinationException("TextWriter provider returned null");
                        // If dictionary does not contain this text writer, add it with an empty string builder attached to it.
                        dict.TryAdd(textWriter, new StringBuilder());
                        // Add log text to the string builder attached to that writer.
                        dict[textWriter].AppendLine(_logSerializer.Serialize(logs[i]));
                    }
                    // Write logs to TextWriters.
                    foreach (var keyValuePair in dict)
                    {
                        // Check if operation has been canceled.
                        if (cancellationToken.IsCancellationRequested)
                            await Task.FromCanceled(cancellationToken);

                        await keyValuePair.Key.WriteAsync(keyValuePair.Value.ToString());
                        // Flush the text writer but does not close.
                        await keyValuePair.Key.FlushAsync();
                    }
                }
            }
            catch (LogDestinationException)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (LogSerializerException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LogDestinationException("Error when writing logs. See the inner exception", ex);
            }
        }
        public void Dispose()
        {
            try
            {
                _logSerializer.Dispose();
            }
            catch { }

            try
            {
                _textWriterProvider.Dispose();
            }
            catch { }
        }
        public override string ToString()
        {
            return new StringBuilder()
                        .AppendLine(this.GetType().FullName) 
                        .AppendLine($"Text writer provider: {_textWriterProvider.ToString().AddIndentationToLines()}")  
                        .Append($"Log serializer: {_logSerializer.ToString().AddIndentationToLines()}")
                        .ToString();
        }

    }
}
