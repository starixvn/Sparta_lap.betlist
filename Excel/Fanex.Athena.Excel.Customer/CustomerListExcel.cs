// --------------------------------------------------------------------------
// <copyright file="CustomerListExcel.cs" company="">
// Copyright (coffee) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Customer
{
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Fanex.Athena.Excel.Base.Extension;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.Customer.ViewModel;
    using Model.Customer.DTO;
    using NPOI.SS.UserModel;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerListExcel : ReportBase
    {
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
        private const int OddsGroupColumn = 9;
        private const int SBCommColumn = 10;
        private const int RCCommColumn = 11;
        private const int NGCommColumn = 12;
        private const int CSCommColumn = 13;
        private const int ABCommColumn = 14;

        private const string GROUP = "Group";

        private const string BASTATUS = "BAStatus";
        private const string CUSTOMERCOMM = "Commission";

        private const int ColumnTemplateCount = 14;

        private readonly IDictionary<int, string> _gridCommonColumNames = new Dictionary<int, string>
        {
            { ExternalIDColumn, "ExternalId" },
            { SystemIDColumn, "SystemId" },
            { AvailableFundsColumn, "AvailableFunds" },
            { PendingFundColumn, "PendingFunds" },
            { StatusColumn, "Status" },
            { SpecialLevelColumn, "SpecialLevel" },
            { OddsGroupColumn, "OddsGroup" }
        };

        private readonly CustomerListViewModel _viewModel;

        private readonly string _reporTitle;
        private readonly IList<string> _headerTexts;
        private readonly IList<string> _showedGridColumns;

        public CustomerListExcel(string templatePath, string reportTitle, CustomerListViewModel model)
            : base(templatePath, model.DataList.Count())
        {
            _viewModel = model;
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
                ViewResources.BAStatus,
                ViewResources.SpecialLevel,
                ViewResources.OddsGroup,
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

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);

            for (int i = 0; i < _headerTexts.Count; i++)
            {
                row.Cells[i].SetCellValue(_headerTexts[i]);
            }

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row.Cells[SBCommColumn].SetCellValue(ViewResources.SB);
            row.Cells[RCCommColumn].SetCellValue(ViewResources.RB);
            row.Cells[NGCommColumn].SetCellValue(ViewResources.NG);
            row.Cells[CSCommColumn].SetCellValue(ViewResources.CS);
            row.Cells[ABCommColumn].SetCellValue(ViewResources.ProductAB);
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemp, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        protected override void AssignSheetData(ISheet sheet)
        {
            var rowCount = _viewModel.DataList.Count();
            var startNumOfRow = (_viewModel.Param.PageIndex - 1) * _viewModel.Param.PageSize;
            startNumOfRow += 1;
            for (int index = 0; index < rowCount; index++)
            {
                var item = _viewModel.DataList.ElementAt(index);
                var rowIndex = index + RowContentTemplate;
                var numofRow = index + startNumOfRow;
                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, rowIndex);

                if (_viewModel.IsAllowedCustomerCommission && _showedGridColumns.Contains(GROUP))
                {
                    row.Cells[GroupColumn].SetCellValue(item.GroupCommId == 0 ? ViewResources.None : item.Group);
                }

                AssignCommissionColumns(item, row);

                if (_viewModel.ProductAccess.IsAllowBA && _showedGridColumns.Contains(BASTATUS))
                {
                    row.Cells[BAStatusColumn].SetCellValue(item.BAStatus ? ViewResources.On : ViewResources.Off);
                }

                AssignCommonColumns(item, numofRow, row);
            }
        }

        protected void AssignCommonColumns(CustomerDTO item, int numofRow, IRow row)
        {
            var itemType = item.GetType();
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

        protected void AssignCommissionColumns(CustomerDTO item, IRow row)
        {
            if (_viewModel.IsAllowedCustomerCommission && _showedGridColumns.Contains(CUSTOMERCOMM))
            {
                if (_viewModel.ProductAccess.IsAllowSB)
                {
                    row.Cells[SBCommColumn].SetCellValue(item.SBComm.ToStringExt(2));
                }

                if (_viewModel.ProductAccess.IsAllowNG)
                {
                    row.Cells[NGCommColumn].SetCellValue(item.NGComm.ToStringExt(2));
                }

                if (_viewModel.ProductAccess.IsAllowRB)
                {
                    row.Cells[RCCommColumn].SetCellValue(item.RBComm.ToStringExt(2));
                }

                if (_viewModel.ProductAccess.IsAllowCS)
                {
                    row.Cells[CSCommColumn].SetCellValue(item.CSComm.ToStringExt(2));
                }

                if (_viewModel.ProductAccess.IsAllowAB)
                {
                    row.Cells[ABCommColumn].SetCellValue(item.ABComm.ToStringExt(2));
                }
            }
        }

        protected override void AssignTitle(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, _reporTitle);

            /*Freeze 2 rows title and header*/
            sheet.CreateFreezePane(0, 3);

            /*Remove content below header before insert new data.*/
            ClearTemplate(sheet, RowContentTemplate);
        }

        protected override void ProcessUI(ISheet sheet)
        {
            var hiddenColumns = new List<int>();
            /*commission and group columns*/
            if (!_viewModel.IsAllowedCustomerCommission || !_showedGridColumns.Contains(CUSTOMERCOMM))
            {
                sheet.HideColumn(SBCommColumn, NGCommColumn, RCCommColumn, CSCommColumn, ABCommColumn);
                sheet.HideRow(RowHeaderTemplate + 1);
            }
            else
            {
                AddHiddenCommissionColumns(hiddenColumns);
            }

            if (!_viewModel.IsAllowedCustomerCommission || !_showedGridColumns.Contains(GROUP))
            {
                hiddenColumns.Add(GroupColumn);
            }

            /*BA column*/
            if (!_viewModel.ProductAccess.IsAllowBA || !_showedGridColumns.Contains(BASTATUS))
            {
                hiddenColumns.Add(BAStatusColumn);
            }

            /*common columns*/
            foreach (var item in _gridCommonColumNames)
            {
                if (!_showedGridColumns.Contains(item.Value))
                {
                    hiddenColumns.Add(item.Key);
                }
            }

            sheet.HideColumn(hiddenColumns.ToArray());
        }

        private void AddHiddenCommissionColumns(IList<int> hidedColumns)
        {
            if (!_viewModel.ProductAccess.IsAllowSB)
            {
                hidedColumns.Add(SBCommColumn);
            }

            if (!_viewModel.ProductAccess.IsAllowNG)
            {
                hidedColumns.Add(NGCommColumn);
            }

            if (!_viewModel.ProductAccess.IsAllowRB)
            {
                hidedColumns.Add(RCCommColumn);
            }

            if (!_viewModel.ProductAccess.IsAllowCS)
            {
                hidedColumns.Add(CSCommColumn);
            }

            if (!_viewModel.ProductAccess.IsAllowAB)
            {
                hidedColumns.Add(ABCommColumn);
            }
        }
    }
}