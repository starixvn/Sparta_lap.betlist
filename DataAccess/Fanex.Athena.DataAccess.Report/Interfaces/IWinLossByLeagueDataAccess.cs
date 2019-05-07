// -----------------------------------------------------------------------
// <copyright file="IWinLossByLeagueDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    public interface IWinLossByLeagueDataAccess
    {
        Task<IEnumerable<WinLossByLeague>> GetSummary(WinLossByLeagueMetaModel metaModel);

        Task<ReportResultModel<WinLossByLeague>> GetCustomers(WinLossByLeagueMetaModel metaModel);
    }
}