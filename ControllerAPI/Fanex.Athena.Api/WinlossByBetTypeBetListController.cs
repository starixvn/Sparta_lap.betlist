namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.Core.DataDomain;

    #endregion Using

    [ApiFunctionAuthorization(PermissionKeys.ReportWinLossByBetType)]
    public class WinlossByBetTypeBetListController : ApiBetListBaseController
    {
        [HttpGet]
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

            if (currencyId != 0 && !currencyList.ContainsKey(currencyId))
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