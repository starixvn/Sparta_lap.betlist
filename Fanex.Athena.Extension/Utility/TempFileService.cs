using System.IO;
using Fanex.Athena.Extension.Utility.Interfaces;

namespace Fanex.Athena.Extension.Utility
{
    public class TempFileService : ITempFileService
    {
        private readonly IAppSettings _appSettings;

        public TempFileService(IAppSettings appSetting)
        {
            _appSettings = appSetting;
        }

        public void Delete(string path)
        {
            try
            {
                var realPath = Path.Combine(_appSettings.TempFilePath, path);

                if (File.Exists(realPath))
                {
                    File.Delete(realPath);
                }
            }
            catch
            {
                // Ignore it
            }
        }

        public string ReadString(string path)
        {
            var realPath = Path.Combine(_appSettings.TempFilePath, path);
            using (var reader = new StreamReader(realPath))
            {
                return reader.ReadToEnd();
            }
        }

        public void Save(string path, string content)
        {
            var realPath = Path.Combine(_appSettings.TempFilePath, path);
            using (var stream = new FileStream(realPath, FileMode.CreateNew, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(content);
                }
            }
        }

        public byte[] ReadBytes(string path)
        {
            var realPath = Path.Combine(_appSettings.TempFilePath, path);
            using (FileStream fileStream = new FileStream(realPath, FileMode.Open, FileAccess.Read))
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