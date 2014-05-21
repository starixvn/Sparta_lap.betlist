// --------------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using DNAide.Utils;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetList;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Service.Implement.BetList;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.BetList;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController : BaseApiController
    {
        /// <summary>
        /// Bet List Service.
        /// </summary>
        private readonly IBetListService _betListService;

        /// <summary>
        /// Balance up Service.
        /// </summary>
        private readonly IBalanceUpService _balanceUpService;

        /// <summary>
        /// Ticket Helper.
        /// </summary>
        private readonly ITicketHelper _ticketHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetListController" /> class.
        /// </summary>
        public BetListController()
        {
            _betListService = new BetListService();
            _balanceUpService = new BalanceUpService();
            _ticketHelper = new TicketHelper();
        }

        /// <summary>
        /// Render ticket data.
        /// </summary>
        /// <param name="ticketResult">TicketResult object.</param>
        /// <param name="metaModel">Meta model object.</param>
        /// <param name="startIndex">Starting value of row counter.</param>
        /// <param name="showOdds">Show odds or not.</param>
        /// <returns>List of TicketText.</returns>
        [NonAction]
        public List<TicketText> Ticket(TicketResult ticketResult, BetListMetaModel metaModel = null, int startIndex = 1, bool showOdds = false)
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
                    LicenseeWl = builder.BuildLicenseeWl(ticket),
                    WinLoss = builder.BuildCustomerWl(ticket, showPlayerComm),
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
        [NonAction]
        private dynamic GetBetListResult(BetListMetaModel metaModel, string uspKey)
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
                    SumLicenseeWl = ticketResult.SumLicenseeWl + ticketResult.SumComm
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
        [NonAction]
        private dynamic GetPagingBetListResult(BetListMetaModel metaModel, string uspKey, int pageIndex, int pageSize)
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
            var sumLicenseeWl = ticketResult.SumLicenseeWl + ticketResult.SumComm;
            var sumWinLossText = NumberFormat.DecFormat(sumWinLoss, 2, false);
            var sumLicenseeWlText = NumberFormat.DecFormat(sumLicenseeWl, 2, false);
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
                    LicenseeWl = sumLicenseeWlText
                }
            };
        }
    }
}