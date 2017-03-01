namespace Fanex.Athena.Service.Core.Users
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Hosting;
    using Common;
    using Common.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Caching;
    using Model.Core.DataDomain;
    using Model.Core.DTO;
    using Model.Core.DTO.Users;
    using Newtonsoft.Json;
    using Resources;
    using User.Common;
    using Utility;

    public class OneAuthenticationService : IOneAuthenticationService
    {
        private readonly ISessionService _sessionService;
        private readonly ILicenseeService _licenseeService;
        private readonly ICurrencyService _currencyService;
        private readonly IUserService _userService;
        private readonly IFormsAuthenticationService _formsAuthenticationService;

        public OneAuthenticationService() : this(null, null, null, null, null)
        {
        }

        public OneAuthenticationService(
            ISessionService sessionService,
            ILicenseeService licenseeService,
            ICurrencyService currencyService,
            IFormsAuthenticationService formsAuthenticationService,
            IUserService userService)
        {
            _sessionService = sessionService ?? new SessionService();
            _licenseeService = licenseeService ?? new LicenseeService();
            _currencyService = currencyService ?? new CurrencyService();
            _formsAuthenticationService = formsAuthenticationService ?? new FormsAuthenticationService();
            _userService = userService ?? new UserService();
        }

        protected internal virtual string GetStringFromFile(string path)
        {
            using (var reader = new StreamReader(HostingEnvironment.MapPath(path)))
            {
                return reader.ReadToEnd();
            }
        }

        public IResultInfo SignIn(HttpContextBase currentContext, OneAuthenticationToken tokenInfo)
        {
            var licensee = _licenseeService.GetLicenseeByName(tokenInfo.SiteName);
            if (licensee == null)
            {
                return new ResultInfo
                {
                    Code = OneAuthorizationErrorCode.LicenseeIsNotExisted,
                    Message = ViewResources.LicenseeIsNotExisted
                };
            }

            var userSession = _sessionService.UserSession;
            var licenseeSession = _sessionService.LicenseeSession;
            var language = Util.MapLanguages.Where(i => i.Value.Equals(tokenInfo.Language))
                       .Select(i => i.Key).DefaultIfEmpty<string>("en-Us").FirstOrDefault();
            if (IsAuthenticated(currentContext, userSession, licenseeSession))
            {
                var isSameSession = VerifySessionWithToken(userSession, licenseeSession, tokenInfo, licensee.Id);
                if (isSameSession)
                {
                    if (!language.Equals(userSession.Language))
                    {
                        _sessionService.UpdateLanguage(language);
                    }

                    return new ResultInfo
                    {
                        ExtendedData = tokenInfo.ActionLink
                    };
                }
            }

            var userCheckingResult = CheckAndSyncUserInfo(licensee, tokenInfo.UserID);

            if (!userCheckingResult.IsSuccessful)
            {
                return userCheckingResult;
            }

            tokenInfo.UserSEQ = int.Parse(userCheckingResult.ExtendedData);
            return SetAuthorize(licensee, language, tokenInfo);
        }

        public OneAuthenticationToken GetTokenInfo(string token)
        {
            var oneBookService = new OneBookSecurityWrapper();
            return oneBookService.Authenticate(token);
        }

        private static bool VerifySessionWithToken(UserSession userSession, LicenseeSession licenseeSession, OneAuthenticationToken tokenInfo, int licenseeId)
        {
            return userSession.UserName == tokenInfo.UserID
                && userSession.VendorID == tokenInfo.VendorID && userSession.VendorIdentityID == tokenInfo.UserID
                && licenseeSession.LicenseeId == licenseeId;
        }

        private static bool IsAuthenticated(HttpContextBase currentContext, UserSession userSession, LicenseeSession licenseeSession)
        {
            var identity = currentContext.User.Identity;
            var identityValidation = identity != null && identity.IsAuthenticated && !string.IsNullOrEmpty(identity.Name);

            var sessionValidation = userSession != null && licenseeSession != null && userSession.UserId > 0 &&
                !string.IsNullOrEmpty(userSession.VendorID) && !string.IsNullOrEmpty(userSession.VendorIdentityID)
                && userSession.IsInternal && licenseeSession.LicenseeId > 0 && userSession.ApplicationMode == AppMode.Parasitic;

            return identityValidation && sessionValidation;
        }

        [CLSCompliant(false)]
        protected internal virtual ICache GetMemCacheService() => Caching.CacheService.CreateInstance(CacheType.MemoryCache);

        protected internal virtual IResultInfo SetAuthorize(Licensee licensee, string languageCode, OneAuthenticationToken tokenInfo)
        {
            var currencyOfLicensee = _currencyService.GetCurrenciesByUser(licensee.Id, tokenInfo.UserSEQ);

            if (currencyOfLicensee.Count() <= 0)
            {
                return new ResultInfo
                {
                    Code = OneAuthorizationErrorCode.LicenseeNotOfferedCurrency,
                    Message = ViewResources.LicenseeNotOfferedCurrency
                };
            }

            CreateSession(languageCode, tokenInfo, licensee, currencyOfLicensee);
            var authName = $"{tokenInfo.VendorID}_{tokenInfo.SiteName}_{tokenInfo.UserID}";
            _formsAuthenticationService.SetAuthCookie(authName, false);

            return new ResultInfo
            {
                ExtendedData = tokenInfo.ActionLink
            };
        }

        /// <summary>
        /// This function will be removed when integrating permission part with LAP.
        /// </summary>
        /// <param name="userSession">The currency user session.</param>
        /// <param name="currencyUserList">The currency of user .</param>
        /// <param name="licenseeId">The licensee ID.</param>
        private static void AssignCurrenciesToUserSession(UserSession userSession, IEnumerable<CurrencyUser> currencyUserList, int licenseeId)
        {
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

        private IResultInfo CheckAndSyncUserInfo(Licensee licensee, string userName)
        {
            var result = GetUser(licensee.Id, userName);
            if (result.IsSuccessful)
            {
                return result;
            }

            result = _userService.SyncUserInfo(licensee.Name, userName);
            return result.IsSuccessful ? GetUser(licensee.Id, userName) : new ResultInfo
            {
                Code = OneAuthorizationErrorCode.NotFoundUser,
                Message = ViewResources.UserNotExist
            };
        }

        private IResultInfo GetUser(int licenseeId, string userName)
        {
            var user = _userService.GetByUsername(userName, licenseeId);
            if (user != null)
            {
                return new ResultInfo
                {
                    ExtendedData = user.UserID.ToString()
                };
            }

            return new ResultInfo
            {
                Code = OneAuthorizationErrorCode.NotFoundUser,
                Message = ViewResources.UserNotExist
            };
        }

        private void CreateSession(
            string languageCode,
            OneAuthenticationToken tokenInfo,
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

            var userSerssion = new UserSession
            {
                UserId = tokenInfo.UserSEQ,
                OnUserId = tokenInfo.UserSEQ,
                UserName = tokenInfo.UserID,
                IsInternal = true,
                FunctionIdList = new List<int>(),
                PortalFunctionList = GetFunctionList(),
                Language = languageCode,
                DefaultReportUrl = "BalanceReport",
                ApplicationMode = AppMode.Parasitic,
                VendorID = tokenInfo.VendorID,
                VendorIdentityID = tokenInfo.UserID,
                ViewType = ClientViewType.Desktop,
            };

            AssignCurrenciesToUserSession(userSerssion, currencyByUser, licenseeInfo.Id);

            _sessionService.CreateNewSession(userSerssion, licSession);
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

            return result == null ? new List<PortalSystemFunction>() : result;
        }
    }
}