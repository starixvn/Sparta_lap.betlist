// -----------------------------------------------------------------------
// <copyright file="BetSettingProduct.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.BetSettings
{
    using Resources;

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
                return ViewResources.ResourceManager.GetString("Product" + ProductCode);
            }
        }

        public string ProductCode { get; set; }

        public bool IsEnable { get; set; }
    }
}