// -----------------------------------------------------------------------
// <copyright file="WareHouseController.cs" company="Nexcel">
// Report:warehouse controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.WareHouse
{
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Implement.Warehouse;
    using Fanex.Athena.Service.Interface.Warehouse;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class WareHouseController : ReadonlyBaseController
    {
        #region field

        private readonly IBalanceUpService _balancUpService;
        private readonly IProductService _productService;
        private readonly ITopCustomerService _topCustomerService;
        private readonly IWinLossService _winLossService;

        #endregion field

        public WareHouseController()
        {
            this._winLossService = new WinLossService();
            this._topCustomerService = new TopCustomerService();
            this._productService = new ProductService();
            this._balancUpService = new BalanceUpService();
        }
    }
}