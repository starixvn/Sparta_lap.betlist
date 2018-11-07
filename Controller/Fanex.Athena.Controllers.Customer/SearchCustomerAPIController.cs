namespace Fanex.Athena.Controllers.Customer
{
    #region Using

    using System.Web.Mvc;
    using System.Web.SessionState;
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using ONEbook.UM.Filters;
    using Service.Core.Users.Interfaces;

    #endregion Using

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    [FunctionAuthorization(
        PermissionKeys.ReportTurnover,
        PermissionKeys.ReportWinLossByProduct,
        PermissionKeys.ReportCustomerWinLoss,
        PermissionKeys.CustomerInfoCustomerLog,
        PermissionKeys.CustomerInfoCustomerList,
        PermissionKeys.BettingTicketCenter)]
    public class SearchCustomerAPIController : BaseController
    {
        private readonly ICustomerListService _customerListService;

        public SearchCustomerAPIController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ICustomerListService customerListService)
            : base(languageService, sessionService, appSetting)
        {
            _customerListService = customerListService;
        }

        [AjaxRequest]
        [HttpGet]
        public JsonResult SearchCustomer(string query)
        {
            var userName = query.Trim();
            var userId = SessionService.UserSession.UserId;
            var customerNames = _customerListService.SearchCustomers(userId, userName, SessionService.GenerateAgentListXml());

            var searchResult = new
            {
                data = new[] { string.Empty },
                query = userName,
                suggestions = customerNames
            };

            return Json(searchResult, JsonRequestBehavior.AllowGet);
        }
    }
}