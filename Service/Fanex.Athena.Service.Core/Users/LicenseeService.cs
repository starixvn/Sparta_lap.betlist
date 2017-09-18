namespace Fanex.Athena.Service.Core.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.DataAccess.Core.Users;
    using Fanex.Athena.DataAccess.Core.Users.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;

    public class LicenseeService : ILicenseeService
    {
        private const int CacheTimeoutSeconds = 60;

        private readonly ILicenseeDataAccess _licenseeDataAccess;

        private readonly IMemoryCacheService _cache;

        public LicenseeService()
        {
            this._licenseeDataAccess = new LicenseeDataAccess();
            _cache = new MemoryCacheService();
        }

        public LicenseeService(ILicenseeDataAccess licenseeDataAccess)
        {
            this._licenseeDataAccess = licenseeDataAccess;
        }

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

        public IList<LicenseeCustomer> GetCustByLicensee(int licenseeId)
        {
            var cacheKey = CacheKey.MemorycacheCommonLicenseeCust + licenseeId;
            var licenseeCustList = _cache.Get<IList<LicenseeCustomer>>(cacheKey);

            if (licenseeCustList != null)
            {
                return licenseeCustList;
            }

            licenseeCustList = this._licenseeDataAccess.GetCustByLicensee(licenseeId)
                                   .OrderBy(s => s.Currency)
                                   .ToList();

            _cache.Set(cacheKey, licenseeCustList, CacheTimeoutSeconds);

            return licenseeCustList;
        }

        #endregion Public Methods and Operators

        public Licensee GetLicenseeByName(string licenseeName)
        {
            return _licenseeDataAccess.GetLicenseeByName(licenseeName);
        }
    }
}