namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Resources;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ReportExchangeWinLoss)]
    public class ExchangeWinLossReportController : ReportBaseController
    {
        private readonly IAppSettings _appSetting;

        public ExchangeWinLossReportController() : this(null)
        {
        }

        public ExchangeWinLossReportController(IAppSettings appSetting)
        {
            _appSetting = appSetting ?? new AppSettings();
        }

        public ActionResult Index()
        {
            var viewModel = CreateViewModel();

            return View("~/Views/Report/ExchangeWinLoss/Main.cshtml", viewModel);
        }

        private ExchangeWinLossViewModel CreateViewModel()
        {
            var licensee = SessionService.LicenseeSession;
            var currencyList = new Dictionary<int, string>
            {
                { BaseCurrencyType.All, ViewResources.All },
                { BaseCurrencyType.Eur, $"{ViewResources.Based} {ViewResources.EUR}" },
                { BaseCurrencyType.Usd, $"{ViewResources.Based} {ViewResources.USD}" },
                { BaseCurrencyType.Gbp, $"{ViewResources.Based} {ViewResources.GBP}" }
            };

            foreach (var item in SessionService.UserSession.CurrencyList)
            {
                currencyList.Add(item.Key, item.Value);
            }

            return new ExchangeWinLossViewModel
            {
                MinDate = _appSetting.HistoryDate,
                MaxDate = _appSetting.MaxDate,
                CurrencyList = currencyList,
                CurrencyId = GetConvetedUSDCurrencyId(licensee.LicenseeId),
                IsAllowedCustomerComm = licensee.IsAllowedCustomerComm,
                LicenseeId = licensee.LicenseeId,
                ReportByOption = new Dictionary<byte, string>
                {
                    { ReportByType.Daily, ViewResources.Daily },
                    { ReportByType.Monthly, ViewResources.Monthly }
                }
            };
        }
    }
}