namespace Fanex.Athena.Service.Customer.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    public static class GroupCommHelper
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