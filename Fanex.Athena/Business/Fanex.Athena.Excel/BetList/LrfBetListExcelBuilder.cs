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
    using System.Diagnostics.CodeAnalysis;
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
        public static BetListExcelBuilder AddCustomerWL(this BetListExcelBuilder builder, Ticket ticket, bool showPlayerComm, IFont font)
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

        public static BetListExcelBuilder AddBetEffective(this BetListExcelBuilder builder, LrfTicket ticket, IFont font)
        {
            string betEffective = string.Format("{0:#,##0.00}\n", ticket.BetEffective);

            var rtfBetEffective = new HSSFRichTextString(betEffective);
            rtfBetEffective.ApplyFont(font);
            builder.AddCell(rtfBetEffective);

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
        /// <param name="sumStake">The sum stake value.</param>
        /// <returns>Return the currency builder.</returns>
        public static BetListExcelBuilder AddFooterForSumStake(
            this BetListExcelBuilder builder,            
            decimal sumStake)
        {
            ICellStyle footerStyle = CreateFooterCellStyle(builder);

            var rtfTotal = new HSSFRichTextString(ViewResources.Total);
            for (int i = 0; i < 6; i++)
            {
                builder.AddCell(rtfTotal, footerStyle);
            }
            
            builder.AddCell(new HSSFRichTextString(string.Format(NumberFormat, sumStake)), footerStyle);

            // add the rest columns
            var emptyText = new HSSFRichTextString();
            builder.AddCell(emptyText, footerStyle);
            builder.AddCell(emptyText, footerStyle);
            builder.AddCell(emptyText, footerStyle);
            builder.AddCell(emptyText, footerStyle);

            int lastRowIndex = builder.LastContentRow;
            builder.MergeCell(new CellRangeAddress(lastRowIndex, lastRowIndex, 0, 5), footerStyle);

            return builder;
        }

        /// <summary>
        /// Add footer row.
        /// </summary>
        /// <param name="builder">The current builder.</param>
        /// <param name="sumCustomerWL">The total customer win-loss.</param>
        /// <param name="sumBetEffective">The total bet effective.</param>
        /// <param name="sumLicenseeWLComm">The total licensee win-loss.</param>
        /// <param name="redFont">The red font.</param>
        /// <param name="blackFont">The black font.</param>
        /// <returns>Return the currency builder.</returns>
        public static BetListExcelBuilder AddFooter(
            this BetListExcelBuilder builder,
            decimal sumCustomerWL,
            decimal sumBetEffective,
            decimal sumLicenseeWLComm,
            IFont redFont,
            IFont blackFont)
        {
            ICellStyle footerStyle = CreateFooterCellStyle(builder);

            var rtfTotal = new HSSFRichTextString(ViewResources.Total);
            for (int i = 0; i < 7; i++)
            {
                builder.AddCell(rtfTotal, footerStyle);
            }

            builder.AddCell(new HSSFRichTextString(string.Format(NumberFormat, sumCustomerWL)), footerStyle);

            builder.AddCell(new HSSFRichTextString(string.Format(NumberFormat, sumBetEffective)), footerStyle);

            var rtfSumLicenseeWl = new HSSFRichTextString(string.Format(NumberFormat, sumLicenseeWLComm));
            rtfSumLicenseeWl.ApplyFont(sumLicenseeWLComm >= 0 ? blackFont : redFont);
            builder.AddCell(rtfSumLicenseeWl, footerStyle);

            // add the rest column
            builder.AddCell(new HSSFRichTextString(), footerStyle);

            int lastRowIndex = builder.LastContentRow;
            builder.MergeCell(new CellRangeAddress(lastRowIndex, lastRowIndex, 0, 6), footerStyle);

            return builder;
        }

        /// <summary>
        /// Add header row.
        /// </summary>
        /// <param name="builder">The current builder.</param>
        /// <param name="custWLHeader">The customer win-loss header.</param>
        /// <returns>Return the current builder.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static BetListExcelBuilder AddHeader(this BetListExcelBuilder builder, string custWLHeader)
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
            var choiceCell = builder.AddCell(new HSSFRichTextString(ViewResources.Choice), headerStyle);
            builder.SetColumnWidth(choiceCell.ColumnIndex, 15500);

            // Odds
            var oddsCell = builder.AddCell(new HSSFRichTextString(ViewResources.Odds), headerStyle);
            builder.SetColumnWidth(oddsCell.ColumnIndex, 2600);

            // Stake
            var stakeCell = builder.AddCell(new HSSFRichTextString(ViewResources.Stake), headerStyle);
            builder.SetColumnWidth(stakeCell.ColumnIndex, 4200);

            // Customer Wl
            var customerWlCell = builder.AddCell(new HSSFRichTextString(custWLHeader), headerStyle);
            builder.SetColumnWidth(customerWlCell.ColumnIndex, 4200);

            // Effective Betting Value
            var betEffectiveCell = builder.AddCell(new HSSFRichTextString(ViewResources.EffectiveBettingValue), headerStyle);
            builder.SetColumnWidth(betEffectiveCell.ColumnIndex, 5400);

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
        public static BetListExcelBuilder AddLicenseeWL(
            this BetListExcelBuilder builder,
            LrfTicket ticket,
            bool is12Bet,
            IFont redFont,
            IFont blackFont)
        {
            bool isRunning = string.Compare(ticket.Status, "running", StringComparison.OrdinalIgnoreCase) == 0;
            bool isRacing = ticket.BetTypeId == (int)BetType.Win || ticket.BetTypeId == (int)BetType.Place ||
                            ticket.BetTypeId == (int)BetType.WinPlace;
            if ((isRacing && is12Bet) || ticket.BetTypeId == (int)BetType.BetTradeBought)
            {
                string licenseeWl = isRunning
                                    ? "-"
                                    : string.Format(NumberFormat, ticket.LicenseeWL);
                string comm = isRunning
                              ? "-"
                              : string.Format(NumberFormat, ticket.Comm);

                string licenseeWlText = string.Format("{0}\n{1}", licenseeWl, comm);

                var rtfLicenseeWl = new HSSFRichTextString(licenseeWlText);
                rtfLicenseeWl.ApplyFont(0, licenseeWl.Length, ticket.LicenseeWL >= 0 ? blackFont : redFont);
                rtfLicenseeWl.ApplyFont(licenseeWl.Length, licenseeWlText.Length, blackFont);

                builder.AddCell(rtfLicenseeWl);
            }
            else
            {
                string licenseeWl = isRunning
                                    ? "-"
                                    : string.Format(NumberFormat, ticket.LicenseeWL + ticket.Comm);
                var rtfLicenseeWl = new HSSFRichTextString(licenseeWl);
                rtfLicenseeWl.ApplyFont(ticket.LicenseeWL + ticket.Comm >= 0 ? blackFont : redFont);
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

        /// <summary>
        /// Create Footer Cell.
        /// </summary>
        /// <param name="builder">Bet list excel.</param>
        /// <returns>Footer Style.</returns>
        private static ICellStyle CreateFooterCellStyle(BetListExcelBuilder builder)
        {
            ICellStyle footerStyle = builder.CreateCellStyle();
            footerStyle.CloneStyleFrom(builder.EvenCellStyle);
            footerStyle.FillForegroundColor = 47;
            footerStyle.Alignment = HorizontalAlignment.Right;
            footerStyle.BorderTop = BorderStyle.None;
            footerStyle.BorderLeft = BorderStyle.None;
            footerStyle.BorderBottom = BorderStyle.None;
            footerStyle.SetFont(builder.RTFHelper.PosFont);
            return footerStyle;
        }
    }
}