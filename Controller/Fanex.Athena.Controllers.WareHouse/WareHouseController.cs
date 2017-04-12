// -----------------------------------------------------------------------
// <copyright file="WareHouseBaseController.cs" company="">
// Report:warehouse controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.WareHouse
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using ONEbook.UM.Filters;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class WareHouseBaseController : BaseController
    {
        private readonly IBalanceUpService _balancUpService;
        private readonly IProductService _productService;

        public WareHouseBaseController()
        {
            this._productService = new ProductService();
            this._balancUpService = new BalanceUpService();
        }

        protected IBalanceUpService BalanceUpService
        {
            get { return _balancUpService; }
        }

        protected IProductService ProductService
        {
            get { return _productService; }
        }
    }
}