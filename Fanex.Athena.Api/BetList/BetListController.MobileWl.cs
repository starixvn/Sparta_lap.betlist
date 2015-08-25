// -----------------------------------------------------------------------
// <copyright file="BetListController.MobileWl.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System.Web.Http;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel.BetList;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Get data for licensee win lost bet list.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <returns>Bet list data.</returns>
        [HttpPost]
        public dynamic MobileWL(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;
            var licenseeId = SessionService.LicenseeSession.LicenseeId;

            if (metaModel.CurrencyId > 0 && !currencyList.Values.Contains(metaModel.CurrencyId) &&
                (licenseeId == Licensees.KeenOcean && metaModel.CurrencyId != CurrencyType.Cny))
            {
                return null;
            }

            metaModel.LicenseeId = licenseeId;
            metaModel.UserId = userSession.UserId;

            const string UspKey = "Lrf_BetList_MobileReport_SelWinloss_Mem";
            return GetBetListResult(metaModel, UspKey);
        }
    }
}