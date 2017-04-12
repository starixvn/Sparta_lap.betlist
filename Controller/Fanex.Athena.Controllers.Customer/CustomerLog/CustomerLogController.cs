// -----------------------------------------------------------------------
// <copyright file="CustomerLogController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Customer.CustomerLog
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;
    using ONEbook.UM.Filters;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerLog)]
    public partial class CustomerLogController : BaseController
    {
        private readonly ProductService _productService;

        public CustomerLogController()
        {
            this._productService = new ProductService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return LogInLog();
        }
    }
}