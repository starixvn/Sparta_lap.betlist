namespace Fanex.Athena.BetList.Excel
{
    using System;
    using System.Linq;
    using BetListNPOI.SS.UserModel;
    using Builders;
    using Common.Constant;
    using Helpers;
    using Model;
    using Resources;
    using Service.Interfaces;

    public class BettingTicketCenterExcel : IBettingTicketCenterExcel
    {
        private readonly IBettingTicketCenterExcelBuilder _builder;

        private readonly ILogAggregateService _logAggregateService;

        private bool disposed;

        public BettingTicketCenterExcel(
            IBettingTicketCenterExcelBuilder builder,
            IIPService ipService,
            ILogAggregateService logAggregateService)
        {
            _logAggregateService = logAggregateService;
            _builder = builder ?? new BettingTicketCenterExcelBuilder(null, null, ipService);
        }

        public virtual IWorkbook Export(BettingTicketCenterExcelModel excelModel)
        {
            if (excelModel.Data.MainTicketList.Any())
            {
                _builder.InitializeFonts();
                _builder.InitializeCellStyles();
                BuildSheets(excelModel);
            }
            else
            {
                _builder.CreateSheet(ViewResources.BettingTicketCenter, excelModel.Option);
                _builder.AddTitle(excelModel.Title, excelModel.Option);
                _builder.AddHeader(excelModel.Option);
                _builder.AddNoInfoRow(excelModel.Option);
            }

            var workbook = _builder.GetWorkbook();
            FixBugIOSMobile(workbook);
            return workbook;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected internal virtual void BuildSheets(BettingTicketCenterExcelModel excelModel)
        {
            var numOfSheets = (int)Math.Ceiling((double)excelModel.Data.MainTicketList.Count / SystemConstants.SheetSizeExcel);
            for (int sheetIndex = 0; sheetIndex < numOfSheets; sheetIndex++)
            {
                _logAggregateService.Append($"Start sheet {sheetIndex}");
                _builder.CreateSheet(ViewResources.Page + (sheetIndex + 1), excelModel.Option);
                _builder.AddTitle(excelModel.Title, excelModel.Option);
                _logAggregateService.Append("Add header data");
                _builder.AddHeader(excelModel.Option);

                var endIndex = Math.Min((sheetIndex + 1) * SystemConstants.SheetSizeExcel, excelModel.Data.MainTicketList.Count);
                _logAggregateService.Append("Assign sheet data");
                AssignSheetData(excelModel, sheetIndex, endIndex);

                _logAggregateService.Append("Add footer data");
                if (excelModel.Option.ShowTotal)
                {
                    AddFooter(excelModel);
                }

                _logAggregateService.Append("Adjust row height for current sheet");
                _builder.GetWorkbook(); // Adjust row height for current sheet
                _logAggregateService.Append($"Done sheet {sheetIndex}");
            }
        }

        protected internal virtual void AssignSheetData(BettingTicketCenterExcelModel excelModel, int sheetIndex, int endIndex)
        {
            for (var i = sheetIndex * SystemConstants.SheetSizeExcel; i < endIndex; i++)
            {
                _builder.AssignRowData(
                    excelModel.Data,
                    excelModel.Data.MainTicketList[i],
                    excelModel.LicenseeId,
                    excelModel.Option);
            }
        }

        protected internal virtual void AddFooter(BettingTicketCenterExcelModel excelModel)
        {
            _builder.AddFooter(
                excelModel.Data.SumStake,
                excelModel.Data.SumWinLoss,
                excelModel.Data.SumPlayerComm,
                excelModel.Data.SumLicenseeWL + excelModel.Data.SumComm,
                excelModel.TotalText,
                excelModel.Option);
        }

        protected virtual void FixBugIOSMobile(IWorkbook workbook)
        {
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                var sheet = workbook.GetSheetAt(i);
                ExcelHelper.FixCrashingInIOSSheet(sheet);
            }
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing && _builder != null)
            {
                ((IDisposable)_builder)?.Dispose();
            }

            disposed = true;
        }
    }
}