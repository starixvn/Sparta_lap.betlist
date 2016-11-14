using System.Web.Mvc;
using Fanex.Athena.Service.Core.Users;
using Fanex.Athena.Service.Core.Users.Interfaces;
using ONEbook.UM.Services;
using ONEbook.UM.Services.Impl;

namespace Fanex.Athena.Controllers
{
    [Route(Name = "Auth")]
    public class OneAuthorizationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly IOneAuthenticationService _oneAuthenticationService;

        public OneAuthorizationController() : this(null, null, null)
        {
        }

        internal OneAuthorizationController(IUserService userService, IMaintenanceService maintenanceService, IOneAuthenticationService oneAuthenticationService)
        {
            _userService = userService ?? new UserService();
            _maintenanceService = maintenanceService ?? new MaintenanceService();
            _oneAuthenticationService = oneAuthenticationService ?? new OneAuthenticationService();
        }

        [HttpGet]
        public RedirectResult CreateSession(string token)
        {
            var returnUrl = Url.Content("~/Utility/OneTokenRestriction");
            if (!string.IsNullOrWhiteSpace(token))
            {
                var tokenInfo = _oneAuthenticationService.GetTokenInfo(token);
            }

            return Redirect(returnUrl);
        }
    }
}