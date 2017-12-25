// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Logging;
    using Model.Core.MetaModel;
    using Service.Core.Common;
    using Service.Core.Helpers;
    using Service.Core.Users;
    using System.IO;
    using System.Web;
    [CustomAuthorize]
    [XFrameOption]
    public class BaseController : Controller
    {
        protected const string DefaultLanguage = "en-US";
        private readonly ILanguageService _langService;
        private readonly ISessionService _sessionService;
        private readonly IAppSettings _appSettings;
        private readonly IClientViewHelper _clientViewHelper;
        public BaseController() : this(null, null, null, null)
        {
        }

        protected BaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper)
        {
            _langService = languageService ?? new LanguageService();
            _sessionService = sessionService ?? new SessionService();
            _appSettings = appSetting ?? new AppSettings();
            _clientViewHelper = clientViewHelper ?? new ClientViewHelper();
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
            metaModel.CurrencyId = GetBaseUsdLicensees().Contains(metaModel.LicenseeId.ToString())
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
            var viewType = GetViewType();

            var viewByOption = _clientViewHelper.GetViewPath(viewName, viewType);
            if (viewByOption.IsNavigationView)
            {
                var returnPath = $"{this.RouteData.Values["Controller"]}/{this.RouteData.Values["Action"]}";
                return base.View(viewByOption.ViewPath, null, returnPath);
            }

            return base.View(viewByOption.ViewPath, masterName, model);
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

        protected ClientViewType GetViewType()
        {
            var userSession = SessionService.UserSession;
            return userSession != null && userSession.ViewType != ClientViewType.None ?
                userSession.ViewType : (ClientViewType)int.Parse(AppSettings.ClientViewType);
        }
    }
}