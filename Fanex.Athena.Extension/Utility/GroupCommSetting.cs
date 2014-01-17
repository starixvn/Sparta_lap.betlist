// <copyright file="GroupCommSetting.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System.Collections.Generic;

    /// <summary>
    /// Group commission setting class.
    /// </summary>
    public class GroupCommSetting
    {
        public static readonly decimal MinKey = 0;
        public static readonly decimal MaxKey = 15;

        public static readonly double MinValue = 0.0;
        public static readonly double MaxValue = 0.15;
        private static double _rangValueKey = 0.0005;
        private static double _rangeKey = 0.05;

        public static Dictionary<decimal, decimal> GetValues()
        {
            var items = new Dictionary<decimal, decimal>();
            for (decimal i = MinKey, j = MaxKey + (decimal)_rangeKey, value = (decimal)MinValue; i < j; i += (decimal)_rangeKey, value += (decimal)_rangValueKey)
            {
                items.Add(i, value);
            }

            return items;
        }

        /// <summary>
        ///  Get valid commission values.
        /// </summary>
        /// <returns>The commission list.</returns>
        public static Dictionary<decimal, decimal> GetValidComm()
        {
            var validComms = new Dictionary<decimal, decimal>();
            for (decimal i = MinKey, j = MaxKey + (decimal)_rangeKey, value = (decimal)MinValue; i < j; i += (decimal)_rangeKey, value += (decimal)_rangValueKey)
            {
                validComms.Add(i, value);
            }

            return validComms;
        }
        
        /// <summary>
        /// Get commissions.
        /// </summary>
        /// <param name="max">The maximum value.</param>
        /// <param name="increase">The seeking value.</param>
        /// <returns>The commission value.</returns>
        public static Dictionary<int, int> GetCommValue(int max, int increase)
        {
            var items = new Dictionary<int, int>();
            for (int i = 0; i <= max; i++)
            {
                items.Add(i * increase, i * increase);
            }

            return items;
        }
    }
}