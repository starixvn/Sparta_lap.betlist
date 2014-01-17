// -----------------------------------------------------------------------
// <copyright file="ReadonlyBaseController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using System.Web.SessionState;

    /// <summary>
    /// Read only controller.
    /// </summary>
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class ReadonlyBaseController : BaseController
    {
    }
}