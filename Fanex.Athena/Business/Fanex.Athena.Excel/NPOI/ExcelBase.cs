namespace Fanex.NPOI.Extension
{
    using System;
    using System.Data;
    using System.IO;
    using System.Web;

    using DNAide.Core;

    using Fanex.Athena.Exce.NPOI;
    using Fanex.Athena.Excel.Entities;

    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public abstract class ExcelBase : IExcel
    {
        public const int NumOfRowInSheet = 60000;

        public ExcelBase(string templatePath, string exportingFileName, ShowingOption showingOption, ConvertingOption convertingOption)
        {
            TemplatePath = templatePath;
            ExportingFileName = exportingFileName;
            ShowingOption = showingOption;
            ConvertingOption = convertingOption;
        }

        public int ColumnCount
        {
            get;
            set;
        }

        public ConvertingOption ConvertingOption
        {
            get;
            set;
        }

        public string ExportingFileName
        {
            get;
            set;
        }

        public ShowingOption ShowingOption
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

        public abstract void AssignData();

        public abstract void AssignEmptyRow(ISheet sheet);

        public abstract void AssignHeader(ISheet sheet);

        public abstract void AssignSheetData(ISheet sheet, int rowFrom, int rowTo);

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

        public virtual void LoadTemplate()
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(TemplatePath, FileMode.Open, FileAccess.Read);
                Workbook = new HSSFWorkbook(stream);
                TemplateSheet = Workbook.GetSheet("Template");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        public virtual void RemoveTemplate()
        {
            Workbook.RemoveSheet("Template");
        }

        public virtual void Write(HttpResponse response)
        {
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", ExportingFileName));
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
        /// Get Currency Column Name
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="currencyConvert"></param>
        /// <returns></returns>
        protected virtual string GetCurrencyColumnName(string columnName, ConvertingOption currencyConvert)
        {
            var columnNameSuffix = currencyConvert.IsConvert
                                   ? currencyConvert.BaseCurrency
                                   : string.Empty;

            columnName = columnName + columnNameSuffix;
            return columnName;
        }

        protected virtual decimal GetCurrencyValue(DataRow row, string columnName, ConvertingOption currencyConvert)
        {
            var currencyColumnName = GetCurrencyColumnName(columnName, currencyConvert);
            return CastAide.AsDecimal(row[currencyColumnName]);
        }
    }
}