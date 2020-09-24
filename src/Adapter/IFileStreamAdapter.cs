using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAFT_Reader.Adapter
{
    public interface IFileStreamAdapter
    {
        string Read(string path);
        void Write(string path, string content);
    }
}
