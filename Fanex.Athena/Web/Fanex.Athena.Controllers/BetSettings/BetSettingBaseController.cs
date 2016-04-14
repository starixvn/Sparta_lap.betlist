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
    using Fanex.Athena.Models.DTO.Customer.BetSetting;
    using Fanex.Athena.Models.MetaModel.Customer.BetSetting;
    using Fanex.Athena.Service.Implement.Customer;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Customer;
    using Fanex.Athena.Service.Interface.Users;
    using Service.Implement.BetSettings;
    using Service.Interface.BetSettings;

    public class BetSettingBaseController : ReadonlyBaseController
    {
        protected const string CustomerListUrl = "Customer/CustomerList";

        private const int BopingResellerId = 7;

        private readonly ICoreBetSettingService _coreBetSettingService;
        private readonly ILicenseeService _licenseeService;
        private readonly ICustomerService _customerService;

        public BetSettingBaseController()
        {
            this._coreBetSettingService = new CoreBetSettingService();
            this._licenseeService = new LicenseeService();
            this._customerService = new CustomerService();
        }
        
        protected ILicenseeService LicenseeService
        {
            get { return _licenseeService; }
        }

        protected ICustomerService CustomerService
        {
            get { return _customerService; }
        }

        protected IList<BetSettingProduct> GetBetSettingProducts(int custId)
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

        protected bool IsBetSettingEditable(int userId, int licenseeId)
        {
            var setting = AppSettings.GetValue("LicenseesEditableBetSettings");
            var licenseeIds = string.IsNullOrEmpty(setting) ? new string[] { } : setting.Split(',');
            if (licenseeIds.Contains(licenseeId.ToString()))
            {
                return true;
            }
            else
            {
                var licensees = _licenseeService.GetByResellerId(userId, BopingResellerId);
                return licensees.Contains(licenseeId);
            }
        }

        private void DisableBetSettingProduct(IEnumerable<BetSettingProduct> products, int productId)
        {
            var product = products.Where(x => x.IsEnable && x.ProductId == productId).FirstOrDefault();
            if (product != null)
            {
                product.IsEnable = false;
            }
        }
    }
}