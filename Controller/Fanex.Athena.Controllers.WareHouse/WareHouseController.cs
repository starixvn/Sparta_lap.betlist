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
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Model.Core.DTO.Configuration;
    using ONEbook.UM.Filters;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class WareHouseBaseController : BaseController
    {
        private readonly IBalanceUpDataAccess _balancUpDataAccess;

        private readonly IProductService _productService;

        public WareHouseBaseController() : this(null, null, null, null, null, null)
        {
        }

        public WareHouseBaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IBalanceUpDataAccess balancUpDataAccess,
            IProductService productService)
        : base(languageService, sessionService, appSetting, clientViewHelper)
        {
            this._productService = productService ?? new ProductService();
            this._balancUpDataAccess = balancUpDataAccess ?? new BalanceUpDataAccess();
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