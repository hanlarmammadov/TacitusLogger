using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Destinations;

namespace TacitusLogger.Diagnostics
{
    public abstract class DiagnosticsManagerBase
    {
        protected string _loggerName;
        protected ILogDestination _logDestination;

        public string LoggerName => _loggerName;
        public ILogDestination LogDestination => _logDestination;

        public abstract void WriteToDiagnostics(Log log);
        public abstract Task WriteToDiagnosticsAsync(Log log, CancellationToken cancellationToken = default(CancellationToken));
        public void SetDependencies(ILogDestination logDestination, string loggerName)
        {
            _logDestination = logDestination;
            _loggerName = loggerName;
        }
        protected virtual void SendToDestination(LogModel logModel)
        {
            if (_logDestination != null)
                _logDestination.Send(new LogModel[] { logModel });
        }
        protected virtual Task SendToDestinationAsync(LogModel logModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            logModel.Source = _loggerName;
            if (cancellationToken.IsCancellationRequested)
                return Task.FromCanceled(cancellationToken);
            if (_logDestination != null)
                return _logDestination.SendAsync(new LogModel[] { logModel }, cancellationToken);
            else return Task.CompletedTask;
        }

    }
}
