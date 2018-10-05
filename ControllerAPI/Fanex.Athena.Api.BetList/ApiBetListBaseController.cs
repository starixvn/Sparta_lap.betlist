// --------------------------------------------------------------------------
// <copyright file="ApiBetListBaseController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Constant.DataDomain;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Service.BetList.Extensions;
    using Fanex.Athena.Service.BetList.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
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

        /// <summary>
        /// Get Bet List Result.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="uspKey">Stored procedure key.</param>
        /// <returns>Dynamic object, contains the bet list result.</returns>
        protected internal virtual dynamic GetBetListResult(BetListMetaModel metaModel, string uspKey)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var balanceUp = GetBalanceUp();

            metaModel.PageIndex = 1;
            metaModel.PageSize = int.MaxValue - 1;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUp.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUp.MoveBetsDay.Date, DateTimeKind.Utc);
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var ticketResult = _betListService.GetBetListData(metaModel, uspKey);

            var leagueName = string.Empty;
            var firstOrDefault = ticketResult.MainTicketList.FirstOrDefault();
            if (firstOrDefault != null)
            {
                leagueName = _cacheRefAdapter.GetLeagueName(firstOrDefault.LeagueId);
            }

            return new
            {
                Main = Ticket(ticketResult, metaModel),
                Total = new
                {
                    SumWinLoss = ticketResult.SumWinLoss + ticketResult.SumPlayerComm,
                    SumLicenseeWl = ticketResult.SumLicenseeWL + ticketResult.SumComm,
                    SumBetEffective = ticketResult.SumBetEffective,
                    SumStake = ticketResult.MainTicketList.Sum(x => Math.Round(x.Stake, 2))
                },
                LeagueName = leagueName,
            };
        }

        protected internal virtual dynamic GetPagingBetListResult(BetListMetaModel metaModel, string uspKey, int pageIndex, int pageSize)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var balanceUp = GetBalanceUp();

            metaModel.IsServerPaging = true;
            metaModel.PageIndex = pageIndex < 1 ? 1 : pageIndex;
            metaModel.PageSize = pageSize < 1 ? 100 : pageSize;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUp.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUp.MoveBetsDay.Date, DateTimeKind.Utc);
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

            var ticketResult = _betListService.GetBetListData(metaModel, uspKey);

            var totalPage = (int)Math.Ceiling((double)ticketResult.TotalRows / metaModel.PageSize);
            metaModel.PageIndex = pageIndex > totalPage ? totalPage : pageIndex;

            var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;

            var sumWinLoss = ticketResult.SumWinLoss + ticketResult.SumPlayerComm;
            var sumLicenseeWl = ticketResult.SumLicenseeWL + ticketResult.SumComm;
            var sumWinLossText = NumberHelper.DecFormat(sumWinLoss, 2);
            var sumLicenseeWlText = NumberHelper.DecFormat(sumLicenseeWl, 2);
            sumLicenseeWlText = sumLicenseeWl < 0
                ? string.Format("<span class='negative-number'>{0}</span>", sumLicenseeWlText)
                : sumLicenseeWlText;

            return new
            {
                page = pageIndex,
                total = totalPage,
                records = ticketResult.TotalRows,
                rows = Ticket(ticketResult, metaModel, startIndex),
                userdata = new
                {
                    WinLoss = sumWinLossText,
                    LicenseeWL = sumLicenseeWlText
                }
            };
        }

        protected bool IsValidCurrencyId(int currencyId)
        {
            return currencyId == 0 || SessionService.UserSession.CurrencyList.ContainsKey(currencyId);
        }
    }
}