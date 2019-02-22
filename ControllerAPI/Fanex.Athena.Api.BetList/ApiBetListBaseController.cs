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
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Configuration;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using Service.StatelessCore.Interfaces;

    public class ApiBetListBaseController : BaseApiController
    {
        private readonly IAuthorizationService _authorizationService;

        private readonly IBetListApiClient _betListApiClient;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public ApiBetListBaseController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IAuthorizationService authorizationService,
            IBetListApiClient betListApiClient,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(sessionService, productService, languageService)
        {
            _authorizationService = authorizationService;
            _betListApiClient = betListApiClient;
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        protected IBetListApiClient BridgeClientService
        {
            get { return _betListApiClient; }
        }

        protected BalanceUp GetBalanceUp()
        {
            return _balanceUpDataAccess.GetBalanceUp();
        }

        protected void CheckDataPermission(int custId, long transId)
        {
            if (!_authorizationService.HasPermissionOnData(
                SessionService.LicenseeSession.LicenseeId,
                custId,
                transId))
            {
                throw new UnauthorizedAccessException();
            }
        }

        protected internal virtual async Task<dynamic> GetBetListResult(
            BetListMetaModel metaModel,
            string uspKey,
            string apiPath = null)
        {
            CheckDataPermission(metaModel.CustId, metaModel.TransId);

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
            return await _betListApiClient.PostAsync(apiPath ?? "/BetList/GetBetList", metaModel).ConfigureAwait(false);
        }

        protected internal virtual async Task<dynamic> GetPagingBetListResult(
            BetListMetaModel metaModel,
            string uspKey,
            int pageIndex,
            int pageSize,
            string apiPath = null)
        {
            CheckDataPermission(metaModel.CustId, metaModel.TransId);

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
            return await _betListApiClient.PostAsync(apiPath ?? "/BetList/GetBetListPaging", metaModel).ConfigureAwait(false);
        }

        protected bool IsValidCurrencyId(int currencyId)
        {
            return currencyId == 0 || SessionService.UserSession.CurrencyList.ContainsKey(currencyId);
        }
    }
}