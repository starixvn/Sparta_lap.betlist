// --------------------------------------------------------------------------
// <copyright file="BetListController.RunningBetListByMultipleBettype.cs" company="Nexcel Solutions Vietnam">
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
    using System.Linq;
    using System.Web.Http;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel.BetList;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class BetListController
    {
        /// <summary>
        /// Get data for running bet list of a match.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <returns>Dynamic object, contains the bet list result.</returns>
        [HttpPost]
        public dynamic RunningBetListByMultipleBettype(BetListMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            var currencyList = userSession.CurrencyList;

            if (metaModel.CurrencyId != 0 && !currencyList.Values.Contains(metaModel.CurrencyId))
            {
                return null;
            }

            metaModel.UserId = userSession.UserId;
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;

            string uspKey = "Lrf_BetList_SelRunningByBettypeMatch";
            return GetBetListResultByMultipleBettype(metaModel, uspKey);
        }

        /// <summary>
        /// Get Bet List Result by multiple bet type.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="uspKey">Stored procedure key.</param>
        /// <returns>Dynamic object, contains the bet list result.</returns>
        private dynamic GetBetListResultByMultipleBettype(BetListMetaModel metaModel, string uspKey)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var balanceUpInfo = _balanceUpService.BalanceUpInfo;

            metaModel.PageIndex = 1;
            metaModel.PageSize = int.MaxValue - 1;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUpInfo.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUpInfo.MoveBetsDay.Date, DateTimeKind.Utc);

            var ticketResult = _betListService.GetBetListData(metaModel, uspKey);

            // Add more first half data
            if (metaModel.BetType == (int)BetType.TotalGoal || metaModel.BetType == (int)BetType.FirstGoalLastGoal)
            {
                metaModel.BetType = metaModel.BetType == (int)BetType.TotalGoal ? (int)BetType.FirstHalfTotalGoal : (int)BetType.FirstHalFirsGoalLastGoal;
                var firstHalfResult = _betListService.GetBetListData(metaModel, uspKey);
                ticketResult.MainTicketList.AddRange(firstHalfResult.MainTicketList);
                ticketResult.TicketDataList.AddRange(firstHalfResult.TicketDataList);
                ticketResult.TotalRows += firstHalfResult.TotalRows;
                ticketResult.SumWinLoss += firstHalfResult.SumWinLoss;
                ticketResult.SumLicenseeWl += firstHalfResult.SumLicenseeWl;
                ticketResult.SumComm += firstHalfResult.SumComm;
                ticketResult.SumPlayerComm += firstHalfResult.SumPlayerComm;
                ticketResult.SumStake += firstHalfResult.SumStake;
            }

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
    }
}