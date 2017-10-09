namespace Fanex.Athena.Model.Core.Helper
{
    public static class BetTypeHelper
    {
        private const int NGMinBetType = 81;
        private const int NGMaxBetType = 119;
        private const int RBMinBetType = 31;
        private const int RBMaxBetType = 33;
        private const int VSMinBetType = 1201;
        private const int VSMaxBetType = 1299;
        private const int LCMinBetType = 1101;
        private const int LCMaxBetType = 1199;
        private const int VS2MinBetType = 2700;
        private const int VS2MaxBetType = 2899;

        public static bool IsNumberGame(int bettype)
        {
            return IsInRange(bettype, NGMinBetType, NGMaxBetType);
        }

        public static bool IsRacing(int bettype)
        {
            return IsInRange(bettype, RBMinBetType, RBMaxBetType);
        }

        public static bool IsLiveCasino(int bettype)
        {
            return IsInRange(bettype, LCMinBetType, LCMaxBetType);
        }

        public static bool IsVirtualSports(int bettype)
        {
            return IsInRange(bettype, VSMinBetType, VSMaxBetType);
        }

        public static bool IsVirtualSports2(int bettype)
        {
            return IsInRange(bettype, VS2MinBetType, VS2MaxBetType);
        }

        private static bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}