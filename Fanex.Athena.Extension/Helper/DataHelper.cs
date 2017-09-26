// -----------------------------------------------------------------------
// <copyright file="DataHelper.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Helper
{
    using Fanex.Athena.Resources;

    public static class DataHelper
    {
        public static string GetCurrencyColumnName(string columnName, bool isConvert, string baseCurrency)
        {
            var suffix = isConvert ? baseCurrency : string.Empty;
            return columnName + suffix;
        }

        public static string GetProductName(string productCode)
        {
            var resourceKey = "Product" + productCode;
            var resourceManager = ViewResources.ResourceManager;
            resourceManager.IgnoreCase = true;
            return resourceManager.GetString(resourceKey);
        }
    }
}