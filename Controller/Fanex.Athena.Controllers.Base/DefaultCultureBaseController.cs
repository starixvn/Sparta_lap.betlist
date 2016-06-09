// -----------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    #region Using

    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
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