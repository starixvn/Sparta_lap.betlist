namespace Fanex.Athena.Extension.Utility
{
    using System.IO;
    using System.Web.Hosting;
    using Interfaces;

    public class FileSystem : IFileSystem
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public bool ViewExists(string relativePath)
        {
            return File.Exists(HostingEnvironment.MapPath(relativePath));
        }

        public string ReadFile(string path)
        {
            using (var reader = new StreamReader(path))
            {
                return reader.ReadToEnd();
            }
        }
    }
}