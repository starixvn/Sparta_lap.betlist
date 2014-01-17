// -----------------------------------------------------------------------
// <copyright file="ICustWinlossService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public interface ICustWinlossService
    {
        IList<CustomerWinLoss> GetDataList(
            CustomerWinLossMetaModel metaModel,
            ref CustomerWinLoss totalData,
           ref int totalRows);
    }
}