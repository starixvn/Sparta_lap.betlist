// --------------------------------------------------------------------------
// <copyright file="CustomerListExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (coffee) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Alex.Nguyen" date="11/13/2013">Created</change>
// </history>

namespace Fanex.Athena.Excel.Customer
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using Fanex.Athena.Excel.Base;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.Customer;
    using Fanex.Common.Extensions;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerListExcel : ExcelBase
    {
        private const int MaxExcelRowCount = 65000;
        private const int RowHeaderTemplate = 1;
        private const int RowTitleTemplate = 0;
        private const int RowNoInfoTemp = 4;
        private const int RowContentTemplate = 3;
        private const int GroupColumn = 1;
        private const int ExternalIDColumn = 2;
        private const int SystemIDColumn = 3;
        private const int AvailableFundsColumn = 4;
        private const int PendingFundColumn = 5;
        private const int StatusColumn = 6;
        private const int BAStatusColumn = 7;
        private const int SpecialLevelColumn = 8;
        private const int SBCommColumn = 9;
        private const int RCCommColumn = 10;
        private const int NGCommColumn = 11;
        private const int LCCommColumn = 12;
        private const int CSCommColumn = 13;
        private const string GROUP = "Group";
        private const string BASTATUS = "BaStatus";
        private const string CUSTOMERCOMM = "Commission";

        private readonly Dictionary<int, string> _gridCommonColumNames = new Dictionary<int, string>
        {
            { ExternalIDColumn, "ExternalId" },
            { SystemIDColumn, "SystemId" },
            { AvailableFundsColumn, "AvailableFunds" },
            { PendingFundColumn, "PendingFunds" },
            { StatusColumn, "Status" },
            { SpecialLevelColumn, "SpecialLevel" }
        };

        private CustomerListViewModel _exportData;
        private string _templatePath;
        private ISheet _templateSheet;
        private ICellStyle _defaultStyle;
        private int _columnTemplateCount = 13;
        private string _reporTitle;
        private IList<string> _headerTexts;
        private IList<string> _showedGridColumns;

        public CustomerListExcel(string templatePath, string reportTitle, CustomerListViewModel model)
        {
            _templatePath = templatePath;
            this._exportData = model;
            _reporTitle = reportTitle;
            _headerTexts = new List<string>
            {
                "#",
                ViewResources.Group,
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.AvailableFunds,
                ViewResources.PendingFunds,
                ViewResources.Status,
                ViewResources.BaStatus,
                ViewResources.SpecialLevel,
                ViewResources.Commission + " (%)",
            };
            _showedGridColumns = new List<string>();
            foreach (var column in model.Param.GridColumns)
            {
                if (column.Checked)
                {
                    _showedGridColumns.Add(column.Name);
                }
            }
        }

        public HSSFWorkbook Workbook { get; set; }

        public void Export(Stream stream)
        {
            Export();
            Write(stream);
        }

        public void Export()
        {
            LoadTemplate();

            AssignData();

            RemoveTemplate();
        }

        /// <summary>
        /// Assign data to sheet.
        /// </summary>
        private void AssignData()
        {
            ISheet sheet = Workbook.CloneSheet("Template", "Data");

            AssignHeader(sheet);
            AssignTitle(sheet);
            if (this._exportData.DataList.Count == 0)
            {
                AssignNoInfoRow(sheet);
            }
            else
            {
                AssignSheetData(sheet);
            }

            ProcessUi(sheet);
        }

        /// <summary>
        /// Assign text to header column.
        /// </summary>
        /// <param name="sheet">The current sheet.</param>
        private void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(_templateSheet, RowHeaderTemplate, RowHeaderTemplate);

            for (int i = 0; i < _headerTexts.Count; i++)
            {
                row.Cells[i].SetCellValue(_headerTexts[i]);
            }

            row = sheet.CopyRow(_templateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row.Cells[SBCommColumn].SetCellValue(ViewResources.SB);
            row.Cells[RCCommColumn].SetCellValue(ViewResources.Racing);
            row.Cells[NGCommColumn].SetCellValue(ViewResources.NG);
            row.Cells[LCCommColumn].SetCellValue(ViewResources.LC);
            row.Cells[CSCommColumn].SetCellValue(ViewResources.CS);
        }

        /// <summary>
        /// Assign an empty row which contain a explained text.
        /// </summary>
        /// <param name="sheet">The current sheet.</param>
        private void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemp, RowContentTemplate, _columnTemplateCount, ViewResources.NoInfo);
            ProcessUi(sheet);
        }

        /// <summary>
        /// Assign report data to current sheet.
        /// </summary>
        /// <param name="sheet">The current sheet.</param>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void AssignSheetData(ISheet sheet)
        {
            int rowCount = _exportData.DataList.Count;
            var startNumOfRow = (_exportData.Param.PageIndex - 1) * _exportData.Param.PageSize;
            startNumOfRow += 1;
            for (int index = 0; index < rowCount; index++)
            {
                var item = _exportData.DataList[index];
                var itemType = item.GetType();
                var rowIndex = index + RowContentTemplate;
                var numofRow = index + startNumOfRow;
                IRow row = sheet.CopyRow(_templateSheet, RowContentTemplate, rowIndex);
                /*group columns*/
                if (_exportData.IsAllowedCustomerCommission && _showedGridColumns.Contains(GROUP))
                {
                    row.Cells[GroupColumn].SetCellValue(item.GroupCommId == 0 ? ViewResources.None : item.GroupName);
                }

                /* commission column*/
                if (_exportData.IsAllowedCustomerCommission && _showedGridColumns.Contains(CUSTOMERCOMM))
                {
                    if (_exportData.ProductAccess.IsAllowSB)
                    {
                        row.Cells[SBCommColumn].SetCellValue((item.SbComm * 100).ToStringExt(2));
                    }

                    if (_exportData.ProductAccess.IsAllowNg)
                    {
                        row.Cells[NGCommColumn].SetCellValue((item.NgComm * 100).ToStringExt(2));
                    }

                    if (_exportData.ProductAccess.IsAllowRb)
                    {
                        row.Cells[RCCommColumn].SetCellValue((item.RbComm * 100).ToStringExt(2));
                    }

                    if (_exportData.ProductAccess.IsAllowLc)
                    {
                        row.Cells[LCCommColumn].SetCellValue((item.LcComm * 100).ToStringExt(2));
                    }

                    if (_exportData.ProductAccess.IsAllowCs)
                    {
                        row.Cells[CSCommColumn].SetCellValue((item.CsComm * 100).ToStringExt(2));
                    }
                }

                /*BA column*/
                if (_exportData.ProductAccess.IsAllowBa && _showedGridColumns.Contains(BASTATUS))
                {
                    row.Cells[BAStatusColumn].SetCellValue(item.BaStatus ? ViewResources.On : ViewResources.Off);
                }

                /* common column */
                row.Cells[0].SetCellValue(numofRow);
                foreach (var columnName in _gridCommonColumNames)
                {
                    if (_showedGridColumns.Contains(columnName.Value))
                    {
                        var cellValue = itemType.GetProperty(columnName.Value).GetValue(item, null);
                        if (columnName.Value.Equals("SpecialLevel") && (int)cellValue == 0)
                        {
                            row.Cells[columnName.Key].SetCellValue(ViewResources.None);
                        }
                        else
                        {
                            row.Cells[columnName.Key].SetCellValue(cellValue);
                        }
                    }
                }
            }
        }

        private void AssignTitle(ISheet sheet)
        {
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, _columnTemplateCount, _reporTitle);

            /*Freeze 2 rows title and header*/
            sheet.CreateFreezePane(0, 3);

            /*Remove content below header before insert new data.*/
            ClearTemplate(sheet, RowContentTemplate);
        }

        /// <summary>
        /// Load template.
        /// </summary>
        private void LoadTemplate()
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(this._templatePath, FileMode.Open, FileAccess.Read);
                this.Workbook = new HSSFWorkbook(stream);
                this._templateSheet = Workbook.GetSheet("Template");
                _defaultStyle = Workbook.CreateCellStyle();
                _defaultStyle.ShrinkToFit = true;
                _defaultStyle.WrapText = true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Hide  columns.
        /// </summary>
        /// <param name="sheet">The current sheet.</param>
        [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private void ProcessUi(ISheet sheet)
        {
            var hidedColumns = new List<int>();
            /*commission and group columns*/
            if (!_exportData.IsAllowedCustomerCommission || !_showedGridColumns.Contains(CUSTOMERCOMM))
            {
                HideColumn(sheet, SBCommColumn, NGCommColumn, RCCommColumn, LCCommColumn, CSCommColumn);
                HideRow(sheet, RowHeaderTemplate + 1);
            }
            else
            {
                if (!_exportData.ProductAccess.IsAllowSB)
                {
                    hidedColumns.Add(SBCommColumn);
                }

                if (!_exportData.ProductAccess.IsAllowNg)
                {
                    hidedColumns.Add(NGCommColumn);
                }

                if (!_exportData.ProductAccess.IsAllowRb)
                {
                    hidedColumns.Add(RCCommColumn);
                }

                if (!_exportData.ProductAccess.IsAllowLc)
                {
                    hidedColumns.Add(LCCommColumn);
                }

                if (!_exportData.ProductAccess.IsAllowCs)
                {
                    hidedColumns.Add(CSCommColumn);
                }
            }

            if (!_exportData.IsAllowedCustomerCommission || !_showedGridColumns.Contains(GROUP))
            {
                hidedColumns.Add(GroupColumn);
            }

            /*BA column*/
            if (!_exportData.ProductAccess.IsAllowBa || !_showedGridColumns.Contains(BASTATUS))
            {
                hidedColumns.Add(BAStatusColumn);
            }

            /*common columns*/
            foreach (var item in _gridCommonColumNames)
            {
                if (!_showedGridColumns.Contains(item.Value))
                {
                    hidedColumns.Add(item.Key);
                }
            }

            HideColumn(sheet, hidedColumns.ToArray());
        }

        /// <summary>
        /// Remove template.
        /// </summary>
        private void RemoveTemplate()
        {
            this.Workbook.RemoveSheet("Template");
        }

        private void Write(Stream stream)
        {
            this.Workbook.Write(stream);
        }

        /// <summary>
        ///     Clear the template from header row.
        /// </summary>
        /// <param name="sheet">The excel sheet.</param>
        /// <param name="startRowIndex">The starting row index of sheet.</param>
        private void ClearTemplate(ISheet sheet, int startRowIndex)
        {
            int lastRowNum = sheet.LastRowNum;
            for (int i = startRowIndex; i <= lastRowNum; i++)
            {
                sheet.DeleteRow(i);
            }
        }

        private void SetTextRowValue(ISheet sheet, int sourceRowIndex, int destRowIndex, int columnCount, string value)
        {
            IRow row = sheet.CopyRow(_templateSheet, sourceRowIndex, destRowIndex);
            sheet.MergeCells(destRowIndex, 0, destRowIndex, columnCount - 1);
            row.Cells[0].SetCellValue(value);
        }
    }
}