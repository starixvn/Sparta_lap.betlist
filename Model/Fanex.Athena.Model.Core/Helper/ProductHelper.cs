namespace Fanex.Athena.Model.Core.Helper
{
    using System;
    using System.Collections.Generic;
    using Constant.DataDomain;
    using Constant.Helper;
    using DTO.Common;
    using Resources;

    public static class ProductHelper
    {
        private static readonly IDictionary<int, string> ProductCodeMap = new Dictionary<int, string>
        {
            { ProductTypes.Sportsbook, Product.SB },
            { ProductTypes.VirtualSports, Product.VS },
            { ProductTypes.VirtualSports2, Product.VS2 },
            { ProductTypes.Lottery, Product.LT },
        };

        private static readonly IDictionary<string, string> ProductNameMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { Product.BA, nameof(ViewResources.ProductBA) },
                { Product.NG, nameof(ViewResources.ProductNG) },
                { Product.SB, nameof(ViewResources.Sportsbook) },
                { Product.MGCLC, nameof(ViewResources.ProductMGCLC) },
                { Product.MGCRNG, nameof(ViewResources.ProductMGCRNG) },
                { Product.VS, nameof(ViewResources.ProductVS) },
                { "VS180", nameof(ViewResources.VirtualSoccer) },
                { "VS181", nameof(ViewResources.VirtualHorseRacing) },
                { "VS182", nameof(ViewResources.VirtualGreyhounds) },
                { "VS183", nameof(ViewResources.VirtualSpeedway) },
                { "VS184", nameof(ViewResources.VirtualMotoRacing) },
                { "VS185", nameof(ViewResources.VirtualCycling) },
                { "VS186", nameof(ViewResources.VirtualTennis) },
                { Product.CS, nameof(ViewResources.ProductCS) },
                { Product.KN, nameof(ViewResources.ProductKN) },
                { Product.VB, nameof(ViewResources.ProductVB) },
                { Product.VS2, nameof(ViewResources.ProductVS2) },
                { "VS2190", nameof(ViewResources.VirtualSoccerLeague) },
                { "VS2191", nameof(ViewResources.VirtualSoccerNation) },
                { "VS2192", nameof(ViewResources.VirtualSoccerWorldCup) },
                { "VS2193", nameof(ViewResources.VirtualBasketball) },
                { "VS2194", nameof(ViewResources.VirtualSoccerAsianCup) },
                { Product.LT, nameof(ViewResources.ProductLT) },
                { Product.AB, nameof(ViewResources.ProductAB) }
            };

        public static string GetProductCode(int productId)
        {
            if (ProductCodeMap.ContainsKey(productId))
            {
                return ProductCodeMap[productId];
            }

            return string.Empty;
        }

        public static string GetProductName(string productCode, string defaultValue = null)
        {
            if (!ProductNameMap.ContainsKey(productCode))
            {
                return defaultValue ?? productCode;
            }

            var resourceKey = ProductNameMap[productCode];
            return ViewResources.ResourceManager.GetString(resourceKey);
        }

        public static int GetProductId(int bettype)
        {
            if (BetTypeHelper.IsNumberGame(bettype))
            {
                return ProductTypes.NumberGame;
            }

            if (BetTypeHelper.IsVirtualSports(bettype))
            {
                return ProductTypes.VirtualSports;
            }

            if (BetTypeHelper.IsVirtualSports2(bettype))
            {
                return ProductTypes.VirtualSports2;
            }

            return ProductTypes.Sportsbook;
        }
    }
}