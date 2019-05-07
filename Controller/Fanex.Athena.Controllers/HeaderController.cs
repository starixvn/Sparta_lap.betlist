// -----------------------------------------------------------------------
// <copyright file="HeaderController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using DataAccess.Core.Users.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.ViewModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Session.Enum;
    using Web.ActionFilter;

    [AuthorizeRestriction(AppMode.Self)]
    public class HeaderController : CultureBaseController
    {
        private readonly ILicenseeDataAccess _licenseeDataAccess;

        public HeaderController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ILicenseeDataAccess licenseeDataAccess)
        : base(languageService, sessionService, appSetting)
        {
            _licenseeDataAccess = licenseeDataAccess;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var userSession = SessionService.UserSession;
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var viewModel = new HeaderPageViewModel
            {
                Language = userSession.Language,
                OnUserId = userSession.OnUserId,
                LicenseeList = _licenseeDataAccess.GetByUser(userSession.UserId),
                IsInternalLogin = userSession.IsInternal,
                HasRecheckIn = !AppSettings.LocalTest,
                SupportedLanguageList = Util.SupportedLanguages,
                UserName = userSession.UserName,
                CurrentLicenseeId = licenseeId,
                IsAllowedManagementMember = userSession.FunctionIdList.Contains((int)FunctionType.CustomerList),
                IsFakeLicensee = IsFakeLicensee(licenseeId),
                IsAllowedNotification = userSession.FunctionIdList.Contains((int)FunctionType.ResultAmendment_Recalculation)
            };
            return View("~/Views/Desktop/Header/Index.cshtml", viewModel);
        }

        public ActionResult ChangeLicensee(int? licenseeId)
        {
            var newLicenseeId = licenseeId.HasValue ? licenseeId.Value : 0;
            var originalLicenseeId = SessionService.LicenseeSession.LicenseeId;
            var originalUserId = SessionService.UserSession.UserId;

            if (newLicenseeId <= 0 || !_licenseeDataAccess.IsLicenseeBelongToUser(originalLicenseeId, originalUserId))
            {
                newLicenseeId = originalLicenseeId;
            }

            if (newLicenseeId != originalLicenseeId)
            {
                SessionService.UpdateLiceseeSession(newLicenseeId);
            }

            ViewBag.DefaultReportUrl = SessionService.UserSession.DefaultReportUrl;
            return Redirect("~");
        }

        /// <summary>
        /// Change language.
        /// </summary>
        /// <param name="languagesite">The language code.</param>
        /// <returns>Return "1" if changing is success.</returns>
        [HttpGet]
        public string ChangeLanguage(string languagesite)
        {
            var changed = false;
            if (!string.IsNullOrEmpty(languagesite))
            {
                var languageCode = languagesite.Trim();
                if (Util.IsValidLanguage(languageCode))
                {
                    var currentLang = LangService.GetCurrentLanguage();
                    if (!currentLang.Equals(languageCode))
                    {
                        changed = LangService.ChangeLanguage(languageCode);
                    }
                }
            }

            return changed ? "1" : "0";
        }
    }
}