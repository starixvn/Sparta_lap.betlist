namespace Fanex.Athena.Service.User.Common
{
    using System;
    using System.Configuration;
    using Core.Exception;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using ONEbook.Security;

    public class OneBookSecurityWrapper : IOneBookSecurityWrapper
    {
        public OneAuthenticationToken Authenticate(string token)
        {
            OneAuthenticationToken result = null;
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString_ClearinghouseDB"].ConnectionString;
                var oneBookSecurity = new Auth<OneAuthenticationToken>(connectionString);
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