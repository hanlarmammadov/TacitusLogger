using System.ComponentModel;
using TacitusLogger.Destinations;

namespace TacitusLogger
{
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ExtensionMethodsForLogger
    {
        /// <summary>
        /// Creates a new LogGroup in the current logger providing a delegate of type LogModelFunc<bool> as <paramref name="rule"/> and one or several ILogDestination.
        /// </summary> 
        /// <exception cref="System.ArgumentNullException">If <paramref name="rule"/> is null or if no ILogDestination was specified or null array specified or one of specified ILogDestination is null.</exception>
        /// <param name="rule">LogModelFunc<bool> delegate.</param>
        /// <param name="logDestinations">One or more log destinations collection.</param>
        public static void AddLogDestinations(this Logger self, LogModelFunc<bool> rule, params ILogDestination[] logDestinations)
        {
            self.NewLogGroup(rule).AddDestinations(logDestinations);
        }
        /// <summary>
        /// Creates a new LogGroup in the current logger with an "always true" rule.
        /// </summary> 
        /// <exception cref="System.ArgumentNullException">If no ILogDestination was specified or null array specified or one of specified ILogDestination is null.</exception>
        /// <param name="logDestinations">One or more log destinations collection.</param>
        public static void AddLogDestinations(this Logger self, params ILogDestination[] logDestinations)
        {
            self.AddLogDestinations(x => true, logDestinations);
        }
        /// <summary>
        /// Creates a new LogGroup in the current logger with a rule bound to the given <paramref name="context"/>.
        /// </summary> 
        /// <exception cref="System.ArgumentNullException">If no ILogDestination was specified or null array specified or one of specified ILogDestination is null.</exception>
        /// <param name="context">Context of type string that is used in rule for created LogGroup</param>
        /// <param name="logDestinations">One or more log destinations collection.</param>
        public static void AddLogDestinations(this Logger self, string context, params ILogDestination[] logDestinations)
        {
            self.AddLogDestinations(x => x.Context == context, logDestinations);
        }
        /// <summary>
        /// Creates a new LogGroup in the current logger with a rule bound to the given <paramref name="logType"/>.
        /// </summary> 
        /// <param name="logType">Log type of type LogType that is used in rule for created LogGroup</param>
        /// <param name="logDestinations">One or more log destinations collection.</param>
        public static void AddLogDestinations(this Logger self, LogType logType, params ILogDestination[] logDestinations)
        {
            self.AddLogDestinations(x => x.LogType == logType, logDestinations);
        }
        /// <summary>
        /// Creates a new LogGroup in the current logger with a rule bound to Success type logs.
        /// </summary> 
        /// <param name="logDestinations">One or more log destinations collection.</param>
        /// <returns>The Logger itself.</returns>
        public static Logger AddSuccessDestinations(this Logger self, params ILogDestination[] logDestinations)
        {
            self.AddLogDestinations(LogType.Success, logDestinations);
            return self;
        }
        /// <summary>
        /// Creates a new LogGroup in the current logger with a rule bound to Info type logs.
        /// </summary> 
        /// <param name="logDestinations">One or more log destinations collection.</param>
        /// <returns>The Logger itself.</returns>
        public static Logger AddInfoDestinations(this Logger self, params ILogDestination[] logDestinations)
        {
            self.AddLogDestinations(LogType.Info, logDestinations);
            return self;
        }
        /// <summary>
        /// Creates a new LogGroup in the current logger with a rule bound to Event type logs.
        /// </summary> 
        /// <param name="logDestinations">One or more log destinations collection.</param>
        /// <returns>The Logger itself.</returns>
        public static Logger AddEventDestinations(this Logger self, params ILogDestination[] logDestinations)
        {
            self.AddLogDestinations(LogType.Event, logDestinations);
            return self;
        }
        /// <summary>
        /// Creates a new LogGroup in the current logger with a rule bound to Warning type logs.
        /// </summary> 
        /// <param name="logDestinations">One or more log destinations collection.</param>
        /// <returns>The Logger itself.</returns>
        public static Logger AddWarningDestinations(this Logger self, params ILogDestination[] logDestinations)
        {
            self.AddLogDestinations(LogType.Warning, logDestinations);
            return self;
        }
        /// <summary>
        /// Creates a new LogGroup in the current logger with a rule bound to Failure type logs.
        /// </summary> 
        /// <param name="logDestinations">One or more log destinations collection.</param>
        /// <returns>The Logger itself.</returns>
        public static Logger AddFailureDestinations(this Logger self, params ILogDestination[] logDestinations)
        {
            self.AddLogDestinations(LogType.Failure, logDestinations);
            return self;
        }
        /// <summary>
        /// Creates a new LogGroup in the current logger with a rule bound to Error type logs.
        /// </summary> 
        /// <param name="logDestinations">One or more log destinations collection.</param>
        /// <returns>The Logger itself.</returns>
        public static Logger AddErrorDestinations(this Logger self, params ILogDestination[] logDestinations)
        {
            self.AddLogDestinations(LogType.Error, logDestinations);
            return self;
        }
        /// <summary>
        /// Creates a new LogGroup in the current logger with a rule bound to Critical type logs.
        /// </summary> 
        /// <param name="logDestinations">One or more log destinations collection.</param>
        /// <returns>The Logger itself.</returns>
        public static Logger AddCriticalDestinations(this Logger self, params ILogDestination[] logDestinations)
        {
            self.AddLogDestinations(LogType.Critical, logDestinations);
            return self;
        }
    }
}