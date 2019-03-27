// -----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Account
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Account;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using Web.ActionFilter;

    public class AccountPermissionController : AccountBaseController
    {
        private readonly IFunctionService _functionService;
        private readonly IUserService _userService;
        private readonly ICurrencyDataAccess _currencyDataAccess;

        private IEnumerable<Function> _allMasterFunctionList;

        private IDictionary<int, IEnumerable<Function>> _allFunctionMap;

        private IEnumerable<Function> _allUserFunctionList;

        public AccountPermissionController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IFunctionService functionService,
            IUserService userService,
            ICurrencyDataAccess currencyDataAccess)
        : base(languageService, sessionService, appSetting)
        {
            _functionService = functionService;
            _userService = userService;
            _currencyDataAccess = currencyDataAccess;
        }

        #region Edit Permission

        [HttpGet]
        public ActionResult PermissionPage()
        {
            var userId = GetUserId();
            if (userId > 0 && ValidatePermissionRestriction(userId))
            {
                var user = _userService.GetUserInfo(SessionService.LicenseeSession.LicenseeId, userId);

                if (user != null)
                {
                    ViewBag.EnableAssignCurrency = AppSettings.EnableAssignCurrency;

                    return View("~/Views/Account/PermissionPage.cshtml", user);
                }
            }

            return Redirect("~/Utility/AccessDenied");
        }

        [HttpGet]
        public async Task<ActionResult> EditPermission()
        {
            int userId = GetUserId();
            if (userId > 0 && ValidatePermissionRestriction(userId))
            {
                var licenseeSession = SessionService.LicenseeSession;
                var user = _userService.GetUserInfo(licenseeSession.LicenseeId, userId);

                if (user != null)
                {
                    await InitializeFunctions(userId).ConfigureAwait(false);

                    var model = new EditPermissionMetaModel
                    {
                        UserId = userId,
                        FunctionIdList = string.Empty,
                        AllMasterFunctions = _allMasterFunctionList,
                        AllFunctionMap = _allFunctionMap,
                        AllUserFunctions = _allUserFunctionList
                    };

                    return View("~/Views/Account/EditPermission.cshtml", model);
                }
            }

            return this.Redirect("~/Utility/AccessDenied");
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult EditPermission(EditPermissionMetaModel model)
        {
            var isValid = ValidatePermissionRestriction(model.UserId);

            if (!isValid)
            {
                return JsonErrorResult(ViewResources.FunctionDeniedMsg);
            }

            var userSession = SessionService.UserSession;
            var licenseeSession = SessionService.LicenseeSession;

            if (model.UserId > 0 && !string.IsNullOrEmpty(model.FunctionIdList))
            {
                var licenseeId = licenseeSession.LicenseeId;
                var userLoginId = userSession.UserId;
                var ip = Request.UserHostAddress;
                var langKey = LangService.GetCurrentLanguage();

                var idlist = model.FunctionIdList.Split(',').Select(word => int.Parse(word));
                var result = _functionService.AssignUserFunctions(licenseeId, model.UserId, idlist, userLoginId, ip, langKey);

                return Json(result);
            }

            return JsonErrorResult(ViewResources.InvalidRequestMsg);
        }

        #endregion Edit Permission

        #region Assign Currency

        [HttpGet]
        public async Task<ActionResult> AssignCurrency(int userId)
        {
            if (!AppSettings.EnableAssignCurrency)
            {
                return new EmptyResult();
            }

            if (userId > 0)
            {
                var currencies = await _currencyDataAccess.GetCurrenciesByUser(SessionService.LicenseeSession.LicenseeId, userId).ConfigureAwait(false);
                if (SessionService.LicenseeSession.LicenseeId == LicenseeIdentity.KeenOcean)
                {
                    KeanOceanBusinessHelper.KeepHKDCurrencyName(currencies);
                }

                ViewBag.CurrencyList = currencies;
                return View("~/Views/Account/AssignCurrency.cshtml");
            }

            return Redirect("~/Utility/AccessDenied");
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public async Task<ActionResult> AssignCurrency(AssignCurrencyMetaModel model)
        {
            if (model.UserId <= 0 || string.IsNullOrEmpty(model.CurrencyList))
            {
                return Json(new ResultInfo
                {
                    Code = -2,
                    Message = ViewResources.InvalidRequestMsg
                });
            }

            if (!AppSettings.EnableAssignCurrency || !ValidatePermissionRestriction(model.UserId))
            {
                return Json(new ResultInfo
                {
                    Code = -1,
                    Message = ViewResources.FunctionDeniedMsg
                });
            }

            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var userSession = SessionService.UserSession;
            model.LicenseeId = licenseeId;
            model.IP = Request.UserHostAddress;
            model.LangKey = LangService.GetCurrentLanguage();
            model.ModifyBy = userSession.UserId;

            // convert currencies into xml
            var currencyDict = await _currencyDataAccess.GetCurrenciesByUser(licenseeId, userSession.UserId).ConfigureAwait(false);
            model.CurrencyList = BuildCurrencyListXmlString(currencyDict, model.CurrencyList.Split(',').ToList());

            var result = _currencyDataAccess.AssignCurrenciesByUser(model);

            if (result.IsSuccessful)
            {
                result.Message = ViewResources.UpdateCurrencySuccessfulMsg;
            }

            return Json(result);
        }

        #endregion Assign Currency

        private static string BuildCurrencyListXmlString(IList<CurrencyUser> userCurrencyList, IList<string> selectedCurrencyList)
        {
            const string XmlPattern = "<currencies>{0}</currencies>";
            const string XmlElementPattern = "<currency currencyid=\"{0}\" status=\"{1}\"></currency>";
            var xmlElementBuilder = new StringBuilder();

            foreach (var currency in userCurrencyList)
            {
                xmlElementBuilder.Append(string.Format(
                    XmlElementPattern,
                    currency.CurrencyId,
                    selectedCurrencyList.Contains(currency.CurrencyId.ToString(CultureInfo.InvariantCulture)) ? 1 : 0));
            }

            return string.Format(XmlPattern, xmlElementBuilder);
        }

        /// <summary>
        /// Validate permission restriction.
        /// </summary>
        /// <param name="editingUserId">The user who edit permission.</param>
        /// <returns>True of false.</returns>
        private bool ValidatePermissionRestriction(int editingUserId)
        {
            var currentUser = SessionService.UserSession;

            if (currentUser.UserId == editingUserId)
            {
                return false;
            }

            // is admin user, allow to edit all other users.
            if (!currentUser.IsInternal)
            {
                return true;
            }

            // is normal user, only allow to edit normal other users
            var editingUser = _userService.GetUserInfo(SessionService.LicenseeSession.LicenseeId, editingUserId);

            if (editingUser == null || !editingUser.IsInternal)
            {
                return false;
            }

            return true;
        }

        private async Task InitializeFunctions(int userId)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;

            // get all functions of licensee
            var allLicenseeFunctionList = await _functionService.GetLicenseeFunctionList(licenseeId).ConfigureAwait(false);

            // get all master functions
            _allMasterFunctionList = await _functionService.GetMasterFunctions(allLicenseeFunctionList).ConfigureAwait(false);

            // map each function with its sub functions
            _allFunctionMap = await _functionService.GetSubFunctions(allLicenseeFunctionList).ConfigureAwait(false);

            // get all function of editing user
            _allUserFunctionList = await _functionService.GetUserFunctionList(licenseeId, userId).ConfigureAwait(false);
        }
    }
}