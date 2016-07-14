// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceUpService.cs" company="">
// The balance up service.
// </copyright>
// <summary>
//   The balance up service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Configuration
{
    using System;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.Core.Configuration;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Core.Helpers;
    using Fanex.Caching;

    public class BalanceUpService : IBalanceUpService
    {
        private const int CacheTimeoutMinute = 1;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public BalanceUpService()
        {
            this._balanceUpDataAccess = new BalanceUpDataAccess();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        public BalanceUp BalanceUpInfo
        {
            get
            {
                BalanceUp balanceup = null;
                try
                {
                    var cache = CacheService.CreateInstance(CacheType.MemoryCache);

                    balanceup = cache.Get<BalanceUp>(CacheKey.MemorycacheCommonBalanceup);

                    if (balanceup != null)
                    {
                        return balanceup;
                    }

                    balanceup = this._balanceUpDataAccess.GetBalanceUpDay();
                    cache.Set(
                        CacheKey.MemorycacheCommonBalanceup, balanceup, DateTime.Now.AddMinutes(CacheTimeoutMinute));
                }
                catch (Exception exception)
                {
                    LogHelper.WriteError(exception);
                }

                return balanceup;
            }
        }
    }
}