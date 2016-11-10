using Fanex.Athena.Service.Core.Users.Interfaces;
using ONEbook.UM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

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
            _userService = userService;
            _maintenanceService = maintenanceService;
        }

        [HttpGet]
        public RedirectResult CreateSession(string Token)
        {
            var returnUrl = Url.Content("~/Utility/OneTokenRestriction");
            return Redirect(returnUrl);
        }
    }
}
