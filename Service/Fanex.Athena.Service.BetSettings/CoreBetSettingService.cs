using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.Constant.DataDomain;
using Fanex.Athena.DataAccess.BetSettings.Interfaces;
using Fanex.Athena.Model.BetSettings.DTO;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Model.Customer.DTO;
using Fanex.Athena.Service.BetSettings.Interfaces;
using Fanex.Athena.Service.Core.Users.Interfaces;
using Fanex.Athena.Service.Customer.Interfaces;
using Fanex.Athena.Service.StatelessCore.Interfaces;
using Fanex.Athena.Session.Enum;
using Fanex.Athena.Session.Model;

namespace Fanex.Athena.Service.BetSettings
{
    public class CoreBetSettingService : ICoreBetSettingService
    {
        private readonly IAuthorizationService _authorizationService;

        private readonly ICoreBetSettingDataAccess _coreBetSettingDataAccess;

        private readonly ICustomerService _customerService;

        public CoreBetSettingService(
            IAuthorizationService authorizationService,
            ICoreBetSettingDataAccess coreBetSettingDataAccess,
            ICustomerService customerService)
        {
            _authorizationService = authorizationService;
            _coreBetSettingDataAccess = coreBetSettingDataAccess;
            _customerService = customerService;
        }

        public CustomerInfo GetCustomerInfo(int custId, int licenseeId)
        {
            return _customerService.GetCustomerInfo(licenseeId, custId);
        }

        public bool IsBetSettingEditable(UserSession userSession)
        {
            return userSession.ApplicationMode == AppMode.Parasitic ?
                      _authorizationService.HasPermissions(userSession.PortalFunctionList, PermissionKeys.CustomerInfoEditBetSetting) :
                      _authorizationService.HasPermissions(userSession.FunctionIdList, PermissionKeys.CustomerInfoEditBetSetting);
        }

        public async Task<IEnumerable<BetSettingProduct>> GetBetSettingProductsAsync(int custId, int licenseeId)
        {
            var customerInfo = _customerService.GetCustomerInfo(licenseeId, custId);

            var betSettingProducts = await _coreBetSettingDataAccess.GetBetSettingProductsAsync(licenseeId).ConfigureAwait(false);

            if (!customerInfo.SyncCasino)
            {
                DisableBetSettingProduct(betSettingProducts, ProductTypes.RngCasino);
            }

            return betSettingProducts.Where(x => x.IsEnable).ToList();
        }

        private static void DisableBetSettingProduct(IEnumerable<BetSettingProduct> products, int productId)
        {
            var product = products.FirstOrDefault(x => x.IsEnable && x.ProductId == productId);
            if (product != null)
            {
                product.IsEnable = false;
            }
        }
    }
}