// -----------------------------------------------------------------------
// <copyright file="IFunctionDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IFunctionDataAccess
    {
        IList<Function> GetFunctionList();

        IList<Function> GetLicenseeFunctionList(int licenseeId);

        IList<Function> GetUserFunctionList(int licenseeId, int userId);

        Task<IResultInfo> AssignUserFunctionAsync(int licenseeId, int custId, int functionId, int userId, string ip, string langKey);

        Task<IResultInfo> RemoveUserFunctionAsync(int licenseeId, int userId, int custId, string ip, int functionId, string langKey);
    }
}