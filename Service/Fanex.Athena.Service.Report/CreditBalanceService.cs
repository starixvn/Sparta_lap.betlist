// -----------------------------------------------------------------------
// <copyright file="CreditBalanceService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;

    /// <summary>
    ///     Credit Balance Service.
    /// </summary>
    public class CreditBalanceService : ICreditBalanceService
    {
        #region Field

        private readonly CreditBalanceDataAccess _balanceDataAccess;

        #endregion Field

        #region Constructor

        public CreditBalanceService()
        {
            this._balanceDataAccess = new CreditBalanceDataAccess();
        }

        #endregion Constructor

        public Balance GetReportData(BalanceMetaModel metaModel)
        {
            return metaModel.IsValid
                       ? _balanceDataAccess.GetReportData(metaModel)
                       : new Balance();
        }
    }
}