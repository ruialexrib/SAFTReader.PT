using System;
using System.IO;
using System.Text;

namespace SAFT_Reader.Adapter
{
    public class FileStreamAdapter : IFileStreamAdapter
    {
        public string Read(string path)
        {
            string fileReader;
            Encoding encoding = Encoding.GetEncoding(1252);
            fileReader = File.ReadAllText(path, encoding);
            return fileReader;
        }

        public void Write(string path, string content)
        {
            throw new NotImplementedException();
        }
    }
}