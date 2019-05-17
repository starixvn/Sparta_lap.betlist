namespace Fanex.Athena.Excel.Report.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Base.Entities;
    using Model.Core.DataDomain;
    using Model.Core.MetaModel.Product;
    using Model.Report.MetaModel;
    using Resources;

    public static class ReportExcelHelper
    {
        private const string FormatDate = "MM/dd/yyyy";

        public static string GenerateReportTitle(string reportName, ConvertingOption convertingOption, ThirdPartyWinLossMetaModel metaModel)
        {
            var title = GetTitleCurrencyName(convertingOption, metaModel);
            return GenerateReportTitle(reportName, metaModel.FromDate, metaModel.ToDate, title);
        }

        public static string GenerateReportTitle(string reportName, ConvertingOption convertingOption, DateTime fromDate, DateTime toDate, int currencyId)
        {
            var title = GetExactTitleCurrencyName(convertingOption, currencyId);
            return GenerateReportTitle(reportName, fromDate, toDate, title);
        }

        public static string GetLicenseeWLProduct(IEnumerable<ProductOption> productOptions, string caption = null)
        {
            var productCodeList = productOptions.Where(p => p.Status)
                            .Aggregate(string.Empty, (current, p) => current + (string.IsNullOrEmpty(current) ? p.Code : ", " + p.Code));

            if (string.IsNullOrEmpty(productCodeList))
            {
                return string.IsNullOrWhiteSpace(caption) ? string.Empty : ", " + caption;
            }

            return string.Format(", {0} ({1})", string.IsNullOrWhiteSpace(caption) ? ViewResources.LicenseeW_L : caption, productCodeList);
        }

        public static string GetExactTitleCurrencyName(ConvertingOption convertingOption, int currencyId)
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
                    title = GetTitleCurrencyName(convertingOption);
                    break;
            }

            return title;
        }

        public static string GetTitleBaseCurrency(string baseCurrency, string currencyName)
        {
            var currency = string.IsNullOrEmpty(baseCurrency) ?
                currencyName :
                baseCurrency;
            return ViewResources.Currency + ": " + currencyName + ", " + 
                ViewResources.BaseCurrency + ": " + currency.ToUpperInvariant();
        }

        public static string GetTitleCurrencyName(ConvertingOption convertingOption)
        {
            return convertingOption.IsConvert
                   ? string.Format(ViewResources.Viewin, convertingOption.BaseCurrency.ToUpperInvariant())
                   : string.Format(ViewResources.Viewin, convertingOption.CurrencyName);
        }

        public static string GetTitleCurrencyName(ConvertingOption convertingOption, BaseDrilldownReportMetaModel metaModel)
        {
            return metaModel.DrillDownLevel > 0 && metaModel.CurrencyId == 0
                ? GetExactTitleCurrencyName(convertingOption, metaModel.DrillDownCurrencyId)
                : GetExactTitleCurrencyName(convertingOption, metaModel.CurrencyId);
        }

        public static string GenerateReportTitle(string reportName, DateTime fromDate, DateTime toDate, string currencyTitle)
        {
            var reportTitle = new StringBuilder(reportName);
            reportTitle.Append($", {fromDate.ToString(FormatDate)} --> {toDate.ToString(FormatDate)}, {currencyTitle}");
            return reportTitle.ToString();
        }
    }
}