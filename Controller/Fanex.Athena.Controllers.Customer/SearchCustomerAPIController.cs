namespace Fanex.Athena.Controllers.Customer
{
    #region Using

    using System.Web.Mvc;
    using System.Web.SessionState;
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Helpers;
    using Fanex.Athena.Service.Customer;
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
        PermissionKeys.CustomerInfoCustomerList)]
    public class SearchCustomerAPIController : BaseController
    {
        private readonly ICustomerListService _customerListService;

        public SearchCustomerAPIController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            ICustomerListService customerListService)
            : base(languageService, sessionService, appSetting, clientViewHelper)
        {
            _customerListService = customerListService;
        }

        public SearchCustomerAPIController()
        {
            _customerListService = new CustomerListService();
        }

        [AjaxRequest]
        [HttpGet]
        public JsonResult SearchCustomer(string query)
        {
            var userName = query.Trim();
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var userId = SessionService.UserSession.UserId;
            var customerNames = _customerListService.SearchCustomers(licenseeId, userId, userName, SessionService.GenerateAgentListXml());

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