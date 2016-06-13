// -----------------------------------------------------------------------
// <copyright file="ReportAPIController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public partial class ReportAPIController : DefaultCultureBaseController
    {
        private readonly IBetTypeService _betTypeService = new BetTypeService();
    }
}