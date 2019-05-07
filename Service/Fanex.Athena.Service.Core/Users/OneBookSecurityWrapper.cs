namespace Fanex.Athena.Service.Core.Users
{
    using System;
    using System.Configuration;
    using Core.Exception;
    using DataAccess.Core.Utilities;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DTO.OneAuth;
    using ONEbook.Security;

    public class OneBookSecurityWrapper : IOneBookSecurityWrapper
    {
        private readonly IConnectionStringDecryptor _connectionStringDecryptor;

        public OneBookSecurityWrapper(IConnectionStringDecryptor connectionStringDecryptor)
        {
            _connectionStringDecryptor = connectionStringDecryptor;
        }

        public OneAuthToken Authenticate(string token)
        {
            OneAuthToken result = null;
            try
            {
                var encryptedConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString_ClearinghouseDB"].ConnectionString;
                var connectionString = _connectionStringDecryptor.Decrypt(encryptedConnectionString);
                var oneBookSecurity = new Auth<OneAuthToken>(connectionString);
                oneBookSecurity.Authenticate(token, ref result);
            }
            catch (Exception exception)
            {
                throw new ServiceException("Error when calling OneBookSecurity.Authenticate - Error =" + exception.Message, nameof(OneBookSecurityWrapper));
            }

            return result;
        }
    }
}