using System.Collections.Generic;
using Fanex.Athena.Resources;

namespace Fanex.Athena.Common.Constant
{
    public static class PlatformType
    {
        public const int All = 0;

        public const int Asia = 1;

        public const int China = 2;

        public const int Euro = 3;

        public const int Wap = 4;

        public const int Mobile = 5;

        public const int Tablet = 6;

        public const int NewAsia = 7;

        public const int MobileApp = 8;

        public const int MobileLiteVersion = 9;

        private static readonly IDictionary<int, string> MapPlatformKeys = new Dictionary<int, string>
        {
            { All, nameof(All) },
            { Asia, nameof(Asia) },
            { China, nameof(China) },
            { Euro, nameof(Euro) },
            { Wap, nameof(Wap) },
            { Mobile, nameof(Mobile) },
            { Tablet, nameof(Tablet) },
            { NewAsia, nameof(NewAsia) },
            { MobileApp, nameof(MobileApp) },
            { MobileLiteVersion, nameof(MobileLiteVersion) }
        };

        public static string GetPlatformKey(int type)
        {
            return MapPlatformKeys.ContainsKey(type) ? MapPlatformKeys[type] : "All";
        }

        public static string GetPlatformName(int type)
        {
            switch (type)
            {
                case Asia: return ViewResources.Asia;
                case China: return ViewResources.China;
                case Euro: return ViewResources.Euro;
                case Wap: return ViewResources.Wap;
                case Mobile: return ViewResources.Mobile;
                case Tablet: return ViewResources.Tablet;
                case NewAsia: return ViewResources.NewAsia;
                case MobileApp: return ViewResources.MobileApp;
                case MobileLiteVersion: return ViewResources.MobileLiteVersion;
                default: return ViewResources.All;
            }
        }

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

        public static bool IsValidPlatform(int type)
        {
            return type >= All && type <= MobileLiteVersion;
        }
    }
}