// -----------------------------------------------------------------------
// <copyright file="AdminController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using System.Xml.Linq;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;

    [CustomAuthorize]
    public class AdminController : Controller
    {
        private static readonly string[] AllowedUsers = new[] { "pgmalex", "pgmjoseph" };

        private readonly ISessionService _sessionService;
        private readonly IAppSettings _appSettings;
        private readonly IMemoryCacheService _cache;

        public AdminController(
            ISessionService sessionService,
            IAppSettings appSetting,
            IMemoryCacheService cache)
        {
            _sessionService = sessionService;
            _appSettings = appSetting;
            _cache = cache;
        }

        public ActionResult ClearCache()
        {
            return DoAction(() => _cache.Clear());
        }

        public ContentResult SpConfig()
        {
            CheckPermission();
            var suffixList = new[] { "_xtest", "_xpre", "_UAT" };
            var text = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~\\SpNameConfig\\FanexSpName.xml"));
            var xdoc = XDocument.Parse(text);
            var builder = new StringBuilder();
            var list = new SortedSet<string>();
            
            foreach (XElement element in xdoc.Descendants("Setting"))
            {
                var spname = element.Attribute("SpName").Value;
                if (suffixList.Any(x => spname.IndexOf(x, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    list.Add(spname.Split('_').Last() + ": " + spname);
                }
            }

            foreach (var item in list)
            {
                builder.AppendLine(item);
            }  

            return Content(builder.ToString(), "text/plain");
        }

        private ActionResult DoAction(Action action)
        {
            CheckPermission();

            action();
            return RedirectToAction("Index", "Home");
        }

        private void CheckPermission()
        {
            var requestHost = Request.Url.AbsoluteUri;
            var isInternalDomain = requestHost.Contains(_appSettings.GetValue("InternalSubDomain"));
            if (!isInternalDomain ||
                !AllowedUsers.Contains(_sessionService.UserSession.UserName.ToLowerInvariant()))
            {
                throw new HttpException(404, "Not found");
            }
        }
    }
}