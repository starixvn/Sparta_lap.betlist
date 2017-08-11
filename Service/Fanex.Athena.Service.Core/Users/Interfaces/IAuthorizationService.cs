// -----------------------------------------------------------------------
// <copyright file="IAuthorizationService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    using System.Collections.Generic;
    using Model.Core.DataDomain;
    using Model.Core.DTO;

    /// <summary>
    /// Authorization Service class.
    /// </summary>
    public interface IAuthorizationService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        bool HasPermissions(IEnumerable<int> userFunctionList, params string[] permissionKeys);

        bool HasPermissions(IEnumerable<PortalSystemFunction> userFunctionList, params string[] permissionKeys);

        bool HasPermissionOnData(int licenseeId, int custId, long transId);
    }
}