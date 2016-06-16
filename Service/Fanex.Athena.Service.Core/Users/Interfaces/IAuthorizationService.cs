// -----------------------------------------------------------------------
// <copyright file="IAuthorizationService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Authorization Service class.
    /// </summary>
    public interface IAuthorizationService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        bool HasPermissions(int licenseeId, IEnumerable<int> userFunctionList, string[] permissionKeys);

        bool HasPermissionOnData(int licenseeId, int custId, long transId);
    }
}