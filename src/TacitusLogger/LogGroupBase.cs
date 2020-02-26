using System;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LogGroupBase: IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        public abstract Setting<LogGroupStatus> Status { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>        
        public abstract bool IsEligible(LogModel log);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public abstract void Send(LogModel log);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task SendAsync(LogModel log, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
