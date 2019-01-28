namespace Fanex.Athena.DataAccess.BetList
{
    using System;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Logging;

    public class BalanceUpDataAccess : BaseDataAccess, IBalanceUpDataAccess
    {
        private const int CacheTimeoutSeconds = 60;

        private readonly ILogger _logger;

        private readonly IMemoryCacheService _cache;

        [CLSCompliant(false)]
        public BalanceUpDataAccess(ILogger logger, IMemoryCacheService cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public BalanceUp GetBalanceUp()
        {
            BalanceUp balanceup = null;
            try
            {
                balanceup = _cache.Get<BalanceUp>(CacheKey.MemorycacheCommonBalanceup);

                if (balanceup != null)
                {
                    return balanceup;
                }

                balanceup = GetBalanceUpDayFromDb();
                _cache.Set(CacheKey.MemorycacheCommonBalanceup, balanceup, CacheTimeoutSeconds);
            }
            catch (Exception exception)
            {
                _logger.Error("BalanceUp: " + exception.Message, exception);
            }

            return balanceup;
        }

        private BalanceUp GetBalanceUpDayFromDb()
        {
            const string UspName = "Lrf_SelBalanceUpDay";

            using (var db = this.GetObjectDb(UspName))
            {
                var balanceUp = new BalanceUp();
                using (var reader = db.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        balanceUp.BalanceUpDay = reader.ToDateTime("balanceupday");
                        balanceUp.MoveBetsDay = reader.ToDateTime("movebetsday");
                    }

                    return balanceUp;
                }
            }
        }
    }
}