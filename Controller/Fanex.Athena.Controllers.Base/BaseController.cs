// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.MetaModel;
    using Service.Core.Helpers;

    [CustomAuthorize]
    [XFrameOption]
    public class BaseController : Controller
    {
        protected const string DefaultLanguage = "en-US";
        private readonly ILanguageService _langService;
        private readonly ISessionService _sessionService;
        private readonly IAppSettings _appSettings;
        private readonly IClientViewHelper _clientViewHelper;

        protected BaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper)
        {
            _langService = languageService;
            _sessionService = sessionService;
            _appSettings = appSetting;
            _clientViewHelper = clientViewHelper;
        }

        public ILanguageService LangService
        {
            get { return _langService; }
        }

        public ISessionService SessionService
        {
            get { return _sessionService; }
        }

        public IAppSettings AppSettings
        {
            get { return _appSettings; }
        }

        protected bool IsFakeLicensee()
        {
            return AppSettings.FakeLicenseeList.Contains(SessionService.LicenseeSession.LicenseeId);
        }

        protected void SetConvetedUSDCurrencyId(BaseMetaModel metaModel)
        {
            metaModel.CurrencyId = GetConvetedUSDCurrencyId(metaModel.LicenseeId);
        }

        protected int GetConvetedUSDCurrencyId(int licenseeId)
        {
            return GetBaseUsdLicensees().Contains(licenseeId.ToString())
                ? BaseCurrencyType.Usd : BaseCurrencyType.All;
        }

        protected void SetConvetedUSDCurrencyName(BaseMetaModel metaModel)
        {
            metaModel.BaseCurrency = GetBaseUsdLicensees().Contains(metaModel.LicenseeId.ToString()) ? "usd" : "eur";
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var currentLanguage = LangService.GetCurrentLanguage();
            LangService.SetPageCulture(currentLanguage);
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            var viewPath = _clientViewHelper.GetViewPath(viewName);

            return base.View(viewPath, masterName, model);
        }

        private IEnumerable<string> GetBaseUsdLicensees()
        {
            var items = _appSettings.GetValue("BaseUsdLicensees");
            if (!string.IsNullOrEmpty(items))
            {
                return items.Split(',');
            }

            return new string[] { };
        }
    }
}