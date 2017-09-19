// --------------------------------------------------------------------------
// <copyright file="ReportBase.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Base
{
    using System.Collections.Generic;
    using System.IO;
    using Excel.Base.Entities;
    using Fanex.Athena.Excel.Base.Extension;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public abstract class ReportBase : ExcelBase
    {
        public const int RowHeaderTemplate = 1;
        public const int RowTitleTemplate = 0;
        public const int MaxRowCount = 30000;

        private readonly string _templatePath;

        private int _pageSize;

        protected ReportBase(
            string templatePath,
            ShowingOption showingOption,
            int pageSize = 0)
        {
            _templatePath = templatePath;
            ShowingOption = showingOption;
            PageSize = pageSize;
        }

        protected int PageSize
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

        protected IList<string> HeaderTextList
        {
            get;
            set;
        }

        public string ReportTitle
        {
            get;
            set;
        }

        protected internal ISheet TemplateSheet
        {
            get;
            private set;
        }

        public virtual IWorkbook Workbook
        {
            get;
            private set;
        }

        public virtual void Export()
        {
            LoadTemplate();

            AssignData();

            RemoveTemplate();
        }

        protected static void ClearTemplate(ISheet sheet, int startRowIndex)
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

        protected internal virtual void AssignData()
        {
        }

        protected virtual void AssignHeader(ISheet sheet)
        {
        }

        protected internal virtual void AssignNoInfoRow(ISheet sheet)
        {
        }

        protected internal virtual void AssignSheetData(ISheet sheet)
        {
        }

        protected virtual int AssignSheetData(ISheet sheet, int pageIndex)
        {
            return 0;
        }

        protected virtual void AssignTitle(ISheet sheet)
        {
        }

        protected virtual void ProcessUI(ISheet sheet)
        {
        }

        protected internal virtual void LoadTemplate()
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(_templatePath, FileMode.Open, FileAccess.Read);
                Workbook = new HSSFWorkbook(stream);
                TemplateSheet = Workbook.GetSheet("Template");
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
            Workbook.RemoveSheet("Template");
        }
    }
}