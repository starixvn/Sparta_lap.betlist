// -----------------------------------------------------------------------
// <copyright file="IGroupComService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Customer
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Users;
    using Fanex.Athena.Models.MetaModel.Customer;
    using Fanex.Athena.Models.ViewModel;

    public interface IGroupComService
    {
        int InsertGroup(GroupCommMetaModel item, IRequest request, int userId, out ResultViewModel result);

        bool UpdateGroup(GroupCommMetaModel newItem, IRequest request, UserSession user, out ResultViewModel result);

        IList<GroupCommMetaModel> GetAll(int licenseeId);

        DataTable GetDataForExcel(int licenseeId);

        GroupCommMetaModel GetById(int groupCommId);

        int GetDefaultGroup(IList<GroupCommMetaModel> groupList);

        bool CheckByName(int licenseeId, string groupName);

        int DeleteGroup(DeleteGroupMetaModel model, out ResultViewModel result);
    }
}