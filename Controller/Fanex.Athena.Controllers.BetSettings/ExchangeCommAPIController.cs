// -----------------------------------------------------------------------
// <copyright file="ExchangeComm.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using ONEbook.UM.Filters;
    using Resources;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;
    using Service.Core.Users.Interfaces;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class ExchangeCommApiController : BaseController
    {
        private static readonly decimal[] ValidComms = new[] { 2, 2.5m, 3, 3.5m, 4, 4.5m, 5, 5.5m, 6, 6.5m, 7 };

        private readonly IExchangeCommApiService _exchangeCommApiService;

        public ExchangeCommApiController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IFileSystem fileSystem,
            IExchangeCommApiService exchangeCommApiService)
            : base(languageService, sessionService, appSetting, fileSystem)
        {
            _exchangeCommApiService = exchangeCommApiService;
        }

        public ExchangeCommApiController()
        {
            _exchangeCommApiService = new ExchangeCommApiService();
        }

        [HttpPost]
        [AjaxRequest]
        [CustomValidateAntiForgery]
        public ActionResult UpdateComm(int custId, decimal commission)
        {
            if (!ValidComms.Contains(commission))
            {
                return Json(new ResultInfo { Code = 100, Message = ViewResources.InvalidChargeCommission });
            }

            var result = _exchangeCommApiService.UpdateCommission(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.UserId,
                custId,
                commission);
            return Json(new ResultInfo { Code = 0, ExtendedData = result.ToString(), Message = ViewResources.UpdateCommSucessfulMsg });
        }
    }
}