// --------------------------------------------------------------------------
// <copyright file="LrfBetListHtmlBuilder.cs" company="Nexcel Solutions Vietnam">
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
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetList;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.BetList.Core.App_GlobalResources;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    #endregion Using

    /// <summary>
    /// Override the Bet List Html Builder.
    /// </summary>
    public static class LrfBetListHtmlBuilder
    {
        /// <summary>
        /// Get ticket status.
        /// </summary>
        /// <param name="builder">Bet list builder.</param>
        /// <param name="ticket">LRF ticket object.</param>
        /// <param name="ticketHelper">Ticket helper.</param>
        /// <param name="ticketData">List of ticket data.</param>
        /// <param name="metaModel">Meta model.</param>
        /// <returns>Ticket status text.</returns>
        [System.CLSCompliant(false)]
        public static string BuildLrfStatus(
            this BetListHTMLBuilder builder,
            LrfTicket ticket,
            ITicketHelper ticketHelper,
            List<ITicketData> ticketData,
            BetListMetaModel metaModel)
        {
            const string StatusTemp = "<div class='status'>{0}</div>";
            var statusText = GetStatusText(ticket);
            var statusNormalText = string.Format(StatusTemp, statusText);
            
            // If Sport type is keno, doesn't show "Result" link.
            if (ticket.SportTypeId == (int)SportType.Keno)
            {
                return statusNormalText;
            }

            switch (ticket.BetTypeId)
            {
                case 71:
                    const string CasinoHandTemplate =
                        "<div id='casino-hand-history-{0}' class='result casino-hand-history-link' url-param='{1}' onclick='BetList.ViewCasinoHandHistory({0});'>{2}</div>";
                    return statusNormalText + string.Format(
                        CasinoHandTemplate,
                        ticket.RefNo,
                        GetCasinoHandHistoryParam(ticket, metaModel),
                        ViewResources.Results);
                case 72:
                case 73:
                    return statusNormalText;

                default:
                    return builder.BuildStatus(ticket, ticketHelper, ticketData, false);
            }
        }

        /// <summary>
        /// Build licensee win lost html.
        /// </summary>
        /// <param name="builder">Bet list builder.</param>
        /// <param name="ticket">LRF ticket object.</param>
        /// <returns>Licensee win loss text.</returns>
        [System.CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        public static string BuildLicenseeWL(this BetListHTMLBuilder builder, LrfTicket ticket)
        {
            var running = string.Compare(ticket.Status, "running", StringComparison.OrdinalIgnoreCase) == 0;
            var racing = ticket.BetTypeId == (int)BetType.Win || ticket.BetTypeId == (int)BetType.Place ||
                           ticket.BetTypeId == (int)BetType.WinPlace;
            if (!racing && ticket.BetTypeId != (int)BetType.BetTradeBought)
            {
                return running
                    ? "-"
                    : string.Format(
                        "<div class='lic-win-loss'>{0}</div>",
                        FormatLicenseeWl(ticket.LicenseeWL + ticket.Comm));
            }

            var licenseeWl = running
                ? "-"
                : FormatLicenseeWl(ticket.LicenseeWL);
            var comm = running
                ? "-"
                : string.Format("{0:#,##0.00}", ticket.Comm);

            return string.Format("<div class ='lic-win-loss'>{0}<br/>{1}</div>", licenseeWl, comm);
        }

        /// <summary>
        /// Build customer win lost html.
        /// </summary>
        /// <param name="builder">Bet list builder.</param>
        /// <param name="ticket">LRF Ticket object.</param>
        /// <param name="showPlayerComm">Show player commission or not.</param>
        /// <returns>Customer win loss text.</returns>
        [System.CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        public static string BuildCustomerWL(this BetListHTMLBuilder builder, Ticket ticket, bool showPlayerComm)
        {
            var running = string.Compare(ticket.Status, "running", StringComparison.OrdinalIgnoreCase) == 0;
            var winloss = running
                            ? "-"
                            : string.Format("<div class='underdog'>{0:#,##0.00}</div>", ticket.Winlost);

            if (!showPlayerComm)
            {
                return winloss;
            }

            winloss += running
                ? string.Empty
                : string.Format("<div class='underdog'>{0:#,##0.00}</div>", ticket.PlayerComm);

            return winloss;
        }

        [System.CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        public static string BuildBetEffective(this BetListHTMLBuilder builder, LrfTicket ticket)
        {
            return string.Format("<div class='underdog'>{0:#,##0.00}</div>", ticket.BetEffective);
        }

        #region Private Utility

        /// <summary>
        /// Get Casino Hand History Parameter.
        /// </summary>
        /// <param name="ticket">LRF Ticket object.</param>
        /// <param name="metaModel">Meta model object.</param>
        /// <returns>Parameter for Casino Hand History.</returns>
        private static string GetCasinoHandHistoryParam(LrfTicket ticket, BetListMetaModel metaModel)
        {
            var param = string.Empty;
            param += "?RefNo=" + ticket.RefNo;
            param += "&FromDate=" + metaModel.FromDate;
            param += "&ToDate=" + metaModel.ToDate;
            param += "&SystemId=" + (string.IsNullOrEmpty(ticket.UserName) ? metaModel.SystemId : ticket.UserName);
            param += "&ExternalId=" + (string.IsNullOrEmpty(ticket.ExternalId) ? metaModel.ExternalId : ticket.ExternalId);
            param += "&CustId=" + (ticket.CustId == 0 ? metaModel.CustId : ticket.CustId);
            param += "&CurrencyId=" + metaModel.CurrencyId;
            param += "&CurrencyName=" + metaModel.CurrencyName;
            param += "&BaseCurrency=" + metaModel.BaseCurrency;
            return param;
        }

        /// <summary>
        /// Format licensee win lost, show red text if the licensee win lost is negative.
        /// </summary>
        /// <param name="licenseeWl">Licensee win loss value.</param>
        /// <returns>Licensee win loss text.</returns>
        private static string FormatLicenseeWl(decimal licenseeWl)
        {
            return licenseeWl < 0
                ? string.Join(
                    null,
                    new[] { "<span class='negative-number'>", string.Format("{0:#,##0.00}", licenseeWl), "</span>" })
                : string.Format("{0:#,##0.00}", licenseeWl);
        }

        /// <summary>
        /// Get status text by status id.
        /// </summary>
        /// <param name="ticket">LRF ticket object.</param>
        /// <returns>Status text.</returns>
        private static string GetStatusText(LrfTicket ticket)
        {
            var statusText = string.Empty;

            switch (ticket.Status.ToLower())
            {
                case BetStatus.Running:
                    statusText = CoreBetList.running;
                    break;

                case BetStatus.Reject:
                    statusText = CoreBetList.reject;
                    break;

                case BetStatus.Won:
                    statusText = CoreBetList.won;
                    break;

                case BetStatus.Lose:
                    statusText = CoreBetList.lose;
                    break;

                case BetStatus.Draw:
                    statusText = CoreBetList.draw;
                    break;

                case BetStatus.Waiting:
                    statusText = CoreBetList.waiting;
                    break;

                case BetStatus.Void:
                    statusText = CoreBetList.voided;
                    break;

                case BetStatus.Refund:
                    statusText = CoreBetList.refund;
                    break;
            }

            return statusText;
        }

        #endregion Private Utility
    }
}