namespace Fanex.Athena.Extension.Utility
{
    using System.IO;
    using Interfaces;

    public class FileSystem : IFileSystem
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}