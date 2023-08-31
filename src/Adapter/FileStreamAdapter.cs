using System.IO;
using System.Text;

namespace SAFT_Reader.Adapter
{
    /// <summary>
    /// Adapter for reading and writing files using file streams.
    /// </summary>
    public class FileStreamAdapter : IFileStreamAdapter
    {
        /// <summary>
        /// Reads the content of a file located at the specified path.
        /// </summary>
        /// <param name="path">The path to the file to be read.</param>
        /// <returns>The content of the file as a string.</returns>
        public string Read(string path)
        {
            return File.ReadAllText(path, Encoding.GetEncoding(1252));
        }

        /// <summary>
        /// Writes content to a file located at the specified path.
        /// </summary>
        /// <param name="path">The path to the file where content will be written.</param>
        /// <param name="content">The content to write to the file.</param>
        public void Write(string path, string content)
        {
            File.WriteAllText(path, content);
        }
    }
}