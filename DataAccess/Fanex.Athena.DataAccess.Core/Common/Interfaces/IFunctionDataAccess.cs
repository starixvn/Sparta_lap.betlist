// -----------------------------------------------------------------------
// <copyright file="IFunctionDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

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