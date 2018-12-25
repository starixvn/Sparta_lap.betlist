// -----------------------------------------------------------------------
// <copyright file="IWinLossByProductDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Report.ResultModel;

    public interface IWinLossByProductDataAccess
    {
        WinLossByProductResultModel GetWinlossByProducts(WinLossByProductMetaModel metaModel, IDictionary<int, string> currencyList);

        WinLossByProductResultModel GetWinlossByProductAgents(WinLossByProductMetaModel metaModel, IDictionary<int, string> currencyList);

        WinLossByProductResultModel GetWinlossByProductCustomers(WinLossByProductMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}