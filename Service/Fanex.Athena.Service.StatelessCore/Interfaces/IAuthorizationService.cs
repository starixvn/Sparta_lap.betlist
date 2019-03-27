// -----------------------------------------------------------------------
// <copyright file="IAuthorizationService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.StatelessCore.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAuthorizationService
    {
        Task<bool> HasPermissions(IEnumerable<int> userFunctionList, params string[] permissionKeys);

        bool HasPermissions(IEnumerable<string> userFunctionList, params string[] permissionKeys);

        bool HasPermissionOnData(int licenseeId, int custId, long transId);
    }
}