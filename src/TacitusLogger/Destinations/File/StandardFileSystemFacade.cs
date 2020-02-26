using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TacitusLogger.Destinations.File
{
    /// <summary>
    /// A facade for file system.
    /// This class is internal.
    /// </summary>
    internal class StandardFileSystemFacade : IFileSystemFacade
    {
        /// <summary>
        /// Opens (creates if it does not exist) the file with the specified file path, writes the specified text to it's end and closes the file.
        /// </summary>
        /// <param name="filePath">Full path to the file.</param>
        /// <param name="text">Text to be append to file.</param>
        public void AppendToFile(string filePath, string text)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");
            if (text == null)
                throw new ArgumentNullException("text");
            System.IO.File.AppendAllText(filePath, text);
        }
        /// <summary> 
        /// Asynchronously opens (creates if it does not exist) the file with the specified file path, writes the specified text to it's end and closes the file.
        /// </summary>
        /// <param name="filePath">Full path to the file.</param>
        /// <param name="text">Text to be append to file.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AppendToFileAsync(string filePath, string text, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                await Task.FromCanceled(cancellationToken);
            if (filePath == null)
                throw new ArgumentNullException("filePath");
            if (text == null)
                throw new ArgumentNullException("text");
            using (FileStream fs = new FileStream(filePath, FileMode.Append))
            {
                var bytes = Encoding.UTF8.GetBytes(text);
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}
