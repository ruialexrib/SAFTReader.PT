using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
