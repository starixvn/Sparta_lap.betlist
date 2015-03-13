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
    using System.Xml.Linq;
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
        protected const string DefaultLanguage = "en-US";

        private readonly ILanguageService _langService = new LanguageService();
        private readonly ISessionService _sessionService = new SessionService();
        private readonly ICurrencyService _currencyService = new CurrencyService();

        #region Members

        public string[] IgnoreGoldDeluxe
        {
            get
            {
                return new string[] { Product.GD };
            }
        }

        public string[] IgnoreAgCasinoAndPlaytechAndGoldDeluxe
        {
            get
            {
                return new string[] { Product.AGC, Product.PTCS, Product.GD };
            }
        }

        public string[] IgnoreCasinoAngBingoAndPlaytechAndGoldDeluxe
        {
            get
            {
                return new string[] { Product.CS, Product.BG, Product.PTCS, Product.GD };
            }
        }

        public string[] IgnoreCasinoAngBingoAndAgCasinoAndPlaytechAndGoldDeluxe
        {
            get
            {
                return new string[] { Product.CS, Product.BG, Product.AGC, Product.PTCS, Product.GD, Product.GD };
            }
        }

        public string[] IgnoreKenoAndPlaytechAndGoldDeluxe
        {
            get
            {
                return new string[] { Product.KN, Product.PTCS, Product.GD, Product.GD };
            }
        }

        public string[] IgnoreBingoAndPlaytechAndGoldDeluxe
        {
            get
            {
                return new string[] { Product.BG, Product.PTCS, Product.GD };
            }
        }

        public string[] IgnoreBingoAndAgCasinoAndPlaytechAndGoldDeluxe
        {
            get
            {
                return new string[] { Product.BG, Product.AGC, Product.PTCS, Product.GD };
            }
        }

        public string[] IgnoreCasinoAndBingoAndKenoAndPlaytechAndGoldDeluxe
        {
            get
            {
                return new string[] { Product.CS, Product.BG, Product.KN, Product.PTCS, Product.GD };
            }
        }

        public string[] IgnoreCasinoAndBingoAndKenoAndAgCasinoAndPlaytechAndGoldDeluxe
        {
            get
            {
                return new string[] { Product.CS, Product.BG, Product.KN, Product.AGC, Product.PTCS, Product.GD };
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

        [NonAction]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed.")]
        protected static IList<string> GetLicenseesOfferOddsGroup()
        {
            var licenseeIds = AppSettings.GetValue("LicenseesOfferOddsGroup");
            if (!string.IsNullOrEmpty(licenseeIds))
            {
                return licenseeIds.Split(',');
            }

            return new List<string>();
        }

        /// <summary>
        /// Get Ignored Products.
        /// </summary>
        /// <param name="reportName">Report name.</param>
        /// <returns>Product code array.</returns>
        protected static string[] GetIgnoredProducts(string reportName = "")
        {
            var document = XDocument.Load(AppSettings.GetValue("ReportProductIgnoreFile"));
            var config = document.Descendants("Report")
                    .Where(x => x.Attribute("Name").Value.Equals(reportName, System.StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

            if (config == null)
            {
                return new string[] { };
            }
            else
            {
                return config.Attribute("Product").Value.Split(',');
            }
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