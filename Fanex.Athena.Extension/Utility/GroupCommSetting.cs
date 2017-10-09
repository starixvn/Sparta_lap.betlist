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
        private const decimal MinKey = 0;
        private const decimal MaxKey = 15;

        private const double MinValue = 0;
        private const decimal RangValueKey = 0.0005m;
        private const decimal RangeKey = 0.05m;

        private static IDictionary<decimal, decimal> commissions;
        private static IDictionary<decimal, decimal> validCommissions;

        public static IDictionary<decimal, decimal> Commissions
        {
            get
            {
                if (commissions == null)
                {
                    commissions = new Dictionary<decimal, decimal>();
                    for (decimal i = MinKey, j = MaxKey + RangeKey, value = (decimal)MinValue; i < j; i += RangeKey, value += RangValueKey)
                    {
                        commissions.Add(i, value);
                    }
                }

                return commissions;
            }
        }

        public static IDictionary<decimal, decimal> ValidCommissions
        {
            get
            {
                if (validCommissions == null)
                {
                    validCommissions = new Dictionary<decimal, decimal>();
                    for (decimal i = MinKey, j = MaxKey + RangeKey, value = (decimal)MinValue; i < j; i += RangeKey, value += RangValueKey)
                    {
                        validCommissions.Add(i, value);
                    }
                }

                return validCommissions;
            }
        }

        public static IDictionary<int, int> GetCommValue(int max, int increase)
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