namespace Fanex.Athena.Model.Core.Helper
{
    using System.Diagnostics.CodeAnalysis;
    using Resources;

    public static class ProductHelper
    {
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        public static string GetProductName(string productCode, string defaultValue = null)
        {
            switch (productCode.ToUpper())
            {
                case "BA":
                    return ViewResources.ProductBA;

                case "LC":
                    return ViewResources.ProductLC;

                case "NG":
                    return ViewResources.ProductNG;

                case "RB":
                    return ViewResources.ProductRB;

                case "SB":
                    return ViewResources.Sportsbook;

                case "MGCLC":
                    return ViewResources.ProductMGCLC;

                case "MGCRNG":
                    return ViewResources.ProductMGCRNG;

                case "VS":
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

                case "CS":
                    return ViewResources.ProductCS;

                case "BG":
                    return ViewResources.ProductBG;

                case "KN":
                    return ViewResources.ProductKN;

                case "VS183":
                    return ViewResources.VirtualSpeedway;

                case "VS185":
                    return ViewResources.VirtualCycling;

                case "VB":
                    return ViewResources.ProductVB;

                case "BRVS":
                    return ViewResources.ProductBRVS;

                case "BRVS190":
                    return ViewResources.VirtualSoccerLeague;

                case "BRVS191":
                    return ViewResources.VirtualSoccerNation;

                default:
                    return defaultValue ?? productCode;
            }
        }
    }
}