// -----------------------------------------------------------------------
// <copyright file="IGroupComService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Session.Model;

    public interface IGroupComService
    {
        IResultInfo InsertGroup(GroupCommMetaModel item, IRequest request, int userId);

        IResultInfo UpdateGroup(GroupCommMetaModel newItem, IRequest request, UserSession user);

        IList<GroupCommMetaModel> GetAll(int licenseeId);

        DataTable GetDataForExcel(int licenseeId);

        GroupCommMetaModel GetById(int groupCommId, int licenseeId);

        int GetDefaultGroup(IList<GroupCommMetaModel> groupList);

        bool CheckExistingGroupByName(int licenseeId, string groupName);

        IResultInfo DeleteGroup(DeleteGroupMetaModel model);
    }
}