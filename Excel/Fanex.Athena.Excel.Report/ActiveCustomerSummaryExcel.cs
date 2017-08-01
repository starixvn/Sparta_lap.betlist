// --------------------------------------------------------------------------
// <copyright file="ActiveCustomerSummaryExcel.cs" company="">
// Copyright (c) . All rights reserved.
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

    public class ActiveCustomerSummaryExcel : ReportBase
    {
        private const int ColumnDate = 0;
        private const int ColumnFirstProduct = 1;
        private const int ColumnLastProduct = 18;
        private const int ColumnTotalTransaction = 19;
        private const int ColumnTemplateCount = 20;

        private const int RowTableHeader = 1;
        private const int RowContentTemplate = 3;
        private const int RowGrantTotal = 4;
        private const int RowNoInfoTemplate = 5;

        private static readonly IEnumerable<string> OfferedProducts = new string[]
        {
            Product.SB, Product.RB, Product.NG, Product.BA, Product.LC, Product.VS, Product.CS, Product.BG, Product.KN,
            Product.PTCS, Product.AGC, Product.MGCLC, Product.CB, Product.MGCRNG, Product.AB, Product.VB, Product.FW, Product.VS2
        };

        private readonly IList<ActiveCustomer> _activeCustomerList;

        private readonly ActiveCustomerMetaModel _metaModel;
        private readonly IList<ProductOption> _allProductOptions;
        private readonly int _totalCustomer;
        private readonly IList<int> _hiddenColumnIndexes = new List<int>();

        public ActiveCustomerSummaryExcel(
                string templatePath,
                ConvertingOption convertingOption,
                ShowingOption showingOption,
                ActiveCustomerMetaModel metaModel,
                ActiveCustomerViewModel viewModel)
            : base(templatePath, showingOption, convertingOption)
        {
            _metaModel = metaModel;
            _activeCustomerList = viewModel.MainResult;
            _totalCustomer = viewModel.TotalCustomer;
            _allProductOptions = GetAllProductOptions(metaModel.ProductOptions);
        }

        protected override void AssignData()
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

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            row.Cells[ColumnDate].SetCellValue(ViewResources.Month);
            row.Cells[ColumnFirstProduct].SetCellValue(ViewResources.Product);
            row.Cells[ColumnTotalTransaction].SetCellValue(ViewResources.Total);

            var row2 = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            int index = ColumnFirstProduct;

            foreach (string productCode in OfferedProducts)
            {
                var header = ViewResources.ResourceManager.GetString($"Product{productCode}");
                row2.Cells[index++].SetCellValue(header);
            }
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        protected override void AssignSheetData(ISheet sheet)
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

        protected override void AssignTitle(ISheet sheet)
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

        protected IList<ProductOption> GetAllProductOptions(IEnumerable<ProductOption> products)
        {
            var result = new List<ProductOption>();

            for (var i = 0; i < OfferedProducts.Count(); i++)
            {
                var product = products.FirstOrDefault(x => x.Code == OfferedProducts.ElementAt(i));
                result.Add(new ProductOption
                {
                    Code = OfferedProducts.ElementAt(i),
                    Status = product != null
                });

                if (product == null)
                {
                    _hiddenColumnIndexes.Add(ColumnFirstProduct + i);
                }
            }

            return result;
        }
    }
}