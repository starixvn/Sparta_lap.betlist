// -----------------------------------------------------------------------
// <copyright file="MenuController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
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

            var canManageMember = userSession.FunctionIdList.Contains((int)FunctionType.CustomerList);

            var viewModel = new MenuViewModel
            {
                IsAllowedCustonmerCommission = licenseeSession.IsAllowedCustomerComm,
                IsInternalLogin = userSession.IsInternal,
                IsAllowedManagementMember = canManageMember,
                IsFakeLicensee = IsFakeLicensee()
            };
            AssignFunctionsData(viewModel, userSession.FunctionIdList);

            return View("~/Views/Desktop/Menu/Index.cshtml", viewModel);
        }

        private void AssignFunctionsData(MenuViewModel viewModel, int[] functionIdList)
        {
            // Child actions must be invoked synchronously
            Task.Run(async () =>
            {
                viewModel.UserFunctions = (await _functionService.GetFunctions())
                                        .Where(i => functionIdList.Contains(i.FunctionId)).ToList();
                viewModel.MasterFunctions = await _functionService.GetMasterFunctions(viewModel.UserFunctions);
                viewModel.UserDetailFunctions = await _functionService.GetSubMenus(viewModel.UserFunctions);
            }).Wait();
        }
    }
}