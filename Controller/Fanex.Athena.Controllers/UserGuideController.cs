// -----------------------------------------------------------------------
// <copyright file="UserGuideController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.IO;
    using System.Web.Mvc;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;

    public class UserGuideController : ReadonlyBaseController
    {
        private const string CustomerListResourceName = "CustomerList";

        [AjaxRequest]
        public ActionResult Index(string resourceName)
        {
            var content = GetFileContent(resourceName);
            if (resourceName.Equals(CustomerListResourceName, System.StringComparison.OrdinalIgnoreCase))
            {
                content = content.Replace("{rooturl}", Url.Content("~"));
            }

            return View("~/Views/UserGuide/Index.cshtml", null, content);
        }

        [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
        public FilePathResult DownloadCustomerListPdf()
        {
            var language = SessionService.UserSession.Language;
            var fileName = SessionService.LicenseeSession.IsEnableBA ? "Odds Group Feature_Licensee.pdf" : "Odds Group Feature_Licensee_NoBA.pdf";
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

        private string GetFileContent(string resourceName)
        {
            var language = SessionService.UserSession.Language;

            var filePath = Server.MapPath(string.Format(@"~/App_Data/UserGuide/{0}/{1}.htm", resourceName, language));
            filePath = GetDefaultFilePathIfNotExist(language, filePath);

            using (var reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }
        }
    }
}