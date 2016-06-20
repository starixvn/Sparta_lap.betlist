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
    using Model.Core.MetaModel;

    /// <summary>
    /// Base controller.
    /// </summary>
    [Authorize]
    public class BaseController : Controller
    {
        protected const string DefaultLanguage = "en-US";

        private readonly ILanguageService _langService;
        private readonly ISessionService _sessionService;

        public BaseController()
        {
            _langService = new LanguageService();
            _sessionService = new SessionService();
        }

        public ILanguageService LangService
        {
            get { return _langService; }
        }

        public ISessionService SessionService
        {
            get { return _sessionService; }
        }

        protected static void SetConvetedUSDCurrencyId(BaseMetaModel metaModel)
        {
            metaModel.CurrencyId = GetBaseUsdLicensees().Contains(metaModel.LicenseeId.ToString())
                ? BaseCurrencyType.Usd : BaseCurrencyType.All;
        }

        protected static void SetConvetedUSDCurrencyName(BaseMetaModel metaModel)
        {
            metaModel.BaseCurrency = GetBaseUsdLicensees().Contains(metaModel.LicenseeId.ToString()) ? "usd" : "eur";
        }

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
            base.Initialize(requestContext);
            var currentLanguage = LangService.GetCurrentLanguage();
            LangService.SetPageCulture(currentLanguage);
        }

        protected IDictionary<string, int> GetUserCurrencies(int licenseeId)
        {
            return SessionService.UserSession.CurrencyList;
        }

        private static IEnumerable<string> GetBaseUsdLicensees()
        {
            var items = AppSettings.GetValue("BaseUsdLicensees");
            if (!string.IsNullOrEmpty(items))
            {
                return items.Split(',');
            }

            return new string[] { };
        }
    }
}