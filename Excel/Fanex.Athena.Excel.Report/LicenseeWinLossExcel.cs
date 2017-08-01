// --------------------------------------------------------------------------
// <copyright file="LicenseeWinLossExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Base;
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using Resources;
    using Service.Report;
    using Service.Report.Interfaces;

    public class LicenseeWinLossExcel : ReportBase
    {
        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnCurrency = 4;

        private const int ColumnTurnoverSportBook = 5;
        private const int ColumnTurnoverTotal = 22;

        private const int ColumnActualStake = 23;

        private const int ColumnBuyBackAmountSportsbook = 24;
        private const int ColumnBuyBackAmountBA = 25;
        private const int ColumnBuyBackAmountTotal = 26;

        private const int ColumnTotalTransaction = 27;

        private const int ColumnCustomerWl = 28;
        private const int ColumnCustomerComm = 29;
        private const int ColumnCustomerTotal = 30;

        private const int ColumnLicenseeWlSportsBook = 31;
        private const int ColumnLicenseeWlTotal = 48;

        private const int ColumnBetTradeCommission = 49;
        private const int ColumnCashOutCommission = 50;
        private const int ColumnBaCommission = 51;
        private const int ColumnRacingRevenue = 52;
        private const int ColumnAmtDueToBetTrade = 53;
        private const int ColumnAmtDueToBa = 54;
        private const int ColumnAmtDueToRacing = 55;

        private const int ColumnTemplateCount = 56;

        private const int RowContentTemplate = 3;
        private const int RowGrantTotal = 6;
        private const int RowNoInfoTemplate = 7;
        private const int RowTotalNoSolidBorder = 5;
        private const int RowTotalSolidBorder = 4;

        private const int TotalLicenseeWlProductColumn = 18;
        private const int TotalTurnoverProductColumn = 18;

        private static readonly string[] IgnoredLicenseeWLProducts = new string[] { Product.RB, Product.BA };
        private static readonly string[] IgnoredTurnoverProducts = new string[] { Product.BT };

        private readonly IDictionary<string, bool> _offeredProducts = new Dictionary<string, bool>
            {
                { Product.SB, false }, { Product.AB, false },
                { Product.AGC, false }, { Product.BA, false },
                { Product.BT, false }, { Product.FW, false }, { Product.KN, false },
                { Product.LC, false }, { Product.VB, false },
                { Product.MGCLC, false }, { Product.MGCRNG, false },
                { Product.NG, false }, { Product.RB, false },
                { Product.CS, false }, { Product.VS, false },
                { Product.VS2, false }, { Product.LT, false }
            };

        private readonly bool _isAllowBA;

        private readonly ILicenseeWinLossService _licenseeWinLossService;

        private readonly IList<LicenseeWinLoss> _dataList;

        private readonly LicenseeWinLossMetaModel _metaModel;
        private readonly IDictionary<string, LicenseeWinLoss> _summaryTotal;

        public LicenseeWinLossExcel(
                ILicenseeWinLossService licenseeWinLossService,
                string templatePath,
                ConvertingOption convertingOption,
                ShowingOption showingOption,
                LicenseeWinLossMetaModel metaModel,
                LicenseeWinLossViewModel viewModel)
            : base(templatePath, showingOption, convertingOption)
        {
            _licenseeWinLossService = licenseeWinLossService ?? new LicenseeWinLossService();
            _metaModel = metaModel;
            _dataList = viewModel.MainResult;
            _summaryTotal = viewModel.SummaryTotal;
            _isAllowBA = metaModel.ProductOptions.Any(x => x.Code == Product.BA);

            var productCodeList = _offeredProducts.Keys.ToList();
            foreach (var productCode in productCodeList)
            {
                _offeredProducts[productCode] = metaModel.ProductOptions.Any(x => x.Status && x.Code == productCode);
            }
        }

        public int Level
        {
            get;
            set;
        }

        protected override void AssignData()
        {
            int numOfDataRow = _dataList.Count;

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

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        protected override void AssignHeader(ISheet sheet)
        {
            // row 1
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            row.Cells[ColumnIndex].SetCellValue("#");
            row.Cells[ColumnExternalId].SetCellValue(ViewResources.ExternalId);
            row.Cells[ColumnSystemId].SetCellValue(ViewResources.SystemId);
            row.Cells[ColumnDate].SetCellValue(ViewResources.Date);
            row.Cells[ColumnCurrency].SetCellValue(ViewResources.Currency);
            row.Cells[ColumnTurnoverSportBook].SetCellValue(ViewResources.Turnover);
            row.Cells[ColumnTurnoverTotal].SetCellValue(ViewResources.Turnover);
            row.Cells[ColumnActualStake].SetCellValue(ViewResources.ActualStake);
            row.Cells[ColumnBuyBackAmountSportsbook].SetCellValue(ViewResources.BuyBackAmount);
            row.Cells[ColumnBuyBackAmountBA].SetCellValue(ViewResources.BuyBackAmount);
            row.Cells[ColumnBuyBackAmountTotal].SetCellValue(ViewResources.BuyBackAmount);
            row.Cells[ColumnTotalTransaction].SetCellValue(ViewResources.TotalTransaction);
            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.CustomerW_L);
            row.Cells[ColumnLicenseeWlSportsBook].SetCellValue(ViewResources.LicenseeW_L);
            row.Cells[ColumnLicenseeWlTotal].SetCellValue(ViewResources.LicenseeW_L);
            row.Cells[ColumnCashOutCommission].SetCellValue(ViewResources.CashOutCommission);
            row.Cells[ColumnBetTradeCommission].SetCellValue(ViewResources.BetTradeCommission);
            row.Cells[ColumnBaCommission].SetCellValue(ViewResources.BACommission);
            row.Cells[ColumnRacingRevenue].SetCellValue(ViewResources.RacingRevenue);
            row.Cells[ColumnAmtDueToBetTrade].SetCellValue(ViewResources.AmtDueToBetTrade);
            row.Cells[ColumnAmtDueToBa].SetCellValue(ViewResources.AmtDueToBA);
            row.Cells[ColumnAmtDueToRacing].SetCellValue(ViewResources.AmtDueToRacing);

            // row 2
            IRow row2 = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);
            row2.Cells[ColumnCustomerWl].SetCellValue(ViewResources.WinLoss);
            row2.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Comm);
            row2.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);

            row2.Cells[ColumnBuyBackAmountSportsbook].SetCellValue(ViewResources.Sportsbook);
            row2.Cells[ColumnBuyBackAmountBA].SetCellValue(ViewResources.ProductBA);
            row2.Cells[ColumnBuyBackAmountTotal].SetCellValue(ViewResources.Total);

            // turnover and licenseewl columns
            var resourceManager = ViewResources.ResourceManager;
            for (int i = 0; i < _offeredProducts.Count; i++)
            {
                var text = resourceManager.GetString("Product" + _offeredProducts.Keys.ElementAt(i));
                row2.Cells[ColumnTurnoverSportBook + i].SetCellValue(text);
                row2.Cells[ColumnLicenseeWlSportsBook + i].SetCellValue(text);
            }

            row2.Cells[ColumnTurnoverTotal].SetCellValue(ViewResources.Total);
            row2.Cells[ColumnLicenseeWlTotal].SetCellValue(ViewResources.Total);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        protected override void AssignSheetData(ISheet sheet)
        {
            var rowSpan = 1;
            var rowIndex = 1;
            var indexDetail = 1;
            int index;
            var rowCount = _dataList.Count;

            for (index = 0; index < rowCount; index++)
            {
                var licenseeWinLoss = _dataList[index];

                // Copy template
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);

                // Report Date
                var reportDateString = _metaModel.ReportBy == ReportByType.Monthly
                                          ? licenseeWinLoss.Date.ToString("MM/yyyy")
                                          : licenseeWinLoss.Date.ToShortDateString();

                // Merge first two columns
                var needMerge = index >= rowCount - 1 ||
                                 licenseeWinLoss.Date.Date != _dataList[index + 1].Date.Date;

                // Merge cell
                if (needMerge && rowSpan > 1)
                {
                    if (Level == 0)
                    {
                        sheet.MergeCells(index + RowContentTemplate - rowSpan + 1, ColumnIndex, index + RowContentTemplate, ColumnIndex);
                    }

                    sheet.MergeCells(index + RowContentTemplate - rowSpan + 1, ColumnDate, index + RowContentTemplate, ColumnDate);
                }

                rowSpan = needMerge ? 1 : rowSpan + 1;

                var indexid = Level == 0 ? rowIndex : indexDetail++;
                row.Cells[ColumnIndex].SetCellValue(indexid);
                rowIndex = needMerge ? rowIndex + 1 : rowIndex;

                row.Cells[ColumnExternalId].SetCellValue(licenseeWinLoss.ExternalId);
                row.Cells[ColumnSystemId].SetCellValue(licenseeWinLoss.SystemId);
                row.Cells[ColumnDate].SetCellValue(reportDateString);
                row.Cells[ColumnCurrency].SetCellValue(licenseeWinLoss.CurrencyName);
                AssignCommonData(row, licenseeWinLoss);
            }

            rowIndex = AssignTotalData(sheet, index + 3);
            AssignGrandTotalInEurData(sheet, rowIndex);
        }

        protected override void AssignTitle(ISheet sheet)
        {
            var fromdate = _metaModel.FromDate.ToString(FormatDate);
            var todate = _metaModel.ToDate.ToString(FormatDate);
            var title = _metaModel.DrillDownLevel > 0 && _metaModel.CurrencyId == 0
                ? GetExactTitleCurrencyName(_metaModel.DrillDownCurrencyId)
                : GetExactTitleCurrencyName(_metaModel.CurrencyId);

            ReportTitle = string.Format(ViewResources.LicenseeWinLoss + ", {0} --> {1} - {2}", fromdate, todate, title);
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        protected override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                HideColumn(sheet, ColumnCustomerTotal, ColumnCustomerComm);
                sheet.MergeCells(RowHeaderTemplate, ColumnCustomerWl, RowHeaderTemplate + 1, ColumnCustomerWl);
            }

            if (!ShowingOption.COVisible)
            {
                HideColumn(sheet, ColumnCashOutCommission);
            }

            if (ShowingOption.BTVisible)
            {
                if (!_isAllowBA)
                {
                    HideColumn(sheet, ColumnBuyBackAmountTotal, ColumnBuyBackAmountBA);
                    sheet.MergeCells(RowHeaderTemplate, ColumnBuyBackAmountSportsbook, RowHeaderTemplate + 1, ColumnBuyBackAmountSportsbook);
                }
            }
            else
            {
                HideColumn(sheet, ColumnBetTradeCommission);
                HideColumn(sheet, ColumnBuyBackAmountSportsbook, ColumnBuyBackAmountBA, ColumnBuyBackAmountTotal);
                HideColumn(sheet, ColumnActualStake);
            }

            if (!ShowingOption.BTVisible ||
                (_metaModel.ProductOptions.Any(x => x.Code == Product.SB) && !ShowingOption.SBVisible) ||
                (_isAllowBA && !ShowingOption.BAVisible))
            {
                HideColumn(sheet, ColumnAmtDueToBetTrade);
            }

            if (!ShowingOption.BAVisible)
            {
                HideColumn(sheet, ColumnBaCommission, ColumnAmtDueToBa);
            }

            if (!ShowingOption.RBVisible)
            {
                HideColumn(sheet, ColumnRacingRevenue, ColumnAmtDueToRacing);
            }

            ProcessUITurnover(sheet);

            ProcessUILicenseeWinLoss(sheet);

            switch (Level)
            {
                case 0:
                    HideColumn(sheet, ColumnExternalId, ColumnSystemId);
                    break;

                case 1:
                    HideColumn(sheet, ColumnExternalId, ColumnDate);
                    break;

                default:
                    HideColumn(sheet, ColumnDate);
                    break;
            }
        }

        protected void AssignCommonData(IRow row, LicenseeWinLoss data)
        {
            // Set data for turnover
            for (int i = 0; i < _offeredProducts.Count; i++)
            {
                var product = _offeredProducts.ElementAt(i);
                if (product.Value && !IgnoredTurnoverProducts.Contains(product.Key))
                {
                    row.Cells[ColumnTurnoverSportBook + i].SetCellValue(data.TurnOver[product.Key]);
                }
            }

            row.Cells[ColumnTurnoverTotal].SetCellValue(data.TurnOver.Sum(a => a.Value));

            row.Cells[ColumnTotalTransaction].SetCellValue(data.TotalTransaction);
            row.Cells[ColumnCustomerWl].SetCellValue(data.CustomerWinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(data.CustomerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(data.CustomerTotal);

            row.Cells[ColumnActualStake].SetCellValue(data.ActualStake);

            row.Cells[ColumnBuyBackAmountSportsbook].SetCellValue(data.BuyBackAmountSB);
            row.Cells[ColumnBuyBackAmountBA].SetCellValue(data.BuyBackAmountBA);
            row.Cells[ColumnBuyBackAmountTotal].SetCellValue(data.TotalBuyBackAmount);

            // Set data for licenseewinloss
            for (int i = 0; i < _offeredProducts.Count; i++)
            {
                var product = _offeredProducts.ElementAt(i);
                if (product.Value && !IgnoredLicenseeWLProducts.Contains(product.Key))
                {
                    row.Cells[ColumnLicenseeWlSportsBook + i].SetCellValue(data.LicenseeWL[product.Key]);
                }
            }

            row.Cells[ColumnLicenseeWlTotal].SetCellValue(data.LicenseeWL.Sum(a => a.Value));
            row.Cells[ColumnCashOutCommission].SetCellValue(data.CashOutCommission);
            row.Cells[ColumnBetTradeCommission].SetCellValue(data.BetTradeCommission);
            row.Cells[ColumnBaCommission].SetCellValue(data.BACommission);
            row.Cells[ColumnRacingRevenue].SetCellValue(data.RacingRevenue);
            row.Cells[ColumnAmtDueToBetTrade].SetCellValue(data.AmtDueToBetTrade);
            row.Cells[ColumnAmtDueToBa].SetCellValue(data.AmtDueToBA);
            row.Cells[ColumnAmtDueToRacing].SetCellValue(data.AmtDueToRacing);
        }

        /// <summary>
        /// Assign grand total data of EUR.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="rowIndex">The starting row index.</param>
        /// <returns>The last row index after assign data.</returns>
        protected int AssignGrandTotalInEurData(ISheet sheet, int rowIndex)
        {
            var currencies = _summaryTotal.Keys.Reverse();

            foreach (string currency in currencies)
            {
                // Copy template
                var row = sheet.CopyRow(TemplateSheet, RowGrantTotal, rowIndex);
                AssignCommonData(row, _summaryTotal[currency]);
                row.Cells[ColumnIndex].SetCellValue(ViewResources.Totalin + " " + currency.ToUpper());

                // Merge first two column
                sheet.MergeCells(rowIndex, ColumnIndex, rowIndex, ColumnCurrency);
                checked
                {
                    rowIndex++;
                }
            }

            return rowIndex;
        }

        /// <summary>
        /// Assign grand total data.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        /// <param name="rowIndex">The starting row index.</param>
        /// <returns>The last row index after assign data.</returns>
        protected int AssignTotalData(ISheet sheet, int rowIndex)
        {
            int beginTotalRow = rowIndex;
            var currencies = _licenseeWinLossService.CalculateTotal(_dataList);

            foreach (LicenseeWinLoss totalCurrency in currencies)
            {
                IRow row;

                if (rowIndex == beginTotalRow)
                {
                    row = sheet.CopyRow(TemplateSheet, RowTotalSolidBorder, rowIndex);
                    row.Cells[0].SetCellValue(ViewResources.Total);
                }
                else
                {
                    row = sheet.CopyRow(TemplateSheet, RowTotalNoSolidBorder, rowIndex);
                }

                row.Cells[ColumnCurrency].SetCellValue(totalCurrency.CurrencyName);

                AssignCommonData(row, totalCurrency);

                rowIndex++;
            }

            // Merge first two column
            sheet.MergeCells(rowIndex - currencies.Count(), 0, rowIndex - 1, 3);

            return rowIndex;
        }

        protected void ProcessUILicenseeWinLoss(ISheet sheet)
        {
            int nextColumns = 0;

            foreach (var product in _offeredProducts)
            {
                if (!product.Value || IgnoredLicenseeWLProducts.Contains(product.Key))
                {
                    HideColumn(sheet, ColumnLicenseeWlSportsBook + nextColumns, ColumnLicenseeWlSportsBook + nextColumns);
                }
                else
                {
                    sheet.GetCell(RowHeaderTemplate, ColumnLicenseeWlSportsBook + nextColumns)
                    .SetCellValue(ViewResources.LicenseeW_L);
                    sheet.MergeCells(RowHeaderTemplate, ColumnLicenseeWlSportsBook + nextColumns, RowHeaderTemplate, ColumnLicenseeWlTotal);
                }

                nextColumns++;
            }

            int countStatus = _offeredProducts.Count(x => x.Value && !IgnoredLicenseeWLProducts.Contains(x.Key));
            if (countStatus <= 1)
            {
                if (countStatus == 1)
                {
                    HideColumn(sheet, ColumnLicenseeWlTotal, ColumnLicenseeWlTotal);
                }

                for (int i = 0; i < TotalLicenseeWlProductColumn; i++)
                {
                    sheet.MergeCells(RowHeaderTemplate, ColumnLicenseeWlSportsBook + i, RowHeaderTemplate + 1, ColumnLicenseeWlSportsBook + i);
                }
            }
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected void ProcessUITurnover(ISheet sheet)
        {
            int nextColumns = 0;

            foreach (var product in _offeredProducts)
            {
                if (!product.Value || IgnoredTurnoverProducts.Contains(product.Key))
                {
                    HideColumn(sheet, ColumnTurnoverSportBook + nextColumns, ColumnTurnoverSportBook + nextColumns);
                }
                else
                {
                    sheet.GetCell(RowHeaderTemplate, ColumnTurnoverSportBook + nextColumns)
                    .SetCellValue(ViewResources.Turnover);
                    sheet.MergeCells(RowHeaderTemplate, ColumnTurnoverSportBook + nextColumns, RowHeaderTemplate, ColumnTurnoverTotal);
                }

                nextColumns++;
            }

            int countStatus = _offeredProducts.Count(x => x.Value && !IgnoredTurnoverProducts.Contains(x.Key));
            if (countStatus <= 1)
            {
                if (countStatus == 1)
                {
                    HideColumn(sheet, ColumnTurnoverTotal, ColumnTurnoverTotal);
                }

                for (int i = 0; i < TotalTurnoverProductColumn; i++)
                {
                    sheet.MergeCells(RowHeaderTemplate, ColumnTurnoverSportBook + i, RowHeaderTemplate + 1, ColumnTurnoverSportBook + i);
                }
            }
        }
    }
}