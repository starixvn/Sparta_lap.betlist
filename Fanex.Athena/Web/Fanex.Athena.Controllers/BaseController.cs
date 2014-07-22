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
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Users;

    #endregion Using

    /// <summary>
    /// Base controller.
    /// </summary>
    [Authorize]
    public class BaseController : Controller
    {
        private readonly ILanguageService _langService = new LanguageService();
        private readonly ISessionService _sessionService = new SessionService();
        private readonly ICurrencyService _currencyService = new CurrencyService();

        #region Members

        public string[] IgnoreCasinoAngBingo
        {
            get
            {
                return new string[] { Product.Cs, Product.Bg };
            }
        }

        public string[] IgnoreKeno
        {
            get
            {
                return new string[] { Product.Kn };
            }
        }

        public string[] IgnoreBingo
        {
            get
            {
                return new string[] { Product.Bg };
            }
        }

        public string[] IgnoreCasinoAndBingoAndKeno
        {
            get
            {
                return new string[] { Product.Cs, Product.Bg, Product.Kn };
            }
        }

        #endregion Members

        public ILanguageService LangService
        {
            get { return _langService; }
        }

        public ISessionService SessionService
        {
            get { return _sessionService; }
        }

        public ICurrencyService CurrencyService
        {
            get { return _currencyService; }
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

        [NonAction]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
        protected static IList<string> GetAllowLicenseesForStatusFunction()
        {
            var items = AppSettings.GetValue("LicenseeOfferStatusFunction");
            if (!string.IsNullOrEmpty(items))
            {
                return items.Split(',');
            }

            return new List<string>();
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
    }
}