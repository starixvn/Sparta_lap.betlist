// -----------------------------------------------------------------------
// <copyright file="IAuthorizationService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.StatelessCore.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Authorization Service class.
    /// </summary>
    public interface IAuthorizationService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        bool HasPermissions(IEnumerable<int> userFunctionList, params string[] permissionKeys);

        bool HasPermissions(IEnumerable<string> userFunctionList, params string[] permissionKeys);

        bool HasPermissionOnData(int licenseeId, int custId, long transId);
    }
}