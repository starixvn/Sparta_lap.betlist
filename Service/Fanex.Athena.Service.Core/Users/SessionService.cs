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
        private IFunctionService _functionService;
        private ILicenseeService _licenseeService;
        private ICurrencyService _currencyService;

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

            private set
            {
                HttpContext.Current.Session[LicenseeSessionKey] = value;
            }
        }

        public UserSession UserSession
        {
            get
            {
                var temp = HttpContext.Current.Session[UserSessionKey];
                return temp != null ? (UserSession)temp : null;
            }

            private set
            {
                HttpContext.Current.Session[UserSessionKey] = value;
            }
        }

        public string CaptchaCode
        {
            get { return HttpContext.Current.Session[CaptchaCodeKey] as string; }
            set { HttpContext.Current.Session[CaptchaCodeKey] = value; }
        }

        public void CreateNewSession(UserSession userSession, LicenseeSession licenseeSession)
        {
            UserSession = userSession;
            LicenseeSession = licenseeSession;
            AssignCurrenciesToUserSession(this.UserSession, licenseeSession.LicenseeId);
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

            this.UserSession = userSession;
            var selectedLicensee = licenseeList.SingleOrDefault(i => i.Id == licenseeId);
            if (licenseeId > 0 && selectedLicensee != null)
            {
                AssignCurrenciesToUserSession(this.UserSession, licenseeId);
                var licenseeSession = new LicenseeSession();
                AssignDataForLicenseeSession(licenseeSession, selectedLicensee);
                licenseeSession.IsAllowedCustomerComm = selectedLicensee.IsAllowedCustomerComm;
                this.LicenseeSession = licenseeSession;
            }
        }

        public void UpdateLiceseeSession(int licenseeId)
        {
            var userSession = this.UserSession;
            var licenseeList = _licenseeService.GetByUser(userSession.UserId);

            if (userSession != null && licenseeId > 0)
            {
                var selectedLicensee = licenseeList.SingleOrDefault(i => i.Id == licenseeId);

                this.LicenseeSession = new LicenseeSession();
                AssignDataForLicenseeSession(this.LicenseeSession, selectedLicensee);

                var functionList = _functionService.GetUserFunctionList(licenseeId, userSession.UserId);
                var functionIdList = functionList.Select(f => f.FunctionId).ToList();
                userSession.FunctionIdList = functionIdList;
                userSession.DefaultReportUrl = GetDefaultReportUrl(functionIdList, functionList);

                AssignCurrenciesToUserSession(userSession, licenseeId);
            }
        }

        public bool UpdateLanguage(string languageCode)
        {
            if (!string.IsNullOrEmpty(languageCode))
            {
                UserSession.Language = languageCode;
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

        private void AssignDataForLicenseeSession(LicenseeSession licenseeSession, Licensee selectedLicensee)
        {
            licenseeSession.LicenseeId = selectedLicensee.Id;
            licenseeSession.LicenseeName = selectedLicensee.Name;
            licenseeSession.IsAllowedCustomerComm = selectedLicensee.IsAllowedCustomerComm;
            licenseeSession.IsEnableBA = selectedLicensee.IsEnableBA;
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