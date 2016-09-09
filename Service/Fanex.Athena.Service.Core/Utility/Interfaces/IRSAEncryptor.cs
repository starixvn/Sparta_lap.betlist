namespace Fanex.Athena.Service.Core.Utility.Interfaces
{
    public interface IRSAEncryptor
    {
        string DecryptString(string inputString, int keySize, string privateKeyPath);

        string EncryptString(string inputString, int keySize, string publicKeyPath);
    }
}