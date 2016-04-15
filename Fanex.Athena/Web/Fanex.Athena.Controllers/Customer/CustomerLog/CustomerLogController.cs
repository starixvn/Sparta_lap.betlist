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
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    /// Customer log.
    /// </summary>
    public partial class CustomerLogController : ReadonlyBaseController
    {
        private const string CustomerLogUrl = "CustomerLog/LogInLog";

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
        [FunctionAuthorization(ParentUrl = CustomerLogUrl)]
        public ActionResult Index()
        {
            return LogInLog();
        }

        #endregion Constructor
    }
}