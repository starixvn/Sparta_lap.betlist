// --------------------------------------------------------------------------
// <copyright file="CasinoProgressiveWinsExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Entities;
    using Fanex.Athena.Excel.Extension;
    using Fanex.Athena.Models.DataDomain;
    using Models.DTO.Common;
    using Models.DTO.Report;
    using Models.MetaModel.Report;
    using Models.ViewModel;
    using Models.ViewModel.Report;
    using NPOI.SS.UserModel;

    public class CasinoProgressiveWinsExcel : ReportBase
    {
        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnCurrency = 4;
        private const int ColumnTurnoverSportBook = 5;
        private const int ColumnTurnoverTotal = 16;
        private const int ColumnTotalTransaction = 17;
        private const int ColumnCustomerWl = 18;
        private const int ColumnCustomerComm = 19;
        private const int ColumnCustomerTotal = 20;
        private const int ColumnLicenseeWlSportsBook = 21;
        private const int ColumnLicenseeWlTotal = 32;
        private const int ColumnBetTradeCommission = 33;
        private const int ColumnBaCommission = 34;
        private const int ColumnRacingRevenue = 35;
        private const int ColumnAmtDueToBetTrade = 36;
        private const int ColumnAmtDueToBa = 37;
        private const int ColumnAmtDueToRacing = 38;
        private const int ColumnTemplateCount = 39;
        private const int RowContentTemplate = 3;
        private const int RowGrantTotal = 6;
        private const int RowNoInfoTemplate = 7;
        private const int RowTotalNoSolidBorder = 5;
        private const int RowTotalSolidBorder = 4;

        private readonly IEnumerable<CasinoProgressiveWins> _dataList;

        private readonly CasinoProgressiveWinsMetaModel _metaModel;

        public CasinoProgressiveWinsExcel(
                string templatePath,
                string exportingFileName,
                ConvertingOption convertingOption,
                CasinoProgressiveWinsMetaModel metaModel,
                CasinoProgressiveWinsViewModel viewModel)
            : base(templatePath, exportingFileName, null, convertingOption)
        {
            _metaModel = metaModel;
            _dataList = viewModel.DataList;
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _dataList.Count();

            ISheet sheet = Workbook.CloneSheet("Template", "Data");

            AssignHeader(sheet);
            AssignTitle(sheet);

            if (numOfDataRow == 0)
            {
                AssignNoInfoRow(sheet);
            }
            else
            {
                AssignSheetData(sheet);
            }

            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign data to header section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            row.Cells[ColumnIndex].SetCellValue("#");
            row.Cells[ColumnExternalId].SetCellValue(ViewResources.ExternalId);
            row.Cells[ColumnSystemId].SetCellValue(ViewResources.SystemId);
            row.Cells[ColumnDate].SetCellValue(ViewResources.Date);
            row.Cells[ColumnCurrency].SetCellValue(ViewResources.Currency);
            row.Cells[ColumnTurnoverSportBook].SetCellValue(ViewResources.Turnover);
            row.Cells[ColumnTurnoverTotal].SetCellValue(ViewResources.Turnover);
            row.Cells[ColumnTotalTransaction].SetCellValue(ViewResources.TotalTransaction);
            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.CustomerW_L);
            row.Cells[ColumnLicenseeWlSportsBook].SetCellValue(ViewResources.LicenseeW_L);
            row.Cells[ColumnLicenseeWlTotal].SetCellValue(ViewResources.LicenseeW_L);
            row.Cells[ColumnBetTradeCommission].SetCellValue(ViewResources.BetTradeCommission);
            row.Cells[ColumnBaCommission].SetCellValue(ViewResources.BACommission);
            row.Cells[ColumnRacingRevenue].SetCellValue(ViewResources.RacingRevenue);
            row.Cells[ColumnAmtDueToBetTrade].SetCellValue(ViewResources.AmtDueToBetTrade);
            row.Cells[ColumnAmtDueToBa].SetCellValue(ViewResources.AmtDueToBA);
            row.Cells[ColumnAmtDueToRacing].SetCellValue(ViewResources.AmtDueToRacing);
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            var rowSpan = 1;
            var rowIndex = 1;
            int index;
            var rowCount = _dataList.Count();

            for (index = 0; index < rowCount; index++)
            {
                var licenseeWinLoss = _dataList.ElementAt(index);

                // Copy template
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);

                // Report Date
                var reportDateString = licenseeWinLoss.WinlostDate.ToShortDateString();

                // Merge first two columns
                var needMerge = index >= rowCount - 1 ||
                                 licenseeWinLoss.WinlostDate.Date != _dataList.ElementAt(index + 1).WinlostDate.Date;

                // Merge cell
                if (needMerge && rowSpan > 1)
                {
                    sheet.MergeCells(index + RowContentTemplate - rowSpan + 1, ColumnIndex, index + RowContentTemplate, ColumnIndex);

                    sheet.MergeCells(index + RowContentTemplate - rowSpan + 1, ColumnDate, index + RowContentTemplate, ColumnDate);
                }

                rowSpan = needMerge ? 1 : rowSpan + 1;

                var indexid = rowIndex;
                row.Cells[ColumnIndex].SetCellValue(indexid);
                rowIndex = needMerge ? rowIndex + 1 : rowIndex;

                row.Cells[ColumnExternalId].SetCellValue(licenseeWinLoss.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(licenseeWinLoss.SystemId);
                row.Cells[ColumnDate].SetCellValue(reportDateString);
                AssignCommonData(row, licenseeWinLoss);
            }
        }

        public override void AssignTitle(ISheet sheet)
        {
            var fromdate = _metaModel.FromDate.ToString(FormatDate);
            var todate = _metaModel.ToDate.ToString(FormatDate);
            var title = GetExactTitleCurrencyName(_metaModel.CurrencyId);

            ReportTitle = string.Format(ViewResources.CasinoProgressiveWins + ", {0} --> {1} - {2}", fromdate, todate, title);
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Assign common data to excel row.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="data">Licensee win loss data.</param>
        protected void AssignCommonData(IRow row, CasinoProgressiveWins data)
        {
            row.Cells[ColumnTotalTransaction].SetCellValue(data.WinAmount);
        }
    }
}