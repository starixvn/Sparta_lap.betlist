// -----------------------------------------------------------------------
// <copyright file="ICustWinlossService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ICustWinlossService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<CustomerWinLoss> GetDataList(
            CustomerWinLossMetaModel metaModel,
            out CustomerWinLoss totalData,
            out int totalRows);
    }
}