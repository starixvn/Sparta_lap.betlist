// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    #region Using

    using System.Collections.Generic;
    using System.Linq;
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

        public string[] IgnoreAgCasino
        {
            get
            {
                return new string[] { Product.AGC };
            }
        }

        public string[] IgnoreCasinoAngBingo
        {
            get
            {
                return new string[] { Product.CS, Product.BG };
            }
        }

        public string[] IgnoreCasinoAngBingoAndAgCasino
        {
            get
            {
                return new string[] { Product.CS, Product.BG, Product.AGC };
            }
        }

        public string[] IgnoreKeno
        {
            get
            {
                return new string[] { Product.KN };
            }
        }

        public string[] IgnoreBingo
        {
            get
            {
                return new string[] { Product.BG };
            }
        }

        public string[] IgnoreBingoAndAgCasino
        {
            get
            {
                return new string[] { Product.BG, Product.AGC };
            }
        }

        public string[] IgnoreCasinoAndBingoAndKeno
        {
            get
            {
                return new string[] { Product.CS, Product.BG, Product.KN };
            }
        }

        public string[] IgnoreCasinoAndBingoAndKenoAndAgCasino
        {
            get
            {
                return new string[] { Product.CS, Product.BG, Product.KN, Product.AGC };
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

        /// <summary>
        /// Check if licensee has column Bet Effective in CustomerWL report or not.
        /// </summary>
        /// <param name="licenseeId">Licensee Id.</param>
        /// <returns>True if has Bet Effective column.</returns>
        [NonAction]
        protected bool GetHasBetEffectiveColumn(int licenseeId)
        {
            var licenseeIds = AppSettings.GetValue("LicenseesHasBetEffectiveColumn");
            var result = false;
            if (licenseeIds != null)
            {
                licenseeIds = licenseeIds.Trim();
                result = licenseeIds == "*" ? true : licenseeIds.Split(',').Contains(licenseeId.ToString());
            }

            return result;
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