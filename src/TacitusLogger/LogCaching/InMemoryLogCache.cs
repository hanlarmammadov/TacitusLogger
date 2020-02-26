using System;
using System.Text;
using TacitusLogger.Components.Time;
using TacitusLogger.Exceptions;

namespace TacitusLogger.Caching
{
    /// <summary>
    /// Used to manage the cache of log model entities of specified size.
    /// </summary>
    public class InMemoryLogCache : ILogCache
    {
        private readonly int _size;
        private readonly int _cacheTime;
        private readonly object _currentIndexLocker; 
        private ITimeProvider _timeProvider;
        private Int64 _ticksOfNextDeadline;
        private LogModel[] _logModelCollection;
        private int _currentIndex;

        /// <summary>
        /// Creates an instance of <c>TacitusLogger.LogCache</c> using time provider, cache size and optional 
        /// number of milliseconds after which AddToCache method will return the collection irrelevant to its filling. 
        /// </summary> 
        /// <param name="size">Max size of caching collection after reaching which the AddToCache method returns the filled collection and creates a new empty one.</param>
        /// <param name="cacheTime">Max time in milliseconds after creating of caching collection after reaching which the AddToCache method returns the filled collection and creates a new empty one irrelevant to its filling.</param>
        public InMemoryLogCache(int size, int cacheTime = -1)
        { 
            if (size < 1)
                throw new ArgumentException("Invalid cache size");
            if ((cacheTime < -1) || (cacheTime == 0))
                throw new ArgumentException("Invalid cache time value");

            _timeProvider = new SystemTimeProvider();
            _size = size;
            _cacheTime = cacheTime;
            _currentIndexLocker = new Object();
            ResetCache();
        }

        /// <summary>
        /// Gets the cache size that was specified during the initialization.
        /// </summary>
        public int CacheSize => _size;
        /// <summary>
        /// Gets the cache time in milliseconds that was specified during the initialization.
        /// </summary>
        public int CacheTime => _cacheTime;
        /// <summary>
        /// Gets the time provider that was specified during the initialization.
        /// </summary>
        internal ITimeProvider TimeProvider => _timeProvider;
        /// <summary>
        /// Gets the current log model collection.
        /// </summary>
        internal LogModel[] LogModelCollection => _logModelCollection;
        /// <summary>
        /// Gets the current index in the log model collection.
        /// </summary>
        internal int CurrentIndex => _currentIndex;

        /// <summary>
        /// Adds log model entity to the cache. If after adding the cache becomes full or if specified time expired, the old collection 
        /// is returned and a new one is initialized. Otherwise, null is returned.
        /// </summary>
        /// <param name="logModel">log model entity that should be cached.</param>
        /// <returns></returns>
        public LogModel[] AddToCache(LogModel logModel, bool forceToFlush = false)
        {
            try
            {
                LogModel[] collection;
                lock (_currentIndexLocker)
                {
                    _logModelCollection[_currentIndex++] = logModel;
                    if (_currentIndex == _size || _timeProvider.GetLocalTime().Ticks > _ticksOfNextDeadline)
                    {
                        collection = _logModelCollection;
                        if (_currentIndex != _size)
                            Array.Resize<LogModel>(ref collection, _currentIndex);
                        ResetCache();
                    }
                    else
                        collection = null;
                }
                return collection;
            }
            catch (Exception ex)
            {
                throw new LogCacheException("Error when trying to add log model to InMemoryLogCache. See the inner exception.", ex);
            }
        }
        public void ResetTimeProvider(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException("timeProvider");
            ResetCache();
        }
        public void Dispose()
        {

        }
        public override string ToString()
        {
            return new StringBuilder()
                   .AppendLine($"TacitusLogger.Caching.InMemoryLogCache")
                   .AppendLine($"Cache size: {_size.ToString()}")
                   .Append($"Cache time (in milliseconds): {_cacheTime.ToString()}")
                   .Append($"Time provider: {_timeProvider.ToString()}")
                   .ToString();
        } 
        /// <summary>
        /// Creates a new collection and resets the current index and deadline ticks.
        /// </summary>
        private void ResetCache()
        {
            _logModelCollection = new LogModel[_size];
            _currentIndex = 0;
            if (_cacheTime < 0)
                _ticksOfNextDeadline = Int64.MaxValue;
            else
                checked
                {
                    _ticksOfNextDeadline = _timeProvider.GetLocalTime().Ticks + (Int64)_cacheTime * 10_000;
                }
        }
    }
}
