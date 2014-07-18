// -----------------------------------------------------------------------
// <copyright file="IFunctionDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Common
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;

    /// <summary>
    /// Function data access interface.
    /// </summary>
    public interface IFunctionDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<Function> GetFunctionList();

        IList<Function> GetLicenseeFunctionList(int licenseeId);

        IList<Function> GetUserFunctionList(int licenseeId, int userId);

        IResultInfo AssignUserFunction(int licenseeId, int custId, int functionId, int userId, string ip, string langKey);

        IResultInfo RemoveUserFunction(int licenseeId, int userId, int custId, string ip, int functionId, string langKey);
    }
}