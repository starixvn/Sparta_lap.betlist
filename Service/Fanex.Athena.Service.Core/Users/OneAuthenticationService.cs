namespace Fanex.Athena.Service.Core.Users
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;
    using DataAccess.Core.Users;
    using DataAccess.Core.Users.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.DTO;
    using Model.Core.DTO.OneAuth;
    using Model.Core.DTO.Users;
    using Newtonsoft.Json;
    using Resources;

    public class OneAuthenticationService : IOneAuthenticationService
    {
        private readonly ISessionService _sessionService;
        private readonly ILicenseeDataAccess _licenseeDataAccess;
        private readonly IFormsAuthenticationService _formsAuthenticationService;
        private readonly IOneBookSecurityWrapper _oneBookSecurityWrapper;

        public OneAuthenticationService() : this(null, null, null, null)
        {
        }

        public OneAuthenticationService(
            ISessionService sessionService,
            ILicenseeDataAccess licenseeDataAccess,
            IFormsAuthenticationService formsAuthenticationService,
            IOneBookSecurityWrapper oneBookSecurityWrapper)
        {
            _sessionService = sessionService ?? new SessionService();
            _licenseeDataAccess = licenseeDataAccess ?? new LicenseeDataAccess();
            _formsAuthenticationService = formsAuthenticationService ?? new FormsAuthenticationService();
            _oneBookSecurityWrapper = oneBookSecurityWrapper ?? new OneBookSecurityWrapper();
        }

        protected internal virtual string GetStringFromFile(string path)
        {
            using (var reader = new StreamReader(HostingEnvironment.MapPath(path)))
            {
                return reader.ReadToEnd();
            }
        }

        public IResultInfo SignIn(HttpContextBase currentContext, OneAuthToken tokenInfo)
        {
            var licensee = _licenseeDataAccess.GetLicenseeByName(tokenInfo.SiteName);
            if (licensee == null)
            {
                return new ResultInfo
                {
                    Code = OneAuthorizationErrorCode.LicenseeIsNotExisted,
                    Message = ViewResources.LicenseeIsNotExisted
                };
            }

            var currencyList = GetCurrencyList(tokenInfo);
            if (currencyList == null || !currencyList.Any())
            {
                return new ResultInfo
                {
                    Code = OneAuthorizationErrorCode.UserNotHaveCurrency,
                    Message = ViewResources.UserNotHaveCurrency
                };
            }

            var userSession = _sessionService.UserSession;
            var licenseeSession = _sessionService.LicenseeSession;
            var language = Util.MapLanguages.Where(i => i.Value.Equals(tokenInfo.Language))
                       .Select(i => i.Key).DefaultIfEmpty<string>("en-Us").FirstOrDefault();
            if (IsAuthenticated(currentContext, userSession, licenseeSession) &&
                IsSameSession(userSession, licenseeSession, tokenInfo, licensee.Id))
            {
                UpdateUserSession(userSession, tokenInfo, licensee.Id, currencyList, language);
                return new ResultInfo
                {
                    ExtendedData = tokenInfo.ActionLink
                };
            }

            CreateSession(language, tokenInfo, licensee, currencyList);
            return SetAuthorize(tokenInfo);
        }

        public OneAuthToken GetTokenInfo(string token)
        {
            return _oneBookSecurityWrapper.Authenticate(token);
        }

        private static bool IsSameSession(UserSession userSession, LicenseeSession licenseeSession, OneAuthToken tokenInfo, int licenseeId)
        {
            return userSession.UserName == tokenInfo.UserID
                && userSession.VendorID == tokenInfo.VendorID && userSession.VendorIdentityID == tokenInfo.UserID
                && licenseeSession.LicenseeId == licenseeId;
        }

        private static bool IsAuthenticated(HttpContextBase currentContext, UserSession userSession, LicenseeSession licenseeSession)
        {
            if (userSession == null || licenseeSession == null || !IsValidIdentity(currentContext))
            {
                return false;
            }

            var isValidUserId = userSession.UserId > 0 && !string.IsNullOrEmpty(userSession.VendorIdentityID) && !string.IsNullOrEmpty(userSession.VendorID);

            return isValidUserId && userSession.IsInternal && licenseeSession.LicenseeId > 0 && userSession.ApplicationMode == AppMode.Parasitic;
        }

        private static bool IsValidIdentity(HttpContextBase currentContext)
        {
            var identity = currentContext.User.Identity;
            return identity != null && identity.IsAuthenticated && !string.IsNullOrEmpty(identity.Name);
        }

        protected internal virtual IResultInfo SetAuthorize(OneAuthToken tokenInfo)
        {
            var authName = $"{tokenInfo.VendorID}_{tokenInfo.SiteName}_{tokenInfo.UserID}";
            _formsAuthenticationService.SetAuthCookie(authName, false);

            return new ResultInfo
            {
                ExtendedData = tokenInfo.ActionLink
            };
        }

        private void UpdateUserSession(UserSession userSession, OneAuthToken tokenInfo, int licenseeId, IEnumerable<CurrencyUser> currencyList, string language)
        {
            if (!language.Equals(userSession.Language))
            {
                _sessionService.UpdateLanguage(language);
            }

            _sessionService.AssignCurrenciesToUserSession(userSession, licenseeId, currencyList);
            userSession.PortalFunctionList = GetPermissionList(tokenInfo);
        }

        private IEnumerable<CurrencyUser> GetCurrencyList(OneAuthToken tokenInfo)
        {
            return tokenInfo.Currency?.Select(x => new CurrencyUser
            {
                IsActive = true,
                CurrencyId = x.CurrencyId,
                CurrencyName = x.Currency,
                AgentIdList = x.AgentId
            }).ToList();
        }

        private void CreateSession(
            string languageCode,
            OneAuthToken tokenInfo,
            Licensee licenseeInfo,
            IEnumerable<CurrencyUser> currencyByUser)
        {
            var licSession = new LicenseeSession
            {
                LicenseeId = licenseeInfo.Id,
                IsAllowedCustomerComm = licenseeInfo.IsAllowedCustomerComm,
                IsEnableBA = licenseeInfo.IsEnableBA,
                LicenseeName = licenseeInfo.Name
            };

            var userSession = new UserSession
            {
                UserId = tokenInfo.UserSEQ,
                OnUserId = tokenInfo.UserSEQ,
                UserName = tokenInfo.UserID,
                IsInternal = true,
                FunctionIdList = new List<int>(),
                PortalFunctionList = GetPermissionList(tokenInfo),
                Language = languageCode,
                DefaultReportUrl = "BalanceReport",
                ApplicationMode = AppMode.Parasitic,
                VendorID = tokenInfo.VendorID,
                VendorIdentityID = tokenInfo.UserID,
                ViewType = ClientViewType.Desktop,
            };

            _sessionService.AssignCurrenciesToUserSession(userSession, licenseeInfo.Id, currencyByUser);

            _sessionService.AssignSessions(userSession, licSession);
        }

        private IList<PortalSystemFunction> GetPermissionList(OneAuthToken tokenInfo)
        {
            return tokenInfo.IsAdminstrator ? GetFunctionList() : tokenInfo.Permissions.Select(x => new PortalSystemFunction
            {
                FunctionKey = x.FunctionKey
            }).ToList();
        }

        /// <summary>
        /// This function will be removed when integrating permission part with LAP.
        /// </summary>
        /// <returns>Return the list portal function.</returns>
        private IList<PortalSystemFunction> GetFunctionList()
        {
            const string FunctionFilePath = "~/App_Data/PortalSystemFunction.json";
            IList<PortalSystemFunction> result = null;
            var temp = GetStringFromFile(FunctionFilePath);

            if (!string.IsNullOrEmpty(temp))
            {
                result = JsonConvert.DeserializeObject<IList<PortalSystemFunction>>(temp);
            }

            return result ?? new List<PortalSystemFunction>();
        }
    }
}