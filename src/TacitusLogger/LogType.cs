
namespace TacitusLogger
{
    /// <summary>
    /// Represents various types of logging event.
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// Informational logs.
        /// </summary>
        Info = 10,
        /// <summary>
        /// Logs representing successful operation.
        /// </summary>
        Success = 11, 
        /// <summary>
        /// Logs representing some unexceptional event occurrence.
        /// </summary>
        Event = 12,

        /// <summary>
        /// Warning logs.
        /// </summary>
        Warning = 20,

        /// <summary>
        /// Error logs.
        /// </summary>
        Error = 30,
        /// <summary>
        ///  Logs representing failure.
        /// </summary>
        Failure = 31, 
        /// <summary>
        ///  Logs representing some critical situations.
        /// </summary>
        Critical = 32
    } 
}
