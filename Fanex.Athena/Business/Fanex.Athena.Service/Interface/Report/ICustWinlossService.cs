// -----------------------------------------------------------------------
// <copyright file="ICustWinlossService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public interface ICustWinlossService
    {
        SortableList<CustomerWinLoss> GetDataList(
            CustomerWinLossMetaModel metaModel,
            ref CustomerWinLoss totalData,
           ref int totalRows);
    }
}