// -----------------------------------------------------------------------
// <copyright file="UserGuideController.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.IO;
    using System.Web.Mvc;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class UserGuideController : ReadonlyBaseController
    {
        #region Action

        /// <summary>
        /// Note Page.
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>The action result.</returns>        
        public ActionResult Index(string resourceName)
        {
            var language = SessionService.UserSession.Language;
            var filePath = Server.MapPath(string.Format(@"~/App_Data/UserGuide/{0}.{1}.htm", resourceName, language));
            if (!System.IO.File.Exists(filePath))
            {
                filePath = filePath.Replace("." + language, string.Empty);
            }

            using (var reader = new StreamReader(filePath))
            {
                var content = reader.ReadToEnd();
                return View("~/Views/UserGuide/Index.cshtml", null, content);
            }
        }

        #endregion Action
    }
}