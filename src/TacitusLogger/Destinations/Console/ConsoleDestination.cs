using System;
using System.Collections.Generic;
using TacitusLogger.Serializers;
using System.Threading.Tasks;
using System.Threading;
using TacitusLogger.Exceptions;
using System.Text;
using TacitusLogger.Components.Strings;

namespace TacitusLogger.Destinations.Console
{
    /// <summary>
    /// Destination that writes log model to the standard output device.
    /// </summary>
    public class ConsoleDestination : ILogDestination
    {
        private readonly ILogSerializer _logSerializer;
        private readonly IDictionary<LogType, ConsoleColor> _colorScheme;
        private IColoredOutputDeviceFacade _consoleFacade;
        private bool _isDisposed;

        /// <summary>     
        /// Initializes a new instance of the TacitusLogger.Destinations.ConsoleDestination class using 
        /// log serializer and custom color scheme.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logSerializer"/> or <paramref name="colorScheme"/> is null.</exception>
        /// <param name="logSerializer">Log serializer.</param>
        /// <param name="colorScheme">Color scheme.</param>
        public ConsoleDestination(ILogSerializer logSerializer, IDictionary<LogType, ConsoleColor> colorScheme)
        {
            this._logSerializer = logSerializer ?? throw new ArgumentNullException("logSerializer");
            this._colorScheme = colorScheme ?? throw new ArgumentNullException("colorScheme");
            this._consoleFacade = new StandardColorConsoleFacade();
        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.ConsoleDestination class using 
        /// log serializer.
        /// </summary> 
        /// <exception cref="ArgumentNullException">If <paramref name="logSerializer"/> is null.</exception>
        /// <param name="logSerializer">Log serializer.</param>
        public ConsoleDestination(ILogSerializer logSerializer)
            : this(logSerializer, GetDefaultColorScheme())
        {

        } 
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.ConsoleDestination class using 
        /// log string template.
        /// </summary> 
        /// <exception cref="ArgumentNullException">If <paramref name="logStringTemplate"/> is null.</exception>
        /// <param name="logStringTemplate">Log string template.</param>
        public ConsoleDestination(string logStringTemplate)
             : this(new SimpleTemplateLogSerializer(logStringTemplate), GetDefaultColorScheme())
        {

        }
        /// <summary>        
        /// Initializes a new instance of the TacitusLogger.Destinations.ConsoleDestination class using 
        /// log string template and default color scheme.
        /// </summary> 
        /// <exception cref="ArgumentNullException">If <paramref name="logStringTemplate"/> or <paramref name="colorScheme"/> is null.</exception>
        /// <param name="logStringTemplate">Log string template.</param>
        /// <param name="colorScheme">Color scheme.</param>
        public ConsoleDestination(string logStringTemplate, Dictionary<LogType, ConsoleColor> colorScheme)
             : this(new SimpleTemplateLogSerializer(logStringTemplate), colorScheme)
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.ConsoleDestination class using 
        /// log string factory method.
        /// </summary> 
        /// <exception cref="ArgumentNullException">If <paramref name="logStringFactoryMethod"/> is null.</exception>
        /// <param name="logStringFactoryMethod">Log string factory method.</param>
        public ConsoleDestination(LogModelFunc<string> logStringFactoryMethod)
         : this(new GeneratorFunctionLogSerializer(logStringFactoryMethod), GetDefaultColorScheme())
        {

        }
        /// <summary>
        /// Initializes a new instance of the TacitusLogger.Destinations.ConsoleDestination class using 
        /// log string factory method and default color scheme.
        /// </summary> 
        /// <exception cref="ArgumentNullException">If <paramref name="logStringFactoryMethod"/> or <paramref name="colorScheme"/> is null.</exception>
        /// <param name="logStringFactoryMethod">Log string factory method.</param>
        /// <param name="colorScheme">Color scheme.</param>
        public ConsoleDestination(LogModelFunc<string> logStringFactoryMethod, Dictionary<LogType, ConsoleColor> colorScheme)
           : this(new GeneratorFunctionLogSerializer(logStringFactoryMethod), colorScheme)
        {

        }
        /// <summary> 
        /// Initializes a new instance of the TacitusLogger.Destinations.ConsoleDestination class.
        /// </summary>
        public ConsoleDestination()
             : this(new SimpleTemplateLogSerializer(), GetDefaultColorScheme())
        {

        }

        /// <summary>
        /// Gets the log serializer that was provided during initialization.
        /// </summary>
        public ILogSerializer LogSerializer => _logSerializer;
        /// <summary>
        /// Gets the color scheme.
        /// </summary>
        public IDictionary<LogType, ConsoleColor> ColorScheme => _colorScheme;
        /// <summary>
        /// Gets the console facade that was provided during initialization. 
        /// </summary>
        internal IColoredOutputDeviceFacade ConsoleFacade => _consoleFacade;

        /// <summary>
        /// Get the default color scheme.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<LogType, ConsoleColor> GetDefaultColorScheme()
        { 
            Dictionary<LogType, ConsoleColor> colorScheme = new Dictionary<LogType, ConsoleColor>()
            {
                { LogType.Success, ConsoleColor.DarkGreen },
                { LogType.Info, ConsoleColor.DarkCyan },
                { LogType.Event, ConsoleColor.Cyan },
                { LogType.Warning, ConsoleColor.DarkYellow },
                { LogType.Failure, ConsoleColor.DarkMagenta },
                { LogType.Error, ConsoleColor.Red },
                { LogType.Critical, ConsoleColor.DarkRed }
            };
            return colorScheme;
        }
        /// <summary>
        /// Writes log model to the destination.
        /// </summary>
        /// <param name="logs">Log models collection.</param>
        public void Send(LogModel[] logs)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("ConsoleDestination");

            try
            {
                for (int i = 0; i < logs.Length; i++)
                {
                    //Generate result text
                    string resultStr = _logSerializer.Serialize(logs[i]);

                    //Send to console
                    _consoleFacade.WriteLine(resultStr, GetConsoleColor(logs[i].LogType));
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
            if (_isDisposed)
                throw new ObjectDisposedException("ConsoleDestination");

            try
            {
                for (int i = 0; i < logs.Length; i++)
                {
                    // Check if operation has been canceled.
                    if (cancellationToken.IsCancellationRequested)
                        await Task.FromCanceled(cancellationToken);

                    //Generate result text
                    string resultStr = _logSerializer.Serialize(logs[i]);

                    //Send to console
                    await _consoleFacade.WriteLineAsync(resultStr, GetConsoleColor(logs[i].LogType));
                }
            }
            catch (LogDestinationException)
            {
                throw;
            }
            catch(OperationCanceledException)
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
            if (_isDisposed)
                return;

            _logSerializer.Dispose();

            _isDisposed = true;
        }
        public override string ToString()
        { 
            return new StringBuilder()
                        .AppendLine(this.GetType().FullName)
                        .AppendLine($"Log serializer: {_logSerializer.ToString().AddIndentationToLines()}")
                        .Append($"Color Scheme: {GetColorSchemeDescription().AddIndentationToLines()}")
                        .ToString(); 
        }
        /// <summary>
        /// Resets default console facade to provided one.
        /// Mostly for use during testing.
        /// </summary>
        /// <param name="consoleFacade">New console facade</param>
        internal void ResetConsoleFacade(IColoredOutputDeviceFacade consoleFacade)
        {
            _consoleFacade = consoleFacade;
        }
        protected ConsoleColor GetConsoleColor(LogType logType)
        {
            ConsoleColor color;
            if (_colorScheme.TryGetValue(logType, out color))
                return color;
            else
                return ConsoleColor.White;
        }
        private string GetColorSchemeDescription()
        {
            return $"[Info: {_colorScheme[LogType.Info]} | Success: {_colorScheme[LogType.Success]} | Event: {_colorScheme[LogType.Event]} | Warning: {_colorScheme[LogType.Warning]} | Error: {_colorScheme[LogType.Error]} | Failure: {_colorScheme[LogType.Failure]} | Critical: {_colorScheme[LogType.Critical]}]";
        }
    }
}
