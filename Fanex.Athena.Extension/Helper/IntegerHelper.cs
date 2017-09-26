namespace Fanex.Athena.Extension.Helper
{
    public static class IntegerHelper
    {
        public static bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}