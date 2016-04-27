namespace Fanex.Athena.Excel.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Base;
    using Entities;
    using Fanex.Athena.Excel.Extension;
    using Models.DataDomain;
    using Models.DTO.Report;
    using Models.MetaModel.Report;
    using Models.ViewModel;
    using NPOI.SS.UserModel;

    public class PlatformWinLossSummaryExcel : ReportBase
    {
        private const int ColumnPlatform = 0;
        private const int ColumnTurnover = 1;
        private const int ColumnTurnoverPercent = 2;
        private const int ColumnTotalTransaction = 3;
        private const int ColumnCustomerWinLoss = 4;
        private const int ColumnCustomerComm = 5;
        private const int ColumnCustomerTotal = 6;
        private const int ColumnLicenseeWinLoss = 7;

        private const int ColumnNoContent = 8;

        private const int RowTableHeaderTemplate = 1;
        private const int RowParentContentTemplate = 3;
        private const int RowChildContentTemplate = 4;
        private const int RowTotal = 5;

        private const int RowNoInfoTemplate = 6;
        
        private readonly string _templatePath;
        private readonly string _fileName;
        private readonly PlatformWinLossMetaModel _model;
        private readonly IList<PlatformWinLossSummary> _dataList;
        private readonly PlatformWinLossSummary _total;

        public PlatformWinLossSummaryExcel(
            string templatePath,
            string fileName,
            PlatformWinLossMetaModel metaModel,
            IList<PlatformWinLossSummary> dataList,
            PlatformWinLossSummary total,
            ConvertingOption convertingOption)
            : base(templatePath, fileName, new ShowingOption(), convertingOption)
        {
            _templatePath = templatePath;
            _fileName = fileName;
            _model = metaModel;
            _dataList = dataList;
            _total = total;

            ProductOptions = metaModel.ProductOptions;
            HeaderTextList = new List<string>
            {
                ViewResources.Platform,
                ViewResources.Turnover,
                ViewResources.TurnoverPercent,
                ViewResources.TotalTransaction,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.CustomerW_L,
                ViewResources.LicenseeW_L,
            };
        }

        public override void AssignData()
        {
            int numOfDataRow = _dataList.Count;
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
        }

        public override void AssignHeader(ISheet sheet)
        {
            IRow row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate, RowHeaderTemplate);

            for (int i = 0; i < HeaderTextList.Count; i++)
            {
                row.Cells[i].SetCellValue(HeaderTextList[i]);
            }

            row = sheet.CopyRow(TemplateSheet, RowHeaderTemplate + 1, RowHeaderTemplate + 1);

            row.Cells[ColumnCustomerWinLoss].SetCellValue(ViewResources.WinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(ViewResources.Comm);
            row.Cells[ColumnCustomerTotal].SetCellValue(ViewResources.Total);
        }

        public override void AssignNoInfoRow(ISheet sheet)
        {
            SetTextRowValue(sheet, RowNoInfoTemplate, RowParentContentTemplate, ColumnNoContent, ViewResources.NoInfo);
        }

        public override void AssignSheetData(ISheet sheet)
        {
            int rowIndex = RowParentContentTemplate;
            foreach (var item in _dataList)
            {
                IRow row = null;
                if (item.Level == 0)
                {
                    row = sheet.CopyRow(TemplateSheet, RowParentContentTemplate, rowIndex);
                }
                else
                {
                    row = sheet.CopyRow(TemplateSheet, RowChildContentTemplate, rowIndex);
                }

                AssignCellData(row, item);
                rowIndex++;
            }

            IRow totalRow = sheet.CopyRow(TemplateSheet, RowTotal, rowIndex);
            _total.Name = ViewResources.Total;
            AssignCellData(totalRow, _total);
        }

        public override void AssignTitle(ISheet sheet)
        {
            string platformText = ViewResources.Platform + ": " + PlatformType.GetPlatformName(_model.PlatformType);
            var currencyText = _model.CurrencyId == 0 ?
                ViewResources.AllCurrencies :
                string.Format("{0} {1}", ViewResources.Currency, _model.CurrencyName);

            ReportTitle = string.Format(
                                        ViewResources.PlatformWinLossTitle,
                                        _model.FromDate.ToString(FormatDate),
                                        _model.ToDate.ToString(FormatDate),
                                        currencyText,
                                        GetBaseCurrencyTitle(),
                                        platformText) + ", " + GetProductTitle();

            SetTextRowValue(sheet, RowTitleTemplate, RowTitleTemplate, ColumnNoContent, ReportTitle);

            // Freeze 2 rows title and header
            sheet.CreateFreezePane(0, 3);

            ClearTemplate(sheet, RowParentContentTemplate);
        }

        private static void AssignCellData(IRow row, PlatformWinLossSummary data)
        {
            row.Cells[ColumnPlatform].SetCellValue(data.Name);
            row.Cells[ColumnTurnover].SetCellValue(data.Turnover);
            row.Cells[ColumnTurnoverPercent].SetCellValue(string.Format("{0:0.00}%", data.TurnoverPercent));
            row.Cells[ColumnTotalTransaction].SetCellValue(data.TotalTransactions);
            row.Cells[ColumnCustomerWinLoss].SetCellValue(data.CustomerWinLoss);
            row.Cells[ColumnCustomerComm].SetCellValue(data.CustomerComm);
            row.Cells[ColumnCustomerTotal].SetCellValue(data.CustomerTotal);
            row.Cells[ColumnLicenseeWinLoss].SetCellValue(data.LicenseeWL);
        }

        private string GetProductTitle()
        {
            var selectedProducts = ProductOptions.Where(x => x.Status);
            if (selectedProducts.Count() == ProductOptions.Count)
            {
                return string.Format("{0}: {1}", ViewResources.Product, ViewResources.All);
            }

            var text = selectedProducts
                .Aggregate(string.Empty, (current, x) => current + (string.IsNullOrEmpty(current) ? x.Code : ", " + x.Code));

            return string.Format("{0}: {1}", ViewResources.Product, text);
        }

        private string GetBaseCurrencyTitle()
        {
            string title;
            var baseCurrencyId = _model.BaseCurrencyId;

            switch (baseCurrencyId)
            {
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
                    title = string.Format(ViewResources.Viewin, _model.CurrencyName);
                    break;
            }

            return title;
        }
    }
}
