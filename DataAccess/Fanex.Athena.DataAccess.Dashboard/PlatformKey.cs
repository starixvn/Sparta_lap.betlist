using System.Collections.Generic;

namespace Fanex.Athena.DataAccess.Dashboard
{
    public static class PlatformKey
    {
        private static readonly IDictionary<int, string> MapPlatformNames = new Dictionary<int, string>
        {
            { All, nameof(All) },
            { Asia, nameof(Asia) },
            { China, nameof(China) },
            { Euro, nameof(Euro) },
            { Wap, nameof(Wap) },
            { Mobile, nameof(Mobile) },
            { Tablet, nameof(Tablet) }
        };

        public const int All = 0;

        public const int Asia = 1;

        public const int China = 2;

        public const int Euro = 3;

        public const int Wap = 4;

        public const int Mobile = 5;

        public const int Tablet = 6;

        public static string GetPlatformName(int type)
        {
            return MapPlatformNames.ContainsKey(type) ? MapPlatformNames[type] : "All";
        }
    }
}