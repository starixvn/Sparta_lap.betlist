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
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Model.Core.DataDomain;
    using Service.Core.Users.Interfaces;
    using Session.Enum;
    using Web.ActionFilter;

    [AuthorizeRestriction(AppMode.Self)]
    public class MenuController : CultureBaseController
    {
        private readonly IFunctionService _functionService;

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

            var canManageMember = userSession.FunctionIdList.Contains((int)FunctionType.CustomerList);

            var viewModel = new MenuViewModel
            {
                UserFunctions = _functionService.Functions.Where(i => systemFunctions.Contains(i.FunctionId)).ToList(),
                IsAllowedCustonmerCommission = licenseeSession.IsAllowedCustomerComm,
                IsInternalLogin = userSession.IsInternal,
                IsAllowedManagementMember = canManageMember,
                IsFakeLicensee = IsFakeLicensee(licenseeSession.LicenseeId)
            };

            viewModel.MasterFunctions = _functionService.GetMasterFunctions(viewModel.UserFunctions);
            viewModel.UserDetailFunctions = _functionService.GetSubMenus(viewModel.UserFunctions);

            return View("~/Views/Desktop/Menu/Index.cshtml", viewModel);
        }
    }
}