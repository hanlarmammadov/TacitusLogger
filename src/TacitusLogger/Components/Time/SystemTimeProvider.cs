using System;

namespace TacitusLogger.Components.Time
{
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTime GetLocalTime()
        {
            return DateTime.Now;
        }
        public DateTime GetUtcTime()
        {
            return DateTime.UtcNow;
        }
    }
}