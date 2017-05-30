// <copyright file="GroupCommSetting.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System.Collections.Generic;

    /// <summary>
    /// Group commission setting class.
    /// </summary>
    public static class GroupCommSetting
    {
        public static readonly decimal MinKey = 0;
        public static readonly decimal MaxKey = 15;

        public static readonly double MinValue = 0.0;
        public static readonly double MaxValue = 0.15;
        private static readonly decimal RangValueKey = 0.0005m;
        private static readonly decimal RangeKey = 0.05m;

        private static Dictionary<decimal, decimal> commissions;
        private static Dictionary<decimal, decimal> validCommissions;

        public static Dictionary<decimal, decimal> Commissions
        {
            get
            {
                if (commissions == null)
                {
                    commissions = new Dictionary<decimal, decimal>();
                    for (decimal i = MinKey, j = MaxKey + (decimal)RangeKey, value = (decimal)MinValue; i < j; i += (decimal)RangeKey, value += (decimal)RangValueKey)
                    {
                        commissions.Add(i, value);
                    }
                }

                return commissions;
            }
        }

        /// <summary>
        ///  Gets valid commission values.
        /// </summary>
        /// <value>The commission list.</value>
        public static Dictionary<decimal, decimal> ValidCommissions
        {
            get
            {
                if (validCommissions == null)
                {
                    validCommissions = new Dictionary<decimal, decimal>();
                    for (decimal i = MinKey, j = MaxKey + (decimal)RangeKey, value = (decimal)MinValue; i < j; i += (decimal)RangeKey, value += (decimal)RangValueKey)
                    {
                        validCommissions.Add(i, value);
                    }
                }

                return validCommissions;
            }
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