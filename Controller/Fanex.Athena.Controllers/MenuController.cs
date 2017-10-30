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
    using Extension.Utility.Interfaces;
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
            IFileSystem fileSystem,
            IFunctionService functionService)
            : base(languageService, sessionService, appSetting, fileSystem)
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
                IsFakeLicensee = IsFakeLicensee()
            };

            viewModel.MasterFunctions = _functionService.GetMasterFunctions(viewModel.UserFunctions);
            viewModel.UserDetailFunctions = _functionService.GetSubMenus(viewModel.UserFunctions);

            return View("~/Views/Menu/Index.cshtml", viewModel);
        }
    }
}