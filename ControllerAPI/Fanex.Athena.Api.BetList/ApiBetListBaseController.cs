// --------------------------------------------------------------------------
// <copyright file="ApiBetListBaseController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Threading.Tasks;
    using Base;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Model.Core.DTO.Configuration;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

    public class ApiBetListBaseController : BaseApiController
    {
        private readonly IBridgeClientService _bridgeClientService;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

#pragma warning disable S107 // Methods should not have too many parameters

        public ApiBetListBaseController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBridgeClientService bridgeClientService,
            IBalanceUpDataAccess balanceUpDataAccess)
#pragma warning restore S107 // Methods should not have too many parameters
        : base(sessionService, productService, languageService)
        {
            _bridgeClientService = bridgeClientService;
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        protected IBridgeClientService BridgeClientService
        {
            get { return _bridgeClientService; }
        }

        protected BalanceUp GetBalanceUp()
        {
            return _balanceUpDataAccess.GetBalanceUp();
        }

        protected internal virtual async Task<dynamic> GetBetListResult(
            BetListMetaModel metaModel,
            string uspKey,
            string url = null)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var balanceUp = GetBalanceUp();
            var currencyList = SessionService.UserSession.CurrencyList;
            metaModel.PageIndex = 1;
            metaModel.PageSize = int.MaxValue - 1;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUp.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUp.MoveBetsDay.Date, DateTimeKind.Utc);
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.HasPlayerCom = licenseeSession.IsAllowedCustomerComm;
            metaModel.CurrencyList = currencyList;
            metaModel.UspKey = uspKey;
            var betlistAPIUrl = new Uri(url ?? $"http://localhost:55050/BetList/GetBetList");
            return await _bridgeClientService.PostAsync(betlistAPIUrl, metaModel).ConfigureAwait(false);
        }

        protected internal virtual async Task<dynamic> GetPagingBetListResult(
            BetListMetaModel metaModel,
            string uspKey,
            int pageIndex,
            int pageSize,
            string url = null)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var balanceUp = GetBalanceUp();
            var currencyList = SessionService.UserSession.CurrencyList;

            metaModel.IsServerPaging = true;
            metaModel.PageIndex = pageIndex < 1 ? 1 : pageIndex;
            metaModel.PageSize = pageSize < 1 ? 100 : pageSize;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUp.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUp.MoveBetsDay.Date, DateTimeKind.Utc);
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
            metaModel.HasPlayerCom = licenseeSession.IsAllowedCustomerComm;
            metaModel.CurrencyList = currencyList;
            metaModel.UspKey = uspKey;
            var betlistAPIUrl = new Uri(url ?? $"http://localhost:55050/BetList/GetBetListPaging");
            return await _bridgeClientService.PostAsync(betlistAPIUrl, metaModel).ConfigureAwait(false);
        }

        protected bool IsValidCurrencyId(int currencyId)
        {
            return currencyId == 0 || SessionService.UserSession.CurrencyList.ContainsKey(currencyId);
        }
    }
}