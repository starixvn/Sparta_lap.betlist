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
            catch (Exception ex)
            {

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
    }
}
