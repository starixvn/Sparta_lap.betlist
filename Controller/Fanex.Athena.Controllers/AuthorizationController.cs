// -----------------------------------------------------------------------
// <copyright file="AuthorizationController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;
    using System.Web.Security;
    using System.Web.UI;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Athena.Model.Core.MetaModel.Authorization;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using ONEbook.UM.Filters;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;
    using Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthorizationController : BaseController
    {
        #region Constant

        private const int InvalidCaptchaError = 101;
        private const int InvalidUserNameError = 102;
        private const int InvalidPasswordError = 103;

        #endregion Constant

        private readonly IUserService _userService;
        private readonly IMaintenanceService _maintenanceService;

        public AuthorizationController()
        {
            _userService = new UserService();
            _maintenanceService = new MaintenanceService();
        }

        /// <summary>
        /// The sign-in page.
        /// </summary>
        /// <returns>The sign in page.</returns>
        [CheckUMFilter]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignIn()
        {
            var requestHost = Request.Url.AbsoluteUri;

            var isInternalDomain = requestHost.Contains(AppSettings.GetValue("internaldomain"));

            if (!isInternalDomain)
            {
                return RedirectToAction("LocationRestriction", "Utility");
            }

            SetErrorForView(0, string.Empty);
            ViewBag.LogInToken = CreateToken();
            return View("~/Views/Authorization/SignIn.cshtml");
        }

        /// <summary>
        /// The sign in page.
        /// </summary>
        /// <param name="model">The sign in metal model.</param>
        /// <returns>The redirected action or sign in page in case of having a error .</returns>
        [CheckUMFilter]
        [AllowAnonymous]
        [HandleAntiForgeryError]
        [HttpPost]
        [CustomValidateAntiForgery]
        [SuppressMessage(
          "StyleCop.CSharp.StylecopPlus.Rules",
          "SP2101:MethodMustNotContainMoreLinesThan",
          Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult SignIn(LoginMetalModel model)
        {
            var isValid = true;

            var isAllowAutomationCaptchaCode = AppSettings.GetValue("AllowAutomationCaptchaCode") == "1";
            if (!isAllowAutomationCaptchaCode && model.Captcha != SessionService.CaptchaCode)
            {
                SessionService.CaptchaCode = CreateRandomCaptchaCode();
                SetErrorForView(InvalidCaptchaError, ViewResources.InvalidCaptchaMsg);
                isValid = false;
            }

            var secretCaptcha = AppSettings.GetValue("SecretCaptcha");
            var companyIP = AppSettings.GetValue("CompanyIP");
            if (!isValid && !string.IsNullOrWhiteSpace(secretCaptcha) && model.Captcha == secretCaptcha &&
                 Regex.IsMatch(Request.UserHostAddress, companyIP))
            {
                isValid = true;
            }

            if (isValid && string.IsNullOrEmpty(model.UserName))
            {
                SetErrorForView(InvalidUserNameError, ViewResources.AlertUserNameMsg);
                isValid = false;
            }

            if (isValid && string.IsNullOrEmpty(model.PassWord))
            {
                SetErrorForView(InvalidPasswordError, ViewResources.AlertPasswordEmptyMsg);
                isValid = false;
            }

            if (isValid)
            {
                IResultInfo resultInfo;
                var requestUser = new User
                {
                    Username = model.UserName,
                    Password = model.PassWord,
                    Request = new Request { Ip = Request.UserHostAddress },
                };

                var user = _userService.ValidateUserForLrfSystem(requestUser, model.LogInToken, out resultInfo);
                if (resultInfo.IsSuccessful)
                {
                    SessionService.CreateNewSession(
                        user.Id,
                        _userService.LoginOnUser(user, Session.SessionID, user.ServerId, Request.UserHostAddress),
                        user.Username,
                        user.IsInternal,
                        user.Licensee.Id);
                    FormsAuthentication.SetAuthCookie(user.Username, false);

                    if (user.IsInRole(UserRole.Admin.ToString()))
                    {
                        return RedirectToAction("Index", "LicenseeList");
                    }
                    else
                    {
                        FormsAuthentication.RedirectFromLoginPage(user.Username, false);
                    }
                }
                else
                {
                    SetErrorForView(resultInfo.Code, resultInfo.Message);
                }
            }

            ViewBag.LogInToken = CreateToken();
            return View("~/Views/Authorization/SignIn.cshtml");
        }

        /// <summary>
        /// Check current user login and clear current authentication if another session using this user id to login.
        /// </summary>
        /// <returns>The JSON data with error code and error message.</returns>
        [CheckUMFilter]
        [HttpGet]
        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None, VaryByParam = "*")]
        public ActionResult RecheckIn()
        {
            var userSession = SessionService.UserSession;
            int currentLoginId = 0;

            if (int.TryParse(Request["onid"], out currentLoginId) == false || currentLoginId == 0 || !Request.IsAuthenticated || userSession == null)
            {
                return Json(RecheckInResult(ErrorType.AccessDenied, ViewResources.AccessDeniedMsg), JsonRequestBehavior.AllowGet);
            }

            if (currentLoginId != userSession.OnUserId)
            {
                return Json(RecheckInResult(ErrorType.SessionOverride, ViewResources.SessionLoggedMsg), JsonRequestBehavior.AllowGet);
            }

            var maintenanceInfo = _maintenanceService.GetUnderMaintenanceTime();

            if (maintenanceInfo.IsUnderMaintenanceTime)
            {
                var message = string.Format(ViewResources.UndermaintenanceMsg, maintenanceInfo.From, maintenanceInfo.To);
                return Json(RecheckInResult(ErrorType.AccessDenied, message), JsonRequestBehavior.AllowGet);
            }

            bool isKickout = _userService.RecheckIn(userSession.OnUserId);

            var errorCode = ErrorType.Success;
            var errorMsg = string.Empty;
            if (isKickout)
            {
                errorCode = ErrorType.KickedOut;
                errorMsg = ViewResources.SessionLoggedMsg;
                FormsAuthentication.SignOut();
            }

            return Json(RecheckInResult(errorCode, errorMsg), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Generate captcha.
        /// </summary>
        /// <returns>Return the image.</returns>
        [AllowAnonymous]
        public FileContentResult Captcha()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            Response.Cache.SetLastModifiedFromFileDependencies();

            string code = CreateRandomCaptchaCode();

            SessionService.CaptchaCode = code;
            FileContentResult img = null;
            var iwidth = (code.Length * 12) + 10;
            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(iwidth, 20))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                var random = new Random();

                var fonts = new[] { "Tahoma", "Arial" };

                var f = new Font(fonts[random.Next(0, fonts.Length - 1)], 11, FontStyle.Bold);
                Brush b = new SolidBrush(Color.White);
                gfx.Clear(Color.DimGray);

                gfx.DrawString(code, f, b, random.Next(2, 12), random.Next(-1, 1));
                gfx.DrawRectangle(new Pen(Color.Black), 0, 0, bmp.Width - 1, bmp.Height - 1);

                for (int i = 0; i < 50; i++)
                {
                    int x = random.Next(bmp.Width);
                    int y = random.Next(bmp.Height);
                    bmp.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                bmp.Save(mem, ImageFormat.Png);
                img = this.File(mem.GetBuffer(), "image/png");
            }

            return File(img.FileContents, img.ContentType);
        }

        [HttpGet]
        [AllowAnonymous]
        public string GetUserSalt(string userName)
        {
            var user = _userService.GetByUsername(userName);
            if (user != null)
            {
                return user.UserSalt;
            }

            return string.Empty;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignOut()
        {
            var userSession = SessionService.UserSession;
            if (userSession != null)
            {
                _userService.Logout(userSession.OnUserId);
            }

            Session.Clear();
            ClearCookies();
            FormsAuthentication.SignOut();
            return RedirectToAction("LocationRestriction", "Utility");
        }

        /// <summary>
        /// The changing password page.
        /// </summary>
        /// <returns>The changing password form.</returns>
        [HttpGet]
        public ActionResult ChangePassword()
        {
            ViewBag.PageData = new Dictionary<string, string>()
            {
            { "alertInputOldPwd", ViewResources.AlertInputOldPwdMsg },
            { "alertPasswordEmpty", ViewResources.AlertPasswordEmptyMsg },
            { "alertErrorConfirm", ViewResources.PasswordConfirmErrorMsg },
            { "alertPasswordRequire", ViewResources.PasswordNoteMsg },
            { "alertPasswordSucc", ViewResources.PasswordChangedMsg }
            };
            return View("~/Views/Utility/ChangePass.cshtml");
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(oldPassword) || !Util.CheckValidPassword(newPassword))
            {
                return Json(new
                {
                    errCode = ErrorType.Unknown,
                    errMsg = ViewResources.PasswordNoteMsg
                });
            }

            var userSession = SessionService.UserSession;
            if (userSession.IsInternal)
            {
                return Json(new
                {
                    errCode = ErrorType.AccessDenied,
                    errMsg = ViewResources.AccessDeniedMsg
                });
            }
            else
            {
                var result = _userService.ChangePassword(
                    userSession.UserName,
                    oldPassword,
                    newPassword,
                    Request.UserHostAddress);
                return Json(new
                {
                    errCode = result.Code,
                    errMsg = result.Message
                });
            }
        }

        private static dynamic RecheckInResult(ErrorType type, string msg)
        {
            return new
            {
                ErrCode = (int)type,
                ErrMsg = msg
            };
        }

        private static string CreateToken()
        {
            return Guid.NewGuid().ToString("N", CultureInfo.CurrentCulture);
        }

        private static string CreateRandomCaptchaCode()
        {
            var randomNumber = new StringBuilder();
            for (var i = 0; i < 4; i++)
            {
                randomNumber.Append(RandomNumber.GenerateRandomInt32(0, 9));
            }

            return randomNumber.ToString();
        }

        private void SetErrorForView(int errorCode, string errorMsg)
        {
            ViewBag.errCode = errorCode;
            ViewBag.errMsg = errorMsg;
        }

        private void ClearCookies()
        {
            var cookies = Request.Cookies.AllKeys;
            foreach (string cookie in cookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
        }
    }
}