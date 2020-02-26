using System;
using System.Threading.Tasks;

namespace TacitusLogger.Serializers
{
    /// <summary>
    /// Represents an interface which all log serializers should implement.
    /// </summary>
    public interface ILogSerializer : IDisposable
    {
        string Serialize(LogModel logModel); 
    }
}
