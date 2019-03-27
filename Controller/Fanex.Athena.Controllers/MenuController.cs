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
        public async Task<ActionResult> Index()
        {
            var userSession = SessionService.UserSession;
            var licenseeSession = SessionService.LicenseeSession;
            var systemFunctions = userSession.FunctionIdList;

            var canManageMember = userSession.FunctionIdList.Contains((int)FunctionType.CustomerList);

            var viewModel = new MenuViewModel
            {
                UserFunctions = (await _functionService.GetFunctions().ConfigureAwait(false))
                                    .Where(i => systemFunctions.Contains(i.FunctionId)).ToList(),
                IsAllowedCustonmerCommission = licenseeSession.IsAllowedCustomerComm,
                IsInternalLogin = userSession.IsInternal,
                IsAllowedManagementMember = canManageMember,
                IsFakeLicensee = IsFakeLicensee()
            };

            viewModel.MasterFunctions = await _functionService.GetMasterFunctions(viewModel.UserFunctions).ConfigureAwait(false);
            viewModel.UserDetailFunctions = await _functionService.GetSubMenus(viewModel.UserFunctions).ConfigureAwait(false);

            return View("~/Views/Desktop/Menu/Index.cshtml", viewModel);
        }
    }
}