namespace Fanex.Athena.DataAccess.Core.Utilities
{
    public interface IConnectionStringDecryptor
    {
        string Decrypt(string connectionString);
    }
}