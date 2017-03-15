// -----------------------------------------------------------------------
// <copyright file="UserGuideController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class UserGuideController : ReadonlyBaseController
    {
        private const string CustomerListResourceName = "CustomerList";

        private static readonly int[] CasinoProducts = new int[]
        {
            ProductTypes.MicroGamingLiveCasino, ProductTypes.MicrogamingRNGCasino, ProductTypes.AllBet, ProductTypes.RngCasino, ProductTypes.Macau
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
            if (resourceName.Equals(CustomerListResourceName, System.StringComparison.OrdinalIgnoreCase))
            {
                content = content.Replace("{rooturl}", Url.Content("~"));
            }
            else
            {
                content = AddCasinoProductList(content);
            }

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

        private string AddCasinoProductList(string content)
        {
            if (content.Contains("{casino-productlist}"))
            {
                var products = _productService.GetProductList(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language);
                var casinoProducts = products.Where(x => CasinoProducts.Contains(x.ProductID)).Select(x => x.ProductName);
                content = content.Replace("{casino-productlist}", string.Join<string>(", ", casinoProducts));
            }

            return content;
        }
    }
}