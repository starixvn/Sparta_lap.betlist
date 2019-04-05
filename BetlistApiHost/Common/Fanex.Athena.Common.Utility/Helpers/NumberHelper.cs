namespace Fanex.Athena.Common.Utility.Helpers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    [SuppressMessage("Sonar Code Smell", "S109:MagicNumbersShouldNotBeUsed",
            Justification = "Reviewed. Suppression is OK here.")]
    public static class NumberHelper
    {
        private static readonly CultureInfo DefaultCulture = new CultureInfo("en-US");

        public static string DecFormat(decimal value, byte digit = 2)
        {
            var format = "N" + digit.ToString();
            return value.ToString(format, DefaultCulture.NumberFormat);
        }

        public static decimal ToPercentage(this decimal value)
        {
            return value * 100;
        }

        public static bool IsOddNumber(this decimal value)
        {
            return value % 2 == 0;
        }

        public static bool IsOddNumber(this int value)
        {
            return value % 2 == 0;
        }
    }
}