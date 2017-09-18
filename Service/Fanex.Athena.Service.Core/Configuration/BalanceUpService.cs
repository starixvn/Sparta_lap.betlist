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
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.DataAccess.Core.Configuration;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Core.Helpers;

    public class BalanceUpService : IBalanceUpService
    {
        private const int CacheTimeoutSeconds = 60;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly IMemoryCacheService _cache;

        public BalanceUpService()
        {
            this._balanceUpDataAccess = new BalanceUpDataAccess();
            _cache = new MemoryCacheService();
        }

        public BalanceUpService(IBalanceUpDataAccess balanceUpDataAccess)
        {
            this._balanceUpDataAccess = balanceUpDataAccess;
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
                    balanceup = _cache.Get<BalanceUp>(CacheKey.MemorycacheCommonBalanceup);

                    if (balanceup != null)
                    {
                        return balanceup;
                    }

                    balanceup = this._balanceUpDataAccess.GetBalanceUpDay();
                    _cache.Set(CacheKey.MemorycacheCommonBalanceup, balanceup, CacheTimeoutSeconds);
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