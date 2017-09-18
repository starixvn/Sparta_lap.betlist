// -----------------------------------------------------------------------
// <copyright file="BalanceUpDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Configuration
{
    using System;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class BalanceUpDataAccess : BaseDataAccess, IBalanceUpDataAccess
    {
        private const int CacheTimeoutSeconds = 60;

        private readonly IMemoryCacheService _cache;

        public BalanceUpDataAccess()
        {
            _cache = new MemoryCacheService();
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
                LogService.Error(exception.Message, exception);
            }

            return balanceup;
        }

        private BalanceUp GetBalanceUpDayFromDb()
        {
            const string UspName = "Lrf_SelBalanceUpDay";
            try
            {
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
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }
    }
}