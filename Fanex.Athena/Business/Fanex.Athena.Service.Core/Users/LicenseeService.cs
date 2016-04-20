// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseeService.cs" company="Nexcel">
// The licensee service.
// </copyright>
// <summary>
//   The licensee service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core.Users;
    using Fanex.Athena.DataAccess.Core.Users.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Service.Core.Exception;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Caching;
    using Fanex.Logging;

    /// <summary>
    ///     The licensee service.
    /// </summary>
    public class LicenseeService : ILicenseeService
    {
        #region Constants and Fields

        /// <summary>
        ///     The cache timeout minute.
        /// </summary>
        private const int CacheTimeoutMinute = 1;

        /// <summary>
        ///     The _licensee data access.
        /// </summary>
        private readonly ILicenseeDataAccess _licenseeDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="LicenseeService" /> class.
        /// </summary>
        public LicenseeService()
        {
            this._licenseeDataAccess = new LicenseeDataAccess();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LicenseeService" /> class.
        /// </summary>
        /// <param name="licenseeDataAccess">
        ///     The licensee data access.
        /// </param>
        public LicenseeService(ILicenseeDataAccess licenseeDataAccess)
        {            
            this._licenseeDataAccess = licenseeDataAccess;
        }

        #endregion Constructors and Destructors

        #region Public Methods and Operators

        public IResultInfo IsAllowUpdateMemberBAStatus(Request request, int licenseeId, out bool isAllow)
        {
            return this._licenseeDataAccess.IsAllowUpdateMemberBAStatus(request, licenseeId, out isAllow);
        }

        public bool IsLicenseeBelongToUser(int licenseeId, int userId)
        {
            var licenseeList = GetByUser(userId);
            return licenseeList.Any(l => l.Id == licenseeId);
        }

        public IList<Licensee> GetByUser(int userId)
        {
            return this._licenseeDataAccess.GetByUser(userId);
        }

        public IList<Currency> GetCurrencies(int licenseeId, int userId)
        {
            var currencies = this._licenseeDataAccess.GetCurrencies(licenseeId, userId);
            return currencies;
        }

        public IList<LicenseeCustomer> GetCustByLicensee(int licenseeId)
        {
            var cache = CacheService.CreateInstance(CacheType.MemoryCache);
            var cacheKey = CacheKey.MemorycacheCommonLicenseeCust + licenseeId;
            var licenseeCustList = cache.Get<IList<LicenseeCustomer>>(cacheKey);

            if (licenseeCustList != null)
            {
                return licenseeCustList;
            }

            licenseeCustList = this._licenseeDataAccess.GetCustByLicensee(licenseeId)
                                   .OrderBy(s => s.Currency)
                                   .ToList();

            cache.Set(cacheKey, licenseeCustList, DateTime.Now.AddMinutes(CacheTimeoutMinute));

            return licenseeCustList;
        }

        /// <summary>
        ///     Get list of licenseeId base on resellerId.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="resellerId">The reseller id.</param>
        /// <returns>The reseller identity list.</returns>
        public IEnumerable<int> GetByResellerId(int userId, int resellerId)
        {
            return _licenseeDataAccess.GetByResellerId(userId, resellerId);
        }

        #endregion Public Methods and Operators

        public string GetNumberOfOnlineUser()
        {
            return _licenseeDataAccess.GetNumberOfOnlineUser();
        }
    }
}