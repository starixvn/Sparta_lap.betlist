// -----------------------------------------------------------------------
// <copyright file="IGroupComDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.MetaModel;

    public interface IGroupComDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        int InsertGroup(GroupCommMetaModel item, IRequest request, int userId, out IResultInfo resultInfo);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        bool UpdateGroup(GroupCommMetaModel newItem, IRequest request, int userId, out IResultInfo resultInfo);

        IList<GroupCommMetaModel> GetAll(int licenseeId);

        DataTable GetDataForExcel(int licenseeId);

        GroupCommMetaModel GetById(int groupCommId);

        bool CheckByName(int licenseeId, string groupName);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
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