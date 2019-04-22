namespace Fanex.Athena.DataAccess.Core.Utilities
{
    public class ConnectionStringDecryptor : IConnectionStringDecryptor
    {
        public string Decrypt(string connectionString)
        {
            return Fanex.Data.Cryptography.DecryptConnectionString(connectionString);
        }
    }
}