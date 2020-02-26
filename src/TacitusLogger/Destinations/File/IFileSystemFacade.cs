using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.File
{
    /// <summary>
    /// An interface defining a facade to file system.
    /// This interface is internal.
    /// </summary>
    internal interface IFileSystemFacade
    {
        void AppendToFile(string filePath, string text);
        Task AppendToFileAsync(string filePath, string text, CancellationToken cancellationToken = default(CancellationToken));
    }
}
