namespace Fanex.Athena.Extension.Utility.Interfaces
{
    public interface IFileSystem
    {
        bool Exists(string path);

        string ReadFile(string path);
    }
}