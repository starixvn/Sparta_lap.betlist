namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Web.Http;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Web.ActionFilter;

    #endregion Using

    public class WinlossByBetTypeBetListController : ApiBetListBaseController
    {
        [HttpGet]
        [ApiFunctionAuthorization]
        public dynamic Index(
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
                SortOrder = sortOrder
            };

            const string UspKey = "Lrf_BetList_SelWinLossByBetType";
            return GetPagingBetListResult(metaModel, UspKey, page, rows);
        }
    }
}