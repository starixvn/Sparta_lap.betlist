// -----------------------------------------------------------------------
// <copyright file="GroupComService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Casino.Interfaces;
    using Fanex.Athena.DataAccess.Customer.Interfaces;
    using Fanex.Athena.Model.Casino.MetaModel;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Session.Model;

    public class GroupComService : IGroupComService
    {
        private readonly ICasinoCustomerService _casinoCustomerService;

        private readonly IGroupComDataAccess _groupDataAccess;

        public GroupComService(
            ICasinoCustomerService casinoCustomerService,
            IGroupComDataAccess groupDataAccess)
        {
            _casinoCustomerService = casinoCustomerService;
            _groupDataAccess = groupDataAccess;
        }

        public Task<IList<GroupCommMetaModel>> GetAll(int licenseeId)
        {
            return _groupDataAccess.GetAll(licenseeId);
        }

        public Task<DataTable> GetDataForExcel(int licenseeId)
        {
            return _groupDataAccess.GetDataForExcel(licenseeId);
        }

        public GroupCommMetaModel GetById(int groupCommId, int licenseeId)
        {
            return _groupDataAccess.GetById(groupCommId, licenseeId);
        }

        public Task<IResultInfo> InsertGroup(GroupCommMetaModel item, IRequest request, int userId)
        {
            return _groupDataAccess.InsertGroup(item, request, userId);
        }

        public async Task<IResultInfo> UpdateGroup(GroupCommMetaModel newItem, IRequest request, UserSession user)
        {
            var resultInfo = await _groupDataAccess.UpdateGroup(newItem, request, user.UserId).ConfigureAwait(false);
            var callApiSuccess = true;
            if (newItem.IsUpdateCS && !string.IsNullOrEmpty(resultInfo.ExtendedData))
            {
                var custtomerList = resultInfo.ExtendedData.Split(new[] { ',' });
                if (custtomerList.Length > 0)
                {
                    var apiCasinoMetalModel = new RNGCasinoUpdateMemberComMetaModel
                    {
                        Comm = newItem.CSComm,
                        CustomerId = custtomerList,
                        FromIp = request.Ip,
                        ModifyBy = user.UserId,
                        ModifyUserName = user.UserName
                    };
                    callApiSuccess = _casinoCustomerService.UpdateCommission(apiCasinoMetalModel);
                }
            }

            if (!callApiSuccess)
            {
                resultInfo.Code = 1;
                resultInfo.Message = "Having errors when calling CasinoAPI to update customer commission.";
            }

            return resultInfo;
        }

        public async Task<IResultInfo> DeleteGroup(DeleteGroupMetaModel model)
        {
            var resultInfo = await _groupDataAccess.DeleteGroup(
                model.GroupId,
                model.LicenseeId,
                model.UserId,
                model.Ip,
                model.Language,
                model.IsRemoveCommSetting,
                model.IsResetDefault).ConfigureAwait(false);
            var callApiSuccess = true;

            if (model.IsRemoveCommSetting && model.IsAllowedCSComm && !string.IsNullOrEmpty(resultInfo.ExtendedData))
            {
                var custtomerList = resultInfo.ExtendedData.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (custtomerList.Length > 0)
                {
                    callApiSuccess = _casinoCustomerService.UpdateCommission(new RNGCasinoUpdateMemberComMetaModel
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
                resultInfo.Code = 1;
                resultInfo.Message = "Having errors when calling CasinoAPI to update customer commission.";
            }

            return resultInfo;
        }

        public bool CheckExistingGroupByName(int licenseeId, string groupName)
        {
            return _groupDataAccess.CheckByName(licenseeId, groupName);
        }
    }
}