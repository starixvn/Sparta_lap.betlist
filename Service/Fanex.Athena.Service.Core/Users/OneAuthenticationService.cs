namespace Fanex.Athena.Service.Core.Users
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;
    using Constant.DataDomain;
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
    using Session.Enum;
    using Session.Model;
    using Utility;
    using Utility.Interfaces;

    public class OneAuthenticationService : IOneAuthenticationService
    {
        private readonly ISessionService _sessionService;
        private readonly ILicenseeDataAccess _licenseeDataAccess;
        private readonly IFormsAuthenticationService _formsAuthenticationService;
        private readonly IOneBookSecurityWrapper _oneBookSecurityWrapper;
        private readonly ICoreLogger _logger;

        private readonly IOneAuthDataAccess _oneAuthDataAccess;

        public OneAuthenticationService() : this(null, null, null, null, null, null)
        {
        }

        public OneAuthenticationService(
            ISessionService sessionService,
            ILicenseeDataAccess licenseeDataAccess,
            IFormsAuthenticationService formsAuthenticationService,
            IOneBookSecurityWrapper oneBookSecurityWrapper,
            ICoreLogger logger,
            IOneAuthDataAccess oneAuthDataAccess)
        {
            _sessionService = sessionService ?? new SessionService();
            _licenseeDataAccess = licenseeDataAccess ?? new LicenseeDataAccess();
            _formsAuthenticationService = formsAuthenticationService ?? new FormsAuthenticationService();
            _oneBookSecurityWrapper = oneBookSecurityWrapper ?? new OneBookSecurityWrapper();
            _logger = logger ?? new CoreLogger();
            _oneAuthDataAccess = oneAuthDataAccess ?? new OneAuthDataAccess();
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

            var userInfo = GetUserInfo(tokenInfo);
            var result = ValidateCurrencyList(userInfo);
            if (!result.IsSuccessful)
            {
                return result;
            }

            var userSession = _sessionService.UserSession;
            var licenseeSession = _sessionService.LicenseeSession;
            var language = Util.MapLanguages.Where(i => i.Value.Equals(tokenInfo.Language))
                       .Select(i => i.Key).DefaultIfEmpty<string>("en-Us").FirstOrDefault();
            if (IsAuthenticated(currentContext, userSession, licenseeSession) &&
                IsSameSession(userSession, licenseeSession, tokenInfo, licensee.Id))
            {
                UpdateUserSession(userSession, tokenInfo, licensee.Id, userInfo, language);
                return new ResultInfo
                {
                    ExtendedData = tokenInfo.ActionLink
                };
            }

            CreateSession(language, tokenInfo, licensee, userInfo);
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

        protected internal virtual IResultInfo ValidateCurrencyList(OneAuthUserInfo userInfo)
        {
            if (userInfo.CurrencyUserList == null || !userInfo.CurrencyUserList.Any())
            {
                return userInfo.UserLevel == UserLevelType.Subsidiary ?
                    new ResultInfo
                    {
                        Code = OneAuthorizationErrorCode.SubsidiaryNotHaveCurrency,
                        Message = ViewResources.SubsidiaryNotHaveCurrencyMsg
                    }
                    : new ResultInfo
                    {
                        Code = OneAuthorizationErrorCode.UserNotHaveCurrency,
                        Message = ViewResources.UserNotHaveCurrency
                    };
            }

            foreach (var currency in userInfo.CurrencyUserList)
            {
                var duplicateAgents = currency.AgentIdList.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
                if (duplicateAgents.Any())
                {
                    _logger.Error($"Duplicate agentid in currency permissions: currency ({currency.CurrencyId}-{currency.CurrencyName})" +
                        $", duplicate agents ({string.Join(",", duplicateAgents)})");

                    return new ResultInfo
                    {
                        Code = OneAuthorizationErrorCode.InvalidCurrencyPermission,
                        Message = ViewResources.InvalidCurrencyPermission
                    };
                }
            }

            return new ResultInfo { Code = 0 };
        }

        private void UpdateUserSession(UserSession userSession, OneAuthToken tokenInfo, int licenseeId, OneAuthUserInfo userInfo, string language)
        {
            if (!language.Equals(userSession.Language))
            {
                _sessionService.UpdateLanguage(language);
            }

            _sessionService.AssignCurrenciesToUserSession(userSession, licenseeId, userInfo.CurrencyUserList);
            userSession.UserLevel = userInfo.UserLevel;
            userSession.IsManageableSubsidiary = userInfo.IsEnabledSubsidiary && userInfo.UserLevel!= UserLevelType.Subsidiary;
            userSession.OperatorId = userInfo.OperatorID;
            userSession.PortalFunctionList = GetPermissionList(tokenInfo);
        }

        private OneAuthUserInfo GetUserInfo(OneAuthToken tokenInfo)
        {
            var userInfo = _oneAuthDataAccess.GetUserInfo(tokenInfo.UserSEQ, tokenInfo.VendorID);

            if (userInfo.UserLevel == UserLevelType.Subsidiary)
            {
                if (tokenInfo.Currency != null)
                {
                    userInfo.CurrencyUserList = userInfo.CurrencyUserList.Join(
                        tokenInfo.Currency,
                        currencyUser => currencyUser.CurrencyId,
                        oneAuthCurrency => oneAuthCurrency.CurrencyId,
                        (currencyUser, oneAuthCurrency) => new CurrencyUser
                        {
                            IsActive = true,
                            CurrencyId = currencyUser.CurrencyId,
                            CurrencyName = oneAuthCurrency.Currency,
                            AgentIdList = currencyUser.AgentIdList
                        }).ToList();
                }
            }
            else
            {
                userInfo.CurrencyUserList = tokenInfo.Currency?.Select(x => new CurrencyUser
                {
                    IsActive = true,
                    CurrencyId = x.CurrencyId,
                    CurrencyName = x.Currency,
                    AgentIdList = x.AgentId
                }).ToList();
            }

            return userInfo;
        }

        private void CreateSession(
            string languageCode,
            OneAuthToken tokenInfo,
            Licensee licenseeInfo,
            OneAuthUserInfo userInfo)
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
                UserLevel = userInfo.UserLevel,
                IsManageableSubsidiary = userInfo.IsEnabledSubsidiary && userInfo.UserLevel != UserLevelType.Subsidiary,
                OperatorId = userInfo.OperatorID,
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

            _sessionService.AssignCurrenciesToUserSession(userSession, licenseeInfo.Id, userInfo.CurrencyUserList);

            _sessionService.AssignSessions(userSession, licSession);
        }

        private IList<string> GetPermissionList(OneAuthToken tokenInfo)
        {
            return tokenInfo.IsAdminstrator ? GetFunctionList() : tokenInfo.Permissions.Select(x => x.FunctionKey).ToList();
        }

        /// <summary>
        /// This function will be removed when integrating permission part with LAP.
        /// </summary>
        /// <returns>Return the list portal function.</returns>
        private IList<string> GetFunctionList()
        {
            const string FunctionFilePath = "~/App_Data/PortalSystemFunction.json";
            IList<PortalSystemFunction> result = null;
            var temp = GetStringFromFile(FunctionFilePath);

            if (!string.IsNullOrEmpty(temp))
            {
                result = JsonConvert.DeserializeObject<IList<PortalSystemFunction>>(temp);
            }

            return (result ?? new List<PortalSystemFunction>()).Select(x => x.FunctionKey).ToList();
        }
    }
}