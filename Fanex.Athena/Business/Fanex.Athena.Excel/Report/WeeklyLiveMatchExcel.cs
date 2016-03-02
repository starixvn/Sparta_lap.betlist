// --------------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchExcel.cs" company="Nexcel Solutions Vietnam">
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
    public class WeeklyLiveMatchExcel : ReportBase
    {
        private const int ColumnDate = 0;
        private const int ColumnFirstProduct = 1;
        private const int ColumnLastProduct = 12;
        private const int ColumnTotalTransaction = 14;
        private const int ColumnTemplateCount = 15;

        private const int RowTableHeader = 1;
        private const int RowContentTemplate = 3;
        private const int RowGrantTotal = 4;
        private const int RowNoInfoTemplate = 5;

        private readonly IEnumerable<WeeklyLiveMatch> _weeklyLiveMatchList;

        private readonly WeeklyLiveMatchMetaModel _metaModel;
        private readonly WeeklyLiveMatch _totalData;

        public WeeklyLiveMatchExcel(
                string templatePath,
                string exportingFileName,
                WeeklyLiveMatchMetaModel metaModel,
                WeeklyLiveMatchViewModel viewModel)
            : base(templatePath, exportingFileName, null, null)
        {
            _metaModel = metaModel;
            _weeklyLiveMatchList = viewModel.Data;
            _totalData = viewModel.TotalData;
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        public override void AssignData()
        {
            int numOfDataRow = _weeklyLiveMatchList.Count();

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
    }
}