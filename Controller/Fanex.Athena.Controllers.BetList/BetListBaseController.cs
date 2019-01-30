namespace Fanex.Athena.Controllers.BetList
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.Base.Helpers;
    using Extension.Utility;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Model.Core.DTO.Configuration;
    using ONEbook.UM.Filters;
    using Resources;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using VendorAdapter.Interfaces;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class BetListBaseController : BaseController
    {
        protected const int PageSizeExcel = 63000;

        protected const int ExternalIdColumn = 1;
        protected const int SystemIdColumn = 2;
        protected const int WinLostColumn = 7;
        protected const int BetEffectiveColumn = 8;
        protected const int LicenseeWLColumn = 9;

        protected static readonly int[] HiddenColumnIndexes = new[] { WinLostColumn, BetEffectiveColumn, LicenseeWLColumn };

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly IProductService _productService;

#pragma warning disable S107 // Methods should not have too many parameters

        public BetListBaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService,
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter)
#pragma warning restore S107 // Methods should not have too many parameters
        : base(languageService, sessionService, appSetting)
        {
            _productService = productService;
            _balanceUpDataAccess = balanceUpDataAccess;
            _cacheRefAdapter = cacheRefAdapter;
        }

        protected IDictionary<int, string> UserCurrencyList => SessionService.UserSession.CurrencyList;

        protected IProductService ProductService
        {
            get { return _productService; }
        }

        protected ICacheRefAdapter CacheRefAdapter
        {
            get { return _cacheRefAdapter; }
        }

        protected static string GetTitleByTransType(string productTitle, long transType)
        {
            const int TransTypeYesterday = 0;
            const int TransTypeTodayRunning = 3;

            var title = productTitle + ViewResources.BetList + " - " + ViewResources.DetailTickets + " - ";
            switch (transType)
            {
                case TransTypeYesterday:
                    title += ViewResources.Yesterday;
                    break;

                case TransTypeTodayRunning:
                    title += ViewResources.Today + " - " + ViewResources.Running;
                    break;

                default:
                    title += ViewResources.Today + " - " + ViewResources.Settled;
                    break;
            }

            return title;
        }

        protected static string GetViewInCurrency(BetListMetaModel metaModel)
        {
            return string.Format(
                ViewResources.Viewin,
                metaModel.IsConvert ? metaModel.BaseCurrency.ToUpperInvariant() : metaModel.CurrencyName);
        }

        protected static string GetViewInCurrency(int curencyId, string defaultCurrencyName)
        {
            string viewinCurrency;
            switch (curencyId)
            {
                case BaseCurrencyType.All:
                    viewinCurrency = ViewResources.AllCurrencies;
                    break;

                case BaseCurrencyType.Eur:
                    viewinCurrency = string.Format(ViewResources.Viewin, FormatKey.Eur);
                    break;

                case BaseCurrencyType.Usd:
                    viewinCurrency = string.Format(ViewResources.Viewin, FormatKey.Usd);
                    break;

                case BaseCurrencyType.Gbp:
                    viewinCurrency = string.Format(ViewResources.Viewin, FormatKey.Gbp);
                    break;

                default:
                    viewinCurrency = string.Format(ViewResources.Viewin, defaultCurrencyName);
                    break;
            }

            return viewinCurrency;
        }

        protected static string GetCustomerBetListTitle(BetListMetaModel metaModel, string currencyTitle)
        {
            return ViewResources.Customer + " " + ViewResources.BetList + string.Format(
                 " - {0} --> {1} - {2}{3} - {4}",
                 metaModel.FromDate.ToString(FormatKey.ShortDateFormat),
                 metaModel.ToDate.ToString(FormatKey.ShortDateFormat),
                 metaModel.ExternalId,
                 metaModel.IsFakeLicensee ? string.Empty : $"({metaModel.SystemId})",
                 currencyTitle);
        }

        protected BalanceUp GetBalanceUp()
        {
            return _balanceUpDataAccess.GetBalanceUp();
        }

        /// <summary>
        /// Set the Licensee Id, Balance Up Date.
        /// Remove the UTC part of DateTime value.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The bet list metal model.</returns>
        protected BetListMetaModel CreateDefaultMetaModel(BetListMetaModel metaModel = null)
        {
            var defaultMetaModel = metaModel ?? new BetListMetaModel();
            defaultMetaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            defaultMetaModel.UserId = SessionService.UserSession.UserId;
            defaultMetaModel.HasPlayerCom = SessionService.LicenseeSession.IsAllowedCustomerComm;

            var balanceUp = GetBalanceUp();
            defaultMetaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUp.BalanceUpDay.Date, DateTimeKind.Unspecified);
            defaultMetaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUp.MoveBetsDay.Date, DateTimeKind.Unspecified);
            defaultMetaModel.FromDate = DateTime.SpecifyKind(defaultMetaModel.FromDate.Date, DateTimeKind.Unspecified);
            defaultMetaModel.ToDate = DateTime.SpecifyKind(defaultMetaModel.ToDate.Date, DateTimeKind.Unspecified);
            defaultMetaModel.Date = DateTime.SpecifyKind(defaultMetaModel.Date.Date, DateTimeKind.Unspecified);

            defaultMetaModel.IsFakeLicensee = IsFakeLicensee();
            defaultMetaModel.CurrencyList = UserCurrencyList;
            defaultMetaModel.AgentListXml = SessionService.GenerateAgentListXml(defaultMetaModel.CurrencyId);

            return defaultMetaModel;
        }

        /// <summary>
        /// Override the max JSON Length.
        /// </summary>
        /// <param name="data">The data object.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="contentEncoding">The encoding.</param>
        /// <param name="behavior">The JSON request behavior.</param>
        /// <returns>The JSON result.</returns>
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }

        protected bool IsValidCurrency(int currencyId)
        {
            return currencyId == BaseCurrencyType.All ||
                KeanOceanBusinessHelper.VerifyKeanOceanCurrencyRequest(
                    SessionService.UserSession.CurrencyList, SessionService.LicenseeSession.LicenseeId, currencyId);
        }

        protected internal virtual FileContentResult EmptyExcel()
        {
            return File(ExcelHelper.ExportEmptyFile(), "application/vnd.ms-excel", "Empty.xls");
        }

        protected virtual ActionResult ExportExcel(BetListExcelMetaModel metaModel)
        {
            return null;
        }

        protected virtual void AssignViewBag(BetListMetaModel model)
        {
        }

        protected virtual bool IsValidExportExcel(BetListMetaModel model)
        {
            return IsValidCurrency(model.CurrencyId);
        }

        protected internal virtual ActionResult ProcessExportExcel(
            BetListExcelMetaModel metaModel,
            string fileName,
            string url = null)
        {
            var betlistAPIUrl = new Uri($"http://localhost:55050" + (url ?? "/BetList/ExportExcel"));
            var stream = new BridgeClientService().PostAsyncStream(betlistAPIUrl, metaModel).Result;
            return File(stream, "application/vnd.ms-excel", fileName + ".xls");
        }

        protected ActionResult ProcessDefaultRequest(BetListExcelMetaModel model, string viewPath, string uspKey)
        {
            if (!model.IsExportExcel)
            {
                AssignViewBag(model);
                return View(viewPath, model);
            }

            if (IsValidExportExcel(model))
            {
                model.UspKey = uspKey;
                return ExportExcel(model);
            }

            return EmptyExcel();
        }
    }
}