// -----------------------------------------------------------------------
// <copyright file="IWinLossByProductDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Report.ResultModel;

    public interface IWinLossByProductDataAccess
    {
        WinLossByProductResultModel GetWinlossByProducts(WinLossByProductMetaModel metaModel);

        WinLossByProductResultModel GetWinlossByProductAgents(WinLossByProductMetaModel metaModel);

        WinLossByProductResultModel GetWinlossByProductCustomers(WinLossByProductMetaModel metaModel);
    }
}