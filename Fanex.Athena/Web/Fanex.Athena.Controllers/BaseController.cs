// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    #region Using

    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.MetaModel;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Users;

    #endregion Using

    /// <summary>
    /// Base controller.
    /// </summary>
    public class BaseController : Controller
    {
        #region Members

        public readonly ILanguageService LangService;
        public readonly ISessionService SessionService;
        public readonly ICurrencyService CurrencyService;

        #endregion Members

        public BaseController()
        {
            LangService = new LanguageService();
            SessionService = new SessionService();
            CurrencyService = new CurrencyService();

            ViewBag.HasCustComm = this.SessionService.LicenseeSession != null && this.SessionService.LicenseeSession.IsAllowedCustomerComm;
        }

        public bool IsDebuggingEnabled
        {
            get
            {
                return HttpContext.IsDebuggingEnabled;
            }
        }

        [NonAction]
        public static void SetConvetedUSDCurrencyId(BaseMetaModel metaModel)
        {
            metaModel.CurrencyId = metaModel.BaseUsdLicensees.Contains(metaModel.LicenseeId.ToString())
                ? BaseCurrencyType.Usd : BaseCurrencyType.All;
        }

        [NonAction]
        public static void SetConvetedUSDCurrencyName(BaseMetaModel metaModel)
        {
            metaModel.BaseCurrency = metaModel.BaseUsdLicensees.Contains(metaModel.LicenseeId.ToString()) ? "usd" : "eur";
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            var currentLanguage = LangService.GetCurrentLanguage();
            LangService.SetPageCulture(currentLanguage);
            base.Initialize(requestContext);
        }

        protected override void Execute(System.Web.Routing.RequestContext requestContext)
        {
            var currentLanguage = LangService.GetCurrentLanguage();
            LangService.SetPageCulture(currentLanguage);
            base.Execute(requestContext);
        }

        [NonAction]
        protected IList<string> GetAllowLicenseesForStatusFunction()
        {
            var items = AppSettings.GetValue("LicenseeOfferStatusFunction");
            if (!string.IsNullOrEmpty(items))
            {
                return items.Split(',');
            }

            return new List<string>();
        }
    }
}