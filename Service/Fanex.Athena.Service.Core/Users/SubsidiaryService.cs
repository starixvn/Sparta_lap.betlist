namespace Fanex.Athena.Service.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Users.Interfaces;
    using DataAccess.Core.Users.Interfaces;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;

    public class SubsidiaryService : ISubsidiaryService
    {
        private const int CacheTimeoutSeconds = 60;

        private readonly ISessionService _sessionService;

        private readonly IMemoryCacheService _cache;

        private readonly ISubsidiaryDataAccess _subLicenseeDataAccess;

        public SubsidiaryService(ISessionService sessionService, ISubsidiaryDataAccess subsidiaryDataAccess, IMemoryCacheService cache)
        {
            _sessionService = sessionService;
            _cache = cache;
            _subLicenseeDataAccess = subsidiaryDataAccess;
        }

        public IEnumerable<string> GetSubsidiaryNames()
        {
            var subsidiaryList = GetSubsidiaryList();
            return subsidiaryList.Select(x => x.OperatorId).Distinct();
        }

        public IEnumerable<Subsidiary> GetSubsidiaryList()
        {
            var licenseeName = _sessionService.LicenseeSession.LicenseeName;
            var cacheKey = string.Format(CacheKey.MemorycacheSubsidiaryListByLicensee, licenseeName);
            var subsidiaryList = _cache.Get(cacheKey, () => _subLicenseeDataAccess.GetSubsidiaryList(licenseeName), CacheTimeoutSeconds);
            var userAgentList = _sessionService.UserSession.CurrencyAgentList.SelectMany(i => i.Value);
            return subsidiaryList.Where(i => userAgentList.Contains(i.AgentId));
        }

        public IDictionary<int, IEnumerable<int>> GetCurrencyAgentList(string subsidiaryNames, bool isExcludeTest = false, int currencyId = 0)
        {
            const int CurrencyAll = 0;
            const string OtherMembersKey = "(others)";
            var currencyAgentList = new Dictionary<int, IEnumerable<int>>();
            if (string.IsNullOrWhiteSpace(subsidiaryNames))
            {
                return currencyAgentList;
            }

            var subsidiaryNameList = subsidiaryNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var subsidiaryData = GetSubsidiaryList();
            var dataGroupByCurrency = subsidiaryData
                .Where(x => (currencyId == CurrencyAll || x.CurrencyId == currencyId) && subsidiaryNameList.Contains(x.OperatorId))
                .GroupBy(x => x.CurrencyId);

            foreach (var item in dataGroupByCurrency)
            {
                currencyAgentList.Add(item.Key, item.Select(x => x.AgentId));
            }

            if (subsidiaryNameList.Contains(OtherMembersKey))
            {
                var exceptedAgentList = subsidiaryData
                    .Where(x => !subsidiaryNameList.Contains(x.OperatorId)).Select(x => x.AgentId).ToList();
                var otherAgentList = _sessionService.UserSession.CurrencyAgentList
                    .Where(x => x.Key == currencyId || currencyId == CurrencyAll).ToDictionary(x => x.Key, x => x.Value.AsEnumerable());

                currencyAgentList = currencyAgentList.Union(otherAgentList).GroupBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.SelectMany(g => g.Value).Except(exceptedAgentList).Distinct());
                currencyAgentList = currencyAgentList.Where(x => x.Value.Any()).ToDictionary(x => x.Key, x => x.Value);
            }

            if (isExcludeTest && currencyAgentList.ContainsKey(CurrencyType.Uus))
            {
                currencyAgentList.Remove(CurrencyType.Uus);
            }

            return currencyAgentList;
        }
    }
}