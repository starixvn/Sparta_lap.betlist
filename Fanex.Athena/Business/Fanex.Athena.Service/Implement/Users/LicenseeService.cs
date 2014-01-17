// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicenseeService.cs" company="Nexcel">
// The licensee service.
// </copyright>
// <summary>
//   The licensee service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Users;
    using Fanex.Athena.DataAccess.Interface.Users;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Users;
    using Fanex.Athena.Service.Exception;
    using Fanex.Athena.Service.Interface.Users;
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

        /// <summary>
        ///     The can update member ba status.
        /// </summary>
        /// <param name="request">
        ///     The request.
        /// </param>
        /// <param name="licenseeId">
        ///     The licensee id.
        /// </param>
        /// <param name="isAllow">
        ///     Is Allowed updating BA status.
        /// </param>
        /// <returns>
        ///  The result information.
        /// </returns>
        /// <exception cref="ServiceException">
        /// The service exception.
        /// </exception>
        public IResultInfo IsAllowUpdateMemberBaStatus(Request request, int licenseeId, out bool isAllow)
        {
            try
            {
                return this._licenseeDataAccess.IsAllowUpdateMemberBaStatus(request, licenseeId, out isAllow);
            }
            catch (System.Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw new ServiceException(ex.Message, "CanUpdateMemberBAStatus");
            }
        }

        /// <summary>
        ///  Return true if licensee is belong to user.Otherwise return false.
        /// </summary>
        /// <param name="licenseeId">
        /// The licensee id.
        /// </param>
        /// <param name="userId">The user id.</param>
        /// <returns>
        ///    The boolean value.
        /// </returns>
        public bool IsLicenseeBelongToUser(int licenseeId, int userId)
        {
            var licenseeList = GetByUser(userId);
            return licenseeList.Any(l => l.Id == licenseeId);
        }

        /// <summary>
        ///     The get by user.
        /// </summary>
        /// <param name="userId">
        ///     The user id.
        /// </param>
        /// <returns>
        /// The licensee list.
        /// </returns>
        public IList<Licensee> GetByUser(int userId)
        {
            return this._licenseeDataAccess.GetByUser(userId);
        }

        /// <summary>
        ///     Get offered currency for a licensee.
        /// </summary>
        /// <param name="licenseeId">
        ///     The licensee id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        ///     The  currency list.
        /// </returns>
        public IList<Currency> GetCurrencies(int licenseeId, int userId)
        {
            var currencies = this._licenseeDataAccess.GetCurrencies(licenseeId, userId);
            return currencies;
        }

        /// <summary>
        ///     The get customer by licensee.
        /// </summary>
        /// <param name="licenseeId">
        ///     The licensee id.
        /// </param>
        /// <returns>
        /// The customer list.
        /// </returns>
        public IList<LicenseeCustomer> GetCustByLicensee(int licenseeId)
        {
            try
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
            catch (System.Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw new ServiceException(ex.Message, "CurrencyService");
            }
        }

        /// <summary>
        ///     Get list of licenseeId base on resellerId.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="resellerId">The reseller id.</param>
        /// <returns>The reseller identity list.</returns>
        public IList<int> GetByResellerId(int userId, int resellerId)
        {
            return _licenseeDataAccess.GetByResellerId(userId, resellerId);
        }

        #endregion Public Methods and Operators
    }
}