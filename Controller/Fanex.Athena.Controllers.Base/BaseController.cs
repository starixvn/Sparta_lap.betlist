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
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.MetaModel;

    /// <summary>
    /// Base controller.
    /// </summary>
    [Authorize]
    [XFrameOption]
    public class BaseController : Controller
    {
        protected const string DefaultLanguage = "en-US";
        private readonly ILanguageService _langService;
        private readonly ISessionService _sessionService;
        private readonly IAppSettings _appSettings;

        public BaseController() : this(null, null, null)
        {
        }

        protected BaseController(ILanguageService languageService, ISessionService sessionService, IAppSettings appSetting)
        {
            _langService = languageService ?? new LanguageService();
            _sessionService = sessionService ?? new SessionService();
            _appSettings = appSetting ?? new AppSettings();
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

        protected void SetConvetedUSDCurrencyId(BaseMetaModel metaModel)
        {
            metaModel.CurrencyId = GetBaseUsdLicensees().Contains(metaModel.LicenseeId.ToString())
                ? BaseCurrencyType.Usd : BaseCurrencyType.All;
        }

        protected void SetConvetedUSDCurrencyName(BaseMetaModel metaModel)
        {
            metaModel.BaseCurrency = GetBaseUsdLicensees().Contains(metaModel.LicenseeId.ToString()) ? "usd" : "eur";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
        protected IList<string> GetAllowLicenseesForStatusFunction()
        {
            var items = _appSettings.GetValue("LicenseeOfferStatusFunction");
            if (!string.IsNullOrEmpty(items))
            {
                return items.Split(',');
            }

            return new List<string>();
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var currentLanguage = LangService.GetCurrentLanguage();
            LangService.SetPageCulture(currentLanguage);
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            var userSession = SessionService.UserSession;

            var option = userSession != null && userSession.ViewType != ClientViewType.None ?
                userSession.ViewType : (ClientViewType)int.Parse(AppSettings.ClientViewType);

            var viewByOption = ClientViewHelper.GetViewPath(viewName, option);
            return base.View(viewByOption, masterName, model);
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