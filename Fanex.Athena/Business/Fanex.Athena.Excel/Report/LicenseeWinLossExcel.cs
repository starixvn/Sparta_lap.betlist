// --------------------------------------------------------------------------
// <copyright file="LicenseeWinLossExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Report
{
    using Base;
    using Entities;
    using Fanex.Athena.Excel.Extension;
    using Fanex.Athena.Models.DataDomain;
    using Models.DTO.Common;
    using Models.DTO.Report;
    using Models.MetaModel.Product;
    using Models.MetaModel.Report;
    using Models.ViewModel;
    using Models.ViewModel.Report;
    using NPOI.SS.UserModel;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Licensee Win Loss - Export excel.
    /// </summary>
    public class LicenseeWinLossExcel : ReportBase
    {
        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnCurrency = 4;

        private const int ColumnTurnoverSportBook = 5;
        private const int ColumnTurnoverTotal = 18;

        private const int ColumnActualStake = 19;

        private const int ColumnBuyBackAmountSportsbook = 20;
        private const int ColumnBuyBackAmountBA = 21;
        private const int ColumnBuyBackAmountTotal = 22;

        private const int ColumnTotalTransaction = 23;

        private const int ColumnCustomerWl = 24;
        private const int ColumnCustomerComm = 25;
        private const int ColumnCustomerTotal = 26;

        private const int ColumnLicenseeWlSportsBook = 27;
        private const int ColumnLicenseeWlTotal = 40;

        private const int ColumnBetTradeCommission = 41;
        private const int ColumnBaCommission = 42;
        private const int ColumnRacingRevenue = 43;
        private const int ColumnAmtDueToBetTrade = 44;
        private const int ColumnAmtDueToBa = 45;
        private const int ColumnAmtDueToRacing = 46;

        private const int ColumnTemplateCount = 47;

        private const int RowContentTemplate = 3;
        private const int RowGrantTotal = 6;
        private const int RowNoInfoTemplate = 7;
        private const int RowTotalNoSolidBorder = 5;
        private const int RowTotalSolidBorder = 4;

        private const int TotalLicenseeWlProductColumn = 13;
        private const int TotalTurnoverProductColumn = 13;

        private static readonly string[] IgnoredLicenseeWLProducts = new string[] { Product.RB, Product.BA };
        private static readonly string[] IgnoredTurnoverProducts = new string[] { Product.BT };

        private readonly bool _isAllowBA;

        private readonly bool _isAllowSB;

        // This is drill down of report
        private readonly IList<LicenseeWinLoss> _licenseeWinLossList;

        private readonly LicenseeWinLossMetaModel _metaModel;
        private readonly IList<ProductOption> _productOption;
        private readonly IDictionary<string, LicenseeWinLoss> _summaryTotal;

        public LicenseeWinLossExcel(
                string templatePath,
                string exportingFileName,
                ConvertingOption convertingOption,
                ShowingOption showingOption,
                LicenseeWinLossMetaModel metaModel,
                LicenseeWinLossViewModel viewModel)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            _metaModel = metaModel;
            _licenseeWinLossList = viewModel.MainResult;
            _summaryTotal = viewModel.SummaryTotal;
            _productOption = GetProductOption(metaModel.ProductOptions);
            _isAllowSB = metaModel.ProductOptions.Any(x => x.Code == Product.SB);
            _isAllowBA = metaModel.ProductOptions.Any(x => x.Code == Product.BA);
        }

        public int Level
        {
            get;
            set;
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _licenseeWinLossList.Count;

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
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        public override void AssignHeader(ISheet sheet)
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

            var headerTexts = new string[]
            {
                ViewResources.Sportsbook,
                ViewResources.ProductRB,
                ViewResources.ProductNG,
                ViewResources.ProductBA,
                ViewResources.ProductLC,
                ViewResources.ProductVS,
                ViewResources.ProductCS,             
                ViewResources.ProductKN,               
                ViewResources.ProductAGC,
                ViewResources.ProductBT,
                ViewResources.ProductMGCLC,
                ViewResources.ProductMGCRNG,
                ViewResources.ProductVB,
                ViewResources.Total
            };

            for (int i = 0; i < headerTexts.Length; i++)
            {
                var text = headerTexts[i];
                row2.Cells[ColumnTurnoverSportBook + i].SetCellValue(text);
                row2.Cells[ColumnLicenseeWlSportsBook + i].SetCellValue(text);
            }
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
            var indexDetail = 1;
            int index;
            var rowCount = _licenseeWinLossList.Count;

            for (index = 0; index < rowCount; index++)
            {
                var licenseeWinLoss = _licenseeWinLossList[index];

                // Copy template
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);

                // Report Date
                var reportDateString = _metaModel.ReportBy == ReportByType.Monthly
                                          ? licenseeWinLoss.Date.ToString("MM/yyyy")
                                          : licenseeWinLoss.Date.ToShortDateString();

                // Merge first two columns
                var needMerge = index >= rowCount - 1 ||
                                 licenseeWinLoss.Date.Date != _licenseeWinLossList[index + 1].Date.Date;

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

        public override void AssignTitle(ISheet sheet)
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
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        public override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                HideColumn(sheet, ColumnCustomerTotal, ColumnCustomerComm);
                sheet.MergeCells(RowHeaderTemplate, ColumnCustomerWl, RowHeaderTemplate + 1, ColumnCustomerWl);
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

            if (!ShowingOption.BTVisible || (_isAllowSB && !ShowingOption.SBVisible) || (_isAllowBA && !ShowingOption.BAVisible))
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

        /// <summary>
        /// Assign common data to excel row.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="data">Licensee win loss data.</param>
        protected void AssignCommonData(IRow row, LicenseeWinLoss data)
        {
            // Set data for turnover
            for (int i = 0; i < _productOption.Count; i++)
            {
                if (_productOption[i].Status && IgnoredTurnoverProducts.Contains(_productOption[i].Code) == false)
                {
                    row.Cells[ColumnTurnoverSportBook + i].SetCellValue(data.TurnOver[_productOption[i].Code]);
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

            // Set data for customerwinloss
            for (int i = 0; i < _productOption.Count; i++)
            {
                if (_productOption[i].Status && IgnoredLicenseeWLProducts.Contains(_productOption[i].Code) == false)
                {
                    row.Cells[ColumnLicenseeWlSportsBook + i].SetCellValue(data.LicenseeWL[_productOption[i].Code]);
                }
            }

            row.Cells[ColumnLicenseeWlTotal].SetCellValue(data.LicenseeWL.Sum(a => a.Value));

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
            var currencies = GetTotalCurrency(_licenseeWinLossList);

            foreach (LicenseeWinLoss totalCurrency in currencies)
            {
                // Copy template
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

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected void ProcessUILicenseeWinLoss(ISheet sheet)
        {
            int countStatus = _productOption.Count(a => a.Status && IgnoredLicenseeWLProducts.Contains(a.Code) == false);
            int nextColumns = 0;

            foreach (var product in _productOption)
            {
                if (product.Status == false || IgnoredLicenseeWLProducts.Contains(product.Code))
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

            if (countStatus <= 1)
            {
                if (countStatus > 0)
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
            int countStatus = _productOption.Count(a => a.Status && IgnoredTurnoverProducts.Contains(a.Code) == false);
            int nextColumns = 0;

            foreach (var product in _productOption)
            {
                if (product.Status == false || IgnoredTurnoverProducts.Contains(product.Code))
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

            if (countStatus <= 1)
            {
                if (countStatus > 0)
                {
                    HideColumn(sheet, ColumnTurnoverTotal, ColumnTurnoverTotal);
                }

                for (int i = 0; i < TotalTurnoverProductColumn; i++)
                {
                    sheet.MergeCells(RowHeaderTemplate, ColumnTurnoverSportBook + i, RowHeaderTemplate + 1, ColumnTurnoverSportBook + i);
                }
            }
        }

        /// <summary>
        /// Get full product option list for licensee win loss report.
        /// </summary>
        /// <param name="products">The product option list.</param>
        /// <returns>The full product option list.</returns>
        private static IList<ProductOption> GetProductOption(IList<ProductOption> products)
        {
            var offeredProducts = new ProductOption[]
            {
                new ProductOption { Code = Product.SB, Status = false },
                new ProductOption { Code = Product.RB, Status = false },
                new ProductOption { Code = Product.NG, Status = false },
                new ProductOption { Code = Product.BA, Status = false },
                new ProductOption { Code = Product.LC, Status = false },
                new ProductOption { Code = Product.VS, Status = false },
                new ProductOption { Code = Product.CS, Status = false },
                new ProductOption { Code = Product.KN, Status = false },
                new ProductOption { Code = Product.AGC, Status = false },
                new ProductOption { Code = Product.BT, Status = false },              
                new ProductOption { Code = Product.MGCLC, Status = false },
                new ProductOption { Code = Product.MGCRNG, Status = false },
                new ProductOption { Code = Product.VB, Status = false }
            };

            var offeredProductsLength = offeredProducts.Count();

            foreach (var product in products)
            {
                for (var j = 0; j < offeredProductsLength; j++)
                {
                    if (product.Code == offeredProducts[j].Code)
                    {
                        offeredProducts[j] = product;
                        break;
                    }
                }
            }

            return offeredProducts;
        }

        /// <summary>
        /// Get total currency data.
        /// </summary>
        /// <param name="dataList">Report data.</param>
        /// <returns>List of LicenseeWinLoss entity.</returns>
        private static IEnumerable<LicenseeWinLoss> GetTotalCurrency(IEnumerable<LicenseeWinLoss> dataList)
        {
            var currencies = dataList.GroupBy(x => new { x.CurrencyName })
                    .Select(group => new LicenseeWinLoss
                    {
                        CurrencyName = group.Key.CurrencyName,
                        TurnOver = new LicenseeWinLoss
                        {
                            LicenseeWL = group.SelectMany(i => i.TurnOver)
                                        .GroupBy(i => i.Key)
                                        .ToDictionary(k => k.Key, k => k.Sum(i => i.Value))
                        }.LicenseeWL,
                        ActualStake = group.Sum(p => p.ActualStake),
                        BuyBackAmountSB = group.Sum(p => p.BuyBackAmountSB),
                        BuyBackAmountBA = group.Sum(p => p.BuyBackAmountBA),
                        TotalTransaction = group.Sum(a => a.TotalTransaction),
                        CustomerWinLoss = group.Sum(a => a.CustomerWinLoss),
                        CustomerComm = group.Sum(a => a.CustomerComm),
                        LicenseeWL = new LicenseeWinLoss
                        {
                            LicenseeWL = group.SelectMany(i => i.LicenseeWL)
                                        .GroupBy(i => i.Key)
                                        .ToDictionary(k => k.Key, k => k.Sum(i => i.Value))
                        }.LicenseeWL,
                        BetTradeCommission = group.Sum(a => a.BetTradeCommission),
                        BACommission = group.Sum(a => a.BACommission),
                        RacingRevenue = group.Sum(a => a.RacingRevenue),
                        AmtDueToBetTrade = group.Sum(a => a.AmtDueToBetTrade),
                        AmtDueToBA = group.Sum(a => a.AmtDueToBA),
                        AmtDueToRacing = group.Sum(a => a.AmtDueToRacing)
                    });

            return currencies;
        }
    }
}