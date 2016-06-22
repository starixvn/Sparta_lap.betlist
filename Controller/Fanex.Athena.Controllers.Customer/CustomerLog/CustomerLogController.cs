// -----------------------------------------------------------------------
// <copyright file="CustomerLogController.cs" company="">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Customer.CustomerLog
{
    using System.Web.Mvc;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Web.ActionFilter;
    using Model.Core.DataDomain;

    /// <summary>
    /// Customer log.
    /// </summary>
    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerLog)]
    public partial class CustomerLogController : ReadonlyBaseController
    {
        private const string CustomerLogUrl = "CustomerLog/LogInLog";

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