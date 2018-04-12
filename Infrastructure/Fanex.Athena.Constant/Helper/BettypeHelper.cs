namespace Fanex.Athena.Constant.Helper
{
    public static class BetTypeHelper
    {
        private const int NGMinBetType = 81;
        private const int NGMaxBetType = 119;
        private const int RBMinBetType = 31;
        private const int RBMaxBetType = 33;
        private const int VSMinBetType = 1201;
        private const int VSMaxBetType = 1299;
        private const int VS2MinBetType = 2700;
        private const int VS2MaxBetType = 2899;
        private const int FastMarketMinBetType = 221;
        private const int FastMarketMaxBetType = 227;

        public static bool IsNumberGame(int bettype)
        {
            return IsInRange(bettype, NGMinBetType, NGMaxBetType);
        }

        public static bool IsRacing(int bettype)
        {
            return IsInRange(bettype, RBMinBetType, RBMaxBetType);
        }

        public static bool IsVirtualSports(int bettype)
        {
            return IsInRange(bettype, VSMinBetType, VSMaxBetType);
        }

        public static bool IsVirtualSports2(int bettype)
        {
            return IsInRange(bettype, VS2MinBetType, VS2MaxBetType);
        }

        public static bool IsFastMarket(int bettype)
        {
            return IsInRange(bettype, FastMarketMinBetType, FastMarketMaxBetType);
        }

        private static bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}