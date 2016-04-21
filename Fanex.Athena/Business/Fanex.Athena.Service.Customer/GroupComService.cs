// -----------------------------------------------------------------------
// <copyright file="GroupComService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Athena.DataAccess.Customer;
    using Fanex.Athena.DataAccess.Customer.Interfaces;
    using Fanex.Athena.Model.Casino.MetaModel;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Service.Casino;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Model.Core.ViewModel;

    #endregion

    /// <summary>
    ///     Group service.
    /// </summary>
    public class GroupComService : IGroupComService
    {
        #region Constants and Fields

        /// <summary>
        ///     The _group data access.
        /// </summary>
        private readonly IGroupComDataAccess _groupDataAccess;

        #endregion

        #region Constructors and Destructors

        public GroupComService()
            : this(new GroupComDataAccess())
        {
        }

        public GroupComService(IGroupComDataAccess groupDataAccess)
        {
            this._groupDataAccess = groupDataAccess;
        }

        #endregion

        public IList<GroupCommMetaModel> GetAll(int licenseeId)
        {
            return this._groupDataAccess.GetAll(licenseeId);
        }

        public DataTable GetDataForExcel(int licenseeId)
        {
            return _groupDataAccess.GetDataForExcel(licenseeId);
        }

        /// <summary>
        /// Get commission group by group id.
        /// </summary>
        /// <param name="groupCommId">The group id.</param>
        /// <returns>The group commission.</returns>
        public GroupCommMetaModel GetById(int groupCommId)
        {
            return _groupDataAccess.GetById(groupCommId);
        }

        /// <summary>
        /// Get default group.
        /// </summary>
        /// <param name="groupList">The group list.</param>
        /// <returns>The group id.</returns>
        public int GetDefaultGroup(IList<GroupCommMetaModel> groupList)
        {
            return groupList.Where(g => g.IsDefaultGroup).Select(g => g.GroupCommId).FirstOrDefault();
        }

        /// <summary>
        /// Insert group.
        /// </summary>
        /// <param name="item">The group info.</param>
        /// <param name="request">The request info.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="result">Out put parameter:Result view model.</param>
        /// <returns>Return the group id if processing is successful.</returns>
        public int InsertGroup(GroupCommMetaModel item, IRequest request, int userId, out ResultViewModel result)
        {
            IResultInfo dataResultInfo;
            var data = this._groupDataAccess.InsertGroup(item, request, userId, out dataResultInfo);

            result = new ResultViewModel
            {
                ErrorCode = dataResultInfo.Code,
                ErrorMessage = dataResultInfo.Message,
                Success = dataResultInfo.IsSuccessful,
                Message = dataResultInfo.Message,
                ExtendedData = data
            };
            return data;
        }

        /// <summary>
        /// Update group.
        /// </summary>
        /// <param name="newItem">The group info.</param>
        /// <param name="request">The request info.</param>
        /// <param name="user">The user session.</param>
        /// <param name="result">Out put parameter:Result view model.</param>
        /// <returns>True if processing's successful.</returns>
        public bool UpdateGroup(GroupCommMetaModel newItem, IRequest request, UserSession user, out ResultViewModel result)
        {
            IResultInfo dataResultInfo;

            var data = this._groupDataAccess.UpdateGroup(newItem, request, user.UserId, out dataResultInfo);
            var callApiSuccess = true;
            result = new ResultViewModel
            {
                ErrorCode = dataResultInfo.Code,
                ErrorMessage = dataResultInfo.Message,
                Success = dataResultInfo.IsSuccessful,
                Message = dataResultInfo.Message,
                ExtendedData = data
            };
            if (newItem.IsUpdateCS && !string.IsNullOrEmpty(dataResultInfo.ExtendedData))
            {
                var custtomerList = dataResultInfo.ExtendedData.Split(new char[] { ',' });
                if (custtomerList.Length > 0)
                {
                    var rngCasinoService = new RNGCasinoService();
                    var apiCasinoMetalModel = new RNGCasinoUpdateMemberComMetaModel
                    {
                        Comm = newItem.CSComm,
                        CustomerId = custtomerList,
                        FromIp = request.Ip,
                        ModifyBy = user.UserId,
                        ModifyUserName = user.UserName
                    };
                    callApiSuccess = rngCasinoService.UpdateMemberCommission(apiCasinoMetalModel);
                }
            }

            if (!callApiSuccess)
            {
                data = false;
                result.Success = false;
                result.ErrorCode = 1;
                result.ErrorMessage = "Having errors when calling CasinoAPI to update customer commission.";
            }

            return data;
        }

        /// <summary>
        /// Delete group.
        /// </summary>
        /// <param name="model">Input parameters.</param>
        /// <param name="result">Out put parameter:Result view model.</param>
        /// <returns>Return group id.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public int DeleteGroup(DeleteGroupMetaModel model, out ResultViewModel result)
        {
            IResultInfo dataResultInfo;
            var returnValue = this._groupDataAccess.DeleteGroup(
                model.GroupId,
                model.LicenseeId,
                model.UserId,
                model.Ip,
                model.Language,
                model.IsRemoveCommSetting,
                model.IsResetDefault,
                out dataResultInfo);
            result = new ResultViewModel
            {
                ErrorCode = dataResultInfo.Code,
                ErrorMessage = dataResultInfo.Message,
                Success = dataResultInfo.IsSuccessful,
                Message = dataResultInfo.Message,
                ExtendedData = returnValue
            };
            var callApiSuccess = true;

            if (model.IsRemoveCommSetting && model.IsAllowedCSComm && !string.IsNullOrEmpty(dataResultInfo.ExtendedData))
            {
                var custtomerList = dataResultInfo.ExtendedData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (custtomerList.Length > 0)
                {
                    var rngCasinoService = new RNGCasinoService();
                    callApiSuccess = rngCasinoService.UpdateMemberCommission(new RNGCasinoUpdateMemberComMetaModel
                    {
                        Comm = 0,
                        CustomerId = custtomerList,
                        FromIp = model.Ip,
                        ModifyBy = model.UserId,
                        ModifyUserName = model.UserName
                    });
                }
            }

            if (!callApiSuccess)
            {
                result.Success = false;
                result.ErrorCode = 1;
                result.ErrorMessage = "Having errors when calling CasinoAPI to update customer commission.";
            }

            return returnValue;
        }

        /// <summary>
        /// Check existence of  group name.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="groupName">The group name.</param>
        /// <returns>True,if this group doesn't exist.</returns>
        public bool CheckByName(int licenseeId, string groupName)
        {
            return this._groupDataAccess.CheckByName(licenseeId, groupName);
        }
    }
}