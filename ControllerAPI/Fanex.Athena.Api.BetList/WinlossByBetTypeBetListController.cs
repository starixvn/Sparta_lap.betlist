namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Core.Configuration.Interfaces;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using Service.StatelessCore.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportWinLossByBetType)]
    public class WinlossByBetTypeBetListController : ApiBetListBaseController
    {
        public WinlossByBetTypeBetListController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IAuthorizationService authorizationService,
            IBetListApiClient betListApiClient,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(sessionService, productService, languageService, authorizationService, betListApiClient, balanceUpDataAccess)
        {
        }

        [HttpGet]
#pragma warning disable S107 // Methods should not have too many parameters
        public async Task<dynamic> Index(
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
#pragma warning restore S107 // Methods should not have too many parameters
        {
            if (!IsValidCurrencyId(currencyId))
            {
                return null;
            }

            var metaModel = new BetListMetaModel
            {
                CurrencyId = currencyId,
                CurrencyName = currencyName,
                BaseCurrency = baseCurrency ?? string.Empty,
                FromDate = fromDate,
                ToDate = toDate,
                BetType = bettype,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            const string UspKey = "Lrf_BetList_SelWinLossByBetType";
            return await GetPagingBetListResult(metaModel, UspKey, page, rows).ConfigureAwait(false);
        }
    }
}