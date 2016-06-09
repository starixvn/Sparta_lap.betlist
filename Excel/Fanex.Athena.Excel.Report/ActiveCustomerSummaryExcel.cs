// --------------------------------------------------------------------------
// <copyright file="ActiveCustomerSummaryExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    /// Active Customer - Export excel.
    /// </summary>
    public class ActiveCustomerSummaryExcel : ReportBase
    {
        private const int ColumnDate = 0;
        private const int ColumnFirstProduct = 1;
        private const int ColumnLastProduct = 14;
        private const int ColumnTotalTransaction = 15;
        private const int ColumnTemplateCount = 16;

        private const int RowTableHeader = 1;
        private const int RowContentTemplate = 3;
        private const int RowGrantTotal = 4;
        private const int RowNoInfoTemplate = 5;

        private readonly IList<ActiveCustomer> _activeCustomerList;

        private readonly ActiveCustomerMetaModel _metaModel;
        private readonly IList<ProductOption> _allProductOptions;
        private readonly int _totalCustomer;
        private IList<int> _hiddenColumnIndexes = new List<int>();

        public ActiveCustomerSummaryExcel(
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
            _allProductOptions = GetAllProductOptions(metaModel.ProductOptions);
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
            sheet.MergeCells(RowTableHeader, ColumnFirstProduct, RowTableHeader, ColumnLastProduct);
            HideColumn(sheet, _hiddenColumnIndexes.ToArray());
        }

        /// <summary>
        /// Assign data to header section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            row.Cells[ColumnDate].SetCellValue(ViewResources.Month);
            row.Cells[ColumnFirstProduct].SetCellValue(ViewResources.Product);
            row.Cells[ColumnTotalTransaction].SetCellValue(ViewResources.Total);

            IRow row2 = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            var prdHeaderList = new List<string>
            {
                ViewResources.Sportsbook,
                ViewResources.ProductRB,
                ViewResources.ProductNG,
                ViewResources.ProductBA,
                ViewResources.ProductLC,
                ViewResources.ProductVS,
                ViewResources.ProductCS,
                ViewResources.ProductBG,
                ViewResources.ProductKN,
                ViewResources.ProductPTCS,
                ViewResources.ProductAGC,
                ViewResources.ProductMGCLC,
                ViewResources.ProductMGCRNG,
                ViewResources.ProductVB
            };

            int index = ColumnFirstProduct;
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
                for (int i = 0; i < _allProductOptions.Count; i++)
                {
                    if (_allProductOptions[i].Status)
                    {
                        row.Cells[ColumnFirstProduct + i].SetCellValue(activeCustomer.CustomerCount[_allProductOptions[i].Code]);
                    }
                    else
                    {
                        row.Cells[ColumnFirstProduct + i].SetCellValue("-");
                    }
                }
            }

            var totalRowIndex = index + RowContentTemplate;
            var totalRow = sheet.CopyRow(TemplateSheet, RowGrantTotal, totalRowIndex);
            totalRow.Cells[0].SetCellValue(ViewResources.TotalActiveCustomer + " = " + _totalCustomer.ToString("#,##0"));
            sheet.MergeCells(totalRowIndex, ColumnDate, totalRowIndex, ColumnTotalTransaction);
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

            ReportTitle = string.Format(ViewResources.ActiveCustomerSummary + ", {0} --> {1} - {2}", fromdate, todate, title);
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Get full product option list for active customer report with exactly 8 products.
        /// </summary>
        /// <param name="products">The product option list. This list may contain 0-8 items.</param>
        /// <returns>The full product option list.</returns>
        protected IList<ProductOption> GetAllProductOptions(IList<ProductOption> products)
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
                new ProductOption { Code = Product.KN, Status = false },
                new ProductOption { Code = Product.PTCS, Status = false },
                new ProductOption { Code = Product.AGC, Status = false },
                new ProductOption { Code = Product.MGCLC, Status = false },
                new ProductOption { Code = Product.MGCRNG, Status = false },
                new ProductOption { Code = Product.VB, Status = false }
            };

            for (var i = 0; i < newProducts.Count; i++)
            {
                var product = products.Where(x => x.Code == newProducts[i].Code).FirstOrDefault();
                if (product == null)
                {
                    _hiddenColumnIndexes.Add(ColumnFirstProduct + i);
                }
                else
                {
                    newProducts[i].Status = product.Status;
                }
            }

            return newProducts;
        }
    }
}