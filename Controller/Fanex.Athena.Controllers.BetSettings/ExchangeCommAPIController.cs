// -----------------------------------------------------------------------
// <copyright file="ExchangeComm.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
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
            _exchangeCommApiService.UpdateCommission(custId, commission);
            return Json(new ResultInfo { Code = 0, Message = ViewResources.UpdateCommSucessfulMsg });
        }
    }
}