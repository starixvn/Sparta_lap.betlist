// -----------------------------------------------------------------------
// <copyright file="IAgCasinoWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ResultModel;

    /// <summary>
    ///     The Ag casino win loss data access.
    /// </summary>
    public interface IAgCasinoWinLossDataAccess
    {
        #region Public Methods and Operators

        ReportResultModel<AgCasinoWinLoss> GetAgentWinlossDetail(AgCasinoWinLossMetaModel metaModel);

        ReportResultModel<AgCasinoWinLoss> GetCustomerWinlossDetail(AgCasinoWinLossMetaModel metaModel);

        ReportResultModel<AgCasinoWinLoss> GetSummaryWinloss(AgCasinoWinLossMetaModel metaModel);

        #endregion Public Methods and Operators
    }
}