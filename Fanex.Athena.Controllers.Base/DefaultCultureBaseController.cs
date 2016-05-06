// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    #region Using

    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using ONEbook.UM.Filters;

    #endregion Using

    /// <summary>
    /// Base controller.
    /// </summary>
    [Authorize]
    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class DefaultCultureBaseController : Controller
    {
        private readonly ISessionService _sessionService = new SessionService();

        public ISessionService SessionService
        {
            get { return _sessionService; }
        }
    }
}