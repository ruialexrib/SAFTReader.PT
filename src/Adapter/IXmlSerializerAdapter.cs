namespace SAFT_Reader.Adapter
{
    public interface IXmlSerializerAdapter
    {
        T ConvertXml<T>(string xml);
    }
}