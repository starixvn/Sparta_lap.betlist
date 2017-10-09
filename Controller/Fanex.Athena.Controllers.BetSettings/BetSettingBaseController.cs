// -----------------------------------------------------------------------
// <copyright file="BetSettingBaseController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using DataAccess.BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Model.Customer.DTO;
    using ONEbook.UM.Filters;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;
    using Service.Customer;
    using Service.Customer.Interfaces;
    using Web.ActionFilter;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class BetSettingBaseController : BaseController
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICoreBetSettingDataAccess _coreBetSettingDataAccess;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly Lazy<ProductAccess> _productAccess;

        public BetSettingBaseController()
        {
            _authorizationService = new AuthorizationService();
            this._coreBetSettingDataAccess = new CoreBetSettingDataAccess();
            this._customerService = new CustomerService();
            this._productService = new ProductService();
            _productAccess = new Lazy<ProductAccess>(() =>
                _productService.GetProductAccess(
                    SessionService.LicenseeSession.LicenseeId,
                    SessionService.UserSession.Language));
        }

        protected IProductService ProductService
        {
            get { return _productService; }
        }

        protected ProductAccess ProductAccess
        {
            get
            {
                return _productAccess.Value;
            }
        }

        protected CustomerInfo GetCustomerInfo(int licenseeId, int custId)
        {
            return _customerService.GetCustomerInfo(licenseeId, custId);
        }

        protected void AssignRequiredProperties(BetSettingBaseViewModel viewModel, BetSettingMetaModel metaModel)
        {
            metaModel.IsFakeLicensee = IsFakeLicensee();
            viewModel.Param = metaModel;
            viewModel.Editable = IsBetSettingEditable();
            viewModel.BetSettingProducts = GetBetSettingProducts(metaModel.CustId);
        }

        protected CommonBetSetting GetBetSettingCommonParam()
        {
            var userSession = SessionService.UserSession;

            var commonItem = new CommonBetSetting
            {
                LicenseeId = SessionService.LicenseeSession.LicenseeId,
                Ip = Request.UserHostAddress,
                Language = userSession.Language,
                UserId = userSession.UserId,
                CustId = int.Parse(Request["custid"])
            };

            return commonItem;
        }

        protected bool IsBetSettingEditable()
        {
            var userSession = SessionService.UserSession;
            return userSession.ApplicationMode == AppMode.Parasitic ?
                      _authorizationService.HasPermissions(userSession.PortalFunctionList, PermissionKeys.CustomerInfoEditBetSetting) :
                      _authorizationService.HasPermissions(userSession.FunctionIdList, PermissionKeys.CustomerInfoEditBetSetting);
        }

        protected decimal GetDecimalRequest(string name, decimal defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Request[name]))
            {
                return defaultValue;
            }

            decimal value;
            return !decimal.TryParse(HttpContext.Request[name], out value)
                       ? defaultValue
                       : value;
        }

        protected int GetIntRequest(string name, int defaultValue)
        {
            if (string.IsNullOrEmpty(HttpContext.Request[name]))
            {
                return defaultValue;
            }

            int value;
            return !int.TryParse(HttpContext.Request[name], out value)
                       ? defaultValue
                       : value;
        }

        private static void DisableBetSettingProduct(IEnumerable<BetSettingProduct> products, int productId)
        {
            var product = products.FirstOrDefault(x => x.IsEnable && x.ProductId == productId);
            if (product != null)
            {
                product.IsEnable = false;
            }
        }

        private IEnumerable<BetSettingProduct> GetBetSettingProducts(int custId)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var customerInfo = _customerService.GetCustomerInfo(licenseeId, custId);

            var betSettingProducts = _coreBetSettingDataAccess.GetBetSettingProducts(licenseeId);

            if (!customerInfo.SyncCasino)
            {
                DisableBetSettingProduct(betSettingProducts, ProductTypes.RngCasino);
            }

            if (!customerInfo.SyncLiveCasino)
            {
                DisableBetSettingProduct(betSettingProducts, ProductTypes.LiveCasino);
            }

            return betSettingProducts.Where(x => x.IsEnable).ToList();
        }
    }
}