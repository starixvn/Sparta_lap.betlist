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
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    public partial class ReportAPIController : DefaultCultureBaseController
    {
        private readonly IBetTypeService _betTypeService;

        private readonly IAppSettings _appSettings;

        private readonly ICustomerTurnoverService _customerTurnoverService;

        private readonly ITurnoverService _turnoverService;

        public ReportAPIController() : this(null, null, null, null, null)
        {
        }

        public ReportAPIController(
            ISessionService sessionService,
            IAppSettings appSettings,
            IBetTypeService betTypeService,
            ICustomerTurnoverService customerTurnoverService,
            ITurnoverService turnoverService)
            : base(sessionService, appSettings)
        {
            _betTypeService = betTypeService ?? new BetTypeService();
            _appSettings = appSettings ?? new AppSettings();
            _customerTurnoverService = customerTurnoverService ?? new CustomerTurnoverService();
            _turnoverService = turnoverService ?? new TurnoverService();
        }
    }
}