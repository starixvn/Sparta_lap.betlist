// -----------------------------------------------------------------------
// <copyright file="ReadonlyBaseController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using ONEbook.UM.Filters;

    /// <summary>
    /// Read only controller.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class ReadonlyBaseController : BaseController
    {
    }
}