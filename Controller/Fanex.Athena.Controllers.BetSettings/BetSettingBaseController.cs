// -----------------------------------------------------------------------
// <copyright file="BetSettingBaseController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetSettings
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Extension.Utility;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DataDomain;
    using Model.BetSettings.MetaModel;
    using Model.BetSettings.ViewModel;
    using Model.Core.DTO.Common;
    using Model.Customer.DTO;
    using ONEbook.UM.Filters;
    using Resources;
    using Service.BetSettings.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    [FunctionAuthorization(PermissionKeys.CustomerInfoCustomerList)]
    public class BetSettingBaseController : ContentBaseController
    {
        private readonly ICoreBetSettingService _coreBetSettingService;

        private readonly IProductService _productService;
        private readonly Lazy<ProductAccess> _productAccess;

        protected BetSettingBaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ICoreBetSettingService coreBetSettingService,
            IProductService productService)
        : base(languageService, sessionService, appSetting)
        {
            _coreBetSettingService = coreBetSettingService;
            _productService = productService;
            _productAccess = new Lazy<ProductAccess>(() =>
                _productService.GetProductAccess(
                    SessionService.LicenseeSession.LicenseeId,
                    SessionService.UserSession.Language));
        }

        protected ProductAccess ProductAccess
        {
            get
            {
                return _productAccess.Value;
            }
        }

        protected JsonResult AccessDenied()
        {
            return Json(new ResultInfo { Code = -1, Message = ViewResources.AccessDeniedMsg });
        }

        protected CustomerInfo GetCustomerInfo(int custId)
        {
            return _coreBetSettingService.GetCustomerInfo(custId);
        }

        protected async Task AssignRequiredProperties(BetSettingBaseViewModel viewModel, BetSettingMetaModel metaModel)
        {
            metaModel.IsFakeLicensee = IsFakeLicensee();
            viewModel.Param = metaModel;
            viewModel.Editable = await IsBetSettingEditable().ConfigureAwait(false);
            viewModel.BetSettingProducts = await _coreBetSettingService.GetBetSettingProducts(metaModel.CustId).ConfigureAwait(false);
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

        protected async Task<bool> IsBetSettingEditable()
        {
            return await _coreBetSettingService.IsBetSettingEditable().ConfigureAwait(false);
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
    }
}