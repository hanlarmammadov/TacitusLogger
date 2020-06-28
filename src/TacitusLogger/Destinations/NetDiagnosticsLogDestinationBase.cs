using System;
using TacitusLogger.Serializers; 
using System.Threading.Tasks;
using System.Threading;
using TacitusLogger.Exceptions;
using System.Text;
using TacitusLogger.Components.Strings;

namespace TacitusLogger.Destinations
{
    /// <summary>
    /// Base class for log destinations that write to destinations from System.Diagnostics.
    /// </summary>
    public abstract class NetDiagnosticsLogDestinationBase : ILogDestination
    {
        private readonly ILogSerializer _logSerializer;
        private IOutputDeviceFacade _consoleFacade;

        /// <summary>
        /// Initialize base class with log serializer and console facade.
        /// </summary>
        /// <exception cref="ArgumentNullException">If <paramref name="logSerializer"/> or <paramref name="consoleFacade"/> is null.</exception>
        /// <param name="logSerializer">Log serializer.</param>
        /// <param name="consoleFacade">Console facade.</param>
        protected NetDiagnosticsLogDestinationBase(ILogSerializer logSerializer, IOutputDeviceFacade consoleFacade)
        {
            _logSerializer = logSerializer ?? throw new ArgumentNullException("logSerializer");
            _consoleFacade = consoleFacade ?? throw new ArgumentNullException("consoleFacade");
        }

        /// <summary>
        /// Gets the log serializer that was provided during initialization.
        /// </summary>
        public ILogSerializer LogSerializer => _logSerializer;
        /// <summary>
        /// Gets the console facade that was provided during initialization. 
        /// </summary>
        public IOutputDeviceFacade ConsoleFacade => _consoleFacade;

        /// <summary>
        /// Writes log model to the destination.
        /// </summary>
        /// <param name="logs">Log models collection.</param>
        public void Send(LogModel[] logs)
        {
            try
            {
                for (int i = 0; i < logs.Length; i++)
                {
                    //Generate result text
                    string resultStr = _logSerializer.Serialize(logs[i]);
                    //Send to console
                    _consoleFacade.WriteLine(resultStr);
                }
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
        public async Task SendAsync(LogModel[] logs, CancellationToken cancellationToken = default)
        {
            try
            {
                for (int i = 0; i < logs.Length; i++)
                {
                    // Check if operation has been canceled.
                    if (cancellationToken.IsCancellationRequested)
                        await Task.FromCanceled(cancellationToken);

                    //Generate result text
                    string resultStr = _logSerializer.Serialize(logs[i]);
                    if (resultStr == null)
                        throw new Exception("Log serializer returned null for result string");

                    //Send to console
                    await _consoleFacade.WriteLineAsync(resultStr);
                }
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
        public virtual void Dispose()
        {
            _logSerializer.Dispose(); 
        }
        public override string ToString()
        {
            return new StringBuilder()
                       .AppendLine(this.GetType().FullName)
                       .Append($"Log serializer: {_logSerializer.ToString().AddIndentationToLines()}")
                       .ToString();
        }
        /// <summary>
        /// Resets default console facade to provided one.
        /// Mostly for use during testing.
        /// </summary>
        /// <param name="consoleFacade">New console facade</param>
        public void ResetConsoleFacade(IOutputDeviceFacade consoleFacade)
        {
            _consoleFacade = consoleFacade;
        }
    }
}
