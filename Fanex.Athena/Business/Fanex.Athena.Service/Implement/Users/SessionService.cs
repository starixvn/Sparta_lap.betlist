// --------------------------------------------------------------------------------------------------------------------
//
// <copyright file="SessionService.cs" company="Nexcel">
// The session service.
// </copyright>
// <summary>
//   The session service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Users;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Users;

    /// <summary>
    ///     The session service.
    /// </summary>
    public class SessionService : ISessionService
    {
        #region Constants and Fields

        /// <summary>
        ///     The licensee session key.
        /// </summary>
        private const string LicenseeSessionKey = "LicenseeSessionKey";

        private const string UserSessionKey = "UserSessionKey";
        private const string CaptchaCodeKey = "CaptchaCodeKey";
        private const string MoveBetKey = "MoveBetKey";
        private const string IsFirstLoadKey = "IsFirstLoadKey";
        private IFunctionService _functionService;
        private ILicenseeService _licenseeService;
        private ICurrencyService _currencyService;
        private IMoveBetService _moveBetService;

        #endregion Constants and Fields

        #region Public Properties

        public bool IsFirstLoad
        {
            get
            {
                var temp = HttpContext.Current.Session[IsFirstLoadKey];
                return temp != null ? (bool)temp : true;
            }

            private set
            {
                HttpContext.Current.Session[IsFirstLoadKey] = value;
            }
        }

        public MoveBet MoveBet
        {
            get
            {
                var temp = HttpContext.Current.Session[MoveBetKey];
                return temp != null ? (MoveBet)temp : null;
            }

            private set
            {
                HttpContext.Current.Session[MoveBetKey] = value;
            }
        }

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

        public void TurnOffIsFirstLoad()
        {
            this.IsFirstLoad = false;
        }

        /// <summary>
        /// Create new session.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="onuserId">The check-in id.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="isInternal">True if internal user.</param>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="language">The language code.</param>
        public void CreateNewSession(
            int userId,
            int onuserId,
            string userName,
            bool isInternal,
            int licenseeId = 0,
            string language = "en-US")
        {
            _currencyService = new CurrencyService();
            _licenseeService = new LicenseeService();
            _functionService = new FunctionService();
            _moveBetService = new MoveBetService();
            
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
                    CurrencyList = _currencyService.GetCurrencyMapByUser(licenseeId, userId)
                };

            this.UserSession = userSession;
            this.MoveBet = _moveBetService.GetMoveBet();
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

        /// <summary>
        /// Update licensee session.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        public void UpdateLiceseeSession(int licenseeId)
        {
            _licenseeService = new LicenseeService();
            _functionService = new FunctionService();
            _currencyService = new CurrencyService();
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
            }
        }

        /// <summary>
        /// Update language.
        /// </summary>
        /// <param name="languageCode">The language code.</param>
        /// <returns>True if processing is successful.</returns>
        public bool UpdateLanguage(string languageCode)
        {
            if (!string.IsNullOrEmpty(languageCode))
            {
                var userSession = this.UserSession;
                userSession.Language = languageCode;
                return true;
            }

            return false;
        }
    }

        #endregion Public Properties
}