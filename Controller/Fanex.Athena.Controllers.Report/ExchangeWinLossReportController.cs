namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Resources;

    public class ExchangeWinLossReportController : ReportBaseController
    {
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
                { BaseCurrencyType.Eur, ViewResources.Based + " " + ViewResources.EUR },
                { BaseCurrencyType.Usd, ViewResources.Based + " " + ViewResources.USD },
                { BaseCurrencyType.Gbp, ViewResources.Based + " " + ViewResources.GBP }
            };

            foreach (var item in SessionService.UserSession.CurrencyList)
            {
                currencyList.Add(item.Key, item.Value);
            }

            return new ExchangeWinLossViewModel
            {
                CurrencyList = currencyList,
                CurrencyId = GetConvetedUSDCurrencyId(licensee.LicenseeId),
                IsAllowedCustomerComm = licensee.IsAllowedCustomerComm,
                LicenseeId = licensee.LicenseeId,
                ReportByOption = new Dictionary<byte, string>
                {
                    { ReportByType.Daily, ViewResources.Daily },
                    { ReportByType.Monthly, ViewResources.Monthly }
                },
                ExcludeTestDataOption = new Dictionary<bool, string>
                {
                    { true, ViewResources.Yes },
                    { false, ViewResources.No }
                }
            };
        }
    }
}