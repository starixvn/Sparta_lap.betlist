using System;
using System.Diagnostics.CodeAnalysis;
using Fanex.Athena.BetList.Excel.Model;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Service.Extensions;
using Fanex.Athena.Common.Constant;
using Fanex.Athena.Common.Utility.Helpers;
using Fanex.Athena.Resources;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace Fanex.Athena.BetList.Excel.Builders
{
    public class BettingTicketCenterExcelBuilder : IBettingTicketCenterExcelBuilder, IDisposable
    {
        private const int NumberOfColumns = 14;

        private const int ColumnTotalStart = 8;

        private const int ContentRowIndex = 2;

        private readonly ITicketHelper _ticketHelper;

        private bool disposed;

        private IBetListExcelBuilder _builder;

        private ICellStyle _evenCellLeftAlign;

        private ICellStyle _oddCellLeftAlign;

        private ICellStyle _evenCellCenterAlign;

        private ICellStyle _oddCellCenterAlign;

        // Default Fonts
        private IFont _negativeWinLossFont;

        private IFont _winLossFont;

        private IFont _liveFont;

        private IFont _mixLiveFont;

        public BettingTicketCenterExcelBuilder(IBetListExcelBuilder builder, ITicketHelper ticketHelper)
        {
            _builder = builder;
            _ticketHelper = ticketHelper;
        }

        public IWorkbook GetWorkbook()
        {
            return _builder.GetBetList();
        }

        public void CreateSheet(string sheetName, BettingTicketCenterExcelOption option)
        {
            short cols = (short)(NumberOfColumns - (option.ShowWinLoss ? 0 : 1) - (option.ShowLicenseeWl ? 0 : 1));
            _builder.BuildWorksheet(sheetName, cols);
        }

        public void AddNoInfoRow(BettingTicketCenterExcelOption option)
        {
            var titleStyle = _builder.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.Center;

            var mergeCol = NumberOfColumns - 1 - (option.ShowWinLoss ? 0 : 1) - (option.ShowLicenseeWl ? 0 : 1);
            _builder.AddCell(new HSSFRichTextString(ViewResources.NoInfo), titleStyle);
            _builder.MergeCell(new CellRangeAddress(ContentRowIndex, ContentRowIndex, 0, mergeCol));
        }

        public void AddTitle(string title, BettingTicketCenterExcelOption option)
        {
            var font = _builder.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.Orange.Index;

            var titleStyle = _builder.CreateCellStyle();
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.SetFont(font);

            var mergeCol = NumberOfColumns - 1 - (option.ShowWinLoss ? 0 : 1) - (option.ShowLicenseeWl ? 0 : 1);
            _builder.AddCell(new HSSFRichTextString(title), titleStyle);
            _builder.MergeCell(new CellRangeAddress(0, 0, 0, mergeCol));
        }

        public void AddHeader(bool isShowCustomerComm, BettingTicketCenterExcelOption option)
        {
            var columnWidths = new short[] { 1500, 4800, 4800, 5400, 5400, 15500, 2600, 2600, 4200, 4200, 5400, 5400, 3400, 3400 };
            var index = 0;

            var headerStyle = _builder.CreateHeaderCellStyle();
            AddHeaderCell("#", columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.ExternalId, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.SystemId, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.RefNo, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.TransTime, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.Choice, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.Odds, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.OddsType, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.Stake, columnWidths[index++], headerStyle);

            if (option.ShowWinLoss)
            {
                AddHeaderCell(ViewResources.WinLoss + (isShowCustomerComm ? "/" + ViewResources.Comm : string.Empty), columnWidths[index++], headerStyle);
            }
            else
            {
                ++index;
            }

            if (option.ShowLicenseeWl)
            {
                AddHeaderCell(ViewResources.LicenseeWL + "/" + ViewResources.Comm, columnWidths[index++], headerStyle);
            }
            else
            {
                ++index;
            }

            AddHeaderCell(ViewResources.Status, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.Platform, columnWidths[index++], headerStyle);
            AddHeaderCell(ViewResources.Live, columnWidths[index], headerStyle);
        }

        public void AddFooter(decimal? sumStake, decimal? sumWinLoss, decimal? sumPlayerComm, decimal? sumLicenseeWL, string totalText)
        {
            var footerStyle = _builder.CreateFooterCellStyle();
            var cols = NumberOfColumns;
            var totalColIndex = ColumnTotalStart;

            var rtfTotal = new HSSFRichTextString(totalText);
            for (int i = 0; i < ColumnTotalStart; i++)
            {
                _builder.AddCell(rtfTotal, footerStyle);
            }

            _builder.AddCell(
                new HSSFRichTextString(sumStake.HasValue ? sumStake.Value.ToString(FormatKey.DecimalFormat) : string.Empty),
                footerStyle);

            if (sumWinLoss.HasValue)
            {
                var sumWinLossString = sumPlayerComm.HasValue
                    ? string.Format("{0:#,##0.00}\n{1:#,##0.00}", sumWinLoss.Value, sumPlayerComm.Value)
                    : string.Format("{0:#,##0.00}", sumWinLoss.Value);
                _builder.AddCell(new HSSFRichTextString(sumWinLossString), footerStyle);

                --cols;
                --totalColIndex;
            }

            if (sumLicenseeWL.HasValue)
            {
                var rtfSumLicenseeWl = new HSSFRichTextString(sumLicenseeWL.Value.ToString(FormatKey.DecimalFormat));
                rtfSumLicenseeWl.ApplyFont(sumLicenseeWL.Value < 0 ? _negativeWinLossFont : _winLossFont);
                _builder.AddCell(rtfSumLicenseeWl, footerStyle);

                --cols;
                --totalColIndex;
            }

            // add the rest column
            for (int i = totalColIndex + 3; i < cols; i++)
            {
                _builder.AddCell(new HSSFRichTextString(), footerStyle);
            }

            var lastRowIndex = _builder.LastContentRow;
            _builder.MergeCell(new CellRangeAddress(lastRowIndex, lastRowIndex, 0, ColumnTotalStart - 1), footerStyle);
        }

        public void AssignRowData(TicketResult ticketResult, LrfTicket ticket, bool isShowCustomerComm, int licenseeId, BettingTicketCenterExcelOption option)
        {
            _ticketHelper.Index++;
            _builder.AddIndex(_ticketHelper, _ticketHelper.Index);
            _builder.AddExternalId(_ticketHelper, ticket.ExternalId, _oddCellLeftAlign, _evenCellLeftAlign);
            _builder.AddSystemId(_ticketHelper, ticket.UserName, _oddCellLeftAlign, _evenCellLeftAlign);
            _builder.AddRefNo(ticket, _ticketHelper);
            _builder.AddTransTime(ticket, _ticketHelper);
            _builder.AddChoice(ticket, _ticketHelper, ticketResult.TicketDataList);
            _builder.AddOddsValue(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById);
            _builder.AddOddsType(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById);
            _builder.AddStake(ticket);

            if (option.ShowWinLoss)
            {
                _builder.AddCustomerWL(ticket, isShowCustomerComm, _winLossFont);
            }

            if (option.ShowLicenseeWl)
            {
                _builder.AddLicenseeWL(ticket, licenseeId == LicenseeIdentity.OneTwoBet, _negativeWinLossFont, _winLossFont);
            }

            _builder.AddStatus(ticket, _ticketHelper, ticketResult.TicketDataList, false);
            AddPlatfromCell(ticket);
            AddLiveCell(ticket);
        }

        public void InitializeCellStyles()
        {
            _oddCellLeftAlign = CreateCellStyle(HorizontalAlignment.Left, false);
            _evenCellLeftAlign = CreateCellStyle(HorizontalAlignment.Left, true);
            _oddCellCenterAlign = CreateCellStyle(HorizontalAlignment.Center, false);
            _evenCellCenterAlign = CreateCellStyle(HorizontalAlignment.Center, true);
        }

        public void InitializeFonts()
        {
            const short MixLiveColor = 13311;

            _negativeWinLossFont = _builder.CreateFont();
            _negativeWinLossFont.Boldweight = (short)FontBoldWeight.Bold;
            _negativeWinLossFont.Color = (short)FontColor.Red;

            _winLossFont = _builder.CreateFont();
            _winLossFont.Boldweight = (short)FontBoldWeight.Bold;
            _winLossFont.Color = (short)FontColor.None;

            _liveFont = _builder.CreateFont();
            _liveFont.Color = (short)FontColor.Red;

            _mixLiveFont = _builder.CreateFont();
            _mixLiveFont.Color = MixLiveColor;
        }

        private void AddPlatfromCell(LrfTicket ticket)
        {
            var rtfPlatfrom = new HSSFRichTextString(PlatformType.GetPlatformName(ticket.BetFrom));
            BuilDataAlignedCenterCell(rtfPlatfrom);
        }

        private void AddLiveCell(LrfTicket ticket)
        {
            var liveStatus = ticket.IsLive ? LiveStatusTypes.Live : 0;

            var status = ticket.BetTypeId == (int)BetType.MixParlay || ticket.BetTypeId == (int)BetType.CombinationMixParlay
                ? ticket.LiveHomeScore
                : liveStatus;

            switch (status)
            {
                case LiveStatusTypes.Live:
                    var rtfLive = new HSSFRichTextString(ViewResources.Live);
                    rtfLive.ApplyFont(_liveFont);
                    BuilDataAlignedCenterCell(rtfLive);
                    return;

                case LiveStatusTypes.Mix:
                    var rtfMixLive = new HSSFRichTextString(ViewResources.MixLiveAndNoneLive);
                    rtfMixLive.ApplyFont(_mixLiveFont);
                    BuilDataAlignedCenterCell(rtfMixLive);
                    return;

                default:
                    BuilDataAlignedCenterCell(new HSSFRichTextString(ViewResources.NoneLive));
                    return;
            }
        }

        private void BuilDataAlignedCenterCell(IRichTextString text)
        {
            _builder.AddCell(text, _ticketHelper.Index.IsEvenNumber() ? _oddCellCenterAlign : _evenCellCenterAlign);
        }

        private ICellStyle CreateCellStyle(HorizontalAlignment align, bool isEvenCell)
        {
            var cellStyle = _builder.CreateCellStyle();
            cellStyle.CloneStyleFrom(isEvenCell ? _builder.EvenCellStyle : _builder.OddCellStyle);
            cellStyle.Alignment = align;
            cellStyle.SetFont(_builder.RTFHelper.NormalFont);
            cellStyle.WrapText = true;
            return cellStyle;
        }

        private void AddHeaderCell(string caption, short width, ICellStyle style)
        {
            var cell = _builder.AddCell(new HSSFRichTextString(caption), style);
            _builder.SetColumnWidth(cell.ColumnIndex, width);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing && _builder != null)
            {
                (_builder as IDisposable)?.Dispose();
                _builder = null;
                _evenCellLeftAlign = null;
                _oddCellLeftAlign = null;
                _evenCellCenterAlign = null;
                _oddCellCenterAlign = null;
                _negativeWinLossFont = null;
                _winLossFont = null;
                _liveFont = null;
                _mixLiveFont = null;
            }

            disposed = true;
        }
    }
}