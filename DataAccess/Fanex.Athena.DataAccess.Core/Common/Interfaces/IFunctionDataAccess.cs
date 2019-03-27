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
        Task<IList<Function>> GetFunctionList();

        Task<IList<Function>> GetLicenseeFunctionList(int licenseeId);

        Task<IList<Function>> GetUserFunctionList(int licenseeId, int userId);

        IResultInfo AssignUserFunction(int licenseeId, int custId, int functionId, int userId, string ip, string langKey);

        IResultInfo RemoveUserFunction(int licenseeId, int userId, int custId, string ip, int functionId, string langKey);
    }
}