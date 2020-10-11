using System.IO;
using System.Xml.Serialization;

namespace SAFT_Reader.Adapter
{
    public class XmlSerializerAdapter : IXmlSerializerAdapter
    {
        public T ConvertXml<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new StringReader(xml));
        }
    }
}