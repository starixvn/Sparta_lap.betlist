// -----------------------------------------------------------------------
// <copyright file="ICustPendingFundsService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public interface ICustPendingFundsService
    {
        IList<CustPendingFunds> GetDataList(
            CustomerPendingFundsMetaModel metaModel,
            ref CustPendingFunds totalData,
           ref int totalRows);
    }
}