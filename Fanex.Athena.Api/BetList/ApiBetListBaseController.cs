// --------------------------------------------------------------------------
// <copyright file="ApiBetListBaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetList;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Implement.BetList;
    using Fanex.Athena.Service.Interface.BetList;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;

    #endregion Using

    public partial class ApiBetListBaseController : BaseApiController
    {
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

        [CLSCompliant(false)]
        protected ITicketHelper TicketHelper
        {
            get { return _ticketHelper; }
        }

        /// <summary>
        /// Render ticket data.
        /// </summary>
        /// <param name="ticketResult">TicketResult object.</param>
        /// <param name="metaModel">Meta model object.</param>
        /// <param name="startIndex">Starting value of row counter.</param>
        /// <param name="showOdds">Show odds or not.</param>
        /// <returns>List of TicketText.</returns>
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
                    const string FormatDetail = @"showCombMPDetail({0},{1},{2},{3},&quot;{4}&quot;,&quot;{5}&quot;)";
                    var betTeam = _ticketHelper.GetBetTypeNameById(ticket.BetTypeId) + " - " +
                                  _ticketHelper.GetCombinationMPOfferName(ticket.BetTeam);
                    _ticketHelper.ClientDetailSystemParlayFunction = string.Format(
                        FormatDetail,
                        ticket.TransId,
                        ticket.RefNo,
                        ticket.BetId,
                        custid,
                        betTeam,
                        baseCurrency);
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

        /// <summary>
        /// Get Paging Bet List Result.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="uspKey">Stored procedure key.</param>
        /// <param name="pageIndex">Page Index.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Dynamic object, contains the bet list result.</returns>
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