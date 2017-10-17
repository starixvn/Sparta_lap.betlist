// <copyright file="GroupCommSetting.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System.Collections.Generic;
    using System.Linq;

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

        private static IEnumerable<decimal> _commissions;

        public static bool IsAllValid(params decimal[] commissions)
        {
            if (_commissions == null)
            {
                BuildCommValues();
            }

            foreach (decimal value in commissions)
            {
                if (!_commissions.Any(x => x == value))
                {
                    return false;
                }
            }

            return true;
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

        private static void BuildCommValues()
        {
            var commissions = new List<decimal>();
            for (decimal i = MinKey, j = MaxKey + RangeKey, value = (decimal)MinValue; i < j; i += RangeKey, value += RangValueKey)
            {
                commissions.Add(value);
            }

            _commissions = commissions;
        }
    }
}