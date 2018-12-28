using System;
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
            finally
            {

            }

        }

        public string ReadString(string path)
        {
            var realPath = Path.Combine(_appSettings.TempFilePath, path);
            var reader = new StreamReader(realPath);

            try
            {
                return reader.ReadToEnd();
            }
            finally
            {
                reader.Close();
                reader.Dispose();
            }

        }

        public void Save(string path, string content)
        {
            var realPath = Path.Combine(_appSettings.TempFilePath, path);
            var stream = new FileStream(realPath, FileMode.CreateNew, FileAccess.Write);
            var writer = new StreamWriter(stream);

            try
            {
                writer.Write(content);
            }
            finally
            {
                writer.Close();
                writer.Dispose();
                stream.Close();
                stream.Dispose();
            }


        }
        public byte[] ReadBytes(string path)
        {
            var realPath = Path.Combine(_appSettings.TempFilePath, path);
            FileStream fsSource = new FileStream(realPath,
           FileMode.Open, FileAccess.Read);
            try
            {
                byte[] bytes = new byte[fsSource.Length];
                int numBytesToRead = (int)fsSource.Length;
                int numBytesRead = 0;
                while (numBytesToRead > 0)
                {
                    // Read may return anything from 0 to numBytesToRead.
                    int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                    // Break when the end of the file is reached.
                    if (n == 0)
                        break;

                    numBytesRead += n;
                    numBytesToRead -= n;
                }

                return bytes;
            }
            finally
            {
                fsSource.Close();
                fsSource.Dispose();
            }

        }
    }
}
