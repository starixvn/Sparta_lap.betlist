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
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Athena.Service.Customer;
    using Fanex.Athena.Service.Customer.Interfaces;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Service.BetSettings;
    using Service.BetSettings.Interfaces;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class BetSettingBaseController : ReadonlyBaseController
    {
        private const int BopingResellerId = 7;

        private readonly ICoreBetSettingService _coreBetSettingService;
        private readonly ILicenseeService _licenseeService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly Lazy<ProductAccess> _productAccess;

        public BetSettingBaseController()
        {
            this._coreBetSettingService = new CoreBetSettingService();
            this._licenseeService = new LicenseeService();
            this._customerService = new CustomerService();
            this._productService = new ProductService();
            _productAccess = new Lazy<ProductAccess>(() =>
                _productService.GetProductAccess(
                    SessionService.LicenseeSession.LicenseeId,
                    SessionService.UserSession.Language));
        }

        protected ICustomerService CustomerService
        {
            get { return _customerService; }
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

        protected void AssignRequiredProperties(BetSettingBaseViewModel viewModel, BetSettingMetaModel metaModel)
        {
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
            return _coreBetSettingService.IsBetSettingEditable(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.UserId);
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

        private static void DisableBetSettingProduct(IEnumerable<BetSettingProduct> products, int productId)
        {
            var product = products.Where(x => x.IsEnable && x.ProductId == productId).FirstOrDefault();
            if (product != null)
            {
                product.IsEnable = false;
            }
        }

        private IEnumerable<BetSettingProduct> GetBetSettingProducts(int custId)
        {
            var licenseeId = SessionService.LicenseeSession.LicenseeId;
            var customerInfo = _customerService.GetCustomerInfo(licenseeId, custId);

            var betSettingProducts = _coreBetSettingService.GetBetSettingProducts(licenseeId);

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