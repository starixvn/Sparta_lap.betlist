// --------------------------------------------------------------------------
// <copyright file="ReportBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Base
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Entities;
    using Fanex.Athena.Excel.Extension;
    using Fanex.Athena.Models.DataDomain;
    using Models.DTO.Common;
    using Models.MetaModel.Product;
    using Models.ViewModel;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Utility;

    /// <summary>
    /// The report base.
    /// </summary>
    public abstract class ReportBase : ExcelBase
    {
        public const int RowHeaderTemplate = 1;
        public const int RowTitleTemplate = 0;
        public const int MaxRowCount = 30000;

        private int _pageSize;

        protected ReportBase(
            string templatePath,
            string exportingFileName,
            ShowingOption showingOption,
            ConvertingOption convertingOption,
            int pageSize = 0)
        {
            TemplatePath = templatePath;
            ExportingFileName = exportingFileName;
            ShowingOption = showingOption;
            ConvertingOption = convertingOption;
            PageSize = pageSize;
        }

        public int PageSize
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

        public ConvertingOption ConvertingOption
        {
            get;
            set;
        }

        public ICellStyle DefaultStyle
        {
            get;
            set;
        }

        public DataTable ExportData
        {
            get;
            set;
        }

        public string ExportingFileName
        {
            get;
            set;
        }

        public IList<string> HeaderTextList
        {
            get;
            set;
        }

        public IList<ProductOption> ProductOptions
        {
            get;
            set;
        }

        public DataTable RacingData
        {
            get;
            set;
        }

        public string ReportTitle
        {
            get;
            set;
        }

        public string TemplatePath
        {
            get;
            set;
        }

        public ISheet TemplateSheet
        {
            get;
            set;
        }

        public IWorkbook Workbook
        {
            get;
            set;
        }

        public int NumOfDataRow
        {
            get;
            set;
        }

        /// <summary>
        /// Assign data.
        /// </summary>
        public virtual void AssignData()
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

        public virtual void AssignHeader(ISheet sheet)
        {
        }

        public virtual void AssignNoInfoRow(ISheet sheet)
        {
        }

        public virtual void AssignSheetData(ISheet sheet, int rowFrom, int rowTo)
        {
        }

        public virtual void AssignSheetData(ISheet sheet)
        {
        }

        public virtual int AssignSheetData(ISheet sheet, int pageIndex)
        {
            return 0;
        }

        public virtual void AssignTitle(ISheet sheet)
        {
        }

        public virtual void Export(HttpResponse response)
        {
            Export();
            Write(response);
        }

        public virtual void Export(Stream stream)
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
        /// Load template.
        /// </summary>
        public virtual void LoadTemplate()
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

        public virtual void ProcessUI(ISheet sheet)
        {
        }

        /// <summary>
        /// Remove template.
        /// </summary>
        public virtual void RemoveTemplate()
        {
            Workbook.RemoveSheet("Template");
        }

        /// <summary>
        /// Write data stream to browser.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        public virtual void Write(HttpResponse response)
        {
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", "attachment;filename={0}" + ExportingFileName);
            response.Clear();

            var excelStream = new MemoryStream();
            Workbook.Write(excelStream);

            response.BinaryWrite(excelStream.GetBuffer());
            response.End();
        }

        public virtual void Write(Stream stream)
        {
            Workbook.Write(stream);
        }

        /// <summary>
        ///     Clear the template from header row.
        /// </summary>
        /// <param name="sheet">The excel sheet.</param>
        /// <param name="startRowIndex">The starting row index of sheet.</param>
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

        /// <summary>
        ///     Assign array decimal to row.
        /// </summary>
        /// <param name="row">Row in sheet export excel.</param>
        /// <param name="cellIndex">Position cell will insert.</param>
        /// <param name="decimalValues">Array decimal values.</param>
        /// <returns>Cell position after insert.</returns>
        protected virtual int AssignDecimalData(IRow row, int cellIndex, params decimal[] decimalValues)
        {
            for (int i = 0, j = decimalValues.Length; i < j && cellIndex < row.Cells.Count; i++)
            {
                row.Cells[cellIndex].SetCellValue(decimalValues[i]);
                cellIndex++;
            }

            return cellIndex;
        }

        /// <summary>
        ///     Assign a list of column in table to excel row.
        /// </summary>
        /// <param name="row">The excel row.</param>
        /// <param name="data">The data table row.</param>
        /// <param name="columnNameList">List of column name.</param>
        /// <param name="columnStart">The starting column index of row.</param>
        /// <returns>Cell position after insert.</returns>
        protected virtual int AssignRowContent(IRow row, DataRow data, IList<string> columnNameList, int columnStart = 0)
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

        /// <summary>
        /// Get exactly title base on currency .
        /// </summary>
        /// <param name="currencyId">The currency id.</param>
        /// <returns>The title.</returns>
        protected virtual string GetExactTitleCurrencyName(int currencyId)
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

        /// <summary>
        /// Get licensee win-loss or Win-loss by product.
        /// </summary>
        /// <param name="caption">Caption text.</param>
        /// <returns>A string value.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        protected virtual string GetLicenseeWLProduct(string caption = null)
        {
            var prdOptionTitleList =
                ProductOptions.Where(p => p.Code != Product.BA && p.Code != Product.RB);
            var prdText = prdOptionTitleList.Where(prd => prd.Status)
                            .Aggregate(string.Empty, (current, prd) => current + (string.IsNullOrEmpty(current) ? prd.Code : ", " + prd.Code));

            return string.IsNullOrEmpty(prdText)
                ? string.IsNullOrWhiteSpace(caption) ? string.Empty : ", " + caption
                      : string.Format(", {0} ({1})", string.IsNullOrWhiteSpace(caption) ? ViewResources.LicenseeW_L : caption, prdText);
        }

        /// <summary>
        /// Get title by currency.
        /// </summary>
        /// <returns>The title.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
           Justification = "Reviewed. Suppression is OK here.")]
        protected virtual string GetTitleCurrencyName()
        {
            return ConvertingOption.IsConvert
                   ? string.Format(ViewResources.Viewin, ConvertingOption.BaseCurrency.ToUpper())
                   : string.Format(ViewResources.Viewin, ConvertingOption.CurrencyName);
        }

        protected void SetTextRowValue(ISheet sheet, int sourceRowIndex, int destRowIndex, int columnCount, string value)
        {
            IRow row = sheet.CopyRow(TemplateSheet, sourceRowIndex, destRowIndex);
            sheet.MergeCells(destRowIndex, 0, destRowIndex, columnCount - 1);
            row.Cells[0].SetCellValue(value);
        }
    }
}