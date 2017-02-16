namespace Fanex.Athena.DataAccess.Dashboard
{
    public static class PlatformKey
    {
        public const int All = 0;

        public const int Asia = 1;

        public const int NewAsia = 2;

        public const int Euro = 3;

        public const int Wap = 4;

        public const int Mobile = 5;

        public static string GetPlatformName(int type)
        {
            switch (type)
            {
                case Asia: return "Asia";
                case NewAsia: return "NewAsia";
                case Euro: return "Euro";
                case Wap: return "Wap";
                case Mobile: return "Mobile";
                default: return "All";
            }
        }
    }
}
