// -----------------------------------------------------------------------
// <copyright file="ReportAPIController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Service.Report.Interfaces;

    public partial class ReportAPIController : DefaultCultureBaseController
    {
        private readonly IBetTypeService _betTypeService;

        private readonly IAppSettings _appSettings;

        private readonly ICustomerTurnoverService _customerTurnoverService;

        private readonly ITurnoverService _turnoverService;

        public ReportAPIController(
            ISessionService sessionService,
            IAppSettings appSettings,
            IClientViewHelper clientViewHelper,
            IBetTypeService betTypeService,
            ICustomerTurnoverService customerTurnoverService,
            ITurnoverService turnoverService)
            : base(sessionService, clientViewHelper)
        {
            _betTypeService = betTypeService;
            _appSettings = appSettings;
            _customerTurnoverService = customerTurnoverService;
            _turnoverService = turnoverService;
        }
    }
}