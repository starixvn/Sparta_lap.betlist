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
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Model.Core.ViewModel;

    public interface IGroupComService
    {
        int InsertGroup(GroupCommMetaModel item, IRequest request, int userId, out ResultViewModel result);

        bool UpdateGroup(GroupCommMetaModel newItem, IRequest request, UserSession user, out ResultViewModel result);

        IList<GroupCommMetaModel> GetAll(int licenseeId);

        DataTable GetDataForExcel(int licenseeId);

        GroupCommMetaModel GetById(int groupCommId);

        int GetDefaultGroup(IList<GroupCommMetaModel> groupList);

        bool CheckExistingGroupByName(int licenseeId, string groupName);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        int DeleteGroup(DeleteGroupMetaModel model, out ResultViewModel result);
    }
}