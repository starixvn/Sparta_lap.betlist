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
        IResultInfo InsertGroup(GroupCommMetaModel item, IRequest request, int userId);

        IResultInfo UpdateGroup(GroupCommMetaModel newItem, IRequest request, int userId);

        IList<GroupCommMetaModel> GetAll(int licenseeId);

        DataTable GetDataForExcel(int licenseeId);

        GroupCommMetaModel GetById(int groupCommId, int licenseeId);

        bool CheckByName(int licenseeId, string groupName);

        IResultInfo DeleteGroup(
            int groupId,
            int licenseeId,
            int userId,
            string ip,
            string language,
            bool isRemoveCommSetting,
            bool isResetDefault);
    }
}