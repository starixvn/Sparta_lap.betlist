// -----------------------------------------------------------------------
// <copyright file="MenuController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.ViewModel;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Model.Core.DataDomain;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;

    [AuthorizeRestriction(AppMode.Self)]
    public class MenuController : BaseController
    {
        private readonly IFunctionService _functionService;

        public MenuController()
        {
            _functionService = new FunctionService();
        }

        public MenuController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IFunctionService functionService)
            : base(languageService, sessionService, appSetting)
        {
            _functionService = functionService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var userSession = SessionService.UserSession;
            var licenseeSession = SessionService.LicenseeSession;
            var systemFunctions = userSession.FunctionIdList;
            var excludeFunctions = AppSettings.ExcludeFunctionIdList;
            var canManageMember = userSession.FunctionIdList.Contains((int)FunctionType.CustomerList);
            var viewModel = new MenuViewModel
            {
                UserFunctions = _functionService.Functions.Where(i => systemFunctions.Contains(i.FunctionId)
              && !excludeFunctions.Contains(i.FunctionId)).ToList(),
                IsAllowedCustonmerCommission = licenseeSession.IsAllowedCustomerComm,
                IsInternalLogin = userSession.IsInternal,
                IsAllowedManagementMember = canManageMember
            };

            viewModel.MasterFunctions = _functionService.GetMasterFunctions(viewModel.UserFunctions);
            viewModel.UserDetailFunctions = _functionService.GetSubMenus(viewModel.UserFunctions);

            return View("~/Views/Menu/Index.cshtml", viewModel);
        }
    }
}