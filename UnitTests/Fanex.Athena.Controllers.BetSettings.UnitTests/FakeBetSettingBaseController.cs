namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System.Web.Mvc;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Customer.DTO;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Service.Customer.Interfaces;

    internal class FakeBetSettingBaseController : BetSettingBaseController
    {
        internal FakeBetSettingBaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IAuthorizationService authorizationService,
            ICoreBetSettingDataAccess coreBetSettingDataAccess,
            ICustomerService customerService,
            IProductService productService)
#pragma warning restore S107 // Methods should not have too many parameters
        : base(languageService, sessionService, appSetting, clientViewHelper, authorizationService, coreBetSettingDataAccess, customerService, productService)
        {
        }

        internal new JsonResult AccessDenied()
        {
            return base.AccessDenied();
        }

        internal new CustomerInfo GetCustomerInfo(int licenseeId, int custId)
        {
            return base.GetCustomerInfo(licenseeId, custId);
        }

        internal new void AssignRequiredProperties(BetSettingBaseViewModel viewModel, BetSettingMetaModel metaModel)
        {
            base.AssignRequiredProperties(viewModel, metaModel);
        }

        internal new CommonBetSetting GetBetSettingCommonParam()
        {
            return base.GetBetSettingCommonParam();
        }

        internal new bool IsBetSettingEditable()
        {
            return base.IsBetSettingEditable();
        }

        internal new decimal GetDecimalRequest(string name, decimal defaultValue)
        {
            return base.GetDecimalRequest(name, defaultValue);
        }

        internal new int GetIntRequest(string name, int defaultValue)
        {
            return base.GetIntRequest(name, defaultValue);
        }
    }
}