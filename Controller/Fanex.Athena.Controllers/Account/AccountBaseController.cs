// -----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Account
{
    using System.Web.Mvc;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using ONEbook.UM.Filters;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Session.Enum;
    using Web.ActionFilter;

    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.AccountManageAccount)]
    [AuthorizeRestriction(AppMode.Self)]
    public class AccountBaseController : BaseController
    {
        public AccountBaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting)
        : base(languageService, sessionService, appSetting)
        {
        }

        protected JsonResult JsonErrorResult(string message)
        {
            return Json(new ResultInfo
            {
                Code = -1,
                Message = message
            });
        }

        protected int GetUserId()
        {
            var userId = 0;
            int.TryParse(Request.Params["UserId"], out userId);
            return userId;
        }
    }
}