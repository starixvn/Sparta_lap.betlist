// -----------------------------------------------------------------------
// <copyright file="IGroupComService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Customer.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Session.Model;

    public interface IGroupComService
    {
        Task<IResultInfo> InsertGroup(GroupCommMetaModel item, IRequest request, int userId);

        Task<IResultInfo> UpdateGroup(GroupCommMetaModel newItem, IRequest request, UserSession user);

        Task<IList<GroupCommMetaModel>> GetAll(int licenseeId);

        Task<DataTable> GetDataForExcel(int licenseeId);

        GroupCommMetaModel GetById(int groupCommId, int licenseeId);

        bool CheckExistingGroupByName(int licenseeId, string groupName);

        Task<IResultInfo> DeleteGroup(DeleteGroupMetaModel model);
    }
}