namespace SAFT_Reader.Adapter
{
    public interface IFileStreamAdapter
    {
        string Read(string path);

        void Write(string path, string content);
    }
}