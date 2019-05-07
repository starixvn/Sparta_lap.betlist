// -----------------------------------------------------------------------
// <copyright file="FunctionService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Resources;

    /// <summary>
    /// Function service class.
    /// </summary>
    public class FunctionService : IFunctionService
    {
        private readonly IFunctionDataAccess _functionDataAccess;

        private readonly IFunctionStorage _functionStorage;

        public FunctionService(IFunctionDataAccess functionDataAccess, IFunctionStorage functionStorage)
        {
            _functionDataAccess = functionDataAccess;
            _functionStorage = functionStorage;
        }

        #region Property

        public IEnumerable<Function> Functions
        {
            get
            {
                return _functionStorage.FunctionList;
            }
        }

        private IEnumerable<Function> MasterFunctions
        {
            get
            {
                return _functionStorage.MasterFunctions;
            }
        }

        private IDictionary<int, IEnumerable<Function>> SubFunctions
        {
            get
            {
                return _functionStorage.SubFunctions;
            }
        }

        #endregion Property

        #region Licensee Function

        public IEnumerable<Function> GetLicenseeFunctionList(int licenseeId)
        {
            var functionIdList = _functionDataAccess.GetLicenseeFunctionList(licenseeId).Select(i => i.FunctionId);
            return Functions.Where(f => functionIdList.Contains(f.FunctionId)).ToList();
        }

        public IEnumerable<Function> GetUserFunctionList(int licenseeId, int userId)
        {
            var userFunctionIdData = _functionDataAccess.GetUserFunctionList(licenseeId, userId);
            var licenseeFunctionData = _functionDataAccess.GetLicenseeFunctionList(licenseeId);

            var userFunctionIdList = userFunctionIdData.AsEnumerable().Select(f => f.FunctionId);
            var licenseeFunctionIdList = licenseeFunctionData.AsEnumerable().Select(f => f.FunctionId);

            var functionIdList = licenseeFunctionIdList.Where(l => userFunctionIdList.Any(u => u == l)).ToList();
            var functionList = Functions.Where(f => functionIdList.Contains(f.FunctionId)).ToList();

            foreach (var masterFunction in MasterFunctions)
            {
                if (functionList.Any(f => f.FunctionId == masterFunction.FunctionId))
                {
                    continue;
                }

                var subFunctionCount = SubFunctions[masterFunction.FunctionId].Count();
                var userSubFunctionCount = functionList.Count(f => f.ParentId == masterFunction.FunctionId);

                // User has permission on all sub functions, so he/she has permission on parent function
                if (subFunctionCount == userSubFunctionCount)
                {
                    functionList.Add(masterFunction);
                }
            }

            return functionList;
        }

        public async Task<IResultInfo> AssignUserFunctions(int licenseeId, int custId, IEnumerable<int> functionIdList, int userId, string ip, string langKey)
        {
            var correctFunctionIdList = functionIdList.Where(f => Functions.Any(l => l.FunctionId == f)).ToList();
            var userFunctionList = GetUserFunctionList(licenseeId, custId).Select(f => f.FunctionId);
            var addingIdList = correctFunctionIdList.Except(userFunctionList);
            var removingIdList = userFunctionList.Except(correctFunctionIdList);

            var addingResult = await AddUserToFunction(licenseeId, custId, userId, ip, langKey, addingIdList).ConfigureAwait(false);

            var removingResult = await RemoveUserFromFunction(licenseeId, custId, userId, ip, langKey, removingIdList).ConfigureAwait(false);

            if (!addingResult.IsSuccessful)
            {
                if (removingResult.IsSuccessful)
                {
                    removingResult = addingResult;
                }
                else
                {
                    removingResult.Message += " " + addingResult.Message;
                }
            }

            return removingResult;
        }

        #endregion Licensee Function

        #region Utilities

        /// <summary>
        ///     Get all master functions which contain any function in function list.
        /// </summary>
        /// <param name="functionList">The function list.</param>
        /// <returns>The master function.</returns>
        public IEnumerable<Function> GetMasterFunctions(IEnumerable<Function> functionList)
        {
            return MasterFunctions.Where(
                    f => functionList.Any(l => l.ParentId == f.FunctionId || l.FunctionId == f.FunctionId))
                                .OrderBy(f => f.DisplayOrder)
                                .ToList();
        }

        public IDictionary<int, IEnumerable<Function>> GetSubMenus(IEnumerable<Function> functionList)
        {
            var detailFunctions = new Dictionary<int, IEnumerable<Function>>();
            foreach (var function in MasterFunctions)
            {
                var subFuntions = functionList.Where(f => f.ParentId == function.FunctionId && f.IsMenu)
                                              .OrderBy(f => f.DisplayOrder)
                                              .ToList();
                if (subFuntions.Count > 0)
                {
                    detailFunctions.Add(function.FunctionId, subFuntions);
                }
            }

            return detailFunctions;
        }

        public IDictionary<int, IEnumerable<Function>> GetSubFunctions(IEnumerable<Function> functionList)
        {
            var detailFunctions = new Dictionary<int, IEnumerable<Function>>();
            foreach (var function in MasterFunctions)
            {
                var subFuntions = functionList.Where(f => f.ParentId == function.FunctionId && f.IsFunction)
                                              .OrderBy(f => f.DisplayOrder)
                                              .ToList();
                if (subFuntions.Count > 0)
                {
                    detailFunctions.Add(function.FunctionId, subFuntions);
                }
            }

            return detailFunctions;
        }

        #endregion Utilities

        private async Task<IResultInfo> AddUserToFunction(
            int licenseeId,
            int custId,
            int userId,
            string ip,
            string langKey,
            IEnumerable<int> addingIdList)
        {
            IResultInfo addedResultInfo = new ResultInfo
            {
                Code = 0,
                Message = ViewResources.UpdateFunctionSuccessfulMsg
            };

            var failedIdList = new StringBuilder();

            foreach (var function in addingIdList)
            {
                var tempResult = await _functionDataAccess.AssignUserFunctionAsync(licenseeId, custId, function, userId, ip, langKey).ConfigureAwait(false);

                if (!tempResult.IsSuccessful)
                {
                    failedIdList.Append(function.ToString());
                    failedIdList.Append(",");
                    addedResultInfo = tempResult;
                }
            }

            if (!addedResultInfo.IsSuccessful)
            {
                addedResultInfo.Message += " [" + failedIdList.ToString().TrimEnd(',') + "];";
            }

            return addedResultInfo;
        }

        private async Task<IResultInfo> RemoveUserFromFunction(
            int licenseeId,
            int custId,
            int userId,
            string ip,
            string langKey,
            IEnumerable<int> removingIdList)
        {
            IResultInfo deletedResultInfo = new ResultInfo
            {
                Code = 0,
                Message = ViewResources.UpdateFunctionSuccessfulMsg
            };

            var deletedFailure = string.Empty;
            var builderDeletedFailure = new StringBuilder();

            foreach (var function in removingIdList)
            {
                var tempResult = await _functionDataAccess.RemoveUserFunctionAsync(licenseeId, userId, custId, ip, function, langKey);

                if (!tempResult.IsSuccessful)
                {
                    builderDeletedFailure.Append(function.ToString() + ",");
                    deletedResultInfo = tempResult;
                }
            }

            deletedFailure = builderDeletedFailure.ToString();

            if (!string.IsNullOrEmpty(deletedFailure))
            {
                deletedResultInfo.Message += " [" + deletedFailure.TrimEnd(',') + "];";
            }

            return deletedResultInfo;
        }
    }
}