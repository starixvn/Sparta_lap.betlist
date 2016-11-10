using System.Web.Mvc;
using Fanex.Athena.Service.Core.Users.Interfaces;
using ONEbook.UM.Services;
using Fanex.Athena.Service.Core.Users;
using ONEbook.UM.Services.Impl;

namespace Fanex.Athena.Controllers
{
    [Route(Name = "Auth")]
    public class OneAuthorizationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMaintenanceService _maintenanceService;
        public OneAuthorizationController() : this(null, null)
        {

        }
        public OneAuthorizationController(IUserService userService, IMaintenanceService maintenanceService)
        {
            _userService = userService ?? new UserService();
            _maintenanceService = maintenanceService ?? new MaintenanceService();
        }

        [HttpGet]
        public RedirectResult CreateSession(string Token)
        {
            var returnUrl = Url.Content("~/Utility/OneTokenRestriction");
            return Redirect(returnUrl);
        }
    }
}
