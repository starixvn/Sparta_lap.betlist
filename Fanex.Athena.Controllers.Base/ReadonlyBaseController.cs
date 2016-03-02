// -----------------------------------------------------------------------
// <copyright file="ReadonlyBaseController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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