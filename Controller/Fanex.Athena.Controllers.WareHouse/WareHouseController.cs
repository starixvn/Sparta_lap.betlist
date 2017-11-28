// -----------------------------------------------------------------------
// <copyright file="WareHouseBaseController.cs" company="">
// Report:warehouse controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.WareHouse
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using DataAccess.Core.Configuration;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.Base.Entities;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Model.Core.DTO.Configuration;
    using ONEbook.UM.Filters;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class WareHouseBaseController : BaseController
    {
        private readonly IBalanceUpDataAccess _balancUpDataAccess;

        private readonly IProductService _productService;

        public WareHouseBaseController()
        {
            this._productService = new ProductService();
            this._balancUpDataAccess = new BalanceUpDataAccess();
        }

        protected IProductService ProductService
        {
            get { return _productService; }
        }

        protected BalanceUp GetBalanceUp()
        {
            return _balancUpDataAccess.GetBalanceUp();
        }

        protected ShowingOption GetShowingOption()
        {
            var productAccess = ProductService.GetProductAccess(SessionService.LicenseeSession.LicenseeId, SessionService.UserSession.Language);
            return new ShowingOption
            {
                BAVisible = productAccess.IsAllowBA,
                RBVisible = productAccess.IsAllowRB,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                NGVisible = productAccess.IsAllowNG,
                SBVisible = productAccess.IsAllowSB
            };
        }
    }
}