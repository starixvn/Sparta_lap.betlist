// -----------------------------------------------------------------------
// <copyright file="ReportController.Turnover.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Web.ActionFilter;
    using Fanex.Athena.Web.ResultExtension;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using Service.Report.Interfaces;

    [FunctionAuthorization(PermissionKeys.ReportTurnover)]
    public class TurnoverReportController : ReportBaseController
    {
        private readonly ITurnoverService _turnoverService;

        public TurnoverReportController() : this(null, null, null, null, null, null)
        {
        }

        public TurnoverReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IClientViewHelper clientViewHelper,
            IProductService productService,
            ITurnoverService turnoverService)
            : base(languageService, sessionService, appSetting, clientViewHelper, productService)
        {
            _turnoverService = turnoverService ?? new TurnoverService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var metaModel = new TurnoverMetaModel
            {
                LicenseeId = licenseeSession.LicenseeId,
                UserId = userSession.UserId,
            };

            SetConvetedUSDCurrencyName(metaModel);
            SetConvetedUSDCurrencyId(metaModel);

            var productAccess = ProductService.GetSingleWalletProductAccess(
                licenseeSession.LicenseeId,
                userSession.Language,
                FunctionNames.Turnover);
            metaModel.ProductOptions = ToProductOptions(productAccess.ProductList.Where(a => !a.ProductCode.Equals(Product.BA)));

            var model = new TurnoverViewModel
            {
                Param = metaModel,
                ProductAccess = productAccess,
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CurrencyList = GetCurrenciesForTurnoverReport(licenseeSession.LicenseeId),
                IsAllowedCasinoTransfer = AppSettings.LicenseeAllowCasinoTransfer.Contains(licenseeSession.LicenseeId),
                IsFakeLicensee = IsFakeLicensee()
            };

            return View("~/Views/Report/Turnover/Main.cshtml", model);
        }

        [HttpPost]
        [CustomValidateAntiForgery]
        public ActionResult Excel(TurnoverMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;

            var placeholderUsername = IsFakeLicensee() ? ViewResources.ExternalId : ViewResources.ExternalIdOrSystemId;
            metaModel.Username = string.IsNullOrWhiteSpace(metaModel.Username) ||
                metaModel.Username.Trim() == placeholderUsername ? null : metaModel.Username;
            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludeTest);

            var dataList = _turnoverService.GetDataList(metaModel);

            var model = new TurnoverViewModel
            {
                DataList = dataList,
                TotalData = _turnoverService.SumTotalData(dataList),
                Param = metaModel,
                ProductAccess = ProductService.GetSingleWalletProductAccess(licenseeSession.LicenseeId, userSession.Language, FunctionNames.Turnover),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm,
                CurrencyList = GetCurrenciesForTurnoverReport(licenseeSession.LicenseeId),
                IsAllowedCasinoTransfer = licenseeSession.LicenseeId == 1
            };

            return ExportExcel(model);
        }

        private ActionResult ExportExcel(TurnoverViewModel viewModel)
        {
            const string FileName = "Turnover";

            var templatePath = HostingEnvironment.MapPath("~/App_Data/Template/Reports/Turnover.xls");

            var licenseeId = SessionService.LicenseeSession.LicenseeId;

            var productAccess = ProductService.GetSingleWalletProductAccess(licenseeId, SessionService.UserSession.Language, FunctionNames.Turnover);

            var convertingOption = new ConvertingOption
            {
                IsConvert = viewModel.Param.IsConvert,
                BaseCurrency = viewModel.Param.BaseCurrency,
                CurrencyName = viewModel.Param.CurrencyName
            };

            var showingOption = new ShowingOption
            {
                CustomerCommVisible = this.SessionService.LicenseeSession.IsAllowedCustomerComm,
                BAVisible = productAccess.IsAllowBA
            };

            viewModel.Param.CurrencyName = viewModel.Param.IsConvert ?
                viewModel.Param.BaseCurrency
                : viewModel.CurrencyList.FirstOrDefault(a => a.Key == viewModel.Param.CurrencyId).Value;
            var isShowCasinoTransfer = AppSettings.LicenseeAllowCasinoTransfer.Contains(licenseeId);
            var excel = CreateExcel(templatePath, viewModel, convertingOption, showingOption, isShowCasinoTransfer);

            excel.Export();

            return new ExcelResult(excel.Workbook, FileName);
        }

        protected internal virtual TurnoverExcel CreateExcel(
            string templatePath,
            TurnoverViewModel viewModel,
            ConvertingOption convertingOption,
            ShowingOption showingOption,
            bool isShowCasinoTransfer)
        {
            return new TurnoverExcel(templatePath, convertingOption, showingOption, viewModel) { IsShowCasinoTransfer = isShowCasinoTransfer };
        }

        private IDictionary<int, string> GetCurrenciesForTurnoverReport(int licenseeId)
        {
            var currencyList = SessionService.UserSession.CurrencyList;
            KeanOceanBusinessHelper.AddHKDCurrencyForTurnOverReport(currencyList, licenseeId);
            return currencyList;
        }
    }
}