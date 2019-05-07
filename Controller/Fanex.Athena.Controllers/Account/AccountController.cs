// -----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Account
{
    using System.Linq;
    using System.Web.Mvc;
    using Extension.Utility;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Account;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;

    public class AccountController : AccountBaseController
    {
        private readonly IUserService _userService;

        public AccountController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSettings,
            IUserService userService)
        : base(languageService, sessionService, appSettings)
        {
            _userService = userService;
        }

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
            int userId = GetUserId();

            if (userId > 0 && userId != userSession.UserId)
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

            return JsonErrorResult(ViewResources.InvalidRequestMsg);
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
                UserId = GetUserId(),
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
                return JsonErrorResult(ViewResources.InvalidRequestMsg);
            }

            var userSession = SessionService.UserSession;
            var user = _userService.GetUserInfo(SessionService.LicenseeSession.LicenseeId, model.UserId);

            if (user == null || user.IsInternal != userSession.IsInternal)
            {
                return JsonErrorResult(ViewResources.InvalidRequestMsg);
            }

            if (!Util.IsPasswordValid(model.Password))
            {
                return JsonErrorResult(ViewResources.PasswordNoteMsg);
            }

            user.Request = new Request
            {
                Ip = Request.UserHostAddress,
                Language = userSession.Language
            };
            user.Password = model.Password;

            var resultInfo = _userService.ResetPassword(user, userSession.UserId);
            const int ErrorCode = -3;

            return Json(new ResultInfo
            {
                Code = resultInfo.IsSuccessful ? 0 : ErrorCode,
                Message = resultInfo.IsSuccessful ? ViewResources.ResetPasswordSuccessMsg : resultInfo.Message
            });
        }

        #endregion Reset Password
    }
}