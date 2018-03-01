// --------------------------------------------------------------------------------------------------------------------
//
// <copyright file="SessionService.cs" company="">
// The session service.
// </copyright>
// <summary>
//   The session service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Configuration;
    using Configuration.Interfaces;
    using DataAccess.Core.Common;
    using DataAccess.Core.Common.Interfaces;
    using DataAccess.Core.Users;
    using DataAccess.Core.Users.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Utility;

    public class SessionService : ISessionService
    {
        private const int ReportBalanceId = 42;

        private const string ReportBalancePath = "BalanceReport";

        private const string LicenseeSessionKey = "LicenseeSessionKey";

        private const string UserSessionKey = "UserSessionKey";
        private const string CaptchaCodeKey = "CaptchaCodeKey";

        private readonly IAppSettings _appSettings;
        private readonly IFunctionService _functionService;
        private readonly ILicenseeDataAccess _licenseeDataAccess;
        private readonly ICurrencyDataAccess _currencyDataAccess;

        public SessionService()
        {
            _appSettings = new AppSettings();
            _functionService = new FunctionService();
            _licenseeDataAccess = new LicenseeDataAccess();
            _currencyDataAccess = new CurrencyDataAccess();
        }

        public SessionService(
            IAppSettings appSettings,
            IFunctionService functionService,
            ILicenseeDataAccess licenseeDataAccess,
            ICurrencyDataAccess currencyDataAccess,
            IFakeLicenseeSettingService fakeLicenseeSettingService)
        {
            _appSettings = appSettings;
            _functionService = functionService;
            _licenseeDataAccess = licenseeDataAccess;
            _currencyDataAccess = currencyDataAccess;
        }

        #region Public Properties

        public LicenseeSession LicenseeSession
        {
            get
            {
                var temp = HttpContext.Current.Session[LicenseeSessionKey];
                return temp != null ? (LicenseeSession)temp : null;
            }
        }

        public UserSession UserSession
        {
            get
            {
                var temp = HttpContext.Current.Session[UserSessionKey];
                return temp != null ? (UserSession)temp : null;
            }
        }

        public string CaptchaCode
        {
            get { return HttpContext.Current.Session[CaptchaCodeKey] as string; }
            set { HttpContext.Current.Session[CaptchaCodeKey] = value; }
        }

        public void AssignSessions(UserSession userSession, LicenseeSession licenseeSession)
        {
            HttpContext.Current.Session[UserSessionKey] = userSession;
            HttpContext.Current.Session[LicenseeSessionKey] = licenseeSession;
        }

        public void CreateNewSession(
            int userId,
            int onuserId,
            string userName,
            bool isInternal,
            int licenseeId = 0,
            string language = "en-US",
            ClientViewType viewType = ClientViewType.None)
        {
            var licenseeList = _licenseeDataAccess.GetByUser(userId);
            var functionList = _functionService.GetUserFunctionList(licenseeId, userId);
            var functionIdList = functionList.Select(f => f.FunctionId).ToList();
            var userSession = new UserSession
            {
                UserId = userId,
                OnUserId = onuserId,
                UserName = userName,
                IsInternal = isInternal,
                FunctionIdList = functionIdList,
                Language = language,
                DefaultReportUrl = GetDefaultReportUrl(functionIdList, functionList),
                ApplicationMode = AppMode.Self,
                ViewType = viewType
            };

            AssignCurrenciesToUserSession(userSession, licenseeId);
            var selectedLicensee = licenseeList.SingleOrDefault(i => i.Id == licenseeId);
            var licenseeSession = licenseeId > 0 && selectedLicensee != null ? GenerateLicenseeInfo(selectedLicensee) : null;

            AssignSessions(userSession, licenseeSession);
        }

        public void UpdateLiceseeSession(int licenseeId)
        {
            var userSession = UserSession;
            var licenseeList = _licenseeDataAccess.GetByUser(userSession.UserId);

            if (licenseeId > 0)
            {
                var selectedLicensee = licenseeList.SingleOrDefault(i => i.Id == licenseeId);
                var functionList = _functionService.GetUserFunctionList(licenseeId, userSession.UserId);
                var functionIdList = functionList.Select(f => f.FunctionId).ToList();

                userSession.FunctionIdList = functionIdList;
                userSession.DefaultReportUrl = GetDefaultReportUrl(functionIdList, functionList);

                AssignCurrenciesToUserSession(userSession, licenseeId);

                AssignSessions(userSession, GenerateLicenseeInfo(selectedLicensee));
            }
        }

        public bool UpdateLanguage(string languageCode)
        {
            var userSession = UserSession;

            if (userSession != null && !string.IsNullOrEmpty(languageCode))
            {
                userSession.Language = languageCode;
                HttpContext.Current.Session[UserSessionKey] = userSession;
                return true;
            }

            return false;
        }

        public bool IsAllowedEditCommission(UserSession user)
        {
            return user.ApplicationMode == AppMode.Parasitic ?
                    user.PortalFunctionList != null && user.PortalFunctionList.Any(i => i.FunctionKey == PermissionKeys.CustomerInfoCommissionGroupList)
                    : user.FunctionIdList != null && user.FunctionIdList.Contains((int)FunctionType.CommissionGroupList);
        }

        public bool IsAllowedSpecialBA(UserSession user)
        {
            return user.ApplicationMode == AppMode.Parasitic ?
                    user.PortalFunctionList != null && user.PortalFunctionList.Any(i => i.FunctionKey == PermissionKeys.CustomerInfoSpecialBA)
                    : user.FunctionIdList != null && user.FunctionIdList.Contains((int)FunctionType.SpecialBA);
        }

        public string GenerateAgentListXml(int currencyId = 0, bool isExcludeTestData = false)
        {
            return GenerateAgentListXml(new[] { currencyId < 0 ? 0 : currencyId }, isExcludeTestData);
        }

        public string GenerateAgentListXml(IEnumerable<int> currencyIdList, bool isExcludeTestData = false)
        {
            const int AllCurrencyId = 0;
            var result = new StringBuilder("<ROOT>");
            var currencyAgentList = UserSession.CurrencyAgentList;
            if (isExcludeTestData && currencyAgentList.ContainsKey(CurrencyType.Uus))
            {
                currencyAgentList = currencyAgentList.Where(x => x.Key != CurrencyType.Uus).ToDictionary(x => x.Key, x => x.Value);
            }

            if (currencyIdList.Contains(AllCurrencyId))
            {
                foreach (var currency in currencyAgentList)
                {
                    result.Append(GenerateAgentXmlBody(currency.Key, currency.Value));
                }
            }
            else
            {
                foreach (var currencyId in currencyIdList)
                {
                    if (currencyAgentList.ContainsKey(currencyId))
                    {
                        var agentList = currencyAgentList.SingleOrDefault(x => x.Key == currencyId).Value;
                        result.Append(GenerateAgentXmlBody(currencyId, agentList));
                    }
                }
            }

            result.Append("</ROOT>");
            return result.ToString();
        }

        public string GenerateAgentListXml(IDictionary<int, IEnumerable<int>> currencyAgentList)
        {
            var result = new StringBuilder("<ROOT>");

            foreach (var item in currencyAgentList)
            {
                result.Append(GenerateAgentXmlBody(item.Key, item.Value));
            }

            result.Append("</ROOT>");
            return result.ToString();
        }

        private static StringBuilder GenerateAgentXmlBody(int currencyId, IEnumerable<int> agentList)
        {
            var result = new StringBuilder();
            foreach (var agentId in agentList)
            {
                result.AppendFormat("<Agent AgentId=\"{0}\" Currency=\"{1}\"/>", agentId, currencyId);
            }

            return result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3995:UriReturnValuesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        private static string GetDefaultReportUrl(IEnumerable<int> functionIdList, IEnumerable<Function> functionList)
        {
            if (!functionIdList.Contains(ReportBalanceId))
            {
                var firstFunction = functionList.FirstOrDefault(x => x.ParentId > 0 && x.IsActive);
                if (firstFunction != null)
                {
                    return firstFunction.Url;
                }
            }

            return ReportBalancePath;
        }

        private static LicenseeSession GenerateLicenseeInfo(Licensee selectedLicensee)
        {
            return new LicenseeSession
            {
                LicenseeId = selectedLicensee.Id,
                LicenseeName = selectedLicensee.Name,
                IsAllowedCustomerComm = selectedLicensee.IsAllowedCustomerComm,
                IsEnableBA = selectedLicensee.IsEnableBA
            };
        }

        public void AssignCurrenciesToUserSession(UserSession userSession, int licenseeId, IEnumerable<CurrencyUser> currencyUserList = null)
        {
            var currencyUsers = currencyUserList ?? _currencyDataAccess.GetCurrenciesByUser(licenseeId, userSession.UserId);

            if (licenseeId == LicenseeIdentity.KeenOcean)
            {
                currencyUsers = KeanOceanBusinessHelper.ModifyCurrenciesForKeenOcean(currencyUsers);
            }

            var currencyList = currencyUsers
                .Where(x => x.IsActive)
                .Select(x => new { x.CurrencyId, x.CurrencyName })
                .ToDictionary(x => x.CurrencyId, x => x.CurrencyName);

            var currencyAgentList = currencyUsers
                .Where(x => x.IsActive)
                .Select(x => new { x.CurrencyId, x.AgentIdList })
                .ToDictionary(x => x.CurrencyId, x => x.AgentIdList);
            MergeAgentListForFakeLicensees(licenseeId, currencyAgentList);

            userSession.CurrencyList = currencyList;
            userSession.CurrencyAgentList = currencyAgentList;
        }

        private void MergeAgentListForFakeLicensees(int licenseeId, IDictionary<int, IEnumerable<int>> currencyAgentList)
        {
            if (!_appSettings.FakeLicenseeList.Contains(licenseeId))
            {
                return;
            }

            var licenseeSourcedSettings = _licenseeDataAccess.GetLicenseeSourcedSettings(licenseeId);
            var currencyList = currencyAgentList.Keys.ToList();
            foreach (var key in currencyList)
            {
                var mergingAgentList = licenseeSourcedSettings.Where(x => x.SourcedCurrencyId == key).Select(x => x.SourcedAgentId).ToArray();
                if (mergingAgentList.Any())
                {
                    currencyAgentList[key] = currencyAgentList[key].Concat(mergingAgentList).Distinct().ToList();
                }
            }
        }
    }

    #endregion Public Properties
}