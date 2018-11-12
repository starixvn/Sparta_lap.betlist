// -----------------------------------------------------------------------
// <copyright file="UserGuideController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Constant.DataDomain;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using ONEbook.UM.Filters;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class UserGuideController : BaseController
    {
        private const string CustomerListResourceName = "CustomerList";

        private static readonly int[] CasinoProducts = new[]
        {
            ProductTypes.MicroGamingLiveCasino, ProductTypes.MicrogamingRNGCasino, ProductTypes.AllBet, ProductTypes.RngCasino, ProductTypes.Voidbridge
        };

        private readonly IProductService _productService;

        private readonly IFileSystem _fileSystem;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public UserGuideController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IFileSystem fileSystem,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess)
        : base(languageService, sessionService, appSetting)
        {
            _productService = productService;
            _fileSystem = fileSystem;
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        [AjaxRequest]
        public ActionResult Index(string resourceName)
        {
            var content = GetFileContent(resourceName);

            content = content.Replace("{rooturl}", Url.Content("~"));
            content = AddSpecialProducts(content);
            content = AddBalanceUpDate(content);

            return View("~/Views/UserGuide/Index.cshtml", null, content);
        }

        [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
        public FilePathResult DownloadCustomerListPdf()
        {
            var language = SessionService.UserSession.Language;
            var fileName = SessionService.LicenseeSession.IsEnableBA ? "Odds Group Feature_Licensee_BA.pdf" : "Odds Group Feature_Licensee.pdf";
            var filePath = Server.MapPath(string.Format(@"~/App_Data/UserGuide/{0}/pdf/{1}/{2}", CustomerListResourceName, language, fileName));
            filePath = GetDefaultFilePathIfNotExist(language, filePath);
            return File(filePath, System.Net.Mime.MediaTypeNames.Application.Pdf, Path.GetFileName(filePath));
        }

        [FunctionAuthorization(PermissionKeys.ReportCashOutWinLoss)]
        public FilePathResult CashoutReportPdf()
        {
            var chineseCodes = new[] { "zh-CN", "zh-TW", "zh-Hans" };
            var suffix = chineseCodes.Contains(SessionService.UserSession.Language) ? "CN" : "EN";
            var fileName = $"Cash Out WL Report guideline_{suffix}.pdf";
            var filePath = Server.MapPath(string.Format(@"~/App_Data/UserGuide/CashOutWL/pdf/{0}", fileName));
            return File(filePath, System.Net.Mime.MediaTypeNames.Application.Pdf, Path.GetFileName(filePath));
        }

        private static string AddCasinoProducts(string content, IEnumerable<Product> products)
        {
            var result = content;
            if (content.Contains("{casino-productlist}"))
            {
                var casinoProducts = products.Where(x => CasinoProducts.Contains(x.ProductID)).Select(x => x.ProductName);
                if (casinoProducts.Any())
                {
                    result = result.Replace("{casino-productlist}", string.Join<string>(", ", casinoProducts));
                    result = result.Replace("{casino-productlist-section}", string.Empty);
                    result = result.Replace("{/casino-productlist-section}", string.Empty);
                }
                else
                {
                    result = new Regex(@"\{casino-productlist-section\}.*\{/casino-productlist-section\}").Replace(result, string.Empty);
                }
            }

            return result;
        }

        private static string RemoveProductSection(string content, IEnumerable<Product> products, int productId, string productCode)
        {
            var openingTag = "{" + productCode + "-productlist-section}";

            var customContent = content;
            if (customContent.Contains(openingTag))
            {
                if (products.Any(x => x.ProductID == productId))
                {
                    var closingTag = "{/" + productCode + "-productlist-section}";
                    customContent = customContent.Replace(openingTag, string.Empty);
                    customContent = customContent.Replace(closingTag, string.Empty);
                }
                else
                {
                    var pattern = @"\{" + productCode + @"-productlist-section\}.*\{/" + productCode + @"-productlist-section\}";
                    customContent = new Regex(pattern).Replace(customContent, string.Empty);
                }
            }

            return customContent;
        }

        private string AddBalanceUpDate(string content)
        {
            const string PlaceHolder = "{balance-up-date}";
            if (content.Contains(PlaceHolder))
            {
                var date = _balanceUpDataAccess.GetBalanceUp().BalanceUpDay;
                return content.Replace(PlaceHolder, date.ToShortDateString());
            }

            return content;
        }

        private string AddSpecialProducts(string content)
        {
            var products = _productService.GetProductList(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language);
            var customcontent = AddCasinoProducts(content, products);
            customcontent = RemoveProductSection(customcontent, products, ProductTypes.VirtualSports, "VS");
            customcontent = RemoveProductSection(customcontent, products, ProductTypes.VirtualSports2, "VS2");
            return customcontent;
        }

        private string GetDefaultFilePathIfNotExist(string language, string filePath)
        {
            if (!language.Equals(DefaultLanguage, System.StringComparison.OrdinalIgnoreCase) && !_fileSystem.Exists(filePath))
            {
                return filePath.Replace(language, DefaultLanguage);
            }

            return filePath;
        }

        private string GetFileContent(string resourceName)
        {
            var language = SessionService.UserSession.Language;

            var filePath = Server.MapPath(string.Format(@"~/App_Data/UserGuide/{0}/{1}.htm", resourceName, language));
            filePath = GetDefaultFilePathIfNotExist(language, filePath);

            return _fileSystem.ReadFile(filePath);
        }
    }
}