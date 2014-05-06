// -----------------------------------------------------------------------
// <copyright file="AuthorizationController.cs" company="nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;
    using System.Web.Security;
    using DNAide.Utils;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Users;
    using Fanex.Athena.Models.MetaModel.Authorization;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Users;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthorizationController : BaseController
    {
        /// <summary>
        /// The sign-in page.
        /// </summary>
        /// <returns>The sign in page.</returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignIn()
        {
            IMaintenanceService maintenanceService = new MaintenanceService();
            var acceptIp = AppSettings.GetValue("AcceptIp");
            var isDeploying = acceptIp != "*" && Request.UserHostAddress != acceptIp;

            if (isDeploying || maintenanceService.MaintenanceInfo.IsDownTime)
            {
                Server.Transfer("~/Utility/UnderMaintenance");
            }

            var requestHost = Request.Url.AbsoluteUri;

            var isInternalDomain = requestHost.Contains(AppSettings.GetValue("internaldomain"));

            if (!isInternalDomain)
            {
                Server.Transfer("~/Utility/LocationRestriction");
            }

            SinglePageData(0, string.Empty);
            return View("~/Views/Authorization/SignIn.cshtml");
        }

        /// <summary>
        /// The sign in page.
        /// </summary>
        /// <param name="model">The sign in metal model.</param>
        /// <returns>The redirected action or sign in page in case of having a error .</returns>
        [AllowAnonymous]
        [HttpPost]
        [SuppressMessage(
  "StyleCop.CSharp.Nexcel.NexcelCustomRules",
  "SP2101:MethodMustNotContainMoreLinesThan",
  Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult SignIn(LoginMetalModel model)
        {
            var isValid = true;
            var isAllowAutomationCaptchaCode =
           AppSettings.GetValue("AllowAutomationCaptchaCode") == "1";
            if (model.Captcha != SessionService.CaptchaCode)
            {
                /*if IsAllowAutomationCaptchaCode = true
                 then allow selenium testing
                 */
                if (!isAllowAutomationCaptchaCode)
                {
                    SessionService.CaptchaCode = Encryption.RandomNumberString(4).ToUpperInvariant();
                    SinglePageData(101, ViewResources.InvalidCaptchaMsg);
                }

                isValid = false;
            }

            if (string.IsNullOrEmpty(model.UserName))
            {
                SinglePageData(102, ViewResources.AlertUserNameMsg);
                isValid = false;
            }

            if (string.IsNullOrEmpty(model.PassWord))
            {
                SinglePageData(103, ViewResources.AlertPasswordEmptyMsg);
                isValid = false;
            }

            if (isValid)
            {
                var userService = new UserService();
                IResultInfo resultInfo;
                var requestUser = new User
                {
                    Username = model.UserName,
                    Password = model.PassWord,
                    Request = new Request { Ip = Request.UserHostAddress },
                };

                User user = userService.ValidateUserForLrfSystem(requestUser, out resultInfo);
                if (resultInfo.IsSuccessful)
                {
                    SessionService.CreateNewSession(
                        user.Id,
                        userService.LoginOnUser(user, Session.SessionID, user.ServerId, Request.UserHostAddress),
                        user.Username,
                        user.IsInternal,
                        user.Licensee.Id);
                    FormsAuthentication.SetAuthCookie(user.Username, false);

                    if (user.IsInRole(UserRole.Admin.ToString()))
                    {
                        return RedirectToAction("SelectLicensee", "Utility");
                    }
                    else
                    {
                        FormsAuthentication.RedirectFromLoginPage(user.Username, false);
                    }
                }
                else
                {
                    SinglePageData(resultInfo.Code, resultInfo.Message);
                }
            }

            return View("~/Views/Authorization/SignIn.cshtml");
        }

        /// <summary>
        /// The re-check in service.
        /// </summary>
        /// <returns>The JSON data.</returns>
        [HttpGet]
        public ActionResult ReCheckin()
        {
            IMaintenanceService maintenanceService = new MaintenanceService();
            int currentLoginId = 0;
            int.TryParse(Request["onid"], out currentLoginId);

            // Invalid re-check-in
            if (currentLoginId == 0 || !Request.IsAuthenticated || SessionService.UserSession == null)
            {
                return Json(RecheckInResult(ErrorType.AccessDenied, ViewResources.AccessDeniedMsg), JsonRequestBehavior.AllowGet);
            }

            if (currentLoginId != SessionService.UserSession.OnUserId)
            {
                return Json(RecheckInResult(ErrorType.SessionOverride, ViewResources.SessionLoggedMsg), JsonRequestBehavior.AllowGet);
            }

            var maintenanceInfo = maintenanceService.MaintenanceInfo;
            var message = string.Format(ViewResources.UndermaintenanceMsg, maintenanceInfo.UmTimeFrom, maintenanceInfo.UmTimeTo);
            if (maintenanceInfo.IsDownTime)
            {
                return Json(RecheckInResult(ErrorType.AccessDenied, message), JsonRequestBehavior.AllowGet);
            }

            IUserService userService = new UserService();
            bool isSuccess = !userService.ReCheckin(SessionService.UserSession.OnUserId);

            var errCode = isSuccess ? ErrorType.Success : ErrorType.KickedOut;
            var errMsg = isSuccess ? SessionService.LicenseeSession.LicenseeId.ToString() : ViewResources.SessionLoggedMsg;

            return Json(RecheckInResult(errCode, errMsg), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Generate captcha.
        /// </summary>
        /// <returns>Return the image.</returns>
        [AllowAnonymous]
        public FileContentResult Captcha()
        {
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            Response.Cache.SetLastModifiedFromFileDependencies();
            string code = Encryption.RandomNumberString(4).ToUpperInvariant();

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

        private dynamic RecheckInResult(ErrorType type, string msg)
        {
            return new
             {
                 ErrCode = (int)type,
                 ErrMsg = msg
             };
        }

        private void SinglePageData(int errorCode, string errorMsg)
        {
            ViewBag.errCode = errorCode;
            ViewBag.errMsg = errorMsg;
        }
    }
}