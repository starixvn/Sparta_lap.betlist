// -----------------------------------------------------------------------
// <copyright file="ICustPendingFundsService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public interface ICustPendingFundsService
    {
        SortableList<CustPendingFunds> GetDataList(
            CustomerPendingFundsMetaModel metaModel,
            ref CustPendingFunds totalData,
           ref int totalRows);
    }
}