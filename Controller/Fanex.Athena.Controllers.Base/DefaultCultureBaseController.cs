namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using ONEbook.UM.Filters;
    using Web.ActionFilter;

    [CustomAuthorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class DefaultCultureBaseController : Controller
    {
        private readonly ISessionService _sessionService;

        public DefaultCultureBaseController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public ISessionService SessionService
        {
            get { return _sessionService; }
        }
    }
}