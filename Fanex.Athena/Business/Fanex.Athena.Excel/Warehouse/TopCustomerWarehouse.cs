// --------------------------------------------------------------------------
// <copyright file="TopCustomerExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>
namespace Fanex.Athena.Excel.Warehouse
{
    using System.Collections.Generic;
    using System.Data;

    using Fanex.Athena.Excel.Entities;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.NPOI.Base;

    using NPOI.SS.UserModel;

    public class TopCustomerWarehouse : ExcelBase
    {
        private const int ColumnCustomer = 1;
        private const int ColumnLicenseeWL = 6;
        private const int RowContentTemplate = 2;
        private const int RowNoInfoTemplate = 4;
        private const int RowTotalTemplate = 3;

        public TopCustomerWarehouse(string templatePath, string exportingFileName, ConvertingOption convertingOption, ShowingOption showingOption, DataTable exportData)
        : base(templatePath, exportingFileName, showingOption, convertingOption)
        {
            this.ExportData = exportData;
        }

        public override void AssignHeader(ISheet sheet)
        {
            SetReportTitle(sheet, TemplateSheet, ColumnTemplateIndexList, RowTitleTemplate, RowTitle, ReportTitle);

            var row = SetReportHeader(sheet, TemplateSheet, ColumnTemplateIndexList, RowHeaderTemp, RowHeader);
            RenderLicenseeWLHeader(row, ColumnLicenseeWL);
            SetEmptyRow(sheet, TemplateSheet, RowTotalTemplate, RowTotalTemplate);

            SetEmptyRow(sheet, TemplateSheet, RowNoInfoTemplate, RowNoInfoTemplate);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 2);
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetReportTitle(sheet, TemplateSheet, ColumnTemplateIndexList, RowNoInfoTemplate, RowContentTemplate, Resources.Excel.NoInfo);
        }

        public override void AssignSheetData(ISheet sheet)
        {
            var rowCount = ExportData.Rows.Count;

            var columnNameList = GetColumnNameList();

            AddColumns();

            for (var index = 0; index < rowCount; index++)
            {
                var exportRow = ExportData.Rows[index];

                if (!CheckError())
                {
                    AssignCellData(sheet, columnNameList, exportRow, index + 2, index);
                }
                else
                {
                    SetReportTitle(sheet,
                                   TemplateSheet,
                                   ColumnTemplateIndexList,
                                   RowContentTemplate,
                                   RowContentTemplate,
                                   Resources.Excel.TopCustomerError);
                }
            }

            AssignTotalCellData(sheet, columnNameList, ExportData.Rows.Count + 2);
        }

        /// <summary>
        /// Add columns licenseewinloss, totalgrossprofit
        /// </summary>
        protected void AddColumns()
        {
            ExportData.Columns.Add("licenseewinloss", typeof(decimal), "-1*memberwl");
            ExportData.Columns.Add("totalgrossprofit", typeof(decimal), "licenseewinloss + commission");
        }

        protected void AssignCellData(ISheet sheet, IList<string> columnNameList, DataRow exportRow, int currentRow, int index)
        {
            var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + 2, ColumnTemplateIndexList, DefaultStyle);

            row.Cells[ColumnIndex].SetCellValue(index + 1);

            foreach (var columnName in columnNameList)
            {
                AssignRowContent(row, exportRow, columnNameList, ColumnCustomer);
            }
        }

        protected void AssignTotalCellData(ISheet sheet, IList<string> columnNameList, int currentRow)
        {
            var cellIndex = 3;

            var row = sheet.CopyRow(TemplateSheet, RowTotalTemplate, currentRow, ColumnTemplateIndexList, DefaultStyle);

            columnNameList = GetTotalColumnNameList(columnNameList);

            foreach (var columnName in columnNameList)
            {
                cellIndex = AssignDecimalData(row, cellIndex, Converter.To<decimal>(ExportData.Compute("Sum(" + columnName + ")", string.Empty)));
            }

            sheet.MergeCells(currentRow, ColumnIndex, currentRow, 2);
        }

        /// <summary>
        /// Check data has error or hasn't error
        /// </summary>
        /// <returns></returns>
        protected bool CheckError()
        {
            var checkError = false;
            if (ExportData.Rows.Count != 0)
            {
                if (ExportData.Columns.Contains("errorcode"))
                {
                    if (ExportData.Rows[0]["errorcode"] != null)
                    {
                        checkError = true;
                    }
                }
            }

            return checkError;
        }

        /// <summary>
        /// Check column lastactivitydate has exist or not exist
        /// </summary>
        /// <returns></returns>
        protected bool CheckLastLoginColumn()
        {
            if (ExportData.Rows.Count != 0)
            {
                // Check exits lastLogin column
                var exitsLastLoginColumn = ExportData.Columns.Contains("lastactivitydate");

                if (!exitsLastLoginColumn)
                {
                    return false;
                }
            }

            return true;
        }

        protected IList<string> GetColumnNameList()
        {
            var columnNameList = new List<string> { "member", "currency", "turnover100", "betcount", "memberwl", "licenseewinloss", "commission", "totalgrossprofit", "lastactivitydate", "errorcode", "message" };

            var checkFieldError = ExportData.Columns.Contains("errorcode");

            var checkLastLoginColumn = CheckLastLoginColumn();

            if (ShowingOption.BaVisible == false)
            {
                columnNameList.Remove("commission");
                columnNameList.Remove("totalgrossprofit");
            }

            if (!checkLastLoginColumn)
            {
                columnNameList.Remove("lastactivitydate");
            }

            if (!checkFieldError)
            {
                columnNameList.Remove("errorcode");
                columnNameList.Remove("message");
            }

            return columnNameList;
        }

        /// <summary>
        /// 0: # (index), 1: customer, 2: currency, 3: turnover, 4: betcount, 5: customer w/l
        /// 6: licensee w/l, 7 ba commission, 8: total gross profit, 9: last activity date
        /// </summary>
        /// <returns></returns>
        protected override IList<int> GetColumnTemplateIndex()
        {
            if (ShowingOption.BaVisible == false)
            {
                return RemoveColumn(new List<int> { 0, 1, 2, 3, 4, 5, 6, 9 });
            }
            else
            {
                return RemoveColumn(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            }
        }

        protected IList<string> GetTotalColumnNameList(IList<string> columnNameList)
        {
            var checkLastLoginColumn = CheckLastLoginColumn();

            columnNameList.Remove("member");
            columnNameList.Remove("currency");

            if (checkLastLoginColumn)
            {
                columnNameList.Remove("lastactivitydate");
            }

            return columnNameList;
        }

        protected IList<int> RemoveColumn(List<int> listColumn)
        {
            // Check exits lastLogin column
            var exitsLastLoginColumn = CheckLastLoginColumn();

            if (!exitsLastLoginColumn)
            {
                listColumn.RemoveAt(listColumn.Count - 1);
            }

            return listColumn;
        }
    }
}