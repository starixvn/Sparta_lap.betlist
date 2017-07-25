// -----------------------------------------------------------------------
// <copyright file="IGroupComDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;

    public interface IGroupComDataAccess
    {
        int InsertGroup(GroupCommMetaModel item, IRequest request, int userId, out IResultInfo resultInfo);

        bool UpdateGroup(GroupCommMetaModel newItem, IRequest request, int userId, out IResultInfo resultInfo);

        IList<GroupCommMetaModel> GetAll(int licenseeId);

        DataTable GetDataForExcel(int licenseeId);

        GroupCommMetaModel GetById(int groupCommId);

        bool CheckByName(int licenseeId, string groupName);

        int DeleteGroup(
            int groupId,
            int licenseeId,
            int userId,
            string ip,
            string language,
            bool isRemoveCommSetting,
            bool isResetDefault,
            out IResultInfo resultInfo);
    }
}