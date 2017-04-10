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
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Service.BetList;
    using Fanex.Athena.Service.BetList.Extensions;
    using Fanex.Athena.Service.BetList.Interfaces;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Helpers;

    #endregion Using

    public class ApiBetListBaseController : BaseApiController
    {
        private static readonly int[] RacingBetType = { 31, 32, 33, 41, 42, 43 };

        private readonly IBetListService _betListService;

        private readonly IBalanceUpService _balanceUpService;

        private readonly ITicketHelper _ticketHelper;

        public ApiBetListBaseController()
        {
            _betListService = new BetListService();
            _balanceUpService = new BalanceUpService();
            _ticketHelper = new TicketHelper();
        }

        [System.CLSCompliant(false)]
        protected IBetListService BetListService
        {
            get { return _betListService; }
        }

        protected IBalanceUpService BalanceUpService
        {
            get { return _balanceUpService; }
        }

        protected static bool IsRacing(int betType)
        {
            return RacingBetType.Contains(betType);
        }

        protected List<TicketText> Ticket(TicketResult ticketResult, BetListMetaModel metaModel = null, int startIndex = 1, bool showOdds = false)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var showPlayerComm = licenseeSession.IsAllowedCustomerComm;
            var baseCurrency = metaModel == null ? string.Empty : metaModel.BaseCurrency;
            var builder = new BetListHTMLBuilder();
            var ticketTextList = new List<TicketText>();

            _ticketHelper.IsShowOddsWithChoice = showOdds;

            foreach (var ticket in ticketResult.MainTicketList)
            {
                if (ticket.BetTypeId == (int)BetType.CombinationMixParlay && metaModel != null)
                {
                    var custid = ticket.CustId != 0 ? ticket.CustId : metaModel.CustId;
                    _ticketHelper.ClientDetailSystemParlayFunction = BetListHelper.GenerateClientDetailSystemParlayFunction(_ticketHelper, baseCurrency, ticket, custid);
                }

                _ticketHelper.IsShowOddsWithChoice = showOdds;

                var ticketText = new TicketText
                {
                    Index = startIndex++,
                    ExternalId = ticket.ExternalId,
                    SystemId = ticket.UserName,
                    TransTime = builder.BuildTrans(ticket),
                    Status = builder.BuildLrfStatus(ticket, _ticketHelper, ticketResult.TicketDataList, metaModel),
                    Choice = builder.BuildChoice(ticket, _ticketHelper, ticketResult.TicketDataList, false),
                    Odds = builder.BuildOdds(ticket, ticketResult.TicketDataList, CacheRef.GetOddsTypeName),
                    Stake = builder.BuildStake(ticket),
                    LicenseeWL = builder.BuildLicenseeWL(ticket),
                    WinLoss = builder.BuildCustomerWL(ticket, showPlayerComm),
                    BetEffective = builder.BuildBetEffective(ticket)
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
        protected dynamic GetBetListResult(BetListMetaModel metaModel, string uspKey)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var balanceUpInfo = _balanceUpService.BalanceUpInfo;

            metaModel.PageIndex = 1;
            metaModel.PageSize = int.MaxValue - 1;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUpInfo.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUpInfo.MoveBetsDay.Date, DateTimeKind.Utc);

            var ticketResult = _betListService.GetBetListData(metaModel, uspKey);

            var leagueName = string.Empty;
            var firstOrDefault = ticketResult.MainTicketList.FirstOrDefault();
            if (firstOrDefault != null)
            {
                leagueName = CacheRef.GetLeagueName(firstOrDefault.LeagueId);
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

        protected dynamic GetPagingBetListResult(BetListMetaModel metaModel, string uspKey, int pageIndex, int pageSize)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var balanceUpInfo = _balanceUpService.BalanceUpInfo;

            metaModel.PageIndex = pageIndex < 1 ? 1 : pageIndex;
            metaModel.PageSize = pageSize < 1 ? 100 : pageSize;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUpInfo.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUpInfo.MoveBetsDay.Date, DateTimeKind.Utc);

            var ticketResult = _betListService.GetBetListData(metaModel, uspKey);

            var totalPage = (ticketResult.TotalRows % metaModel.PageSize) == 0
                ? ticketResult.TotalRows / metaModel.PageSize
                : (ticketResult.TotalRows / metaModel.PageSize) + 1;
            metaModel.PageIndex = pageIndex > totalPage ? totalPage : pageIndex;

            var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;

            var sumWinLoss = ticketResult.SumWinLoss + ticketResult.SumPlayerComm;
            var sumLicenseeWl = ticketResult.SumLicenseeWL + ticketResult.SumComm;
            var sumWinLossText = NumberHelper.DecFormat(sumWinLoss, 2);
            var sumLicenseeWlText = NumberHelper.DecFormat(sumLicenseeWl, 2);
            sumLicenseeWlText = sumLicenseeWl < 0
                ? string.Format("<span class='negative'>{0}</span>", sumLicenseeWlText)
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
    }
}