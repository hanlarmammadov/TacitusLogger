using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TacitusLogger.Contributors;
using TacitusLogger.LogIdGenerators;
using TacitusLogger.Strategies.ExceptionHandling;

namespace TacitusLogger.Strategies.LogCreation
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LogCreationStrategyBase
    {
        protected ILogIdGenerator _logIdGenerator;
        protected IList<LogContributorBase> _logContributors;
        protected ExceptionHandlingStrategyBase _exceptionHandlingStrategy;

        public LogCreationStrategyBase()
        {

        }

        /// <summary>
        /// Implementation of ILogIdGenerator that was specified during the initialization.
        /// </summary>
        public ILogIdGenerator LogIdGenerator => _logIdGenerator;
        /// <summary>
        /// Gets the collection of all log contributors that was specified during the initialization.
        /// </summary>
        public IEnumerable<LogContributorBase> LogContributors => _logContributors;
        /// <summary>
        /// Gets the exception handling strategy that was specified during the initialization.
        /// </summary>
        public ExceptionHandlingStrategyBase ExceptionHandlingStrategy => _exceptionHandlingStrategy;

        /// <summary>
        /// Sets main dependencies of the strategy: log id generator of type <c>TacitusLogger.LogIdGenerators.ILogIdGenerator</c> 
        /// and the list of log contributors of type <c>TacitusLogger.Contributors.ILogContributor</c>.
        /// </summary>
        /// <param name="logIdGenerator">Log id generator that will be used to add ids to created log models.</param>
        /// <param name="logContributors">List of log contributors that will be used to add log attachments to created log model.</param>
        public void InitStrategy(ILogIdGenerator logIdGenerator, IList<LogContributorBase> logContributors, ExceptionHandlingStrategyBase exceptionHandlingStrategy)
        {
            _logIdGenerator = logIdGenerator ?? throw new ArgumentNullException("logIdGenerator");
            _logContributors = logContributors ?? throw new ArgumentNullException("logContributors");
            _exceptionHandlingStrategy = exceptionHandlingStrategy;
        }
        /// <summary>
        /// Creates an instance of <c>TacitusLogger.LogModel</c> model using specified log.
        /// </summary>
        /// <param name="log"></param> 
        /// <param name="source">Log source.</param>
        /// <returns>Created log model.</returns>
        public abstract LogModel CreateLogModel(Log log, string source);
        /// <summary>
        /// Asynchronously creates an instance of <c>TacitusLogger.LogModel</c> model using specified log.
        /// </summary>
        /// <param name="log"></param> 
        /// <param name="source">Log source.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Created log model.</returns>
        public abstract Task<LogModel> CreateLogModelAsync(Log log, string source, CancellationToken cancellationToken = default(CancellationToken));
    }
}
