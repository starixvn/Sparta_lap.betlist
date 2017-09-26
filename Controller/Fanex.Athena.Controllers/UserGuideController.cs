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
    using Extension.Utility;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using ONEbook.UM.Filters;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class UserGuideController : BaseController
    {
        private const string CustomerListResourceName = "CustomerList";

        private static readonly int[] CasinoProducts = new int[]
        {
            ProductTypes.MicroGamingLiveCasino, ProductTypes.MicrogamingRNGCasino, ProductTypes.AllBet, ProductTypes.RngCasino, ProductTypes.Voidbridge
        };

        private readonly IProductService _productService;

        public UserGuideController()
        {
            _productService = new ProductService();
        }

        public UserGuideController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService)
            : base(languageService, sessionService, appSetting)
        {
            _productService = productService;
        }

        [AjaxRequest]
        public ActionResult Index(string resourceName)
        {
            var content = GetFileContent(resourceName);

            content = resourceName.Equals(CustomerListResourceName, System.StringComparison.OrdinalIgnoreCase) ?
                content.Replace("{rooturl}", Url.Content("~")) : AddSpecificProductList(content);

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

        private static string GetDefaultFilePathIfNotExist(string language, string filePath)
        {
            if (!language.Equals(DefaultLanguage, System.StringComparison.OrdinalIgnoreCase) && !System.IO.File.Exists(filePath))
            {
                filePath = filePath.Replace(language, DefaultLanguage);
            }

            return filePath;
        }

        protected internal virtual string GetFileContent(string resourceName)
        {
            var language = SessionService.UserSession.Language;

            var filePath = Server.MapPath(string.Format(@"~/App_Data/UserGuide/{0}/{1}.htm", resourceName, language));
            filePath = GetDefaultFilePathIfNotExist(language, filePath);

            using (var reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }
        }

        private static string AddCasinoProducts(string content, IEnumerable<Product> products)
        {
            if (content.Contains("{casino-productlist}"))
            {
                var casinoProducts = products.Where(x => CasinoProducts.Contains(x.ProductID)).Select(x => x.ProductName);
                if (casinoProducts.Any())
                {
                    content = content.Replace("{casino-productlist}", string.Join<string>(", ", casinoProducts));
                    content = content.Replace("{casino-productlist-section}", string.Empty);
                    content = content.Replace("{/casino-productlist-section}", string.Empty);
                }
                else
                {
                    content = new Regex(@"\{casino-productlist-section\}.*\{/casino-productlist-section\}").Replace(content, string.Empty);
                }
            }

            return content;
        }

        private static string RemoveProductSection(string content, IEnumerable<Product> products, int productId, string productCode)
        {
            var openingTag = "{" + productCode + "-productlist-section}";
            if (content.Contains(openingTag))
            {
                if (products.Any(x => x.ProductID == productId))
                {
                    var closingTag = "{/" + productCode + "-productlist-section}";
                    content = content.Replace(openingTag, string.Empty);
                    content = content.Replace(closingTag, string.Empty);
                }
                else
                {
                    var pattern = @"\{" + productCode + @"-productlist-section\}.*\{/" + productCode + @"-productlist-section\}";
                    content = new Regex(pattern).Replace(content, string.Empty);
                }
            }

            return content;
        }

        private string AddSpecificProductList(string content)
        {
            var products = _productService.GetProductList(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language);
            content = AddCasinoProducts(content, products);
            content = RemoveProductSection(content, products, ProductTypes.VirtualSports, "VS");
            content = RemoveProductSection(content, products, ProductTypes.VirtualSports2, "VS2");
            return content;
        }
    }
}