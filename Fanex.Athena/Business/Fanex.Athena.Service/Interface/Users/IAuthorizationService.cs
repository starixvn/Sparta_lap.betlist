// -----------------------------------------------------------------------
// <copyright file="IAuthorizationService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Users
{
    using System.Collections.Generic;

    /// <summary>
    /// Authorization Service class.
    /// </summary>
    public interface IAuthorizationService
    {
        bool AuthorizePath(
            int licenseeId,
            IList<int> userFunctionList,
                           int custId,
                           long transId,
                           string url);

        bool AuthorizePath(int licenseeId, IList<int> userFunctionList, string url);

        bool HasPermissionOnData(int custId, long transId);
    }
}