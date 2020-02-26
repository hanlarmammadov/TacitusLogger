using System;
using System.Collections.Generic;
using System.ComponentModel;
using TacitusLogger.Contributors;

namespace TacitusLogger.Builders
{
    /// <summary>
    /// Builds a list of log contributors for the given logger builder.
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class LogContributorsBuilder : ILogContributorsBuilder
    {
        private readonly Func<IList<LogContributorBase>, ILoggerBuilder> _buildCallback;
        private readonly IList<LogContributorBase> _logContributors;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.Builders.LogContributorsBuilder</c> using build callback.
        /// </summary>
        /// <param name="buildCallback">The build callback delegate used to complete the log contributors building process.</param>
        public LogContributorsBuilder(Func<IList<LogContributorBase>, ILoggerBuilder> buildCallback)
        {
            _buildCallback = buildCallback;
            _logContributors = new List<LogContributorBase>();
        }

        /// <summary>
        /// Gets build callback that was specified during the initialization.
        /// </summary>
        public Func<IList<LogContributorBase>, ILoggerBuilder> BuildCallback => _buildCallback;
        /// <summary>
        /// The list of log contributors that was created during the build process.
        /// </summary>
        public IList<LogContributorBase> LogContributors => _logContributors;

        /// <summary>
        /// Adds a new log contributor of type <c>TacitusLogger.Contributors.ILogContributor</c> to the builder.
        /// </summary>
        /// <param name="logContributor">Log contributor.</param>
        /// <param name="isActive"></param>
        /// <returns>Self.</returns>
        public ILogContributorsBuilder Custom(LogContributorBase logContributor, Setting<bool> isActive)
        {
            if (logContributor == null)
                throw new ArgumentNullException("logContributor");
            // Set the status.
            logContributor.SetActive(isActive);
            // Add to list
            _logContributors.Add(logContributor);
            return this;
        }
        /// <summary>
        /// Completes the log contributors build process.
        /// </summary>
        /// <returns>Logger builder.</returns>
        public ILoggerBuilder BuildContributors()
        {
            return _buildCallback(_logContributors);
        }
    }
}
