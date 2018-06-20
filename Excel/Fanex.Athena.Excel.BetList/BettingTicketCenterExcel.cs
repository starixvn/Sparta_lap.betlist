namespace Fanex.Athena.Excel.BetList
{
    using System;
    using System.Linq;
    using Athena.Model.BetList.DTO;
    using Athena.Model.Core.DataDomain;
    using Constant.DataDomain;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Model;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using Resources;
    using VendorAdapter;
    using VendorAdapter.Interfaces;

    public class BettingTicketCenterExcel
    {
        private const int NumberOfColumns = 12;

        private const int ColumnTotalStart = 6;

        private readonly ITicketHelper _ticketHelper;

        private readonly BettingTicketCenterExcelModel _excelModel;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private ICellStyle _evenCellStyleLeftAligned;

        // Default Fonts
        private IFont _negativeWinLossFont;

        private ICellStyle _oddCellStyleLeftAligned;

        private IFont _winLossFont;

        private IFont _liveFont;

        private IFont _mixLiveFont;

        public BettingTicketCenterExcel(BettingTicketCenterExcelModel excelModel)
        {
            _cacheRefAdapter = new CacheRefAdapter();
            _ticketHelper = new TicketHelper();
            _excelModel = excelModel;
        }

        protected internal BettingTicketCenterExcel(
            ICacheRefAdapter cacheRefAdapter,
            ITicketHelper ticketHelper,
            BettingTicketCenterExcelModel excelModel)
        {
            _cacheRefAdapter = cacheRefAdapter;
            _ticketHelper = ticketHelper;
            _excelModel = excelModel;
        }

        public IWorkbook Export()
        {
            var builder = CreateExcelBuilder();
            Build(builder);
            var workbook = builder.GetBetList();
            return workbook;
        }

        protected internal virtual IBetListExcelBuilder CreateExcelBuilder()
        {
            return new BetListExcelBuilder();
        }

        private static ICellStyle CreateFooterCellStyle(IBetListExcelBuilder builder)
        {
            var footerStyle = builder.CreateCellStyle();
            footerStyle.CloneStyleFrom(builder.EvenCellStyle);
            footerStyle.FillForegroundColor = 47;
            footerStyle.Alignment = HorizontalAlignment.Right;
            footerStyle.BorderTop = BorderStyle.None;
            footerStyle.BorderLeft = BorderStyle.None;
            footerStyle.BorderBottom = BorderStyle.None;
            footerStyle.SetFont(builder.RTFHelper.PosFont);
            return footerStyle;
        }

        private static ICellStyle CreateHeaderCellStyle(IBetListExcelBuilder builder)
        {
            var headerStyle = builder.CreateCellStyle();
            headerStyle.CloneStyleFrom(builder.EvenCellStyle);
            headerStyle.FillForegroundColor = 44;
            headerStyle.Alignment = HorizontalAlignment.Center;
            headerStyle.VerticalAlignment = VerticalAlignment.Center;
            headerStyle.SetFont(builder.RTFHelper.PosFont);

            return headerStyle;
        }

        private static void AddNoInfoRow(IBetListExcelBuilder builder)
        {
            var titleStyle = builder.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.Center;

            builder.AddCell(new HSSFRichTextString(ViewResources.NoInfo), titleStyle);
            builder.MergeCell(new CellRangeAddress(2, 2, 0, NumberOfColumns));
        }

        private void AddTitle(IBetListExcelBuilder builder)
        {
            var font = builder.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.Orange.Index;

            var titleStyle = builder.CreateCellStyle();
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.SetFont(font);

            builder.AddCell(new HSSFRichTextString(_excelModel.Title), titleStyle);
            builder.MergeCell(new CellRangeAddress(0, 0, 0, NumberOfColumns));
        }

        private void Build(IBetListExcelBuilder builder)
        {
            InitializeFonts(builder);
            InitializeCellStyles(builder);

            // Content
            if (_excelModel.Data.MainTicketList.Any())
            {
                AssignSheetsData(builder);
            }
            else
            {
                builder.BuildWorksheet(ViewResources.BetList, 11);
                AddTitle(builder);
                AddHeader(builder);
                AddNoInfoRow(builder);
            }
        }

        private void AssignSheetsData(IBetListExcelBuilder builder)
        {
            var numOfSheets = (int)Math.Ceiling((double)_excelModel.Data.MainTicketList.Count / SystemConstants.SheetSizeExcel);
            for (int sheetIndex = 0; sheetIndex < numOfSheets; sheetIndex++)
            {
                builder.BuildWorksheet(ViewResources.Page + (sheetIndex + 1), NumberOfColumns);

                AddTitle(builder);
                AddHeader(builder);

                var endIndex = Math.Min((sheetIndex + 1) * SystemConstants.SheetSizeExcel, _excelModel.Data.MainTicketList.Count);

                for (var i = sheetIndex * SystemConstants.SheetSizeExcel; i < endIndex; i++)
                {
                    _ticketHelper.Index++;
                    AsignData(builder, _excelModel.Data, _excelModel.Data.MainTicketList[i]);
                }

                if (_excelModel.ShowTotal)
                {
                    AddFooter(builder);
                }

                builder.GetBetList(); // Adjust row height for current sheet
            }
        }

        private void AddFooter(IBetListExcelBuilder builder)
        {
            var footerStyle = CreateFooterCellStyle(builder);

            var rtfTotal = new HSSFRichTextString(ViewResources.GrandTotal);
            for (int i = 0; i <= ColumnTotalStart; i++)
            {
                builder.AddCell(rtfTotal, footerStyle);
            }

            builder.AddCell(
                _excelModel.ShowSumStake
                    ? new HSSFRichTextString(_excelModel.Data.SumStake.ToString(FormatKey.NumberFormat))
                    : new HSSFRichTextString(),
                footerStyle);

            builder.AddCell(
                _excelModel.ShowWinLoss
                    ? new HSSFRichTextString((_excelModel.Data.SumWinLoss + _excelModel.Data.SumPlayerComm).ToString(FormatKey.NumberFormat))
                    : new HSSFRichTextString(),
                footerStyle);

            if (_excelModel.ShowLicenseeWl)
            {
                var sumLicenseeWLComm = _excelModel.Data.SumLicenseeWL + _excelModel.Data.SumComm;
                var rtfSumLicenseeWl = new HSSFRichTextString(sumLicenseeWLComm.ToString(FormatKey.NumberFormat));
                rtfSumLicenseeWl.ApplyFont(sumLicenseeWLComm >= 0 ? _negativeWinLossFont : _winLossFont);
                builder.AddCell(rtfSumLicenseeWl, footerStyle);
            }
            else
            {
                builder.AddCell(new HSSFRichTextString(), footerStyle);
            }

            // add the rest column
            for (int i = ColumnTotalStart + 3; i < NumberOfColumns; i++)
            {
                builder.AddCell(new HSSFRichTextString(), footerStyle);
            }

            var lastRowIndex = builder.LastContentRow;
            builder.MergeCell(new CellRangeAddress(lastRowIndex, lastRowIndex, 0, ColumnTotalStart), footerStyle);
        }

        private void AddHeader(IBetListExcelBuilder builder)
        {
            var headerStyle = CreateHeaderCellStyle(builder);
            var captions = new[]
            {
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.TransTime,
                ViewResources.Choice,
                ViewResources.Odds,
                ViewResources.Stake,
                ViewResources.WinLoss + (_excelModel.CustomerCommVisible ? "/" + ViewResources.Comm : string.Empty),
                ViewResources.LicenseeWL + "/" + ViewResources.Comm,
                ViewResources.Status,
                ViewResources.Platform,
                ViewResources.Live
            };
            var columWidths = new short[] { 1500, 4800, 4800, 5400, 15500, 2600, 4200, 4200, 5400, 5400, 3400, 3400, 3400 };

            for (int i = 0; i < captions.Length; i++)
            {
                var cell = builder.AddCell(new HSSFRichTextString(captions[i]), headerStyle);
                builder.SetColumnWidth(cell.ColumnIndex, columWidths[i]);
            }
        }

        private void AsignData(IBetListExcelBuilder builder, TicketResult ticketResult, LrfTicket ticket)
        {
            builder.AddIndex(_ticketHelper, _ticketHelper.Index);
            builder.AddExternalId(_ticketHelper, ticket.ExternalId, _oddCellStyleLeftAligned, _evenCellStyleLeftAligned);
            builder.AddSystemId(_ticketHelper, ticket.UserName, _oddCellStyleLeftAligned, _evenCellStyleLeftAligned);
            builder.AddTrans(ticket, _ticketHelper);
            builder.AddChoice(ticket, _ticketHelper, ticketResult.TicketDataList);
            builder.AddOdds(ticket, ticketResult.TicketDataList, _cacheRefAdapter.GetOddsTypeName);
            builder.AddStake(ticket);
            builder.AddCustomerWL(ticket, _excelModel.CustomerCommVisible, _winLossFont);
            builder.AddLicenseeWL(ticket, _excelModel.LicenseeId == LicenseeIdentity.OneTwoBet, _negativeWinLossFont, _winLossFont);
            builder.AddStatus(ticket, _ticketHelper, ticketResult.TicketDataList, false);
            AddPlatfromCell(builder, ticket);
            AddLiveCell(builder, ticket);
        }

        private void AddPlatfromCell(IBetListExcelBuilder builder, LrfTicket ticket)
        {
            var rtfPlatfrom = new HSSFRichTextString(PlatformType.GetPlatformName(ticket.BetFrom));
            BuilDataCell(builder, rtfPlatfrom);
        }

        private void AddLiveCell(IBetListExcelBuilder builder, LrfTicket ticket)
        {
            var liveStatus = ticket.BetTypeId == (int)BetType.MixParlay
                ? ticket.LiveHomeScore
                : (ticket.IsLive ? LiveStatus.Live : 0);

            switch (liveStatus)
            {
                case LiveStatus.Live:
                    var rtfLive = new HSSFRichTextString(ViewResources.Live);
                    rtfLive.ApplyFont(_liveFont);
                    BuilDataCell(builder, rtfLive);
                    return;

                case LiveStatus.MixLiveAndNone:
                    var rtfMixLive = new HSSFRichTextString(ViewResources.MixLiveAndNoneLive);
                    rtfMixLive.ApplyFont(_mixLiveFont);
                    BuilDataCell(builder, rtfMixLive);
                    return;

                default:
                    BuilDataCell(builder, new HSSFRichTextString(ViewResources.NoneLive));
                    return;
            }
        }

        private void BuilDataCell(IBetListExcelBuilder builder, IRichTextString text)
        {
            builder.AddCell(text, _ticketHelper.Index % 2 == 0 ? _oddCellStyleLeftAligned : _evenCellStyleLeftAligned);
        }

        private void InitializeCellStyles(IBetListExcelBuilder builder)
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

        private void InitializeFonts(IBetListExcelBuilder builder)
        {
            _negativeWinLossFont = builder.CreateFont();
            _negativeWinLossFont.Boldweight = (short)FontBoldWeight.Bold;
            _negativeWinLossFont.Color = (short)FontColor.Red;

            _winLossFont = builder.CreateFont();
            _winLossFont.Boldweight = (short)FontBoldWeight.Bold;
            _winLossFont.Color = (short)FontColor.None;

            _liveFont = builder.CreateFont();
            _liveFont.Color = (short)FontColor.Red;

            _mixLiveFont = builder.CreateFont();
            _mixLiveFont.Color = 13311;
        }
    }
}