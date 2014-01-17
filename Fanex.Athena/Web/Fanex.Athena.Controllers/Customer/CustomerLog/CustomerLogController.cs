// -----------------------------------------------------------------------
// <copyright file="CustomerLogController.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Customer.CustomerLog
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DTO.Customer.CustomerLog;
    using Fanex.Athena.Models.MetaModel.Customer.CustomerLog;
    using Fanex.Athena.Models.ViewModel.Customer.CustomerLog;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    /// Customer log.
    /// </summary>
    public partial class CustomerLogController : ReadonlyBaseController
    {
        #region Field

        /// <summary>
        /// Product service.
        /// </summary>
        private readonly ProductService _productService;

        #endregion Field

        #region Constructor

        public CustomerLogController()
        {
            this._productService = new ProductService();
        }

        /// <summary>
        /// Customer Log index.
        /// </summary>
        /// <returns>Log in log view.</returns>
        [FunctionAuthorization]
        public ActionResult Index()
        {
            var model = new LogInLogViewModel { Param = new CustomerLogMetaModel(), MainData = new List<LogInLog>() };
            return this.View("~/Views/Customer/CustomerLog/LogInLog/Main.cshtml", model);
        }

        #endregion Constructor
    }
}