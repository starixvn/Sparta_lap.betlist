// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalanceUpService.cs" company="Nexcel">
// The balance up service.
// </copyright>
// <summary>
//   The balance up service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Configuration
{
    using System;
    using Fanex.Athena.DataAccess.Implement.Configuration;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Service.Helpers;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Caching;

    /// <summary>
    ///     The balance up service.
    /// </summary>
    public class BalanceUpService : IBalanceUpService
    {
        #region Constants and Fields

        /// <summary>
        ///     The cache timeout minute.
        /// </summary>
        private const int CacheTimeoutMinute = 1;

        /// <summary>
        ///     The _balance up data access.
        /// </summary>
        private readonly BalanceUpDataAccess _balanceUpDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BalanceUpService" /> class.
        /// </summary>
        public BalanceUpService()
        {
            this._balanceUpDataAccess = new BalanceUpDataAccess();
        }

        #endregion Constructors and Destructors

        #region Public Properties

        /// <summary>
        ///     Gets GetBalanceUp.
        /// </summary>
        /// <value>The balance up day.</value>
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

        #endregion Public Properties
    }
}