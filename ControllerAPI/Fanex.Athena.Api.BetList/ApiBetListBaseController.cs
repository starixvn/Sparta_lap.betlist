// --------------------------------------------------------------------------
// <copyright file="ApiBetListBaseController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Base;
    using Constant.DataDomain;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Service.BetList.Extensions;
    using Fanex.Athena.Service.BetList.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Helpers;
    using Model.Core.DTO.Configuration;
    using Service.BetList.Helpers;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter.Interfaces;

    #endregion Using

    public class ApiBetListBaseController : BaseApiController
    {
        private readonly IBetListService _betListService;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly ITicketHelper _ticketHelper;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly IBetListHTMLBuilder _betlistHtmlBuilder;

#pragma warning disable S107 // Methods should not have too many parameters

        public ApiBetListBaseController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService languageService,
            IBetListService betListService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ITicketHelper ticketHelper,
            ICacheRefAdapter cacheRefAdapter,
            IBetListHTMLBuilder betlistHtmlBuilder)
#pragma warning restore S107 // Methods should not have too many parameters
        : base(sessionService, productService, languageService)
        {
            _betListService = betListService;
            _balanceUpDataAccess = balanceUpDataAccess;
            _ticketHelper = ticketHelper;
            _cacheRefAdapter = cacheRefAdapter;
            _betlistHtmlBuilder = betlistHtmlBuilder;
        }

        protected IBetListService BetListService
        {
            get { return _betListService; }
        }

        protected ICacheRefAdapter CacheRefAdapter
        {
            get { return _cacheRefAdapter; }
        }

        protected static void ClearData(TicketResult ticketResult)
        {
            ticketResult.MainTicketList.Clear();
            ticketResult.TicketDataList.Clear();
            ticketResult.MainTicketList.TrimExcess();
            ticketResult.TicketDataList.TrimExcess();
            ticketResult.MainTicketList = null;
            ticketResult.TicketDataList = null;
        }

        protected BalanceUp GetBalanceUp()
        {
            return _balanceUpDataAccess.GetBalanceUp();
        }

        protected internal virtual List<TicketText> Ticket(TicketResult ticketResult, BetListMetaModel metaModel = null, int startIndex = 1, bool showOdds = false)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var showPlayerComm = licenseeSession.IsAllowedCustomerComm;
            var baseCurrency = metaModel == null ? string.Empty : metaModel.BaseCurrency;
            var ticketTextList = new List<TicketText>();

            _ticketHelper.IsShowOddsWithChoice = showOdds;

            foreach (var ticket in ticketResult.MainTicketList)
            {
                if (ticket.BetTypeId == (int)BetType.CombinationMixParlay && metaModel != null)
                {
                    var custid = ticket.CustId != 0 ? ticket.CustId : metaModel.CustId;
                    _ticketHelper.ClientDetailSystemParlayFunction = BetListHelper.GenerateClientDetailSystemParlayFunction(_ticketHelper, baseCurrency, ticket, custid);
                }

                var ticketText = new TicketText
                {
                    Index = startIndex++,
                    ExternalId = ticket.ExternalId,
                    SystemId = ticket.UserName,
                    TransTime = _betlistHtmlBuilder.BuildTrans(ticket),
                    Status = _betlistHtmlBuilder.BuildLrfStatus(ticket, _ticketHelper, ticketResult.TicketDataList, metaModel),
                    Choice = _betlistHtmlBuilder.BuildChoice(ticket, _ticketHelper, ticketResult.TicketDataList, false),
                    Odds = _betlistHtmlBuilder.BuildOdds(ticket, ticketResult.TicketDataList, _cacheRefAdapter.GetOddsTypeName),
                    Stake = _betlistHtmlBuilder.BuildStake(ticket),
                    LicenseeWL = _betlistHtmlBuilder.BuildLicenseeWL(ticket),
                    WinLoss = _betlistHtmlBuilder.BuildCustomerWL(ticket, showPlayerComm),
                    BetEffective = _betlistHtmlBuilder.BuildBetEffective(ticket)
                };

                ticketTextList.Add(ticketText);
            }

            return ticketTextList;
        }

        protected internal virtual async Task<dynamic> GetBetListResult(BetListMetaModel metaModel, string uspKey)
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
            metaModel.IsShowPlayerComm = licenseeSession.IsAllowedCustomerComm;

            var betlistAPIUrl = $"http://localhost:55050/api/ApiBetListBase/GetBetList";
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(betlistAPIUrl,
                        new BetListResultMetaModel
                        {
                            CurrencyList = currencyList,
                            MetaModel = metaModel,
                            UspKey = uspKey
                        }).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<dynamic>().ConfigureAwait(false);
            }
        }

        protected internal virtual async Task<dynamic> GetPagingBetListResult(BetListMetaModel metaModel, string uspKey, int pageIndex, int pageSize)
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
            metaModel.IsShowPlayerComm = licenseeSession.IsAllowedCustomerComm;

            var param = new Hashtable();
            BetListParamBuilder.CreateBetListParameters(param, uspKey, metaModel);
            var betlistAPIUrl = $"http://localhost:55050/api/ApiBetList/GetBetListPaging";
            using (var client = new HttpClient())
            {
                var response = await client.PostAsJsonAsync(betlistAPIUrl,
                        new BetListResultMetaModel
                        {
                            CurrencyList = currencyList,
                            MetaModel = metaModel,
                            UspKey = uspKey,
                            Params = param
                        }).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<dynamic>().ConfigureAwait(false);
            }
        }

        protected bool IsValidCurrencyId(int currencyId)
        {
            return currencyId == 0 || SessionService.UserSession.CurrencyList.ContainsKey(currencyId);
        }
    }
}