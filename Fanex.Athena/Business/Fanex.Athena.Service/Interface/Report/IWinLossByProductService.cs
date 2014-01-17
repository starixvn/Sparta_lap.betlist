// -----------------------------------------------------------------------
// <copyright file="IWinLossByProductService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    public interface IWinLossByProductService
    {
        IList<WinLossByProduct> GetWinlossByProducts(WinLossByProductMetaModel metaModel, out IDictionary<string, IList<WinLossByProduct>> summaryTotal);

        IList<WinLossByProduct> GetWinlossByProductAgents(WinLossByProductMetaModel metaModel, out IDictionary<string, IList<WinLossByProduct>> summaryTotal);

        IList<WinLossByProduct> GetWinlossByProductCustomers(WinLossByProductMetaModel metaModel, out IDictionary<string, IList<WinLossByProduct>> summaryTotal);

        IList<WinLossByProduct> TotalCalc(IList<WinLossByProduct> dataList);
    }
}