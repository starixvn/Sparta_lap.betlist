// -----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Account
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Account;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Users;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class AccountController : BaseController
    {
        private const string ParentUrl = "Account/Manage";

        #region Fields

        private readonly ILicenseeService _licenseeService;
        private readonly IFunctionService _functionService;
        private readonly IUserService _userService;

        private IList<Function> _allMasterFunctionList;

        private IDictionary<int, IList<Function>> _allFunctionMap;

        private IList<Function> _allUserFunctionList;

        #endregion Fields

        #region Constructor

        public AccountController()
        {
            this._licenseeService = new LicenseeService();
            this._functionService = new FunctionService();
            this._userService = new UserService();
        }

        #endregion Constructor

        #region Actions

        /// <summary>
        /// Change licensee.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        /// <returns>The action result.</returns>
        public ActionResult ChangeLicensee(int? licenseeId)
        {
            var newLicenseeId = licenseeId.HasValue ? licenseeId.Value : 0;
            var originalLicenseeId = SessionService.LicenseeSession.LicenseeId;
            var originalUserId = SessionService.UserSession.UserId;

            if (newLicenseeId > 0)
            {
                newLicenseeId = _licenseeService.IsLicenseeBelongToUser(originalLicenseeId, originalUserId)
                                    ? newLicenseeId
                                    : originalLicenseeId;
            }
            else
            {
                newLicenseeId = originalLicenseeId;
            }

            if (newLicenseeId != originalLicenseeId)
            {
                SessionService.UpdateLiceseeSession(newLicenseeId);
            }

            return View("~/Views/Account/ChangeLicensee.cshtml");
        }

        /// <summary>
        /// Manage users.
        /// </summary>
        /// <returns>The action result.</returns>
        [FunctionAuthorization]
        public ActionResult Manage()
        {
            var userSession = SessionService.UserSession;
            var licenseeSession = SessionService.LicenseeSession;
            var model = _userService.GetByLicensee(licenseeSession.LicenseeId, userSession.IsInternal)
                .Where(i => i.Id != userSession.UserId).ToList();
            ViewBag.PageSession = userSession;

            return View("~/Views/Account/Manage.cshtml", model);
        }

        #region Edit Account

        /// <summary>
        /// Internal user only allow editing internal users.
        /// And licensee admin only allow editing licensee user.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization(ParentUrl = ParentUrl)]
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
                        UserId = user.Id
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
        [FunctionAuthorization(ParentUrl = ParentUrl)]
        [ValidateAntiForgeryToken]
        public JsonResult EditAccount(EditAccountMetaModel model)
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
                IResultInfo resultInfo = _userService.UpdateInfo(user, userSession.UserId);
                if (resultInfo.IsSuccessful)
                {
                    resultInfo.Message = ViewResources.UpdateAccountSuccessMsg;
                }

                return Json(resultInfo);
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
        [FunctionAuthorization(ParentUrl = ParentUrl)]
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

        /// <summary>
        /// Reset password.
        /// </summary>
        /// <param name="model">The account metal model.</param>
        /// <returns>The JSON result.</returns>
        [SuppressMessage(
          "StyleCop.CSharp.Nexcel.NexcelCustomRules",
          "SP2101:MethodMustNotContainMoreLinesThan",
          Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [FunctionAuthorization(ParentUrl = ParentUrl)]
        [ValidateAntiForgeryToken]
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

            if (user != null && user.IsInternal == userSession.IsInternal)
            {
                user.Request = new Request
                {
                    Ip = Request.UserHostAddress,
                    Language = userSession.Language
                };

                var validPassword = ValidatePassword(model.Password);
                user.Password = model.Password;

                if (!validPassword)
                {
                    return Json(new ResultInfo
                    {
                        Code = -2,
                        Message = ViewResources.PasswordNoteMsg
                    });
                }

                var resultInfo = _userService.ResetPassword(user, userSession.UserId);

                if (resultInfo.IsSuccessful)
                {
                    return Json(new ResultInfo
                    {
                        Code = 0,
                        Message = ViewResources.ResetPasswordSuccessMsg
                    });
                }

                return this.Json(new ResultInfo
                {
                    Code = -3,
                    Message = resultInfo.Message
                });
            }

            return Json(new ResultInfo
            {
                Code = -4,
                Message = ViewResources.InvalidRequestMsg
            });
        }

        #endregion Reset Password

        #region Edit Permission

        /// <summary>
        /// Permission page.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization(ParentUrl = ParentUrl)]
        public ActionResult PermissionPage()
        {
            int userId; // user is being editted
            if (int.TryParse(Request.Params["UserId"], out userId) && ValidatePermissionRestriction(userId))
            {
                var user = _userService.GetUserInfo(SessionService.LicenseeSession.LicenseeId, userId);

                if (user != null)
                {
                    ViewBag.EnableAssignCurrency = AppSettings.EnableAssignCurrency;

                    return View("~/Views/Account/PermissionPage.cshtml", user);
                }
            }

            return this.Redirect("~/Utility/AccessDenied");
        }

        /// <summary>
        /// Edit permission.
        /// </summary>
        /// <returns>The action result.</returns>
        [HttpGet]
        [FunctionAuthorization(ParentUrl = ParentUrl)]
        public ActionResult EditPermission()
        {
            int userId; // user is being editted
            if (int.TryParse(Request.Params["UserId"], out userId) && userId > 0 && ValidatePermissionRestriction(userId))
            {
                var licenseeSession = SessionService.LicenseeSession;
                var user = _userService.GetUserInfo(licenseeSession.LicenseeId, userId);

                if (user != null)
                {
                    InitializeFunctions(userId);
                    ViewBag.AllMasterFunctions = _allMasterFunctionList;
                    ViewBag.AllFunctionMap = _allFunctionMap;
                    ViewBag.AllUserFunctions = _allUserFunctionList;
                    ViewBag.UserName = user.Username;

                    var model = new EditPermissionMetaModel
                    {
                        UserId = userId,
                        FunctionIdList = string.Empty
                    };

                    return View("~/Views/Account/EditPermission.cshtml", model);
                }
            }

            return this.Redirect("~/Utility/AccessDenied");
        }

        /// <summary>
        /// Edit permission.
        /// </summary>
        /// <param name="model">The permission metal model.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [FunctionAuthorization(ParentUrl = ParentUrl)]
        [ValidateAntiForgeryToken]
        public ActionResult EditPermission(EditPermissionMetaModel model)
        {
            bool isValid = ValidatePermissionRestriction(model.UserId);

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

            if (model.UserId > 0)
            {
                var licenseeId = licenseeSession.LicenseeId;
                var userLoginId = userSession.UserId;
                var ip = Request.UserHostAddress;
                var langKey = LangService.GetCurrentLanguage();

                if (!string.IsNullOrEmpty(model.FunctionIdList))
                {
                    var idlist = ParseToIntList(model.FunctionIdList);
                    var result = _functionService.AssignUserFunctions(licenseeId, model.UserId, idlist, userLoginId, ip, langKey);

                    return Json(result);
                }
            }

            return Json(new ResultInfo
            {
                Code = -2,
                Message = ViewResources.InvalidRequestMsg
            });
        }

        #endregion Edit Permission

        #region Restrict Currency

        /// <summary>
        /// Assign currency.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [FunctionAuthorization(ParentUrl = ParentUrl)]
        public ActionResult AssignCurrency(int userId = 0)
        {
            if (!AppSettings.EnableAssignCurrency)
            {
                return new EmptyResult();
            }

            if (userId > 0)
            {
                var currencies = CurrencyService.GetCurrenciesByUser(SessionService.LicenseeSession.LicenseeId, userId);
                if (SessionService.LicenseeSession.LicenseeId == Licensees.KeenOcean)
                {
                    var currency = currencies.Where(x => x.CurrencyId == CurrencyType.Hkd).FirstOrDefault();
                    if (currency != null)
                    {
                        currency.CurrencyName = "HKD";
                    }
                }

                ViewBag.CurrencyList = currencies;
                return View("~/Views/Account/AssignCurrency.cshtml");
            }

            return Redirect("~/Utility/AccessDenied");
        }

        /// <summary>
        /// Assign Currencies to user.
        /// </summary>
        /// <param name="model">The currency metal model.</param>
        /// <returns>The action result.</returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [FunctionAuthorization(ParentUrl = ParentUrl)]
        [ValidateAntiForgeryToken]
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

            if (AppSettings.EnableAssignCurrency == false || ValidatePermissionRestriction(model.UserId) == false)
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
            var currencyDict = CurrencyService.GetCurrenciesByUser(licenseeId, userSession.UserId);
            var xmlPattern = "<currencies>{0}</currencies>";
            var xmlElement = "<currency currencyid=\"{0}\" status=\"{1}\"></currency>";
            var xmlElementBuilder = new StringBuilder();
            var selectedCurrencyList = model.CurrencyList.Split(',');

            foreach (var currency in currencyDict)
            {
                xmlElementBuilder.Append(string.Format(
                    xmlElement,
                    currency.CurrencyId,
                    selectedCurrencyList.Contains(currency.CurrencyId.ToString(CultureInfo.InvariantCulture)) ? 1 : 0));
            }

            model.CurrencyList = string.Format(xmlPattern, xmlElementBuilder);

            var result = CurrencyService.AssignCurrenciesByUser(model);

            if (result.IsSuccessful)
            {
                result.Message = ViewResources.UpdateCurrencySuccessfulMsg;
            }

            return Json(result);
        }

        #endregion Restrict Currency

        #endregion Actions

        #region Validations

        /// <summary>
        /// Validate password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>True or false.</returns>
        [SuppressMessage(
          "StyleCop.CSharp.Nexcel.NexcelCustomRules",
          "SP2101:MethodMustNotContainMoreLinesThan",
          Justification = "Reviewed. Suppression is OK here.")]
        private static bool ValidatePassword(string password)
        {
            password = password.ToLower();
            if (password.Length < 6)
            {
                return false;
            }

            var bvalid = false;
            var j3 = -1;
            for (var i = 0; i < 10; i++)
            {
                var j1 = password.IndexOf(i.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal);
                var j2 = password.LastIndexOf(i.ToString(CultureInfo.InvariantCulture), StringComparison.Ordinal);
                if (j1 != j2)
                {
                    bvalid = true;
                    break;
                }

                if (j3 == -1)
                {
                    j3 = j1;
                    continue;
                }

                if (j3 == -1 || j3 == j1)
                {
                    continue;
                }

                bvalid = true;
                break;
            }

            if (bvalid)
            {
                const string Az = "abcdefghijklmnopqrstuvwxyz";
                var count = 0;
                for (var i = 0; i < password.Length; i++)
                {
                    if (Az.Contains(password.Substring(i, 1)))
                    {
                        ++count;
                    }

                    if (count != 2)
                    {
                        continue;
                    }

                    break;
                }

                if (count < 2)
                {
                    bvalid = false;
                }
            }

            return bvalid;
        }

        #endregion Validations

        #region Helpers

        /// <summary>
        /// Parse a comma separated string into  IList.
        /// </summary>
        /// <param name="values">The function string.</param>
        /// <returns>The list id of functions.</returns>
        private static IList<int> ParseToIntList(string values)
        {
            var result = new List<int>();
            if (!string.IsNullOrEmpty(values))
            {
                var words = values.Split(',');
                result.AddRange(words.Select(word => Convert.ToInt32(word)));
            }

            return result;
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