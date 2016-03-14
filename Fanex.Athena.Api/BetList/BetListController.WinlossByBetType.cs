namespace Fanex.Athena.Api.BetList
{
    #region Using
    using System;
    using System.Web.Http;
    using Fanex.Athena.Models.MetaModel.BetList;  
    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Get data for win lost by bet type bet list.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <returns>Bet list data.</returns>
        [HttpGet]
        public dynamic WinlossByBetType(
            int page,
            int rows,
            string licenseeType,
            int currencyId,
            string currencyName,
            string baseCurrency,
            DateTime fromDate,
            DateTime toDate,
            int bettype,
            string sortBy = null,
            string sortOrder = null)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (currencyId != 0 && !currencyList.Values.Contains(currencyId))
            {
                return null;
            }

            var metaModel = new BetListMetaModel
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId,
                LicenseeType = licenseeType,
                CurrencyId = currencyId,
                CurrencyName = currencyName,
                BaseCurrency = baseCurrency ?? string.Empty,
                FromDate = fromDate,
                ToDate = toDate,
                UserId = userSession.UserId,
                BetType = bettype,
                SortBy = sortBy,
                SortOrder = sortOrder,
                IsServerPaging = true
            };

            const string UspKey = "Lrf_BetList_SelWinLossByBetType";
            return GetPagingBetListResult(metaModel, UspKey, page, rows);
        }
    }
}
