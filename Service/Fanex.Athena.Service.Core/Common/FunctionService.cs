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
    using Fanex.Athena.DataAccess.Core.Common;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Resources;

    /// <summary>
    /// Function service class.
    /// </summary>
    public class FunctionService : IFunctionService
    {
        private static IList<Function> _functionList;
        private static IList<Function> _masterFunctions;
        private static IDictionary<int, IList<Function>> _subFunctions;
        private readonly IFunctionDataAccess _functionDataAccess;

        public FunctionService()
        {
            this._functionDataAccess = new FunctionDataAccess();
        }

        public FunctionService(IFunctionDataAccess functionDataAccess)
        {
            this._functionDataAccess = functionDataAccess;
        }

        #region Property

        public IList<Function> Functions
        {
            get
            {
                if (_functionList == null)
                {
                    _functionList = _functionDataAccess.GetFunctionList();
                }

                return _functionList;
            }
        }

        private IList<Function> MasterFunctions
        {
            get
            {
                if (_masterFunctions == null)
                {
                    _masterFunctions = Functions.Where(f => f.ParentId == 0)
                                         .OrderBy(f => f.DisplayOrder)
                                         .ToList();
                }

                return _masterFunctions;
            }
        }

        private IDictionary<int, IList<Function>> SubFunctions
        {
            get
            {
                if (_subFunctions == null)
                {
                    _subFunctions = new Dictionary<int, IList<Function>>();

                    foreach (var function in MasterFunctions)
                    {
                        var subFunctions = Functions.Where(f => f.ParentId == function.FunctionId)
                                                     .OrderBy(f => f.DisplayOrder)
                                                     .ToList();
                        _subFunctions.Add(function.FunctionId, subFunctions);
                    }
                }

                return _subFunctions;
            }
        }

        #endregion Property

        #region Licensee Function

        /// <summary>
        /// Get function list of licensee.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        /// <returns>The function list.</returns>
        public IList<Function> GetLicenseeFunctionList(int licenseeId)
        {
            var functionIdList = _functionDataAccess.GetLicenseeFunctionList(licenseeId)
                                                    .Select(i => i.FunctionId);

            return Functions.Where(f => functionIdList.Contains(f.FunctionId)).ToList();
        }

        /// <summary>
        /// Get function list of user.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>The function list.</returns>
        public IList<Function> GetUserFunctionList(int licenseeId, int userId)
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

                var subFunctionCount = SubFunctions[masterFunction.FunctionId].Count;
                var userSubFunctionCount = functionList.Count(f => f.ParentId == masterFunction.FunctionId);

                // User has permission on all sub functions, so he/she has permission on parent function
                if (subFunctionCount == userSubFunctionCount)
                {
                    functionList.Add(masterFunction);
                }
            }

            return functionList;
        }

        public IResultInfo AssignUserFunctions(int licenseeId, int custId, IList<int> functionIdList, int userId, string ip, string langKey)
        {
            var correctFunctionIdList = functionIdList.Where(f => Functions.Any(l => l.FunctionId == f)).ToList();
            var userFunctionList = GetUserFunctionList(licenseeId, custId).Select(f => f.FunctionId);
            var addingIdList = correctFunctionIdList.Where(f => userFunctionList.All(u => u != f));
            var removingIdList = userFunctionList.Where(f => correctFunctionIdList.All(c => c != f));

            var addingResult = AddUserToFunction(licenseeId, custId, userId, ip, langKey, addingIdList);

            var removingResult = RemoveUserFromFunction(licenseeId, custId, userId, ip, langKey, removingIdList);

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
        public IList<Function> GetMasterFunctions(IList<Function> functionList)
        {
            return MasterFunctions.Where(
                    f => functionList.Any(l => l.ParentId == f.FunctionId || l.FunctionId == f.FunctionId))
                                .OrderBy(f => f.DisplayOrder)
                                .ToList();
        }

        /// <summary>
        /// Get sub functions.
        /// </summary>
        /// <param name="functionList">The functions.</param>
        /// <returns>The sub functions.</returns>
        public IDictionary<int, IList<Function>> GetSubMenus(IList<Function> functionList)
        {
            var detailFunctions = new Dictionary<int, IList<Function>>();

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

        public IDictionary<int, IList<Function>> GetSubFunctions(IList<Function> functionList)
        {
            var detailFunctions = new Dictionary<int, IList<Function>>();

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

        private IResultInfo AddUserToFunction(
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

            var failedIdList = string.Empty;

            foreach (var function in addingIdList)
            {
                var tempResult = _functionDataAccess.AssignUserFunction(licenseeId, custId, function, userId, ip, langKey);

                if (!tempResult.IsSuccessful)
                {
                    failedIdList += function.ToString() + ",";
                    addedResultInfo = tempResult;
                }
            }

            if (!addedResultInfo.IsSuccessful)
            {
                addedResultInfo.Message += " [" + failedIdList.TrimEnd(',') + "];";
            }

            return addedResultInfo;
        }

        private IResultInfo RemoveUserFromFunction(
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
                var tempResult = _functionDataAccess.RemoveUserFunction(licenseeId, userId, custId, ip, function, langKey);

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