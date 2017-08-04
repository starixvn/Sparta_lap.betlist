namespace Fanex.Athena.Model.Core.DataDomain
{
    using Fanex.Athena.Resources;

    public static class PlatformType
    {
        public const int All = 0;

        public const int Asia = 1;
        
        public const int China = 2;
        
        public const int Euro = 3;
        
        public const int Wap = 4;
        
        public const int SmartSite = 5;

        public const int Tablet = 6;

        public static string GetPlatformName(int type)
        {
            switch (type)
            {
                case Asia: return ViewResources.Asia;
                case China: return ViewResources.China;
                case Euro: return ViewResources.Euro;
                case Wap: return ViewResources.Wap;
                case SmartSite: return ViewResources.SmartSite;
                case Tablet: return ViewResources.Tablet;
                default: return ViewResources.All;
            }
        }
    }
}