using System;

namespace TacitusLogger.Caching
{
    public interface ILogCache : IDisposable
    {
        LogModel[] AddToCache(LogModel logModel, bool forceToFlush = false);
    }
}
