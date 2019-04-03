// -----------------------------------------------------------------------
// <copyright file="IGroupComDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;

    public interface IGroupComDataAccess
    {
        Task<IResultInfo> InsertGroup(GroupCommMetaModel item, IRequest request, int userId);

        Task<IResultInfo> UpdateGroup(GroupCommMetaModel newItem, IRequest request, int userId);

        Task<IList<GroupCommMetaModel>> GetAll(int licenseeId);

        Task<DataTable> GetDataForExcel(int licenseeId);

        GroupCommMetaModel GetById(int groupCommId, int licenseeId);

        bool CheckByName(int licenseeId, string groupName);

        Task<IResultInfo> DeleteGroup(
            int groupId,
            int licenseeId,
            int userId,
            string ip,
            string language,
            bool isRemoveCommSetting,
            bool isResetDefault);
    }
}