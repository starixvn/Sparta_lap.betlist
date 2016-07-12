// --------------------------------------------------------------------------
// <copyright file="BaseBetList.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Base
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using BetList;
    using Entities;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Model.BetList.DTO;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using Resources;

    /// <summary>
    /// Base bet list.
    /// </summary>
    public abstract class BaseBetList : ExcelBase
    {
        private const int ColumnWinlossIndex = 7;

        private readonly IList<int> _hiddenColumnIndexes;
        private readonly bool _is12Bet;
        private readonly ITicketHelper _ticketHelper;

        private ICellStyle _evenCellStyleLeftAligned;

        private bool _isShowSumStake;

        // Default Fonts
        private IFont _negativeWinLossFont;

        private ICellStyle _oddCellStyleLeftAligned;
        private IFont _winLossFont;

        protected BaseBetList(ShowingOption showingOption, bool is12Bet, IList<int> hiddenColumnIndexes = null, bool isShowSumStake = false)
        {
            _is12Bet = is12Bet;
            _hiddenColumnIndexes = hiddenColumnIndexes;
            _ticketHelper = new TicketHelper();
            _isShowSumStake = isShowSumStake;
            ShowingOption = showingOption;
        }

        public static void AddNoInfoRow(BetListExcelBuilder builder)
        {
            ICellStyle titleStyle = builder.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.Center;

            builder.AddCell(new HSSFRichTextString(ViewResources.NoInfo), titleStyle);
            builder.MergeCell(new CellRangeAddress(2, 2, 0, 9));
        }

        /// <summary>
        /// Add title.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="title">The title.</param>
        public static void AddTitle(BetListExcelBuilder builder, string title)
        {
            IFont font = builder.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.Orange.Index;

            ICellStyle titleStyle = builder.CreateCellStyle();
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.SetFont(font);

            builder.AddCell(new HSSFRichTextString(title), titleStyle);
            builder.MergeCell(new CellRangeAddress(0, 0, 0, 10));
        }

        public IWorkbook Export(TicketResult ticketResult, string title, int startIndex = 1)
        {
            BetListExcelBuilder builder = Build(ticketResult, title, startIndex);
            IWorkbook workbook = builder.GetBetList();

            ProcessUi(workbook);

            return workbook;
        }

        /// <summary>
        /// Build bet list.
        /// </summary>
        /// <param name="ticketResult">The ticket result.</param>
        /// <param name="title">The title.</param>
        /// <param name="startIndex">The start index.</param>
        /// <returns>The bet list builder.</returns>
        [SuppressMessage(
            "StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        protected BetListExcelBuilder Build(TicketResult ticketResult, string title, int startIndex = 1)
        {
            var builder = new BetListExcelBuilder();

            // Initialize styles & fonts
            InitializeFonts(builder);
            InitializeCellStyles(builder);

            builder.BuildWorksheet(ViewResources.BetList, 11);
            AddTitle(builder, title);

            string custWlheader = ViewResources.WinLoss
                                  + (ShowingOption.CustomerCommVisible ? "/" + ViewResources.Comm : string.Empty);
            builder.AddHeader(custWlheader);

            // Content
            if (ticketResult.MainTicketList.Any())
            {
                _ticketHelper.Index = startIndex;
                foreach (var ticket in ticketResult.MainTicketList)
                {
                    builder
                    .AddIndex(_ticketHelper, _ticketHelper.Index)
                    .AddExternalId(_ticketHelper, ticket.ExternalId, _oddCellStyleLeftAligned, _evenCellStyleLeftAligned)
                    .AddSystemId(_ticketHelper, ticket.UserName, _oddCellStyleLeftAligned, _evenCellStyleLeftAligned)
                    .AddTrans(ticket, _ticketHelper)
                    .AddChoice(ticket, _ticketHelper, ticketResult.TicketDataList)
                    .AddOdds(ticket, ticketResult.TicketDataList, CacheRef.GetOddsTypeName)
                    .AddStake(ticket)
                    .AddCustomerWL(ticket, ShowingOption.CustomerCommVisible, _winLossFont)
                    .AddBetEffective(ticket, _winLossFont)
                    .AddLicenseeWL(ticket, _is12Bet, _negativeWinLossFont, _winLossFont)
                    .AddStatus(ticket, _ticketHelper, ticketResult.TicketDataList, false);

                    _ticketHelper.Index++;
                }

                if (_isShowSumStake)
                {
                    builder.AddFooterForSumStake(ticketResult.MainTicketList.Sum(x => x.Stake));
                }
                else if (ShowingOption.GrandTotalVisible && (_hiddenColumnIndexes == null || !_hiddenColumnIndexes.Contains(ColumnWinlossIndex)))
                {
                    var sumWl = ticketResult.SumWinLoss + ticketResult.SumPlayerComm;
                    var sumLicenseeWl = ticketResult.SumLicenseeWL + ticketResult.SumComm;
                    var sumBetEffective = ticketResult.SumBetEffective;
                    builder.AddFooter(sumWl, sumBetEffective, sumLicenseeWl, _negativeWinLossFont, _winLossFont);
                }
            }
            else
            {
                AddNoInfoRow(builder);
            }

            return builder;
        }

        /// <summary>
        /// Initialize cell styles.
        /// </summary>
        /// <param name="builder">The builder.</param>
        private void InitializeCellStyles(BetListExcelBuilder builder)
        {
            _oddCellStyleLeftAligned = builder.CreateCellStyle();
            _oddCellStyleLeftAligned.CloneStyleFrom(builder.OddCellStyle);
            _oddCellStyleLeftAligned.Alignment = HorizontalAlignment.Left;
            _oddCellStyleLeftAligned.SetFont(builder.RTFHelper.NormalFont);
            _oddCellStyleLeftAligned.WrapText = true;

            _evenCellStyleLeftAligned = builder.CreateCellStyle();
            _evenCellStyleLeftAligned.CloneStyleFrom(builder.EvenCellStyle);
            _evenCellStyleLeftAligned.Alignment = HorizontalAlignment.Left;
            _evenCellStyleLeftAligned.SetFont(builder.RTFHelper.NormalFont);
            _evenCellStyleLeftAligned.WrapText = true;
        }

        private void InitializeFonts(BetListExcelBuilder builder)
        {
            _negativeWinLossFont = builder.CreateFont();
            _negativeWinLossFont.Boldweight = (short)FontBoldWeight.Bold;
            _negativeWinLossFont.Color = (short)FontColor.Red;

            _winLossFont = builder.CreateFont();
            _winLossFont.Boldweight = (short)FontBoldWeight.Bold;
            _winLossFont.Color = (short)FontColor.None;
        }

        /// <summary>
        /// Processing UI.
        /// </summary>
        /// <param name="workbook">The work-book.</param>
        private void ProcessUi(IWorkbook workbook)
        {
            if (_hiddenColumnIndexes != null)
            {
                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    ISheet sheet = workbook.GetSheetAt(i);

                    HideColumn(sheet, _hiddenColumnIndexes.ToArray());
                }
            }
        }
    }
}