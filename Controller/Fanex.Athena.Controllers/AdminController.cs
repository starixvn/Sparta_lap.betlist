// -----------------------------------------------------------------------
// <copyright file="AdminController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;

    [CustomAuthorize]
    public class AdminController : Controller
    {
        private static readonly string[] AllowedUsers = new[] { "pgmalex", "pgmjoseph" };

        private readonly ISessionService _sessionService;
        private readonly IAppSettings _appSettings;
        private readonly IMemoryCacheService _cache;

        public AdminController() : this(null, null, null)
        {
        }

        public AdminController(
            ISessionService sessionService,
            IAppSettings appSetting,
            IMemoryCacheService cache)
        {
            _sessionService = sessionService ?? new SessionService();
            _appSettings = appSetting ?? new AppSettings();
            _cache = cache ?? new MemoryCacheService();
        }

        public ActionResult ClearCache()
        {
            return DoAction(() => _cache.Clear());
        }

        private ActionResult DoAction(Action action)
        {
            var requestHost = Request.Url.AbsoluteUri;
            var isInternalDomain = requestHost.Contains(_appSettings.GetValue("InternalSubDomain"));
            var userSession = _sessionService.UserSession;
            if (action == null ||
                !isInternalDomain ||
                !AllowedUsers.Contains(userSession.UserName.ToLowerInvariant()))
            {
                throw new HttpException(404, "Not found");
            }

            action();
            return RedirectToAction("Index", "Home");
        }
    }
}