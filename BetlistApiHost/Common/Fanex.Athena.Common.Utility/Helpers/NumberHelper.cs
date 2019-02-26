namespace Fanex.Athena.Common.Utility.Helpers
{
    using System.Globalization;

    /// <summary>
    /// Number helper class.
    /// </summary>
    public static class NumberHelper
    {
        private static readonly CultureInfo DefaultCulture = new CultureInfo("en-US");

        public static string DecFormat(decimal value, byte digit = 2)
        {
            var format = "N" + digit.ToString();
            return value.ToString(format, DefaultCulture.NumberFormat);
        }
    }
}