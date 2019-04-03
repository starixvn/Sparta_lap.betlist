// --------------------------------------------------------------------------
// <copyright file="CustomerCommGroup.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Customer
{
    using System;
    using System.Data;
    using Base;
    using Base.Entities;
    using Common;
    using Fanex.Athena.Excel.Base.Extension;
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    /// Customer group commission.
    /// </summary>
    public class CustomerCommGroupExcel : ReportBase
    {
        private const int ColumnDefaultGroup = 2;
        private const int ColumnGroupName = 1;
        private const int ColumnNoOfCustomer = 3;
        private const int ColumnSbCommission = 4;
        private const int ColumnNgCommission = 5;
        private const int ColumnCsCommission = 6;

        private const int ColumnTemplateCount = 7;
        private const int RowContentTemplate = 2;
        private const int RowNoInfoTemplate = 5;

        private readonly string _excelTitle = ViewResources.CommissionGroupList;

        private readonly DataTable _exportData;

        public CustomerCommGroupExcel(
                                 string templatePath,
                                 DataTable exportData,
                                 ShowingOption showingOption)
            : base(templatePath, exportData.Rows.Count, showingOption)
        {
            _exportData = exportData;
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var headerTextList = new[]
            {
                "#",
                ViewResources.GroupName,
                ViewResources.DefaultGroup,
                ViewResources.NoOfCustmer,
                ViewResources.SBComm + " (%)",
                ViewResources.NGComm + " (%)",
                ViewResources.CSComm + " (%)"
            };
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate);
            for (int i = 0; i < headerTextList.Length; i++)
            {
                row.Cells[i].SetCellValue(headerTextList[i]);
            }
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void AssignSheetData(ISheet sheet)
        {
            var rowCount = _exportData.Rows.Count;
            int index;

            for (index = 0; index < rowCount; index++)
            {
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);
                var exportRow = _exportData.Rows[index];

                var isDefault = Convert.ToBoolean(exportRow["IsDefault"].ToString());
                var sportsbook = Converter.ToDecimal(exportRow["SBComm"].ToString()) * 100;
                var numberGameCommValue = Converter.ToDecimal(exportRow["NGComm"].ToString()) * 100;
                var rngCommValue = Converter.ToDecimal(exportRow["RNGComm"].ToString()) * 100;

                row.Cells[ColumnIndex].SetCellValue(index + 1);
                row.Cells[ColumnGroupName].SetCellValue(exportRow["GroupName"]);
                row.Cells[ColumnDefaultGroup].SetCellValue(isDefault ? "X" : string.Empty);
                row.Cells[ColumnNoOfCustomer].SetCellValue(exportRow["NumberOfCustomer"]);
                row.Cells[ColumnSbCommission].SetCellValue(sportsbook.ToString("#,##0.00"));
                row.Cells[ColumnNgCommission].SetCellValue(numberGameCommValue.ToString("#,##0.00"));
                row.Cells[ColumnCsCommission].SetCellValue(rngCommValue.ToString("#,##0.00"));
            }
        }

        protected override void AssignTitle(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, ColumnTemplateCount, _excelTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, RowContentTemplate);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        protected override void ProcessUI(ISheet sheet)
        {
            if (!ShowingOption.SBVisible)
            {
                sheet.HideColumn(ColumnSbCommission);
            }

            if (!ShowingOption.NGVisible)
            {
                sheet.HideColumn(ColumnNgCommission);
            }

            if (!ShowingOption.CSVisible)
            {
                sheet.HideColumn(ColumnCsCommission);
            }
        }
    }
}