using System;
using Fanex.Athena.Constant.DataDomain;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Resources;
using Fanex.Athena.BetList.Service.Extensions;
using Fanex.Athena.VendorAdapter.Interfaces;
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
        private const int NumberOfColumns = 12;

        private const int ColumnTotalStart = 6;

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

        public void CreateSheet(string sheetName)
        {
            _builder.BuildWorksheet(sheetName, NumberOfColumns);
        }

        public void AddNoInfoRow()
        {
            var titleStyle = _builder.CreateCellStyle();
            titleStyle.Alignment = HorizontalAlignment.Center;

            _builder.AddCell(new HSSFRichTextString(ViewResources.NoInfo), titleStyle);
            _builder.MergeCell(new CellRangeAddress(2, 2, 0, NumberOfColumns - 1));
        }

        public void AddTitle(string title)
        {
            var font = _builder.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold;
            font.Color = HSSFColor.Orange.Index;

            var titleStyle = _builder.CreateCellStyle();
            titleStyle.VerticalAlignment = VerticalAlignment.Center;
            titleStyle.SetFont(font);

            _builder.AddCell(new HSSFRichTextString(title), titleStyle);
            _builder.MergeCell(new CellRangeAddress(0, 0, 0, NumberOfColumns - 1));
        }

        public void AddHeader(bool isShowCustomerComm)
        {
            var headerStyle = _builder.CreateHeaderCellStyle();
            var captions = new[]
            {
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.TransTime,
                ViewResources.Choice,
                ViewResources.Odds,
                ViewResources.Stake,
                ViewResources.WinLoss + (isShowCustomerComm ? "/" + ViewResources.Comm : string.Empty),
                ViewResources.LicenseeWL + "/" + ViewResources.Comm,
                ViewResources.Status,
                ViewResources.Platform,
                ViewResources.Live
            };
            var columWidths = new short[] { 1500, 4800, 4800, 5400, 15500, 2600, 4200, 4200, 5400, 5400, 3400, 3400 };

            for (int i = 0; i < captions.Length; i++)
            {
                var cell = _builder.AddCell(new HSSFRichTextString(captions[i]), headerStyle);
                _builder.SetColumnWidth(cell.ColumnIndex, columWidths[i]);
            }
        }

        public void AddFooter(decimal? sumStake, decimal? sumWinLoss, decimal? sumPlayerComm, decimal? sumLicenseeWL)
        {
            var footerStyle = _builder.CreateFooterCellStyle();

            var rtfTotal = new HSSFRichTextString(ViewResources.Total);
            for (int i = 0; i < ColumnTotalStart; i++)
            {
                _builder.AddCell(rtfTotal, footerStyle);
            }

            _builder.AddCell(
                new HSSFRichTextString(sumStake.HasValue ? sumStake.Value.ToString(FormatKey.DecimalFormat) : string.Empty),
                footerStyle);

            var sumWinLossString = sumWinLoss.HasValue
                ? (sumPlayerComm.HasValue
                    ? string.Format("{0:#,##0.00}\n{1:#,##0.00}", sumWinLoss.Value, sumPlayerComm.Value)
                    : string.Format("{0:#,##0.00}", sumWinLoss.Value))
                : string.Empty;
            _builder.AddCell(new HSSFRichTextString(sumWinLossString), footerStyle);

            if (sumLicenseeWL.HasValue)
            {
                var rtfSumLicenseeWl = new HSSFRichTextString(sumLicenseeWL.Value.ToString(FormatKey.DecimalFormat));
                rtfSumLicenseeWl.ApplyFont(sumLicenseeWL.Value < 0 ? _negativeWinLossFont : _winLossFont);
                _builder.AddCell(rtfSumLicenseeWl, footerStyle);
            }
            else
            {
                _builder.AddCell(new HSSFRichTextString(), footerStyle);
            }

            // add the rest column
            for (int i = ColumnTotalStart + 3; i < NumberOfColumns; i++)
            {
                _builder.AddCell(new HSSFRichTextString(), footerStyle);
            }

            var lastRowIndex = _builder.LastContentRow;
            _builder.MergeCell(new CellRangeAddress(lastRowIndex, lastRowIndex, 0, ColumnTotalStart - 1), footerStyle);
        }

        public void AssignRowData(TicketResult ticketResult, LrfTicket ticket, bool isShowCustomerComm, int licenseeId)
        {
            _ticketHelper.Index++;
            _builder.AddIndex(_ticketHelper, _ticketHelper.Index);
            _builder.AddExternalId(_ticketHelper, ticket.ExternalId, _oddCellLeftAlign, _evenCellLeftAlign);
            _builder.AddSystemId(_ticketHelper, ticket.UserName, _oddCellLeftAlign, _evenCellLeftAlign);
            _builder.AddTrans(ticket, _ticketHelper);
            _builder.AddChoice(ticket, _ticketHelper, ticketResult.TicketDataList);
            _builder.AddOdds(ticket, ticketResult.TicketDataList, _ticketHelper.GetValidOddsTypeNameById);
            _builder.AddStake(ticket);
            _builder.AddCustomerWL(ticket, isShowCustomerComm, _winLossFont);
            _builder.AddLicenseeWL(ticket, licenseeId == LicenseeIdentity.OneTwoBet, _negativeWinLossFont, _winLossFont);
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
            _negativeWinLossFont = _builder.CreateFont();
            _negativeWinLossFont.Boldweight = (short)FontBoldWeight.Bold;
            _negativeWinLossFont.Color = (short)FontColor.Red;

            _winLossFont = _builder.CreateFont();
            _winLossFont.Boldweight = (short)FontBoldWeight.Bold;
            _winLossFont.Color = (short)FontColor.None;

            _liveFont = _builder.CreateFont();
            _liveFont.Color = (short)FontColor.Red;

            _mixLiveFont = _builder.CreateFont();
            _mixLiveFont.Color = 13311;
        }

        private void AddPlatfromCell(LrfTicket ticket)
        {
            var rtfPlatfrom = new HSSFRichTextString(PlatformType.GetPlatformName(ticket.BetFrom));
            BuilDataAlignedCenterCell(rtfPlatfrom);
        }

        private void AddLiveCell(LrfTicket ticket)
        {
            var liveStatus = ticket.BetTypeId == (int)BetType.MixParlay || ticket.BetTypeId == (int)BetType.CombinationMixParlay
                ? ticket.LiveHomeScore
                : (ticket.IsLive ? LiveStatusTypes.Live : 0);

            switch (liveStatus)
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
            _builder.AddCell(text, _ticketHelper.Index % 2 == 0 ? _oddCellCenterAlign : _evenCellCenterAlign);
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