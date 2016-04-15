// -----------------------------------------------------------------------
// <copyright file="BetSettingBaseController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System.Collections.Generic;
    using System.Linq;
    using Extension.Utility;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Athena.Service.Implement.Customer;
    using Fanex.Athena.Service.Interface.Customer;
    using Models.DTO.Common;
    using Models.MetaModel.BetSettings;
    using Models.ViewModel.BetSettings;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Implement.BetSettings;
    using Service.Interface.BetSettings;

    public class BetSettingBaseController : ReadonlyBaseController
    {
        protected const string CustomerListUrl = "Customer/CustomerList";

        private const int BopingResellerId = 7;

        private readonly ICoreBetSettingService _coreBetSettingService;
        private readonly ILicenseeService _licenseeService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly ProductAccess _productAccess;

        public BetSettingBaseController()
        {
            this._coreBetSettingService = new CoreBetSettingService();
            this._licenseeService = new LicenseeService();
            this._customerService = new CustomerService();
            this._productService = new ProductService();

            _productAccess = _productService.GetProductAccess(
                    SessionService.LicenseeSession.LicenseeId,
                    SessionService.UserSession.Language);
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
            get { return _productAccess; }
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
            var customerInfo = _customerService.GetCustomerInfo(custId);

            var betSettingProducts = _coreBetSettingService.GetBetSettingProducts(SessionService.LicenseeSession.LicenseeId);

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

        private bool IsBetSettingEditable()
        {
            var setting = AppSettings.GetValue("LicenseesEditableBetSettings");
            var licenseeIds = string.IsNullOrEmpty(setting) ? new string[] { } : setting.Split(',');
            if (licenseeIds.Contains(SessionService.LicenseeSession.LicenseeId.ToString()))
            {
                return true;
            }
            else
            {
                var licensees = _licenseeService.GetByResellerId(SessionService.UserSession.UserId, BopingResellerId);
                return licensees.Contains(SessionService.LicenseeSession.LicenseeId);
            }
        }
    }
}