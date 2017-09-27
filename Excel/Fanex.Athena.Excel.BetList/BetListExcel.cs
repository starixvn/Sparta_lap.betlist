// --------------------------------------------------------------------------
// <copyright file="BaseBetList.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.BetList
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Base.Extension;
    using Excel.Base.Entities;
    using Extension.Utility;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Model.BetList.DTO;
    using Model.Core.DataDomain;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using Resources;

    public class BetListExcel
    {
        private const int ColumnSystemIdIndex = 2;

        private const int ColumnWinlossIndex = 7;

        private readonly IEnumerable<int> _hiddenColumnIndexes;

        private readonly int _licenseeId;

        private readonly ITicketHelper _ticketHelper;

        private readonly IAppSettings _appSettings;

        private readonly bool _isShowSumStake;

        private readonly ShowingOption _showingOption;

        private ICellStyle _evenCellStyleLeftAligned;

        // Default Fonts
        private IFont _negativeWinLossFont;

        private ICellStyle _oddCellStyleLeftAligned;

        private IFont _winLossFont;

        public BetListExcel(ShowingOption showingOption, int licenseeId, IEnumerable<int> hiddenColumnIndexes, bool isShowSumStake = false)
        {
            _appSettings = new AppSettings();
            _licenseeId = licenseeId;
            _ticketHelper = new TicketHelper();
            _isShowSumStake = isShowSumStake;
            _showingOption = showingOption;
            _hiddenColumnIndexes = hiddenColumnIndexes;
        }

        public IWorkbook Export(TicketResult ticketResult, string title, int startIndex = 1)
        {
            using (var builder = this.CreateExcelBuilder())
            {
                Build(builder, ticketResult, title, startIndex);
                var workbook = builder.GetBetList();

                ProcessUi(workbook);

                return workbook;
            }
        }

        protected internal virtual BetListExcelBuilder CreateExcelBuilder()
        {
            return new BetListExcelBuilder();
        }

        private static void AddNoInfoRow(IBetListExcelBuilder builder)
        {
            var titleStyle = builder.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.Center;

            builder.AddCell(new HSSFRichTextString(ViewResources.NoInfo), titleStyle);
            builder.MergeCell(new CellRangeAddress(2, 2, 0, 9));
        }

        private static void AddTitle(IBetListExcelBuilder builder, string title)
        {
            var font = builder.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.Orange.Index;

            var titleStyle = builder.CreateCellStyle();
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.SetFont(font);

            builder.AddCell(new HSSFRichTextString(title), titleStyle);
            builder.MergeCell(new CellRangeAddress(0, 0, 0, 10));
        }

        [SuppressMessage(
            "StyleCop.CSharp.StylecopPlus.Rules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void Build(BetListExcelBuilder builder, TicketResult ticketResult, string title, int startIndex = 1)
        {
            InitializeFonts(builder);
            InitializeCellStyles(builder);

            builder.BuildWorksheet(ViewResources.BetList, 11);
            AddTitle(builder, title);

            var custWlheader = ViewResources.WinLoss + (_showingOption.CustomerCommVisible ? "/" + ViewResources.Comm : string.Empty);
            builder.AddHeader(custWlheader);

            // Content
            if (ticketResult.MainTicketList.Any())
            {
                _ticketHelper.Index = startIndex;
                foreach (var ticket in ticketResult.MainTicketList)
                {
                    BuildTicket(builder, ticketResult, ticket);
                    _ticketHelper.Index++;
                }

                if (_isShowSumStake)
                {
                    builder.AddFooterForSumStake(ticketResult.MainTicketList.Sum(x => x.Stake));
                }
                else if (_showingOption.GrandTotalVisible && !_hiddenColumnIndexes.Contains(ColumnWinlossIndex))
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
        }

        private void BuildTicket(BetListExcelBuilder builder, TicketResult ticketResult, LrfTicket ticket)
        {
            builder
            .AddIndex(_ticketHelper, _ticketHelper.Index)
            .AddExternalId(_ticketHelper, ticket.ExternalId, _oddCellStyleLeftAligned, _evenCellStyleLeftAligned)
            .AddSystemId(_ticketHelper, ticket.UserName, _oddCellStyleLeftAligned, _evenCellStyleLeftAligned)
            .AddTrans(ticket, _ticketHelper)
            .AddChoice(ticket, _ticketHelper, ticketResult.TicketDataList)
            .AddOdds(ticket, ticketResult.TicketDataList, CacheRef.GetOddsTypeName)
            .AddStake(ticket)
            .AddCustomerWL(ticket, _showingOption.CustomerCommVisible, _winLossFont)
            .AddBetEffective(ticket, _winLossFont)
            .AddLicenseeWL(ticket, _licenseeId == LicenseeIdentity.OneTwoBet, _negativeWinLossFont, _winLossFont)
            .AddStatus(ticket, _ticketHelper, ticketResult.TicketDataList, false);
        }

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

        private void ProcessUi(IWorkbook workbook)
        {
            if (_hiddenColumnIndexes != null)
            {
                var columns = _hiddenColumnIndexes;
                if (_appSettings.FakeLicenseeList.Contains(_licenseeId) && !_hiddenColumnIndexes.Contains(ColumnSystemIdIndex))
                {
                    columns = columns.Concat(new[] { ColumnSystemIdIndex });
                }

                for (int i = 0; i < workbook.NumberOfSheets; i++)
                {
                    var sheet = workbook.GetSheetAt(i);

                    sheet.HideColumn(columns.ToArray());
                }
            }
        }
    }
}