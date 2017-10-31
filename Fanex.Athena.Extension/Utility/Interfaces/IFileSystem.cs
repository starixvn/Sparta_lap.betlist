namespace Fanex.Athena.Extension.Utility.Interfaces
{
    public interface IFileSystem
    {
        bool Exists(string path);

        bool ViewExists(string relativePath);

        string ReadFile(string path);
    }
}