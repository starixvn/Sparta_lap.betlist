namespace Fanex.Athena.Service.Core.Users
{
    using System;
    using System.Configuration;
    using Core.Exception;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DTO.OneAuth;
    using ONEbook.Security;

    public class OneBookSecurityWrapper : IOneBookSecurityWrapper
    {
        public OneAuthToken Authenticate(string token)
        {
            OneAuthToken result = null;
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString_ClearinghouseDB"].ConnectionString;
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