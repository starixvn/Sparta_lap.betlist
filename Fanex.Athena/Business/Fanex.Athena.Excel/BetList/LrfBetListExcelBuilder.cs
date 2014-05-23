// --------------------------------------------------------------------------
// <copyright file="LrfBetListExcelBuilder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.BetList
{
    using System;
    using System.Globalization;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Models.DataDomain;
    using Models.DTO.BetList;
    using Models.ViewModel;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// Excel bet-list builder.
    /// </summary>
    public static class LrfBetListExcelBuilder
    {
        private const string NumberFormat = "{0:#,##0.00}";

        /// <summary>
        /// Add more customer win-loss cell.
        /// </summary>
        /// <param name="builder">The current builder.</param>
        /// <param name="ticket">The ticket.</param>
        /// <param name="showPlayerComm">If it equals "True" , Showing player commission.</param>
        /// <param name="font">The font style.</param>
        /// <returns>Return the current builder.</returns>
        public static BetListExcelBuilder AddCustomerWl(this BetListExcelBuilder builder, LrfTicket ticket, bool showPlayerComm, IFont font)
        {
            var isRunning = string.Compare(ticket.Status, "running", StringComparison.OrdinalIgnoreCase) == 0;
            string winloss = isRunning
                             ? "-"
                             : string.Format("{0:#,##0.00}\n", ticket.Winlost);

            if (showPlayerComm)
            {
                string comm = isRunning ? "-" : string.Format("{0:#,##0.00}", ticket.PlayerComm);
                winloss += comm;
            }

            var rtfCustomerWl = new HSSFRichTextString(winloss);
            rtfCustomerWl.ApplyFont(font);
            builder.AddCell(rtfCustomerWl);

            return builder;
        }

        public static BetListExcelBuilder AddExternalId(
                this BetListExcelBuilder builder,
                ITicketHelper ticketHelper,
                string externalId,
                ICellStyle oddStyle,
                ICellStyle evenStyle)
        {
            var rtfExternalId = new HSSFRichTextString(externalId);
            builder.AddCell(rtfExternalId, ticketHelper.Index % 2 == 0 ? oddStyle : evenStyle);

            return builder;
        }

        /// <summary>
        /// Add footer row.
        /// </summary>
        /// <param name="builder">The current builder.</param>
        /// <param name="sumCustomerWl">The total customer win-loss.</param>
        /// <param name="sumLicenseeWlComm">The total licensee win-loss.</param>
        /// <param name="redFont">The red font.</param>
        /// <param name="blackFont">The black font.</param>
        /// <returns>Return the currency builder.</returns>
        public static BetListExcelBuilder AddFooter(
            this BetListExcelBuilder builder,
            decimal sumCustomerWl,
            decimal sumLicenseeWlComm,
            IFont redFont,
            IFont blackFont)
        {
            ICellStyle footerStyle = builder.CreateCellStyle();
            footerStyle.CloneStyleFrom(builder.EvenCellStyle);
            footerStyle.FillForegroundColor = 47;
            footerStyle.Alignment = HorizontalAlignment.Right;
            footerStyle.BorderTop = BorderStyle.None;
            footerStyle.BorderLeft = BorderStyle.None;
            footerStyle.BorderBottom = BorderStyle.None;
            footerStyle.SetFont(builder.RTFHelper.PosFont);

            // Total Text
            var rtfTotal = new HSSFRichTextString(ViewResources.Total);
            for (int i = 0; i < 7; i++)
            {
                builder.AddCell(rtfTotal, footerStyle);
            }

            // Sum Customer Wl
            var rtfSumCustomerWl = new HSSFRichTextString(string.Format(NumberFormat, sumCustomerWl));
            builder.AddCell(rtfSumCustomerWl, footerStyle);

            // Sum Licensee Wl
            var rtfSumLicenseeWl = new HSSFRichTextString(string.Format(NumberFormat, sumLicenseeWlComm));
            rtfSumLicenseeWl.ApplyFont(sumLicenseeWlComm >= 0 ? blackFont : redFont);
            builder.AddCell(rtfSumLicenseeWl, footerStyle);

            // Last footer cell
            builder.AddCell(new HSSFRichTextString(), footerStyle);

            // Merge cells
            int lastRowIndex = builder.LastContentRow;
            builder.MergeCell(new CellRangeAddress(lastRowIndex, lastRowIndex, 0, 6), footerStyle);

            // Return
            return builder;
        }

        /// <summary>
        /// Add header row.
        /// </summary>
        /// <param name="builder">The current builder.</param>
        /// <param name="custWlHeader">The customer win-loss header.</param>
        /// <returns>Return the current builder.</returns>
        public static BetListExcelBuilder AddHeader(this BetListExcelBuilder builder, string custWlHeader)
        {
            var headerStyle = builder.CreateCellStyle();
            headerStyle.CloneStyleFrom(builder.EvenCellStyle);
            headerStyle.FillForegroundColor = 44;
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.SetFont(builder.RTFHelper.PosFont);

            // Index
            var indexCell = builder.AddCell(new HSSFRichTextString("#"), headerStyle);
            builder.SetColumnWidth(indexCell.ColumnIndex, 1200);

            // External Id
            var externalIdCell = builder.AddCell(new HSSFRichTextString(ViewResources.ExternalId), headerStyle);
            builder.SetColumnWidth(externalIdCell.ColumnIndex, 4800);

            // System Id
            var systemIdCell = builder.AddCell(new HSSFRichTextString(ViewResources.SystemId), headerStyle);
            builder.SetColumnWidth(systemIdCell.ColumnIndex, 4800);

            // Trans. Time
            var transTimeCell = builder.AddCell(new HSSFRichTextString(ViewResources.TransTime), headerStyle);
            builder.SetColumnWidth(transTimeCell.ColumnIndex, 5400);

            // Choice
            CellAddress choiceCell = builder.AddCell(new HSSFRichTextString(ViewResources.Choice), headerStyle);
            builder.SetColumnWidth(choiceCell.ColumnIndex, 15500);

            // Odds
            var oddsCell = builder.AddCell(new HSSFRichTextString(ViewResources.Odds), headerStyle);
            builder.SetColumnWidth(oddsCell.ColumnIndex, 2600);

            // Stake
            var stakeCell = builder.AddCell(new HSSFRichTextString(ViewResources.Stake), headerStyle);
            builder.SetColumnWidth(stakeCell.ColumnIndex, 4200);

            // Customer Wl
            var customerWlCell = builder.AddCell(new HSSFRichTextString(custWlHeader), headerStyle);
            builder.SetColumnWidth(customerWlCell.ColumnIndex, 4200);

            // Licensee Wl
            var licenseeWlCell = builder.AddCell(new HSSFRichTextString(ViewResources.LicenseeWL + "/" + ViewResources.Comm), headerStyle);
            builder.SetColumnWidth(licenseeWlCell.ColumnIndex, 5400);

            // Status
            CellAddress statusCell = builder.AddCell(new HSSFRichTextString(ViewResources.Status), headerStyle);
            builder.SetColumnWidth(statusCell.ColumnIndex, 3400);

            return builder;
        }

        public static BetListExcelBuilder AddIndex(this BetListExcelBuilder builder, ITicketHelper ticketHelper, int index)
        {
            var rtfNo = new HSSFRichTextString(index.ToString(CultureInfo.InvariantCulture));
            builder.AddCell(
                            rtfNo,
                            ticketHelper.Index % 2 == 0 ? builder.OddCellStyleCenterAligned : builder.EvenCellStyleCenterAligned);

            return builder;
        }

        /// <summary>
        ///     Add Licensee win lost cell.
        /// </summary>
        /// <param name="builder">Bet List Excel builder.</param>
        /// <param name="ticket">Ticket helper.</param>
        /// <param name="is12Bet">The licensee is 12bet or not.</param>
        /// <param name="redFont">The font for negative licensee win lost.</param>
        /// <param name="blackFont">The font for positive licensee win lost.</param>
        /// <returns>Bet list excel builder.</returns>
        public static BetListExcelBuilder AddLicenseeWl(
            this BetListExcelBuilder builder,
            LrfTicket ticket,
            bool is12Bet,
            IFont redFont,
            IFont blackFont)
        {
            bool isRunning = string.Compare(ticket.Status, "running", StringComparison.OrdinalIgnoreCase) == 0;
            bool isRacing = ticket.BetTypeId == (int)BetType.Win || ticket.BetTypeId == (int)BetType.Place ||
                            ticket.BetTypeId == (int)BetType.WinPlace;
            if (isRacing && is12Bet)
            {
                string licenseeWl = isRunning
                                    ? "-"
                                    : string.Format(NumberFormat, ticket.LicenseeWl);
                string comm = isRunning
                              ? "-"
                              : string.Format(NumberFormat, ticket.Comm);

                string licenseeWlText = string.Format("{0}\n{1}", licenseeWl, comm);

                var rtfLicenseeWl = new HSSFRichTextString(licenseeWlText);
                rtfLicenseeWl.ApplyFont(0, licenseeWl.Length, ticket.LicenseeWl >= 0 ? blackFont : redFont);
                rtfLicenseeWl.ApplyFont(licenseeWl.Length, licenseeWlText.Length, blackFont);

                builder.AddCell(rtfLicenseeWl);
            }
            else
            {
                string licenseeWl = isRunning
                                    ? "-"
                                    : string.Format(NumberFormat, ticket.LicenseeWl + ticket.Comm);
                var rtfLicenseeWl = new HSSFRichTextString(licenseeWl);
                rtfLicenseeWl.ApplyFont(ticket.LicenseeWl + ticket.Comm >= 0 ? blackFont : redFont);
                builder.AddCell(rtfLicenseeWl);
            }

            return builder;
        }

        public static BetListExcelBuilder AddSystemId(
            this BetListExcelBuilder builder,
            ITicketHelper ticketHelper,
            string systemId,
            ICellStyle oddStyle,
            ICellStyle evenStyle)
        {
            var rtfSystemId = new HSSFRichTextString(systemId);
            builder.AddCell(rtfSystemId, ticketHelper.Index % 2 == 0 ? oddStyle : evenStyle);

            return builder;
        }
    }
}