using System.IO;
using System.Xml.Serialization;

namespace SAFT_Reader.Adapter
{
    /// <summary>
    /// Adapter for converting XML data to objects and vice versa using XML serialization.
    /// </summary>
    public class XmlSerializerAdapter : IXmlSerializerAdapter
    {
        /// <summary>
        /// Converts XML data into an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="xml">The XML data to be deserialized.</param>
        /// <returns>An object of the specified type deserialized from the XML data.</returns>
        public T ConvertXml<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new StringReader(xml));
        }
    }
}