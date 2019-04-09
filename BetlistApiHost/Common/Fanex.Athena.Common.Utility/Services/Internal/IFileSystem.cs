using System.IO;

namespace Fanex.Athena.Common.Utility.Services.Internal
{
    public interface IFileSystem
    {
        void Delete(string path);

        FileStream OpenRead(string path);

        string ReadString(string path);

        void Save(string path, string content);
    }
}