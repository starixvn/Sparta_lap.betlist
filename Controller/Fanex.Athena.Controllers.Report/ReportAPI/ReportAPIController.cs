// -----------------------------------------------------------------------
// <copyright file="ReportAPIController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    public partial class ReportAPIController : DefaultCultureBaseController
    {
        private readonly IBetTypeService _betTypeService;

        private readonly IAppSettings _appSettings;

        private readonly ICustomerTurnoverService _customerTurnoverService;

        public ReportAPIController()
        {
            _betTypeService = new BetTypeService();
            _appSettings = new AppSettings();
            _customerTurnoverService = new CustomerTurnoverService();
        }
    }
}