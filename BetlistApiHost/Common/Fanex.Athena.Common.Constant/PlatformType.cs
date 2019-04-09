namespace Fanex.Athena.Common.Constant
{
    using Resources;

    public static class PlatformType
    {
#pragma warning disable S1541 // Methods and properties should not be too complex

        public static string GetPlatformName(string code)
#pragma warning restore S1541 // Methods and properties should not be too complex
        {
            switch (code)
            {
                case "a":
                case "z": return ViewResources.China;
                case "d":
                case "g": return ViewResources.Asia;
                case "e":
                case "u":
                case "y": return ViewResources.Euro;
                case "m": return ViewResources.Wap;
                case "s":
                case "l": return ViewResources.Mobile;
                case "r": return ViewResources.Tablet;
                case "x": return ViewResources.NewAsia;
                case "o": return ViewResources.MobileApp;
                case "c": return ViewResources.MobileLiteVersion;
                default: return ViewResources.Others;
            }
        }
    }
}