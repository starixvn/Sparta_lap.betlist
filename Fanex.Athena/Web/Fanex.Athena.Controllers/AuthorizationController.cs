// -----------------------------------------------------------------------
// <copyright file="AuthorizationController.cs" company="nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;
    using System.Web.Security;
    using DNAide.Utils;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Users;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.MetaModel.Authorization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AuthorizationController : BaseController
    {
        private const string PageError = "_page={errCode:{0},errMsg:\"{1}\"};";
        [AllowAnonymous]
        [HttpGet]
        public ActionResult SignIn()
        {
            IMaintenanceService maintenanceService = new MaintenanceService();
            var acceptIp = AppSettings.GetValue("AcceptIp");
            var isDeploying = acceptIp != "*" && Request.UserHostAddress != acceptIp;

            if (isDeploying || maintenanceService.MaintenanceInfo.IsDownTime)
            {
                return RedirectToAction("UnderMaintenance", "Utility");
            }

            var requestHost = Request.Url.AbsoluteUri;

            var isInternalDomain = requestHost.Contains(AppSettings.GetValue("internaldomain"));

            if (!isInternalDomain)
            {
                return RedirectToAction("LocationRestriction", "Utility");
            }
            ViewBag.PageError = string.Format(PageError, 0, string.Empty);
            return View("~/Views/Authorization/SignIn.cshtml");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SignIn(LoginMetalModel model)
        {
            var isAllowAutomationCaptchaCode =
           AppSettings.GetValue("AllowAutomationCaptchaCode") == "1";
            if (model.Captcha != SessionService.CaptchaCode)
            {
                // if IsAllowAutomationCaptchaCode = true 
                // then allow selenium testing
                if (!isAllowAutomationCaptchaCode)
                {
                    // Create new code
                    SessionService.CaptchaCode = Encryption.RandomNumberString(4).ToUpperInvariant();
                    // Return error
                    ViewBag.PageError = string.Format(PageError, 0, string.Empty);

                }
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ReCheckin()
        {
            IMaintenanceService maintenanceService = new MaintenanceService();
            int onId = 0;
            int.TryParse(Request["onid"], out onId);

            // Invalid re-check-in
            if (onId == 0 || !Request.IsAuthenticated || SessionService.UserSession == null)
            {
                return Json(RecheckInResult(ErrorType.AccessDenied, ViewResources.AccessDeniedMsg), JsonRequestBehavior.AllowGet);
            }

            if (onId != SessionService.UserSession.OnUserId)
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

        [HttpGet]
        public ActionResult SignOut()
        {
            // Kick-out
            IUserService userService = new UserService();
            if (SessionService.UserSession != null)
            {
                userService.Logout(SessionService.UserSession.OnUserId);
            }

            Session.Clear();
            ClearCookies();
            FormsAuthentication.SignOut();
            return RedirectToAction("LocationRestriction", "Utility");
        }
        [AllowAnonymous]
        public FileContentResult Captcha()
        {
            Response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            Response.Cache.SetLastModifiedFromFileDependencies();
            string code = Encryption.RandomNumberString(4).ToUpperInvariant();

            SessionService.CaptchaCode = code;
            FileContentResult img = null;
            int iwidth = code.Length * 12 + 10;
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

        private void ClearCookies()
        {
            var cookieList = HttpContext.Response.Cookies;
            var cookieNameList = HttpContext.Response.Cookies.AllKeys;
            foreach (var name in cookieNameList)
            {
                if (Request.Cookies[name] == null)
                {
                    continue;
                }

                var cookie = new System.Web.HttpCookie(name);
                cookie.Expires = System.DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
            }
        }
    }
}