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
            //// CheckAndSyncCurrency(licensee.Id, tokenInfo, currentContext.Request.UserHostAddress);
            return SetAuthorize(licensee, language, tokenInfo);
        }

        /*
        private void CheckAndSyncCurrency(int licenseeId, OneAuthenticationToken tokenInfo, string ip)
        {
             * var currentCurrencyRestriction = _currencyService.GetCurrenciesByUser(licenseeId, tokenInfo.UserSEQ);
             var currencyRestriction = currentCurrencyRestriction;// new List<CurrencyUser>();//get from token

             var result = from t in currencyRestriction
                          from c in currentCurrencyRestriction
                          where t.CurrencyId == c.CurrencyId
                          select c;
             if (result.Count() != currentCurrencyRestriction.Count)
             {
                 // convert currencies into xml
                 const string XmlPattern = "<currencies>{0}</currencies>";
                 const string XmlElementPattern = "<currency currencyid=\"{0}\" status=\"{1}\"></currency>";
                 var xmlElementBuilder = new StringBuilder();

                 foreach (var currency in currentCurrencyRestriction)
                 {
                     xmlElementBuilder.Append(string.Format(
                         XmlElementPattern,
                         currency.CurrencyId,
                         currencyRestriction.Any(i => i.CurrencyId == currency.CurrencyId) ? 1 : 0));
                 }

                 _currencyService.AssignCurrenciesByUser(new Model.Core.MetaModel.Account.AssignCurrencyMetaModel
                 {
                     LicenseeId = licenseeId,
                     CurrencyList = string.Format(XmlPattern, xmlElementBuilder),
                     ModifyBy = tokenInfo.UserSEQ,
                     IP = ip
                 });
             }
        }
        */

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
            var currencyOfLicensee = _currencyService.GetCurrencyMapByUser(licensee.Id, tokenInfo.UserSEQ);

            if (currencyOfLicensee.Count <= 0)
            {
                return new ResultInfo
                {
                    Code = OneAuthorizationErrorCode.LicenseeNotOfferedCurrency,
                    Message = ViewResources.LicenseeNotOfferedCurrency
                };
            }

            var agentList = _licenseeService.GetCustByLicensee(licensee.Id);
            if (agentList.Count <= 0)
            {
                return new ResultInfo
                {
                    Code = OneAuthorizationErrorCode.LicenseeIsNotExisted,
                    Message = ViewResources.LicenseeIsNotExisted
                };
            }

            CreateSession(languageCode, tokenInfo, licensee, currencyOfLicensee, agentList);
            var authName = $"{tokenInfo.VendorID}_{tokenInfo.SiteName}_{tokenInfo.UserID}";
            _formsAuthenticationService.SetAuthCookie(authName, false);

            return new ResultInfo
            {
                ExtendedData = tokenInfo.ActionLink
            };
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
            IDictionary<string, int> currencyOfUser,
            IList<LicenseeCustomer> agentList)
        {
            var licSession = new LicenseeSession
            {
                LicenseeId = licenseeInfo.Id,
                LicenseeType = agentList
                                        .Where(s => !string.IsNullOrEmpty(s.Description))
                                        .Select(s => s.Description)
                                        .Distinct()
                                        .ToList(),
                CurrencyName = agentList.Select(s => s.CurrencyName).Distinct().ToList(),
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
                CurrencyList = currencyOfUser,
                DefaultReportUrl = "BalanceReport",
                ApplicationMode = AppMode.Parasitic,
                VendorID = tokenInfo.VendorID,
                VendorIdentityID = tokenInfo.UserID,
                ViewType = ClientViewType.Desktop,
                HostDomain = tokenInfo.HostDomain
            };

            _sessionService.CreateNewSession(userSerssion, licSession);
        }

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