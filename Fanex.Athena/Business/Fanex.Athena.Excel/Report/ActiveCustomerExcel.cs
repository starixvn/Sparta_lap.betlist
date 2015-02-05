// --------------------------------------------------------------------------
// <copyright file="ActiveCustomerExcel.cs" company="Nexcel Solutions Vietnam">
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
    using Models.MetaModel.Product;
    using Models.MetaModel.Report;
    using Models.ViewModel;
    using Models.ViewModel.Report;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Active Customer - Export excel.
    /// </summary>
    public class ActiveCustomerExcel : ReportBase
    {
        private const int ColumnTurnoverTotal = 14;
        private const int ColumnTotalTransaction = 15;
        private const int ColumnCustomerWl = 16;
        private const int ColumnCustomerComm = 17;
        private const int ColumnCustomerTotal = 18;
        private const int ColumnLicenseeWlSportsBook = 19;
        private const int ColumnLicenseeWlTotal = 28;
        private const int ColumnBaCommission = 29;
        private const int ColumnRacingRevenue = 30;
        private const int ColumnAmtDueToBa = 31;
        private const int ColumnAmtDueToRacing = 32;
        private const int ColumnTemplateCount = 33;
        private const int ColumnDate = 1;
        private const int ColumnExternalId = 2;
        private const int ColumnSystemId = 3;
        private const int ColumnCurrency = 4;
        private const int ColumnTurnoverSportBook = 5;
        private const int RowContentTemplate = 3;
        private const int RowGrantTotal = 6;
        private const int RowNoInfoTemplate = 7;
        private const int RowTotalNoSolidBorder = 5;
        private const int RowTotalSolidBorder = 4;

        private const int TotalLicenseeWlProductColumn = 9;
        private const int TotalTurnoverProductColumn = 9;

        // This is drill down of report
        private readonly IList<ActiveCustomer> _activeCustomerList;

        private readonly ActiveCustomerMetaModel _metaModel;
        private readonly IList<ProductOption> _productOption;
        private readonly int _totalCustomer;

        public ActiveCustomerExcel(
                string templatePath,
                string exportingFileName,
                ConvertingOption convertingOption,
                ShowingOption showingOption,
                ActiveCustomerMetaModel metaModel,
                ActiveCustomerViewModel viewModel)
            : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            _metaModel = metaModel;
            _activeCustomerList = viewModel.MainResult;
            _totalCustomer = viewModel.TotalCustomer;
            _productOption = GetProductOption(metaModel.ProductOptions);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _activeCustomerList.Count;

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
            // row 1
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            row.Cells[ColumnIndex].SetCellValue("#");
            row.Cells[ColumnExternalId].SetCellValue(ViewResources.ExternalId);
            row.Cells[ColumnSystemId].SetCellValue(ViewResources.SystemId);
            row.Cells[ColumnDate].SetCellValue(ViewResources.Date);
            row.Cells[ColumnCurrency].SetCellValue(ViewResources.Currency);
            row.Cells[ColumnTurnoverSportBook].SetCellValue(ViewResources.Turnover);
            row.Cells[ColumnTotalTransaction].SetCellValue(ViewResources.TotalTransaction);
            row.Cells[ColumnCustomerWl].SetCellValue(ViewResources.CustomerW_L);
            row.Cells[ColumnLicenseeWlSportsBook].SetCellValue(ViewResources.LicenseeW_L);
            row.Cells[ColumnLicenseeWlTotal].SetCellValue(ViewResources.LicenseeW_L);
            row.Cells[ColumnBaCommission].SetCellValue(ViewResources.BACommission);
            row.Cells[ColumnRacingRevenue].SetCellValue(ViewResources.RacingRevenue);
            row.Cells[ColumnAmtDueToBa].SetCellValue(ViewResources.AmtDueToBA);
            row.Cells[ColumnAmtDueToRacing].SetCellValue(ViewResources.AmtDueToRacing);

            // row 2
            IRow row2 = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);
            row2.Cells[ColumnCustomerWl].SetCellValue(ViewResources.WinLoss);
            row2.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Comm);
            row2.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);

            var prdHeaderList = new List<string>
            {
                ViewResources.Sportsbook,
                ViewResources.Racing,
                ViewResources.NumberGame,
                ViewResources.BA,
                ViewResources.LiveCasino,
                ViewResources.VirtualSports,
                ViewResources.RNGCasino,
                ViewResources.Bingo,
                ViewResources.Keno,
                ViewResources.Total
            };

            int index = ColumnTurnoverSportBook;
            foreach (string item in prdHeaderList)
            {
                row2.Cells[index++].SetCellValue(item);
            }

            index = ColumnLicenseeWlSportsBook;
            foreach (string item in prdHeaderList)
            {
                row2.Cells[index++].SetCellValue(item);
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
            var rowIndex = 1;
            int index;
            var rowCount = _activeCustomerList.Count;

            for (index = 0; index < rowCount; index++)
            {
                var activeCustomer = _activeCustomerList[index];

                // Copy template
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);

                var indexid = rowIndex;
                row.Cells[ColumnIndex].SetCellValue(indexid);

                row.Cells[ColumnDate].SetCellValue(activeCustomer.TransactionDate);                
                AssignCommonData(row, activeCustomer);
            }
             
            var totalRow = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);
            totalRow.Cells[0].SetCellValue(ViewResources.TotalActiveCustomer + " = " + _totalCustomer); 
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            var fromdate = _metaModel.FromDate.ToString(FormatDate);
            var todate = _metaModel.ToDate.ToString(FormatDate);
            var title = GetExactTitleCurrencyName(_metaModel.CurrencyId);

            ReportTitle = string.Format(ViewResources.ActiveCustomer + ", {0} --> {1} - {2}", fromdate, todate, title);
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
        public override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.CustomerCommVisible)
            {
                HideColumn(sheet, ColumnCustomerTotal, ColumnCustomerComm);
                sheet.MergeCells(RowHeaderTemplate, ColumnCustomerWl, RowHeaderTemplate + 1, ColumnCustomerWl);
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

            ProcessUIActiveCustomer(sheet);

            HideColumn(sheet, ColumnExternalId, ColumnSystemId);
        }

        /// <summary>
        /// Get full product option list for active customer report with exactly 8 products.
        /// </summary>
        /// <param name="products">The product option list. This list may contain 0-8 items.</param>
        /// <returns>The full product option list.</returns>
        protected static IList<ProductOption> GetProductOption(IList<ProductOption> products)
        {
            var newProducts = new List<ProductOption>
            {
                new ProductOption { Code = Product.SB, Status = false },
                new ProductOption { Code = Product.RB, Status = false },
                new ProductOption { Code = Product.NG, Status = false },
                new ProductOption { Code = Product.BA, Status = false },
                new ProductOption { Code = Product.LC, Status = false },
                new ProductOption { Code = Product.VS, Status = false },
                new ProductOption { Code = Product.CS, Status = false },
                new ProductOption { Code = Product.BG, Status = false },
                new ProductOption { Code = Product.KN, Status = false }
            };

            foreach (var t in products)
            {
                for (var j = 0; j < newProducts.Count; j++)
                {
                    if (t.Code == newProducts[j].Code)
                    {
                        newProducts[j] = t;
                        break;
                    }
                }
            }

            return newProducts;
        }

        /// <summary>
        /// Assign common data to excel row.
        /// </summary>
        /// <param name="row">Excel row.</param>
        /// <param name="data">Licensee win loss data.</param>
        protected void AssignCommonData(IRow row, ActiveCustomer data)
        {
            //// Set data for turnover
            for (int i = 0; i < _productOption.Count; i++)
            {
                if (_productOption[i].Status)
                {
                    row.Cells[ColumnTurnoverSportBook + i].SetCellValue(data.CustomerCountPerProduct[_productOption[i].Code]);
                }
            }

            row.Cells[ColumnTurnoverTotal].SetCellValue(data.CustomerCountPerProduct.Sum(a => a.Value));
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected void ProcessUIActiveCustomer(ISheet sheet)
        {
            int countStatus = _productOption.Count(a => a.Status && a.Code != Product.RB && a.Code != Product.BA);
            int nextColumns = 0;

            foreach (ProductOption product in _productOption)
            {
                if (product.Status == false || product.Code == Product.RB || product.Code == Product.BA)
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
                if (_productOption.Any(a => a.Status && a.Code != Product.RB && a.Code != Product.BA))
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
            int countStatus = _productOption.Count(a => a.Status);
            int nextColumns = 0;

            foreach (ProductOption product in _productOption)
            {
                if (product.Status == false)
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
                HideColumn(sheet, ColumnTurnoverTotal, ColumnTurnoverTotal);
                for (int i = 0; i < TotalTurnoverProductColumn; i++)
                {
                    sheet.MergeCells(RowHeaderTemplate, ColumnTurnoverSportBook + i, RowHeaderTemplate + 1, ColumnTurnoverSportBook + i);
                }
            }
        }
    }
}