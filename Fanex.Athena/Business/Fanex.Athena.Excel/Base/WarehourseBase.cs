// --------------------------------------------------------------------------
// <copyright file="WarehourseBase.cs" company="Nexcel Solutions Vietnam">
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
    using System.IO;
    using System.Linq;
    using System.Web;
    using Common.Extensions;
    using Entities;
    using Models.DTO.Common;
    using Models.MetaModel.Product;
    using Models.ViewModel;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Utility;

    /// <summary>
    /// The report base.
    /// </summary>
    public abstract class WarehourseBase : ExcelBase
    {
        public const int RowHeaderTemplate = 1;
        public const int RowTitleTemplate = 0;
        public const int MaxRowCount = 30000;

        private int _pageSize;

        protected WarehourseBase(
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

        public IList<int> ColumnTemplateIndexList
        {
            get;
            set;
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
            ISheet sheet = Workbook.CloneSheet("Template", "Data");

            ColumnTemplateIndexList = GetColumnTemplateIndex();

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
            ProcessUi(sheet);
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

        public virtual int AssignSheetData(ISheet sheet, int startIndex)
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

        public virtual void ProcessUi(ISheet sheet)
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

        /// <summary>
        ///     Clear the template from header row.
        /// </summary>
        /// <param name="sheet">The excel sheet.</param>
        /// <param name="startRowIndex">The starting row index of sheet.</param>
        protected void ClearTemplate(ISheet sheet, int startRowIndex)
        {
            int lastRowNum = sheet.LastRowNum;
            for (int i = startRowIndex; i <= lastRowNum; i++)
            {
                sheet.DeleteRow(i);
            }
        }

        /// <summary>
        /// Get column index.
        /// </summary>
        /// <returns>The integer array.</returns>
        protected virtual IList<int> GetColumnTemplateIndex()
        {
            return new List<int>();
        }

        protected string GetCurrencyColumn(string columnName)
        {
            return DataHelper.GetCurrencyColumnName(columnName, ConvertingOption);
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
                case 0:
                    title = ViewResources.AllCurrencies;
                    break;

                case -1:
                    title = ViewResources.Viewin + " " + ViewResources.EUR;
                    break;

                case -2:
                    title = ViewResources.Viewin + " " + ViewResources.USD;
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
        /// <returns>A string value.</returns>
        protected virtual string GetLicenseeWlProduct()
        {
            var prdOptionTitleList =
                ProductOptions.Where(p => p.Code != Product.Ba && p.Code != Product.Rb);
            var prdText = prdOptionTitleList.Where(prd => prd.Status)
                            .Aggregate(string.Empty, (current, prd) => current + (string.IsNullOrEmpty(current) ? prd.Code : ", " + prd.Code));

            return string.IsNullOrEmpty(prdText)
                      ? string.Empty
                      : string.Format(", {0} ({1})", ViewResources.LicenseeW_L, prdText);
        }

        /// <summary>
        /// Get title by currency.
        /// </summary>
        /// <returns>The title.</returns>
        protected virtual string GetTitleCurrencyName()
        {
            return ConvertingOption.IsConvert
                   ? ViewResources.Viewin + " " + ConvertingOption.BaseCurrency.ToUpper()
                   : ViewResources.Viewin + " " + ConvertingOption.CurrencyName;
        }

        protected void SetTextRowValue(ISheet sheet, int sourceRowIndex, int destRowIndex, int columnCount, string value)
        {
            IRow row = sheet.CopyRow(TemplateSheet, sourceRowIndex, destRowIndex);
            sheet.MergeCells(destRowIndex, 0, destRowIndex, columnCount - 1);
            row.Cells[0].SetCellValue(value);
        }

        protected int GetNumberOfSheets(int numberOfItems, int pageSize)
        {
            return numberOfItems % pageSize == 0 ? numberOfItems / pageSize : (numberOfItems / pageSize) + 1;
        }
    }
}