using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Components.Helpers;
using TacitusLogger.Components.Strings;
using TacitusLogger.Exceptions;
using TacitusLogger.Serializers;

namespace TacitusLogger.Destinations.File
{
    /// <summary>
    /// Destination that writes log model to file system.
    /// </summary>
    public class FileDestination : ILogDestination
    {
        private readonly ILogSerializer _logSerializer;
        private readonly ILogSerializer _filePathGenerator;
        private IFileSystemFacade _fileSystemFacade;

        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.FileDestination class using 
        /// log serializer and log file path generator.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logSerializer"/> or <paramref name="filePathGenerator"/> is null.</exception>
        /// <param name="logSerializer">Log serializer.</param>
        /// <param name="filePathGenerator">Log file path generator.</param>
        public FileDestination(ILogSerializer logSerializer, ILogSerializer filePathGenerator)
        {
            this._logSerializer = logSerializer ?? throw new ArgumentNullException("logSerializer");
            this._filePathGenerator = filePathGenerator ?? throw new ArgumentNullException("filePathGenerator");
        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.FileDestination class using 
        ///  log string template, custom json serializer settings and log file path generator.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logStringTemplate"/> or <paramref name="filePathGenerator"/> is null.</exception>
        /// <param name="logStringTemplate">Log string template.</param>
        /// <param name="jsonSerializerSettings">Json serializer settings.</param>
        /// <param name="filePathGenerator">Log file path generator.</param>
        public FileDestination(string logStringTemplate, JsonSerializerSettings jsonSerializerSettings, ILogSerializer filePathGenerator)
            : this(new ExtendedTemplateLogSerializer(logStringTemplate, jsonSerializerSettings), filePathGenerator)
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.FileDestination class using 
        /// log string template and log file path generator.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logStringTemplate"/> or <paramref name="filePathGenerator"/> is null.</exception>
        /// <param name="logStringTemplate">Log string template.</param>
        /// <param name="filePathGenerator">Log file path generator.</param>
        public FileDestination(string logStringTemplate, ILogSerializer filePathGenerator)
            : this(new ExtendedTemplateLogSerializer(logStringTemplate), filePathGenerator)
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.FileDestination class using log file path generator.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="filePathGenerator"/> is null.</exception>
        /// <param name="filePathGenerator">Log file path generator.</param>
        public FileDestination(ILogSerializer filePathGenerator)
             : this(new ExtendedTemplateLogSerializer(), filePathGenerator)
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.FileDestination class using 
        /// log serializer and log file path generator.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logSerializer"/> or <paramref name="filePathGenerator"/> is null.</exception>
        /// <param name="logSerializer">Log serializer.</param>
        /// <param name="filePathTemplate">File path template.</param>
        public FileDestination(ILogSerializer logSerializer, string filePathTemplate)
            : this(logSerializer, new FilePathTemplateLogSerializer(filePathTemplate))
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.FileDestination class using 
        /// log string template, custom json serializer settings and log file path template.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logStringTemplate"/> or <paramref name="filePathTemplate"/> is null.</exception>
        /// <param name="logStringTemplate">Log string template.</param>
        /// <param name="jsonSerializerSettings">Json serializer settings.</param>
        /// <param name="filePathTemplate">Log file path template.</param>
        public FileDestination(string logStringTemplate, JsonSerializerSettings jsonSerializerSettings, string filePathTemplate)
             : this(new ExtendedTemplateLogSerializer(logStringTemplate, jsonSerializerSettings), new FilePathTemplateLogSerializer(filePathTemplate))
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.FileDestination class using 
        /// log string template and log file path template.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logStringTemplate"/> or <paramref name="filePathTemplate"/> is null.</exception>
        /// <param name="logStringTemplate">Log string template.</param> 
        /// <param name="filePathTemplate">Log file path template.</param>
        public FileDestination(string logStringTemplate, string filePathTemplate)
         : this(new ExtendedTemplateLogSerializer(logStringTemplate), new FilePathTemplateLogSerializer(filePathTemplate))
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.FileDestination class using 
        /// log file path template.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="filePathTemplate"/> is null.</exception>
        /// <param name="filePathTemplate">Log file path template.</param>
        public FileDestination(string filePathTemplate)
            : this(new ExtendedTemplateLogSerializer(), new FilePathTemplateLogSerializer(filePathTemplate))
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.FileDestination class using 
        /// log string factory method and default log file path template.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logStringFactoryMethod"/> or <paramref name="filePathTemplate"/> is null.</exception>
        /// <param name="logStringFactoryMethod">Log string factory method.</param>
        /// <param name="filePathTemplate">Log file path template.</param>
        public FileDestination(LogModelFunc<string> logStringFactoryMethod, string filePathTemplate)
            : this(new GeneratorFunctionLogSerializer(logStringFactoryMethod), new FilePathTemplateLogSerializer(filePathTemplate))
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.FileDestination class using 
        /// log string template and file path factory method.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logStringTemplate"/> or <paramref name="filePathFactoryMethod"/> is null.</exception>
        /// <param name="logStringTemplate">Log string template.</param>
        /// <param name="filePathFactoryMethod">File path factory method.</param>
        public FileDestination(string logStringTemplate, LogModelFunc<string> filePathFactoryMethod)
        : this(new ExtendedTemplateLogSerializer(logStringTemplate), new GeneratorFunctionLogSerializer(filePathFactoryMethod))
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.FileDestination class using 
        /// log string template, json serializer settings and file path factory method.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logStringTemplate"/> or <paramref name="filePathFactoryMethod"/> is null.</exception>
        /// <param name="logStringTemplate">Log string template.</param>
        /// <param name="jsonSerializerSettings">Json serializer settings.</param>
        /// <param name="filePathFactoryMethod">File path factory method.</param>
        public FileDestination(string logStringTemplate, JsonSerializerSettings jsonSerializerSettings, LogModelFunc<string> filePathFactoryMethod)
          : this(new ExtendedTemplateLogSerializer(logStringTemplate, jsonSerializerSettings), new GeneratorFunctionLogSerializer(filePathFactoryMethod))
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.FileDestination class using 
        /// log string factory method and file path factory method.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logStringFactoryMethod"/> or <paramref name="filePathFactoryMethod"/> is null.</exception>
        /// <param name="logStringFactoryMethod">Log string factory method</param>
        /// <param name="filePathFactoryMethod">File path factory method</param>
        public FileDestination(LogModelFunc<string> logStringFactoryMethod, LogModelFunc<string> filePathFactoryMethod)
        : this(new GeneratorFunctionLogSerializer(logStringFactoryMethod), new GeneratorFunctionLogSerializer(filePathFactoryMethod))
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.FileDestination class using 
        /// default log serializer and default log file path generator.
        /// </summary>
        public FileDestination()
           : this(new ExtendedTemplateLogSerializer(), new FilePathTemplateLogSerializer())
        {

        }

        /// <summary>
        /// Gets the file system facade that was provided during initialization. 
        /// </summary>
        internal IFileSystemFacade FileSystemFacade => _fileSystemFacade;
        /// <summary> 
        /// Gets the log serializer that was provided during initialization.
        /// </summary>
        public ILogSerializer LogSerializer => _logSerializer;
        /// <summary>
        /// Gets the log file path generator that was provided during initialization. 
        /// </summary>
        public ILogSerializer LogFilePathGenerator => _filePathGenerator;

        /// <summary>
        /// Writes log model to the destination.
        /// </summary>
        /// <param name="logs">Log model collection.</param> 
        public void Send(LogModel[] logs)
        {
            try
            {
                if (logs.Length == 1)
                    _fileSystemFacade.AppendToFile(_filePathGenerator.Serialize(logs[0]) ?? throw new LogDestinationException("Log file path generator returned null"),
                                                   _logSerializer.Serialize(logs[0]) + Environment.NewLine);
                else
                {
                    // Use caching.
                    var dict = new Dictionary<string, StringBuilder>();
                    for (int i = 0; i < logs.Length; i++)
                    {
                        var filePath = _filePathGenerator.Serialize(logs[i]) ?? throw new LogDestinationException("Log file path generator returned null");
                        // If dictionary does not contain this file path, add it with an empty string builder attached to it.
                        dict.TryAdd(filePath, new StringBuilder());
                        // Add generated log text to the string builder attached to that file path.
                        dict[filePath].AppendLine(_logSerializer.Serialize(logs[i]));
                    }
                    // Write logs to files.
                    foreach (var keyValuePair in dict)
                        _fileSystemFacade.AppendToFile(keyValuePair.Key, keyValuePair.Value.ToString());
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
        /// <param name="logs">Log model collection.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendAsync(LogModel[] logs, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                // Check if operation has been canceled.
                if (cancellationToken.IsCancellationRequested)
                    await Task.FromCanceled(cancellationToken);

                if (logs.Length == 1)
                    await _fileSystemFacade.AppendToFileAsync(_filePathGenerator.Serialize(logs[0]) ?? throw new LogDestinationException("Log file path generator returned null"),
                                                              _logSerializer.Serialize(logs[0]) + Environment.NewLine);
                else
                {
                    // Use caching.
                    var dict = new Dictionary<string, StringBuilder>();
                    for (int i = 0; i < logs.Length; i++)
                    {
                        var filePath = _filePathGenerator.Serialize(logs[i]) ?? throw new LogDestinationException("Log file path generator returned null");
                        // If dictionary does not contain this file path, add it with an empty string builder attached to it.
                        dict.TryAdd(filePath, new StringBuilder());
                        // Add generated log text to the string builder attached to that file path.
                        dict[filePath].AppendLine(_logSerializer.Serialize(logs[i]));
                    }
                    // Write logs to files.
                    foreach (var keyValuePair in dict)
                    {
                        // Check if operation has been canceled.
                        if (cancellationToken.IsCancellationRequested)
                            await Task.FromCanceled(cancellationToken);
                        await _fileSystemFacade.AppendToFileAsync(keyValuePair.Key, keyValuePair.Value.ToString());
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
                _filePathGenerator.Dispose();
            }
            catch { }
        }
        public override string ToString()
        {
            return new StringBuilder()
                        .AppendLine(this.GetType().FullName) 
                        .AppendLine($"Path generator: {_filePathGenerator.ToString().AddIndentationToLines()}") 
                        .Append($"Log serializer: {_logSerializer.ToString().AddIndentationToLines()}")
                        .ToString();
        }
        /// <summary>
        /// Resets default file system facade to provided one.
        /// Mostly for use during testing.
        /// </summary>
        /// <param name="consoleFacade">New file system facade</param>
        internal void ResetFileSystemFacade(IFileSystemFacade fileSystemFacade)
        {
            _fileSystemFacade = fileSystemFacade;
        }
    }
}
