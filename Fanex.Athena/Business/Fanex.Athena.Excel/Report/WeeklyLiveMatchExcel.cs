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
    using Fanex.Athena.Excel.Extension;
    using Models.DTO.Report;
    using Models.MetaModel.Report;
    using NPOI.SS.UserModel;
    using Resources;

    public class WeeklyLiveMatchExcel : ReportBase
    {
        private const int ColumnSport = 1;
        private const int ColumnTemplateCount = 12;
        private const int NumberOfStaticColumns = 2; // Column: Index, Sport.
        private const int MaxWeekColumns = 10;

        private const int RowContentTemplate = 2;
        private const int RowTotalIndex = 3;
        private const int RowNoInfoTemplate = 4;

        private readonly IEnumerable<WeeklyLiveMatch> _dataList;

        private readonly WeeklyLiveMatchMetaModel _metaModel;
        private readonly int[] _totalData;
        private readonly IEnumerable<string> _weekHeaders;

        public WeeklyLiveMatchExcel(
                string templatePath,
                string exportingFileName,
                WeeklyLiveMatchMetaModel metaModel,
                IEnumerable<WeeklyLiveMatch> dataList,
                int[] totalData,
                IEnumerable<string> weekHeaders)
            : base(templatePath, exportingFileName, null, null)
        {
            _totalData = totalData;
            _metaModel = metaModel;
            _dataList = dataList;
            _weekHeaders = weekHeaders;
        }

        public override void AssignData()
        {
            int numOfDataRow = _dataList.Count();

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
            AssignTotalRow(sheet, numOfDataRow + 2);
            var hiddenColumns = Enumerable.Range(_metaModel.NumberOfWeeks + NumberOfStaticColumns, MaxWeekColumns - _metaModel.NumberOfWeeks);
            HideColumn(sheet, hiddenColumns.ToArray());
        }

        public override void AssignTitle(ISheet sheet)
        {
            var fromdate = _metaModel.FromDate.ToString(FormatDate);
            var todate = _metaModel.ToDate.ToString(FormatDate);

            ReportTitle = string.Format(ViewResources.WeeklyLiveMatches + ", {0} --> {1}", fromdate, todate);
            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnTemplateCount, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 2);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        public override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);
            row.Cells[ColumnSport].SetCellValue(ViewResources.Sport);

            var index = ColumnSport + 1;
            foreach (var header in _weekHeaders)
            {
                row.Cells[index++].SetCellValue(header);
            }
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
            ProcessUI(sheet);
        }

        public override void AssignSheetData(ISheet sheet)
        {
            int index;
            var rowCount = _dataList.Count();

            for (index = 0; index < rowCount; index++)
            {
                var dataItem = _dataList.ElementAt(index);

                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);

                row.Cells[ColumnIndex].SetCellValue(dataItem.Index);
                row.Cells[ColumnSport].SetCellValue(dataItem.SportName);

                var weekColumnIndex = ColumnSport + 1;
                for (int i = 0; i < _metaModel.NumberOfWeeks; i++)
                {
                    row.Cells[weekColumnIndex++].SetCellValue(dataItem.NumbersOfMatches.ElementAt(i));
                }
            }
        }

        private void AssignTotalRow(ISheet sheet, int rowIndex)
        {
            int cellIndex = 2;

            IRow row = sheet.CopyRow(TemplateSheet, RowTotalIndex, rowIndex);

            row.Cells[ColumnSport].SetCellValue(ViewResources.Total);
            sheet.MergeCells(rowIndex, 0, rowIndex, 1);

            for (int i = 0; i < _metaModel.NumberOfWeeks; i++)
            {
                row.Cells[cellIndex++].SetCellValue(_totalData[i]);
            }
        }
    }
}