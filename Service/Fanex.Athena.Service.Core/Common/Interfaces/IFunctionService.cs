// -----------------------------------------------------------------------
// <copyright file="IFunctionService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IFunctionService
    {
        IEnumerable<Function> Functions { get; }

        IEnumerable<Function> GetUserFunctionList(int licenseeId, int userId);

        IEnumerable<Function> GetLicenseeFunctionList(int licenseeId);

        IEnumerable<Function> GetMasterFunctions(IEnumerable<Function> functionList);

        IDictionary<int, IEnumerable<Function>> GetSubMenus(IEnumerable<Function> functionList);

        IDictionary<int, IEnumerable<Function>> GetSubFunctions(IEnumerable<Function> functionList);

        Task<IResultInfo> AssignUserFunctions(int licenseeId, int custId, IEnumerable<int> functionIdList, int userId, string ip, string langKey);
    }
}