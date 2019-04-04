// --------------------------------------------------------------------------
// <copyright file="ReportBase.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Base
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Excel.Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public abstract class ReportBase
    {
        public const int RowHeaderTemplate = 1;
        public const int RowTitleTemplate = 0;
        public const int MaxRowCount = 30000;

        public const string TemplateSheetName = "Template";

        public const string FormatDate = "MM/dd/yyyy";

        protected const int ColumnIndex = 0;

        private readonly string _templatePath;

        private readonly int _totalRows;

        private int _pageSize;

        protected ReportBase(string templatePath, int totalRows, int pageSize = 0)
            : this(templatePath, totalRows, null, pageSize)
        {
        }

        protected ReportBase(
            string templatePath,
            int totalRows,
            ShowingOption showingOption,
            int pageSize = 0)
        {
            _templatePath = templatePath;
            _totalRows = totalRows;
            ShowingOption = showingOption;
            PageSize = pageSize;
        }

        protected bool IsMultiSheet { get; set; }

        protected ShowingOption ShowingOption { get; set; }

        protected int TotalRows
        {
            get
            {
                return _totalRows;
            }
        }

        protected internal int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = (value == 0 || value > MaxRowCount) ? MaxRowCount : value;
            }
        }

        protected internal virtual ISheet TemplateSheet { get; private set; }

        public virtual IWorkbook Workbook { get; private set; }

        protected virtual int NumberOfSheets
        {
            get
            {
                return GetNumberOfSheets(_totalRows, PageSize);
            }
        }

        public virtual void Export()
        {
            LoadTemplate();

            if (IsMultiSheet)
            {
                AssignDataMultiSheet();
            }
            else
            {
                AssignData();
            }

            RemoveTemplate();
        }

        protected internal static void ClearTemplate(ISheet sheet, int startRowIndex)
        {
            int lastRowNum = sheet.LastRowNum;
            for (int i = startRowIndex; i <= lastRowNum; i++)
            {
                sheet.DeleteRow(i);
            }
        }

        protected static int GetNumberOfSheets(int numberOfItems, int pageSize)
        {
            return numberOfItems % pageSize == 0 ? numberOfItems / pageSize : (numberOfItems / pageSize) + 1;
        }

        protected static void SetTextRowHeader(IRow row, IEnumerable<string> headerTextList)
        {
            for (int i = 0; i < headerTextList.Count(); i++)
            {
                row.Cells[i].SetCellValue(headerTextList.ElementAt(i));
            }
        }

        protected internal virtual void AssignData()
        {
            var sheet = Workbook.CloneSheet(TemplateSheetName, "Data");

            AssignHeader(sheet);
            AssignTitle(sheet);

            if (_totalRows == 0)
            {
                AssignNoInfoRow(sheet);
            }
            else
            {
                AssignSheetData(sheet);
            }

            ProcessUI(sheet);
        }

        protected internal virtual void AssignDataMultiSheet()
        {
            ISheet sheet;

            if (_totalRows == 0)
            {
                sheet = Workbook.CloneSheet(TemplateSheetName, "Data");
                AssignTitle(sheet);
                AssignHeader(sheet);
                AssignNoInfoRow(sheet);
                ProcessUI(sheet);
            }
            else
            {
                for (int i = 0; i < NumberOfSheets; i++)
                {
                    sheet = Workbook.CloneSheet(TemplateSheetName, "Data" + (i + 1).ToString());
                    AssignTitle(sheet);
                    AssignHeader(sheet);
                    AssignSheetData(sheet, i);
                    ProcessUI(sheet);
                }
            }
        }

        protected internal abstract void AssignTitle(ISheet sheet);

        protected internal abstract void AssignHeader(ISheet sheet);

        protected internal virtual void ProcessUI(ISheet sheet)
        {
        }

        protected internal virtual void AssignNoInfoRow(ISheet sheet)
        {
        }

        protected internal virtual void AssignSheetData(ISheet sheet)
        {
        }

        protected internal virtual void AssignSheetData(ISheet sheet, int pageIndex)
        {
        }

        protected internal virtual void LoadTemplate()
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(_templatePath, FileMode.Open, FileAccess.Read);
                Workbook = new HSSFWorkbook(stream);
                TemplateSheet = Workbook.GetSheet(TemplateSheetName);
                var defaultStyle = Workbook.CreateCellStyle();
                defaultStyle.ShrinkToFit = true;
                defaultStyle.WrapText = true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        protected internal virtual void RemoveTemplate()
        {
            Workbook.RemoveSheet(TemplateSheetName);
        }
    }
}