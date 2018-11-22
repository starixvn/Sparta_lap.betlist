namespace Fanex.Athena.Controllers.Customer
{
    #region Using

    using System.Web.Mvc;
    using System.Web.SessionState;
    using DataAccess.Customer.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Common.Interfaces;
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
        private readonly ISearchCustomerDataAccess _searchCustomerDataAccess;

        public SearchCustomerAPIController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ISearchCustomerDataAccess searchCustomerDataAccess)
            : base(languageService, sessionService, appSetting)
        {
            _searchCustomerDataAccess = searchCustomerDataAccess;
        }

        [AjaxRequest]
        [HttpGet]
        public JsonResult SearchCustomer(string query)
        {
            return Search(query, false);
        }

        [AjaxRequest]
        [HttpGet]
        public JsonResult SearchActiveAndDormantCustomer(string query)
        {
            return Search(query, true);
        }

        private JsonResult Search(string query, bool isIncludeDormant)
        {
            var userName = query.Trim();
            var userId = SessionService.UserSession.UserId;
            var customerNames = _searchCustomerDataAccess.SearchCustomers(userId, userName, SessionService.GenerateAgentListXml(), isIncludeDormant);

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