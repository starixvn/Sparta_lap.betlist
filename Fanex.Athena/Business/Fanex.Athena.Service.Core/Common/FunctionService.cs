// -----------------------------------------------------------------------
// <copyright file="FunctionService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Common;
    using Fanex.Athena.DataAccess.Interface.Common;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Core.Common.Interfaces;

    /// <summary>
    /// Function service class.
    /// </summary>
    public class FunctionService : IFunctionService
    {
        private static readonly IList<Function> FunctionList;
        private static readonly IList<Function> MasterFunctions;
        private static readonly IDictionary<int, IList<Function>> SubFunctions;
        private readonly IFunctionDataAccess _functionDataAccess;

        #region Constructor

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline",
            Justification = "Reviewed. Suppression is OK here.")]
        static FunctionService()
        {
            var functionDataAccess = new FunctionDataAccess();
            FunctionList = functionDataAccess.GetFunctionList();
            SubFunctions = new Dictionary<int, IList<Function>>();
            MasterFunctions = FunctionList.Where(f => f.ParentId == 0)
                                         .OrderBy(f => f.DisplayOrder)
                                         .ToList();
            foreach (var function in MasterFunctions)
            {
                var subFunctions = FunctionList.Where(f => f.ParentId == function.FunctionId)
                                             .OrderBy(f => f.DisplayOrder)
                                             .ToList();
                SubFunctions.Add(function.FunctionId, subFunctions);
            }
        }

        public FunctionService()
        {
            this._functionDataAccess = new FunctionDataAccess();
        }

        #endregion Constructor

        #region Property

        public IList<Function> Functions
        {
            get { return FunctionList; }
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

            return FunctionList.Where(f => functionIdList.Contains(f.FunctionId)).ToList();
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
            var functionList = FunctionList.Where(f => functionIdList.Contains(f.FunctionId)).ToList();

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

        /// <summary>
        /// Assign function to user.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="custId">The customer id.</param>
        /// <param name="functionIdList">The function list.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="langKey">The language code.</param>
        /// <returns>The result.</returns>
        public IResultInfo AssignUserFunctions(int licenseeId, int custId, IList<int> functionIdList, int userId, string ip, string langKey)
        {
            var correctFunctionIdList = functionIdList.Where(f => FunctionList.Any(l => l.FunctionId == f)).ToList();
            var userFunctionList = GetUserFunctionList(licenseeId, custId).Select(f => f.FunctionId);
            var addingIdList = correctFunctionIdList.Where(f => userFunctionList.All(u => u != f));
            var removingIdList = userFunctionList.Where(f => correctFunctionIdList.All(c => c != f));

            var code = 0;
            var message = string.Empty;
            var addedFailure = string.Empty;

            IResultInfo addedResultInfo = AddUserToFunction(licenseeId, custId, userId, ip, langKey, addingIdList, ref addedFailure);

            RemoveUserFromFunction(licenseeId, custId, userId, ip, langKey, removingIdList, ref code, ref message, ref addedFailure, addedResultInfo);

            if (code == 0)
            {
                message = ViewResources.UpdateFunctionSuccessfulMsg;
            }

            return new ResultInfo
            {
                Code = code,
                Message = message
            };
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
            return
                MasterFunctions.Where(
                    f => functionList.Any(l => l.ParentId == f.FunctionId || l.FunctionId == f.FunctionId))
                                .OrderBy(f => f.DisplayOrder)
                                .ToList();
        }

        /// <summary>
        /// Get sub functions.
        /// </summary>
        /// <param name="functionList">The functions.</param>
        /// <returns>The sub functions.</returns>
        public IDictionary<int, IList<Function>> GetSubFunctions(IList<Function> functionList)
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

        #endregion Utilities

        /// <summary>
        /// Add user to function.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="custId">The customer id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="langKey">The language code.</param>
        /// <param name="addingIdList">The function id list.</param>
        /// <param name="addedFailure">The function id list that processing is failed.</param>
        /// <returns>The result.</returns>
        private IResultInfo AddUserToFunction(
            int licenseeId,
            int custId,
            int userId,
            string ip,
            string langKey,
            IEnumerable<int> addingIdList,
            ref string addedFailure)
        {
            IResultInfo addedResultInfo = new ResultInfo
            {
                Code = 0,
                Message = ViewResources.UpdateFunctionSuccessfulMsg
            };

            foreach (var function in addingIdList)
            {
                var tempResult = _functionDataAccess.AssignUserFunction(licenseeId, custId, function, userId, ip, langKey);

                if (!tempResult.IsSuccessful)
                {
                    addedFailure += function.ToString() + ",";
                    addedResultInfo = tempResult;
                }
            }

            return addedResultInfo;
        }

        /// <summary>
        /// Remove user from function.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="custId">The customer id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="langKey">The language code.</param>
        /// <param name="removingIdList">The functions.</param>
        /// <param name="code">The result code.</param>
        /// <param name="message">The result message.</param>
        /// <param name="addedFailure">The function id list that processing is failed.</param>
        /// <param name="addedResultInfo">The result info.</param>
        private void RemoveUserFromFunction(
            int licenseeId,
            int custId,
            int userId,
            string ip,
            string langKey,
            IEnumerable<int> removingIdList,
            ref int code,
            ref string message,
            ref string addedFailure,
            IResultInfo addedResultInfo)
        {
            IResultInfo deletedResultInfo = new ResultInfo
            {
                Code = 0,
                Message = ViewResources.UpdateFunctionSuccessfulMsg
            };

            var deletedFailure = string.Empty;

            foreach (var function in removingIdList)
            {
                var tempResult = _functionDataAccess.RemoveUserFunction(licenseeId, userId, custId, ip, function, langKey);

                if (!tempResult.IsSuccessful)
                {
                    deletedFailure += function.ToString() + ",";
                    deletedResultInfo = tempResult;
                }
            }

            if (!string.IsNullOrEmpty(addedFailure))
            {
                addedFailure = addedFailure.Substring(0, addedFailure.Length - 1);
                code = addedResultInfo.Code;
                message = addedResultInfo.Message + " [" + addedFailure + "];";
            }

            if (!string.IsNullOrEmpty(deletedFailure))
            {
                deletedFailure = deletedFailure.Substring(0, addedFailure.Length - 1);
                code = deletedResultInfo.Code;
                if (!string.IsNullOrEmpty(message))
                {
                    message += "; " + deletedResultInfo.Message + " [" + deletedFailure + "];";
                }
            }
        }
    }
}