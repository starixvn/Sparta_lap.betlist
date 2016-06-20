// --------------------------------------------------------------------------------------------------------------------
//
// <copyright file="SessionService.cs" company="Nexcel">
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
    using Model.Core.DTO.Common;

    public class SessionService : ISessionService
    {
        private const int ReportBalanceId = 42;
        private const string ReportBalanceUrl = "BalanceReport";

        /// <summary>
        ///     The licensee session key.
        /// </summary>
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

        public void CreateNewSession(
            int userId,
            int onuserId,
            string userName,
            bool isInternal,
            int licenseeId = 0,
            string language = "en-US")
        {
            var licenseeList = _licenseeService.GetByUser(userId);
            var isAdmin = licenseeList != null & licenseeList.Count > 0;
            var functionList = _functionService.GetUserFunctionList(licenseeId, userId);
            var functionIdList = functionList.Select(f => f.FunctionId).ToList();

            var userSession = new UserSession
            {
                UserId = userId,
                OnUserId = onuserId,
                UserName = userName,
                IsAdmin = isAdmin,
                IsInternal = isInternal,
                FunctionIdList = functionIdList,
                Language = language,
                CurrencyList = _currencyService.GetCurrencyMapByUser(licenseeId, userId),
                DefaultReportUrl = GetDefaultReportUrl(functionIdList, functionList)
            };

            this.UserSession = userSession;

            if (licenseeId > 0)
            {
                var selectedLicensee = licenseeList.SingleOrDefault(i => i.Id == licenseeId);
                var agentList = _licenseeService.GetCustByLicensee(licenseeId) as List<LicenseeCustomer>;
                agentList = agentList ?? new List<LicenseeCustomer>();

                var licenseeSession = new LicenseeSession
                {
                    LicenseeId = licenseeId,
                    UserId = userId,
                    LicenseeName = selectedLicensee.Name,
                    UserName = userName,
                    LicenseeType = agentList
                            .Where(s => !string.IsNullOrEmpty(s.Description))
                            .Select(s => s.Description)
                            .Distinct()
                            .ToList(),
                    CurrencyName = agentList.Select(s => s.CurrencyName).Distinct().ToList()
                };

                licenseeSession.IsAllowedCustomerComm = selectedLicensee.IsAllowedCustomerComm;
                this.LicenseeSession = licenseeSession;
            }
        }

        public void UpdateLiceseeSession(int licenseeId)
        {
            var currentUserSession = this.UserSession;
            var licenseeList = _licenseeService.GetByUser(currentUserSession.UserId);

            if (currentUserSession != null && licenseeId > 0)
            {
                var selectedLicensee = licenseeList.SingleOrDefault(i => i.Id == licenseeId);
                var agentList = _licenseeService.GetCustByLicensee(licenseeId) as List<LicenseeCustomer>;
                agentList = agentList ?? new List<LicenseeCustomer>();

                this.LicenseeSession = new LicenseeSession
                {
                    LicenseeId = licenseeId,
                    UserId = currentUserSession.UserId,
                    LicenseeName = selectedLicensee.Name,
                    UserName = currentUserSession.UserName,
                    LicenseeType = agentList
                            .Where(s => !string.IsNullOrEmpty(s.Description))
                            .Select(s => s.Description)
                            .Distinct()
                            .ToList(),
                    CurrencyName = agentList.Select(s => s.CurrencyName).Distinct().ToList(),
                    IsAllowedCustomerComm = selectedLicensee.IsAllowedCustomerComm,
                };

                var functionList = _functionService.GetUserFunctionList(licenseeId, currentUserSession.UserId);
                var functionIdList = functionList.Select(f => f.FunctionId).ToList();
                currentUserSession.CurrencyList = _currencyService.GetCurrencyMapByUser(licenseeId, currentUserSession.UserId);
                currentUserSession.FunctionIdList = functionIdList;
                currentUserSession.DefaultReportUrl = GetDefaultReportUrl(functionIdList, functionList);
            }
        }

        private static string GetDefaultReportUrl(IList<int> functionIdList, IList<Function> functionList)
        {
            if (!functionIdList.Contains(ReportBalanceId))
            {
                var firstFunction = functionList.Where(x => x.ParentId > 0 && x.IsActive).FirstOrDefault();
                if (firstFunction != null)
                {
                    return firstFunction.Url;
                }
            }

            return ReportBalanceUrl;
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
    }

    #endregion Public Properties
}