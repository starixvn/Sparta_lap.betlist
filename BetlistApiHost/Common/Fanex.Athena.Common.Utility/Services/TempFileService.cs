using System.IO;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.Athena.Common.Utility.Services.Internal;

namespace Fanex.Athena.Common.Utility.Services
{
    public class TempFileService : ITempFileService
    {
        private readonly IAppSettings _appSettings;

        private readonly IFileSystem _fileSystem;

        public TempFileService(IAppSettings appSetting, IFileSystem fileSystem)
        {
            _appSettings = appSetting;
            _fileSystem = fileSystem;
        }

        public void Delete(string path)
        {
            try
            {
                var realPath = Path.Combine(_appSettings.TempFilePath, path);

                _fileSystem.Delete(realPath);
            }
            catch
            {
                // Ignore it
            }
        }

        public string ReadString(string path)
        {
            var realPath = Path.Combine(_appSettings.TempFilePath, path);
            return _fileSystem.ReadString(realPath);
        }

        public void Save(string path, string content)
        {
            var realPath = Path.Combine(_appSettings.TempFilePath, path);
            _fileSystem.Save(realPath, content);
        }

        public byte[] ReadBytes(string path)
        {
            var realPath = Path.Combine(_appSettings.TempFilePath, path);
            using (FileStream fileStream = _fileSystem.OpenRead(realPath))
            {
                byte[] bytes = new byte[fileStream.Length];
                int numBytesToRead = (int)fileStream.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = fileStream.Read(bytes, numBytesRead, numBytesToRead);

                    if (n > 0)
                    {
                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                }

                return bytes;
            }
        }
    }
}