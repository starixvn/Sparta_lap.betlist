// -----------------------------------------------------------------------
// <copyright file="CustomerLogController.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Customer.CustomerLog
{
    using System.Web.Mvc;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    /// Customer log.
    /// </summary>
    public partial class CustomerLogController : ReadonlyBaseController
    {
        private const string CustomerLogUrl = "CustomerLog/LogInLog";

        private readonly ProductService _productService;

        public CustomerLogController()
        {
            this._productService = new ProductService();
        }

        [FunctionAuthorization(ParentUrl = CustomerLogUrl)]
        public ActionResult Index()
        {
            return LogInLog();
        }
    }
}