// --------------------------------------------------------------------------
// <copyright file="ReportBase.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Base
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using Excel.Base.Entities;
    using Excel.Base.Utility;
    using Fanex.Athena.Excel.Base.Extension;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.Core.MetaModel.Product;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Resources;

    public abstract class ReportBase : ExcelBase
    {
        public const int RowHeaderTemplate = 1;
        public const int RowTitleTemplate = 0;
        public const int MaxRowCount = 30000;

        private int _pageSize;

        protected ReportBase(
            string templatePath,
            ShowingOption showingOption,
            ConvertingOption convertingOption,
            int pageSize = 0)
        {
            TemplatePath = templatePath;
            ShowingOption = showingOption;
            ConvertingOption = convertingOption;
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

        protected ConvertingOption ConvertingOption
        {
            get;
            set;
        }

        protected ICellStyle DefaultStyle
        {
            get;
            set;
        }

        public DataTable ExportData
        {
            get;
            set;
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

        protected string TemplatePath
        {
            get;
            set;
        }

        protected internal ISheet TemplateSheet
        {
            get;
            set;
        }

        public virtual IWorkbook Workbook
        {
            get;
            set;
        }

        protected int NumOfDataRow
        {
            get;
            set;
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
            if (ExportData != null && ExportData.Rows.Count > 0)
            {
                NumOfDataRow = ExportData.Rows.Count;
            }

            ISheet sheet = Workbook.CloneSheet("Template", "Data");

            AssignHeader(sheet);

            AssignTitle(sheet);

            if (NumOfDataRow == 0)
            {
                AssignNoInfoRow(sheet);
            }
            else
            {
                AssignSheetData(sheet);
            }

            // Hide unnecessary column
            ProcessUI(sheet);
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
                stream = new FileStream(TemplatePath, FileMode.Open, FileAccess.Read);
                Workbook = new HSSFWorkbook(stream);
                TemplateSheet = Workbook.GetSheet("Template");
                DefaultStyle = Workbook.CreateCellStyle();
                DefaultStyle.ShrinkToFit = true;
                DefaultStyle.WrapText = true;
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

        protected internal virtual int AssignRowContent(IRow row, DataRow data, IEnumerable<string> columnNameList, int columnStart = 0)
        {
            foreach (string columnName in columnNameList)
            {
                if (data.Table.Columns.Contains(columnName))
                {
                    row.Cells[columnStart].SetCellValue(data[columnName]);
                }

                columnStart++;
            }

            return columnStart;
        }

        protected string GetCurrencyColumn(string columnName)
        {
            return DataHelper.GetCurrencyColumnName(columnName, this.ConvertingOption);
        }

        protected internal virtual string GetExactTitleCurrencyName(int currencyId)
        {
            string title;

            switch (currencyId)
            {
                case BaseCurrencyType.All:
                    title = ViewResources.AllCurrencies;
                    break;

                case BaseCurrencyType.Eur:
                    title = string.Format(ViewResources.Viewin, FormatKey.Eur);
                    break;

                case BaseCurrencyType.Usd:
                    title = string.Format(ViewResources.Viewin, FormatKey.Usd);
                    break;

                case BaseCurrencyType.Gbp:
                    title = string.Format(ViewResources.Viewin, FormatKey.Gbp);
                    break;

                default:
                    title = GetTitleCurrencyName();
                    break;
            }

            return title;
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        protected internal virtual string GetLicenseeWLProduct(IEnumerable<ProductOption> productOptions, string caption = null)
        {
            var productCodeList = productOptions.Where(p => p.Status)
                            .Aggregate(string.Empty, (current, p) => current + (string.IsNullOrEmpty(current) ? p.Code : ", " + p.Code));

            return string.IsNullOrEmpty(productCodeList)
                ? string.IsNullOrWhiteSpace(caption) ? string.Empty : ", " + caption
                      : string.Format(", {0} ({1})", string.IsNullOrWhiteSpace(caption) ? ViewResources.LicenseeW_L : caption, productCodeList);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
           Justification = "Reviewed. Suppression is OK here.")]
        protected virtual string GetTitleCurrencyName()
        {
            return ConvertingOption.IsConvert
                   ? string.Format(ViewResources.Viewin, ConvertingOption.BaseCurrency.ToUpper())
                   : string.Format(ViewResources.Viewin, ConvertingOption.CurrencyName);
        }

        protected internal void SetTextRowValue(ISheet sheet, int sourceRowIndex, int destRowIndex, int columnCount, string value)
        {
            IRow row = sheet.CopyRow(TemplateSheet, sourceRowIndex, destRowIndex);
            sheet.MergeCells(destRowIndex, 0, destRowIndex, columnCount - 1);
            row.Cells[0].SetCellValue(value);
        }
    }
}