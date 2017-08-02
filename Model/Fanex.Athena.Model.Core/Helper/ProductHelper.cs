namespace Fanex.Athena.Model.Core.Helper
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DataDomain;
    using DTO.Common;
    using Resources;

    public static class ProductHelper
    {
        private static readonly IDictionary<int, string> ProductCodeMap = new Dictionary<int, string>
        {
            { ProductTypes.Sportsbook, Product.SB },
            { ProductTypes.VirtualSports, Product.VS },
            { ProductTypes.Lottery, Product.LT },
        };

        public static string GetProductCode(int productId)
        {
            if (ProductCodeMap.ContainsKey(productId))
            {
                return ProductCodeMap[productId];
            }

            return string.Empty;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        public static string GetProductName(string productCode, string defaultValue = null)
        {
            switch (productCode.ToUpper())
            {
                case Product.BA:
                    return ViewResources.ProductBA;

                case Product.LC:
                    return ViewResources.ProductLC;

                case Product.NG:
                    return ViewResources.ProductNG;

                case Product.RB:
                    return ViewResources.ProductRB;

                case Product.SB:
                    return ViewResources.Sportsbook;

                case Product.MGCLC:
                    return ViewResources.ProductMGCLC;

                case Product.MGCRNG:
                    return ViewResources.ProductMGCRNG;

                case Product.VS:
                    return ViewResources.ProductVS;

                case "VS180":
                    return ViewResources.VirtualSoccer;

                case "VS181":
                    return ViewResources.VirtualHorseRacing;

                case "VS182":
                    return ViewResources.VirtualGreyhounds;

                case "VS184":
                    return ViewResources.VirtualMotoRacing;

                case "VS186":
                    return ViewResources.VirtualTennis;

                case Product.CS:
                    return ViewResources.ProductCS;

                case Product.BG:
                    return ViewResources.ProductBG;

                case Product.KN:
                    return ViewResources.ProductKN;

                case "VS183":
                    return ViewResources.VirtualSpeedway;

                case "VS185":
                    return ViewResources.VirtualCycling;

                case Product.VB:
                    return ViewResources.ProductVB;

                case Product.VS2:
                    return ViewResources.ProductVS2;

                case "VS2190":
                    return ViewResources.VirtualSoccerLeague;

                case "VS2191":
                    return ViewResources.VirtualSoccerNation;

                case Product.LT:
                    return ViewResources.ProductLT;

                default:
                    return defaultValue ?? productCode;
            }
        }
    }
}