// --------------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchExcel.cs" company="">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Base.Extension;
    using Model.Report.MetaModel;
    using Model.Report.ResultModel;
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

        private readonly WeeklyLiveMatchMetaModel _metaModel;
        private readonly WeeklyLiveMatchResultModel _resultModel;

        private readonly IEnumerable<string> _weekHeaders;

        protected internal WeeklyLiveMatchExcel() : base(string.Empty, 0)
        {
        }

        public WeeklyLiveMatchExcel(
                string templatePath,
                WeeklyLiveMatchMetaModel metaModel,
                WeeklyLiveMatchResultModel resultModel,
                IEnumerable<string> weekHeaders)
            : base(templatePath, resultModel.DataList.Count())
        {
            _metaModel = metaModel;
            _resultModel = resultModel;
            _weekHeaders = weekHeaders;
        }

        protected override void ProcessUI(ISheet sheet)
        {
            AssignTotalRow(sheet, TotalRows + RowContentTemplate);
            var hiddenColumns = Enumerable.Range(_resultModel.NumberOfWeeks + NumberOfStaticColumns, MaxWeekColumns - _resultModel.NumberOfWeeks);
            sheet.HideColumn(hiddenColumns.ToArray());
        }

        protected override void AssignTitle(ISheet sheet)
        {
            var fromdate = _metaModel.FromDate.ToString(FormatDate);
            var todate = _metaModel.ToDate.ToString(FormatDate);

            var reportTitle = string.Format(ViewResources.WeeklyLiveMatches + ", {0} --> {1}", fromdate, todate);
            sheet.SetTextRowValue(TemplateSheet, RowTitleTemplate, ColumnTemplateCount, reportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, RowContentTemplate);

            // Remove content below header before insert new data
            ClearTemplate(sheet, RowContentTemplate);
        }

        protected override void AssignHeader(ISheet sheet)
        {
            var row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate);
            row.Cells[ColumnSport].SetCellValue(ViewResources.Sport);

            var index = ColumnSport + 1;
            foreach (var header in _weekHeaders)
            {
                row.Cells[index++].SetCellValue(header);
            }
        }

        protected override void AssignNoInfoRow(ISheet sheet)
        {
            sheet.SetTextRowValue(TemplateSheet, RowNoInfoTemplate, RowContentTemplate, ColumnTemplateCount, ViewResources.NoInfo);
        }

        protected override void AssignSheetData(ISheet sheet)
        {
            int index;
            var rowCount = _resultModel.DataList.Count();

            for (index = 0; index < rowCount; index++)
            {
                var dataItem = _resultModel.DataList.ElementAt(index);

                var row = sheet.CopyRow(TemplateSheet, RowContentTemplate, index + RowContentTemplate);

                row.Cells[ColumnIndex].SetCellValue(dataItem.Index);
                row.Cells[ColumnSport].SetCellValue(dataItem.SportName);

                var weekColumnIndex = ColumnSport + 1;
                for (int i = 0; i < _resultModel.NumberOfWeeks; i++)
                {
                    row.Cells[weekColumnIndex++].SetCellValue(dataItem.NumbersOfMatches.ElementAt(i));
                }
            }
        }

        protected internal virtual void AssignTotalRow(ISheet sheet, int rowIndex)
        {
            var cellIndex = 2;

            var row = sheet.CopyRow(TemplateSheet, RowTotalIndex, rowIndex);

            row.Cells[ColumnSport].SetCellValue(ViewResources.Total);
            sheet.MergeCols(ColumnIndex, ColumnSport, rowIndex);

            for (int i = 0; i < _resultModel.NumberOfWeeks; i++)
            {
                row.Cells[cellIndex++].SetCellValue(_resultModel.TotalData[i]);
            }
        }
    }
}