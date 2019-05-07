namespace Fanex.Athena.Controllers.BetSettings.UnitTests
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Extension.Utility;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Customer.DTO;
    using Service.BetSettings.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Session.Model;

    internal class FakeBetSettingBaseController : BetSettingBaseController
    {
        internal FakeBetSettingBaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ICoreBetSettingService coreBetSettingService,
            IProductService productService)
        : base(languageService, sessionService, appSetting, coreBetSettingService, productService)
        {
        }

        internal new JsonResult AccessDenied()
        {
            return base.AccessDenied();
        }

        internal new CustomerInfo GetCustomerInfo(int custId)
        {
            return base.GetCustomerInfo(custId);
        }

        internal new async Task AssignRequiredProperties(BetSettingBaseViewModel viewModel, BetSettingMetaModel metaModel)
        {
            await base.AssignRequiredProperties(viewModel, metaModel);
        }

        internal new CommonBetSetting GetBetSettingCommonParam(int userId, int licenseeId, string language)
        {
            return base.GetBetSettingCommonParam(userId, licenseeId, language);
        }

        internal new bool IsBetSettingEditable(UserSession userSession)
        {
            return base.IsBetSettingEditable(userSession);
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