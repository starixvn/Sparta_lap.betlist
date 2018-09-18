namespace Fanex.Athena.Excel.BetList
{
    using System;
    using System.Linq;
    using Athena.Model.Core.DataDomain;
    using Builders;
    using Model;
    using NPOI.SS.UserModel;
    using Resources;

    public class BettingTicketCenterExcel : IBettingTicketCenterExcel
    {
        private readonly IBettingTicketCenterExcelBuilder _builder;

        protected internal BettingTicketCenterExcel(IBettingTicketCenterExcelBuilder builder)
        {
            _builder = builder;
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
                _builder.CreateSheet(ViewResources.BettingTicketCenter);
                _builder.AddTitle(excelModel.Title);
                _builder.AddHeader(excelModel.CustomerCommVisible);
                _builder.AddNoInfoRow();
            }

            return _builder.GetWorkbook();
        }

        protected internal virtual void BuildSheets(BettingTicketCenterExcelModel excelModel)
        {
            var numOfSheets = (int)Math.Ceiling((double)excelModel.Data.MainTicketList.Count / SystemConstants.SheetSizeExcel);
            for (int sheetIndex = 0; sheetIndex < numOfSheets; sheetIndex++)
            {
                _builder.CreateSheet(ViewResources.Page + (sheetIndex + 1));
                _builder.AddTitle(excelModel.Title);
                _builder.AddHeader(excelModel.CustomerCommVisible);

                var endIndex = Math.Min((sheetIndex + 1) * SystemConstants.SheetSizeExcel, excelModel.Data.MainTicketList.Count);

                AssignSheetData(excelModel, sheetIndex, endIndex);

                if (excelModel.ShowTotal)
                {
                    AddFooter(excelModel);
                }

                _builder.GetWorkbook(); // Adjust row height for current sheet
            }
        }

        protected internal virtual void AssignSheetData(BettingTicketCenterExcelModel excelModel, int sheetIndex, int endIndex)
        {
            for (var i = sheetIndex * SystemConstants.SheetSizeExcel; i < endIndex; i++)
            {
                _builder.AssignRowData(
                    excelModel.Data,
                    excelModel.Data.MainTicketList[i],
                    excelModel.CustomerCommVisible,
                    excelModel.LicenseeId);
            }
        }

        protected internal virtual void AddFooter(BettingTicketCenterExcelModel excelModel)
        {
            var sumStake = excelModel.ShowSumStake ? excelModel.Data.SumStake : (decimal?)null;
            var sumWinLoss = excelModel.ShowWinLoss ? excelModel.Data.SumWinLoss : (decimal?)null;
            var sumPlayerComm = excelModel.ShowWinLoss && excelModel.CustomerCommVisible ? excelModel.Data.SumPlayerComm : (decimal?)null;
            var sumLicenseeWL = excelModel.ShowLicenseeWl ? (excelModel.Data.SumLicenseeWL + excelModel.Data.SumComm) : (decimal?)null;

            _builder.AddFooter(sumStake, sumWinLoss, sumPlayerComm, sumLicenseeWL);
        }
    }
}