using System;

namespace TacitusLogger.Components.Time
{
    public interface ITimeProvider
    {
        DateTime GetLocalTime();
        DateTime GetUtcTime();
    }
}