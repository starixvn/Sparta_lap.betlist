// -----------------------------------------------------------------------
// <copyright file="ReadonlyBaseController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Extension.Utility;
    using ONEbook.UM.Filters;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    /// <summary>
    /// Read only controller.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class ReadonlyBaseController : BaseController
    {
        public ReadonlyBaseController()
        {
        }

        public ReadonlyBaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting)
            : base(languageService, sessionService, appSetting)
        {
        }
    }
}