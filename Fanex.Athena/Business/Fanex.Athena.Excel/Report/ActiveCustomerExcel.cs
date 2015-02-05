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
        private const int ColumnDate = 0;
        private const int ColumnTurnoverSportBook = 1;
        private const int ColumnTotalTransaction = 10;
        private const int ColumnTemplateCount = 11;
        private const int RowContentTemplate = 3;
        private const int RowGrantTotal = 4;
        private const int RowNoInfoTemplate = 5;

        private const int TotalTurnoverProductColumn = 8;

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
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            row.Cells[ColumnDate].SetCellValue(ViewResources.Month);
            row.Cells[ColumnTurnoverSportBook].SetCellValue(ViewResources.Product);
            row.Cells[ColumnTotalTransaction].SetCellValue(ViewResources.Total);
            
            IRow row2 = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);
            
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
                ViewResources.Keno
            };

            int index = ColumnTurnoverSportBook;
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
            int index;
            var rowCount = _activeCustomerList.Count;

            for (index = 0; index < rowCount; index++)
            {
                var activeCustomer = _activeCustomerList[index];

                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);

                row.Cells[ColumnDate].SetCellValue(activeCustomer.TransactionDate.ToString("MM/yyyy"));
                row.Cells[ColumnTotalTransaction].SetCellValue(activeCustomer.TotalCustomer);
                for (int i = 0; i < _productOption.Count; i++)
                {
                    if (_productOption[i].Status)
                    {
                        row.Cells[ColumnTurnoverSportBook + i].SetCellValue(activeCustomer.CustomerCountPerProduct[_productOption[i].Code]);
                    }
                }
            }
             
            var totalRow = sheet.CopyRow(TemplateSheet, RowGrantTotal, index + RowContentTemplate);
            totalRow.Cells[0].SetCellValue(ViewResources.TotalActiveCustomer + " = " + _totalCustomer.ToString("#,##0")); 
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
            HideBlankProductColumns(sheet);
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
        /// Hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected void HideBlankProductColumns(ISheet sheet)
        {
            int nextColumns = 0;

            foreach (ProductOption product in _productOption)
            {
                if (product.Status)
                {
                    sheet.MergeCells(RowHeaderTemplate, ColumnTurnoverSportBook + nextColumns, RowHeaderTemplate, ColumnTotalTransaction - 1);
                }
                else
                {
                    HideColumn(sheet, ColumnTurnoverSportBook + nextColumns, ColumnTurnoverSportBook + nextColumns);
                }

                nextColumns++;
            }
        }
    }
}