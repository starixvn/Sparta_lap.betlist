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

        private const string ReportBalanceUrl = "BalanceReport";

        private const string LicenseeSessionKey = "LicenseeSessionKey";

        private const string UserSessionKey = "UserSessionKey";
        private const string CaptchaCodeKey = "CaptchaCodeKey";
        private readonly IFunctionService _functionService;
        private readonly ILicenseeService _licenseeService;
        private readonly ICurrencyService _currencyService;

        public SessionService()
        {
            _functionService = new FunctionService();
            _licenseeService = new LicenseeService();
            _currencyService = new CurrencyService();
        }

        public SessionService(
            IFunctionService functionService,
            ILicenseeService licenseeService,
            ICurrencyService currencyService)
        {
            _functionService = functionService;
            _licenseeService = licenseeService;
            _currencyService = currencyService;
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

        public void CreateNewSession(UserSession userSession, LicenseeSession licenseeSession)
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
            string language = "en-US")
        {
            var licenseeList = _licenseeService.GetByUser(userId);
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
                ApplicationMode = AppMode.Self
            };

            AssignCurrenciesToUserSession(userSession, licenseeId);
            var selectedLicensee = licenseeList.SingleOrDefault(i => i.Id == licenseeId);
            var licenseeSession = licenseeId > 0 && selectedLicensee != null ? GenerateLicenseeInfo(selectedLicensee) : null;

            this.CreateNewSession(userSession, licenseeSession);
        }

        public void UpdateLiceseeSession(int licenseeId)
        {
            var userSession = this.UserSession;
            var licenseeList = _licenseeService.GetByUser(userSession.UserId);

            if (userSession != null && licenseeId > 0)
            {
                var selectedLicensee = licenseeList.SingleOrDefault(i => i.Id == licenseeId);
                var functionList = _functionService.GetUserFunctionList(licenseeId, userSession.UserId);
                var functionIdList = functionList.Select(f => f.FunctionId).ToList();

                userSession.FunctionIdList = functionIdList;
                userSession.DefaultReportUrl = GetDefaultReportUrl(functionIdList, functionList);

                AssignCurrenciesToUserSession(userSession, licenseeId);

                this.CreateNewSession(userSession, GenerateLicenseeInfo(selectedLicensee));
            }
        }

        public bool UpdateLanguage(string languageCode)
        {
            var userSession = this.UserSession;

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
            var result = new StringBuilder("<ROOT>");
            var currencyAgentList = this.UserSession.CurrencyAgentList;
            if (isExcludeTestData)
            {
                currencyAgentList = currencyAgentList.Where(x => x.Key != CurrencyType.Uus).ToDictionary(x => x.Key, x => x.Value);
            }

            if (currencyId == 0)
            {
                foreach (var currency in currencyAgentList)
                {
                    result.Append(GenerateAgentXmlBody(currency.Key, currency.Value));
                }
            }
            else if (currencyAgentList.ContainsKey(currencyId))
            {
                var agentList = currencyAgentList.SingleOrDefault(x => x.Key == currencyId).Value;
                result.Append(GenerateAgentXmlBody(currencyId, agentList));
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

        private static string GetDefaultReportUrl(IList<int> functionIdList, IList<Function> functionList)
        {
            if (!functionIdList.Contains(ReportBalanceId))
            {
                var firstFunction = functionList.FirstOrDefault(x => x.ParentId > 0 && x.IsActive);
                if (firstFunction != null)
                {
                    return firstFunction.Url;
                }
            }

            return ReportBalanceUrl;
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

        private void AssignCurrenciesToUserSession(UserSession userSession, int licenseeId)
        {
            var currencyUserList = _currencyService.GetCurrenciesByUser(licenseeId, userSession.UserId);

            if (licenseeId == LicenseeIdentityDefinition.KeenOcean)
            {
                currencyUserList = KeanOceanBusinessHelper.ModifyCurrenciesForKeenOcean(currencyUserList);
            }

            var currencyList = currencyUserList
                .Where(x => x.IsActive)
                .Select(x => new { x.CurrencyId, x.CurrencyName })
                .ToDictionary(x => x.CurrencyId, x => x.CurrencyName);

            var currencyAgentList = currencyUserList
                .Where(x => x.IsActive)
                .Select(x => new { x.CurrencyId, x.AgentIdList })
                .ToDictionary(x => x.CurrencyId, x => x.AgentIdList);

            userSession.CurrencyList = currencyList;
            userSession.CurrencyAgentList = currencyAgentList;
        }
    }

    #endregion Public Properties
}