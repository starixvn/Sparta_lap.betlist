// -----------------------------------------------------------------------
// <copyright file="WareHouseController.cs" company="Nexcel">
// Report:warehouse controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.WareHouse
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Implement.Warehouse;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Users;
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