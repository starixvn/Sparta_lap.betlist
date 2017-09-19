namespace Fanex.Athena.Excel.Report.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Base.Entities;
    using Model.Core.DataDomain;
    using Model.Core.MetaModel.Product;
    using Resources;

    public static class ReportExcelHelper
    {
        public static string GetLicenseeWLProduct(IEnumerable<ProductOption> productOptions, string caption = null)
        {
            var productCodeList = productOptions.Where(p => p.Status)
                            .Aggregate(string.Empty, (current, p) => current + (string.IsNullOrEmpty(current) ? p.Code : ", " + p.Code));

            return string.IsNullOrEmpty(productCodeList)
                ? string.IsNullOrWhiteSpace(caption) ? string.Empty : ", " + caption
                      : string.Format(", {0} ({1})", string.IsNullOrWhiteSpace(caption) ? ViewResources.LicenseeW_L : caption, productCodeList);
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

        public static string GetTitleCurrencyName(ConvertingOption convertingOption)
        {
            return convertingOption.IsConvert
                   ? string.Format(ViewResources.Viewin, convertingOption.BaseCurrency.ToUpper())
                   : string.Format(ViewResources.Viewin, convertingOption.CurrencyName);
        }
    }
}