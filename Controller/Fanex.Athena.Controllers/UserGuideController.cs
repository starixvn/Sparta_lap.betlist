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
            var fileName = SessionService.LicenseeSession.IsEnableBA ? "BA.xls" : "nonBA.xls";
            var filePath = Server.MapPath(string.Format(@"~/App_Data/UserGuide/{0}/pdf/{1}", CustomerListResourceName, fileName));
            return File(filePath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(filePath));
        }

        private string GetFileContent(string resourceName)
        {
            var language = SessionService.UserSession.Language;

            var filePath = Server.MapPath(string.Format(@"~/App_Data/UserGuide/{0}/{1}.htm", resourceName, language));
            if (!language.Equals(DefaultLanguage, System.StringComparison.OrdinalIgnoreCase) && !System.IO.File.Exists(filePath))
            {
                filePath = filePath.Replace(language, DefaultLanguage);
            }

            using (var reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }
        }
    }
}