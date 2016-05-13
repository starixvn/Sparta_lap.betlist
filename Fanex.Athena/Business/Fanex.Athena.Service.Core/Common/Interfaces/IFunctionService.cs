// -----------------------------------------------------------------------
// <copyright file="IFunctionService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IFunctionService
    {
        IList<Function> Functions { get; }

        IList<Function> GetUserFunctionList(int licenseeId, int userId);

        IList<Function> GetLicenseeFunctionList(int licenseeId);

        IList<Function> GetMasterFunctions(IList<Function> functionList);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Reviewed. Suppression is OK here.")]
        IDictionary<int, IList<Function>> GetSubFunctions(IList<Function> functionList);

        IResultInfo AssignUserFunctions(int licenseeId, int custId, IList<int> functionIdList, int userId, string ip, string langKey);
    }
}