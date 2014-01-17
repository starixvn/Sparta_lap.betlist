// --------------------------------------------------------------------------
// <copyright file="CustomerCommGroup.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Base;
    using Common;
    using Common.Extensions;
    using Entities;
    using Models.ViewModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Customer group commission.
    /// </summary>
    public class CustomerCommGroup : ReportBase
    {
        private const int ColumnDefaultGroup = 2;
        private const int ColumnGroupName = 1;
        private const int ColumnNgCommission = 6;
        private const int ColumnLcCommission = 7;
        private const int ColumnCsCommission = 8;
        private const int ColumnNoOfCustomer = 3;
        private const int ColumnRbCommission = 5;
        private const int ColumnSbCommission = 4;
        private const int ColumnTemplateCount = 7;
        private const int RowContentTemplate = 2;
        private const int RowNoInfoTemplate = 5;

        private readonly string _excelTitle = ViewResources.CommissionGroupList;
        private readonly DataTable _exportData;

        public CustomerCommGroup(
                                 string templatePath,
                                 string exportingFileName,
                                 DataTable exportData,
                                 ShowingOption showingOption)
            : base(templatePath, exportingFileName, showingOption, new ConvertingOption())
        {
            _exportData = exportData;
            ExportData = exportData;
            HeaderTextList = new List<string>
            {
                "#",
                ViewResources.GroupName,
                ViewResources.DefaultGroup,
                ViewResources.NoOfCustmer,
                ViewResources.SBComm + " (%)",
                ViewResources.RacingComm + " (%)",
                ViewResources.NGComm + " (%)",
                ViewResources.LCComm + " (%)",
                ViewResources.CSComm + " (%)",
            };
        }

        /// <summary>
        /// Assign data to header section.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        /// <summary>
        /// Assign data to excel sheet.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignSheetData(ISheet sheet)
        {
            int rowCount = _exportData.Rows.Count;
            int index;

            for (index = 0; index < rowCount; index++)
            {
                IRow row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);
                DataRow exportRow = _exportData.Rows[index];

                bool isDefault = Convert.ToBoolean(exportRow["IsDefault"].ToString());
                decimal sportsbook = Converter.ToDecimal(exportRow["SBComm"].ToString()) * 100;
                decimal racingCommValue = Converter.ToDecimal(exportRow["RacingComm"].ToString()) * 100;
                decimal numberGameCommValue = Converter.ToDecimal(exportRow["NGComm"].ToString()) * 100;
                decimal liveCasinoCommValue = Converter.ToDecimal(exportRow["LCComm"].ToString()) * 100;
                decimal rngCommValue = Converter.ToDecimal(exportRow["RNGComm"].ToString()) * 100;

                row.Cells[ColumnIndex].SetCellValue(index + 1);
                row.Cells[ColumnGroupName].SetCellValue(exportRow["GroupName"]);
                row.Cells[ColumnDefaultGroup].SetCellValue(isDefault ? "X" : string.Empty);
                row.Cells[ColumnNoOfCustomer].SetCellValue(exportRow["NumberOfCustomer"]);
                row.Cells[ColumnSbCommission].SetCellValue(sportsbook.ToString("#,##0.00"));
                row.Cells[ColumnRbCommission].SetCellValue(racingCommValue.ToString("#,##0.00"));
                row.Cells[ColumnNgCommission].SetCellValue(numberGameCommValue.ToString("#,##0.00"));
                row.Cells[ColumnLcCommission].SetCellValue(liveCasinoCommValue.ToString("#,##0.00"));
                row.Cells[ColumnCsCommission].SetCellValue(rngCommValue.ToString("#,##0.00"));
            }
        }

        /// <summary>
        /// Assign title for the report.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void AssignTitle(ISheet sheet)
        {
            ReportTitle = _excelTitle;
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 2);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);

            AssignHeader(sheet);
        }

        /// <summary>
        /// Process UI, hide redundant row and column.
        /// </summary>
        /// <param name="sheet">Excel sheet.</param>
        public override void ProcessUi(ISheet sheet)
        {
            if (!ShowingOption.RbVisible)
            {
                HideColumn(sheet, ColumnRbCommission);
            }

            if (!ShowingOption.NgVisible)
            {
                HideColumn(sheet, ColumnNgCommission);
            }

            if (!ShowingOption.CsVisible)
            {
               HideColumn(sheet, ColumnCsCommission);
            }
        }
    }
}