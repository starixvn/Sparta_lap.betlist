// -----------------------------------------------------------------------
// <copyright file="CustomerLogController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Customer.CustomerLog
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using ONEbook.UM.Filters;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Helpers;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerLog)]
    public partial class CustomerLogController : BaseController
    {
        private readonly IProductService _productService;

        public CustomerLogController()
        {
            this._productService = new ProductService();
        }

        public CustomerLogController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
                   IClientViewHelper clientViewHelper
            )
            : base(languageService, sessionService, appSetting, clientViewHelper)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return LogInLog();
        }
    }
}