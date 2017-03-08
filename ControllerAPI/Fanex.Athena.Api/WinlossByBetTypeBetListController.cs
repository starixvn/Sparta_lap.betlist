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
            int currencyId,
            string currencyName,
            string baseCurrency,
            DateTime fromDate,
            DateTime toDate,
            int bettype,
            string sortBy = null,
            string sortOrder = null)
        {
            if (currencyId != 0 && !SessionService.UserSession.CurrencyList.ContainsKey(currencyId))
            {
                return null;
            }

            var metaModel = new BetListMetaModel
            {
                CurrencyName = currencyName,
                BaseCurrency = baseCurrency ?? string.Empty,
                FromDate = fromDate,
                ToDate = toDate,
                BetType = bettype,
                SortBy = sortBy,
                SortOrder = sortOrder,
                AgentListXml = SessionService.GenerateAgentListXml(currencyId)
            };

            const string UspKey = "Lrf_BetList_SelWinLossByBetType";
            return GetPagingBetListResult(metaModel, UspKey, page, rows);
        }
    }
}