// -----------------------------------------------------------------------
// <copyright file="MenuController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Interface.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MenuController : ReadonlyBaseController
    {
        private readonly IFunctionService _functionManager = new FunctionService();
        private readonly IFunctionAccessService _functionAccess = new FunctionAccessService();

        /// <summary>
        /// The menu page.
        /// </summary>
        /// <returns>The menu list.</returns>
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var userSession = SessionService.UserSession;
            var licenseeSession = SessionService.LicenseeSession;
            var systemFunctions = userSession.FunctionIdList;
            var excludeFunctions = AppSettings.ExcludeFunctionIdList;
            var viewModel = new MenuViewModel
            {
                UserFunctions = _functionManager.Functions.Where(i => systemFunctions.Contains(i.FunctionId)
              && !excludeFunctions.Contains(i.FunctionId)).ToList(),
                IsAllowedCustonmerCommission = licenseeSession.IsAllowedCustomerComm,
                IsInternalLogin = userSession.IsInternal,
                IsAllowedManagementMember = _functionAccess.CanManageMember
            };

            viewModel.MasterFunctions = _functionManager.GetMasterFunctions(viewModel.UserFunctions);
            viewModel.UserDetailFunctions = _functionManager.GetSubFunctions(viewModel.UserFunctions);

            return View("~/Views/Menu/Index.cshtml", viewModel);
        }
    }
}