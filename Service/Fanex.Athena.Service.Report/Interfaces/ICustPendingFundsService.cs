// -----------------------------------------------------------------------
// <copyright file="ICustPendingFundsService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface ICustPendingFundsService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", 
            Justification = "Reviewed. Suppression is OK here.")]
        SortableList<CustPendingFunds> GetDataList(
            CustomerPendingFundsMetaModel metaModel,
            ref CustPendingFunds totalData,
           ref int totalRows);
    }
}