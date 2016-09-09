namespace Fanex.Athena.Service.Core.Utility
{
    using Interfaces;
    using RSACryptoPad;

    public class RSAEncryptor : IRSAEncryptor
    {
        private readonly EncryptionThread _encryptionThread = new EncryptionThread();

        public string DecryptString(string inputString, int keySize, string privateKeyPath)
        {
            return _encryptionThread.DecryptString(inputString, keySize, privateKeyPath);
        }

        public string EncryptString(string inputString, int keySize, string publicKeyPath)
        {
            return _encryptionThread.EncryptString(inputString, keySize, publicKeyPath);
        }
    }
}