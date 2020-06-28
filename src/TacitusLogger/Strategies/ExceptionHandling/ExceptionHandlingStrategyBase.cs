using System;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Diagnostics;

namespace TacitusLogger.Strategies.ExceptionHandling
{
    public abstract class ExceptionHandlingStrategyBase
    {
        private DiagnosticsManagerBase _diagnosticsManager;

        public abstract bool ShouldRethrow { get; }
        public DiagnosticsManagerBase DiagnosticsManager => _diagnosticsManager;

        public abstract void HandleException(Exception exception, string context);
        public abstract Task HandleExceptionAsync(Exception exception, string context, CancellationToken cancellationToken = default(CancellationToken));
        public void SetDiagnosticsManager(DiagnosticsManagerBase diagnosticsManager)
        {
            _diagnosticsManager = diagnosticsManager??throw new ArgumentNullException("diagnosticsManager");
        }
    }
}
