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
    using System.Web.Mvc;
    using DataAccess.Core.Common;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Account;
    using ONEbook.UM.Filters;
    using Resources;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using Web.ActionFilter;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.AccountManageAccount)]
    [AuthorizeRestriction(AppMode.Self)]
    public class AccountController : BaseController
    {
        private readonly IFunctionService _functionService;
        private readonly IUserService _userService;
        private readonly ICurrencyDataAccess _currencyDataAccess;

        private IEnumerable<Function> _allMasterFunctionList;

        private IDictionary<int, IEnumerable<Function>> _allFunctionMap;

        private IEnumerable<Function> _allUserFunctionList;

        public AccountController()
        {
            _functionService = new FunctionService();
            _userService = new UserService();
            _currencyDataAccess = new CurrencyDataAccess();
        }

        public AccountController(
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

        #region Actions

        public ActionResult Manage()
        {
            var userSession = SessionService.UserSession;
            var licenseeSession = SessionService.LicenseeSession;
            var model = _userService.GetByLicensee(licenseeSession.LicenseeId, userSession.IsInternal)
                .Where(i => i.UserID != userSession.UserId).ToList();

            return View("~/Views/Account/Manage.cshtml", model);
        }

        #region Edit Account

        /// <summary>
        /// Internal user only allow editing internal users.
        /// And licensee admin only allow editing licensee user.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult EditAccount()
        {
            var userSession = SessionService.UserSession;
            int userId;

            if (int.TryParse(Request.Params["UserId"], out userId) && userId > 0 && userId != userSession.UserId)
            {
                var user = _userService.GetUserInfo(SessionService.LicenseeSession.LicenseeId, userId);

                if (user != null && user.IsInternal == userSession.IsInternal)
                {
                    var model = new EditAccountMetaModel
                    {
                        UserName = user.Username,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Status = user.Status,
                        UserId = user.UserID
                    };

                    return View("~/Views/Account/EditAccount.cshtml", model);
                }
            }

            return View("~/Views/Account/EditAccount.cshtml", null);
        }

        /// <summary>
        /// Edit account.
        /// </summary>
        /// <param name="model">The account metal model.</param>
        /// <returns>The JSON result.</returns>
        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult EditAccount(EditAccountMetaModel model)
        {
            if (ModelState.IsValid)
            {
                var userSession = SessionService.UserSession;
                var licenseeId = SessionService.LicenseeSession.LicenseeId;
                var user = _userService.GetUserInfo(licenseeId, model.UserId);

                if (user != null && user.IsInternal == userSession.IsInternal)
                {
                    // set info
                    user.FirstName = string.IsNullOrEmpty(model.FirstName) ? string.Empty : model.FirstName.Trim();
                    user.LastName = string.IsNullOrEmpty(model.LastName) ? string.Empty : model.LastName.Trim();
                    user.Status = model.Status;
                    user.Request = new Request
                    {
                        Ip = Request.UserHostAddress,
                        Language = userSession.Language
                    };
                    user.Licensee = new Licensee { Id = licenseeId };

                    // proceed updating
                    var resultInfo = _userService.UpdateInfo(user, userSession.UserId);
                    if (resultInfo.IsSuccessful)
                    {
                        resultInfo.Message = ViewResources.UpdateAccountSuccessMsg;
                    }

                    return Json(resultInfo);
                }
            }

            return Json(new ResultInfo
            {
                Code = -1,
                Message = ViewResources.InvalidRequestMsg
            });
        }

        #endregion Edit Account

        #region Reset Password

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        public ActionResult ResetPassword()
        {
            var model = new ResetPasswordMetaModel
            {
                UserId = int.Parse(this.Request.Params["UserId"]),
                UserName = this.Request.Params["UserName"],
                FirstName = this.Request.Params["FirstName"],
                LastName = this.Request.Params["LastName"]
            };

            return View("~/Views/Account/ResetPassword.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public JsonResult ResetPassword(ResetPasswordMetaModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new ResultInfo
                {
                    Code = -1,
                    Message = ViewResources.InvalidRequestMsg
                });
            }

            var userSession = SessionService.UserSession;
            var user = _userService.GetUserInfo(SessionService.LicenseeSession.LicenseeId, model.UserId);

            if (user == null || user.IsInternal != userSession.IsInternal)
            {
                return Json(new ResultInfo
                {
                    Code = -4,
                    Message = ViewResources.InvalidRequestMsg
                });
            }

            if (!Util.IsPasswordValid(model.Password))
            {
                return Json(new ResultInfo
                {
                    Code = -2,
                    Message = ViewResources.PasswordNoteMsg
                });
            }

            user.Request = new Request
            {
                Ip = Request.UserHostAddress,
                Language = userSession.Language
            };
            user.Password = model.Password;

            var resultInfo = _userService.ResetPassword(user, userSession.UserId);

            return Json(new ResultInfo
            {
                Code = resultInfo.IsSuccessful ? 0 : -3,
                Message = resultInfo.IsSuccessful ? ViewResources.ResetPasswordSuccessMsg : resultInfo.Message
            });
        }

        #endregion Reset Password

        #region Edit Permission

        [HttpGet]
        public ActionResult PermissionPage()
        {
            int userId;
            if (int.TryParse(Request.Params["UserId"], out userId) && ValidatePermissionRestriction(userId))
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
        public ActionResult EditPermission()
        {
            int userId;
            if (int.TryParse(Request.Params["UserId"], out userId) && userId > 0 && ValidatePermissionRestriction(userId))
            {
                var licenseeSession = SessionService.LicenseeSession;
                var user = _userService.GetUserInfo(licenseeSession.LicenseeId, userId);

                if (user != null)
                {
                    InitializeFunctions(userId);

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
                return Json(new ResultInfo
                {
                    Code = -1,
                    Message = ViewResources.FunctionDeniedMsg
                });
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

            return Json(new ResultInfo
            {
                Code = -2,
                Message = ViewResources.InvalidRequestMsg
            });
        }

        #endregion Edit Permission

        #region Assign Currency

        [HttpGet]
        public ActionResult AssignCurrency(int userId)
        {
            if (!AppSettings.EnableAssignCurrency)
            {
                return new EmptyResult();
            }

            if (userId > 0)
            {
                var currencies = _currencyDataAccess.GetCurrenciesByUser(SessionService.LicenseeSession.LicenseeId, userId);
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
        public ActionResult AssignCurrency(AssignCurrencyMetaModel model)
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
            var currencyDict = _currencyDataAccess.GetCurrenciesByUser(licenseeId, userSession.UserId);
            model.CurrencyList = BuildCurrencyListXmlString(currencyDict, model.CurrencyList.Split(',').ToList());

            var result = _currencyDataAccess.AssignCurrenciesByUser(model);

            if (result.IsSuccessful)
            {
                result.Message = ViewResources.UpdateCurrencySuccessfulMsg;
            }

            return Json(result);
        }

        #endregion Assign Currency

        #endregion Actions

        #region Helpers

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

        /// <summary>
        /// Get all functions of user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        private void InitializeFunctions(int userId)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;

            // get all functions of licensee
            var allLicenseeFunctionList = _functionService.GetLicenseeFunctionList(licenseeId);

            // get all master functions
            _allMasterFunctionList = _functionService.GetMasterFunctions(allLicenseeFunctionList);

            // map each function with its sub functions
            _allFunctionMap = _functionService.GetSubFunctions(allLicenseeFunctionList);

            // get all function of editing user
            _allUserFunctionList = _functionService.GetUserFunctionList(licenseeId, userId);
        }

        #endregion Helpers
    }
}