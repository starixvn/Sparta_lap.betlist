namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Users;
    using Core.Users.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using DataAccess.Report;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;

    public class SubsidiaryService : ISubsidiaryService
    {
        private const int CacheTimeoutSeconds = 300;

        private readonly ISessionService _sessionService;

        private readonly IMemoryCacheService _cache;

        private readonly ISubsidiaryDataAccess _subLicenseeDataAccess;

        public SubsidiaryService()
        {
            _sessionService = new SessionService();
            _cache = new MemoryCacheService();
            _subLicenseeDataAccess = new SubsidiaryDataAccess();
        }

        public SubsidiaryService(ISessionService sessionService, ISubsidiaryDataAccess subsidiaryDataAccess, IMemoryCacheService cache)
        {
            _sessionService = sessionService;
            _cache = cache;
            _subLicenseeDataAccess = subsidiaryDataAccess;
        }

        public IEnumerable<string> GetOperatorNames(string licenseeName)
        {
            var subsidiaryList = GetOperatorList(licenseeName);
            var filteredSubsidiaryList = subsidiaryList.Where(x => _sessionService.UserSession.CurrencyList.ContainsKey(x.CurrencyId));
            return filteredSubsidiaryList.Any() ? filteredSubsidiaryList.Select(x => x.SubsidiaryName).Distinct() : new List<string>();
        }

        public IEnumerable<Subsidiary> GetOperatorList(string licenseeName)
        {
            var cacheKey = string.Format(CacheKey.MemorycacheSubsidiaryListByLicensee, licenseeName);
            return _cache.Get(cacheKey, () => _subLicenseeDataAccess.GetSubsidiaryList(licenseeName), CacheTimeoutSeconds);
        }
    }
}