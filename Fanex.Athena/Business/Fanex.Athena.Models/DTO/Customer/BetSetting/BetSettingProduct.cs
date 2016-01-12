// -----------------------------------------------------------------------
// <copyright file="BetSettingProduct.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Customer.BetSetting
{
    using Fanex.Athena.Models.ViewModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetSettingProduct
    {
        public int ProductId { get; set; }

        public string ProductName 
        {
            get
            {
                var viewResource = ViewResources.ResourceManager;
                viewResource.IgnoreCase = true;
                var resourceKey = "Product" + ProductCode;
                var productName = viewResource.GetString(resourceKey);
                if (string.IsNullOrWhiteSpace(productName))
                {
                    productName = ProductCode;
                }

                return productName;
            }
        }

        public string ProductCode { get; set; }

        public bool IsEnable { get; set; }
    }
}
