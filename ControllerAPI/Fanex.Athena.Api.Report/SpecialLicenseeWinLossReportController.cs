namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportLicenseeWinLoss)]
    public class SpecialLicenseeWinLossReportController : ApiReportBaseController
    {
        private readonly IReportSettingService _reportSettingService;

        private readonly ISpecialLicenseeWinLossDataAccess _licenseeWinLossDataAccess;

        private readonly IDictionary<int, string> _siteNameList = new Dictionary<int, string>
        {
            { LicenseeIdentity.IvoryBet, "ivorysport" },
            { LicenseeIdentity.Opal88, "opal88" }
        };

        public SpecialLicenseeWinLossReportController()
        {
            _reportSettingService = new ReportSettingService();
            _licenseeWinLossDataAccess = new SpecialLicenseeWinLossDataAccess();
        }

        public SpecialLicenseeWinLossReportController(
            ISessionService sessionService,
            IProductService productService,
            IReportSettingService reportSettingService,
            ILanguageService langService,
            ISpecialLicenseeWinLossDataAccess licenseeWinLossDataAccess)
            : base(sessionService, productService, langService)
        {
            _reportSettingService = reportSettingService;
            _licenseeWinLossDataAccess = licenseeWinLossDataAccess;
        }

        [HttpPost]
        public dynamic Index(SpecialLicenseeWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel);
        }

        private static LicenseeWinLossGridViewModel MapToGridView(LicenseeWinLoss item)
        {
            var licenseeWinLossGrid = new LicenseeWinLossGridViewModel
            {
                CurrencyId = item.CurrencyId,
                CurrencyName = item.CurrencyName,
                CustomerWinLoss = item.CustomerWinLoss,
                TotalTransaction = item.TotalTransaction,
                Date = item.Date
            };
            SetLicenseeWinLossProductData(licenseeWinLossGrid, item.LicenseeWL);
            SetTurnOverProductData(licenseeWinLossGrid, item.TurnOver);

            return licenseeWinLossGrid;
        }

        private static void SetLicenseeWinLossProductData(LicenseeWinLossGridViewModel licenseeWinLoss, IDictionary<string, decimal> licenseeWLProducts)
        {
            licenseeWinLoss.LicenseeWLSB = licenseeWLProducts.GetValueOrDefault(Product.SB, 0);
            licenseeWinLoss.LicenseeWLNG = licenseeWLProducts.GetValueOrDefault(Product.NG, 0);
        }

        private static void SetTurnOverProductData(LicenseeWinLossGridViewModel licenseeWinLoss, IDictionary<string, decimal> turnOverProducts)
        {
            licenseeWinLoss.TurnOverSB = turnOverProducts.GetValueOrDefault(Product.SB, 0);
            licenseeWinLoss.TurnOverNG = turnOverProducts.GetValueOrDefault(Product.NG, 0);
        }

        private static bool IsValidDateRange(SpecialLicenseeWinLossMetaModel metaModel, ReportSetting setting)
        {
            return setting != null && metaModel.FromDate <= metaModel.ToDate &&
                DateTimeHelper.IsBetween(metaModel.FromDate, setting.MinDate, setting.MaxDate) &&
                DateTimeHelper.IsBetween(metaModel.ToDate, setting.MinDate, setting.MaxDate);
        }

        private dynamic ProcessRequest(SpecialLicenseeWinLossMetaModel metaModel)
        {
            var setting = _reportSettingService.GetSetting(PermissionKeys.ReportLicenseeWinLoss);

            if (IsValidParams(metaModel, setting))
            {
                metaModel.BaseCurrencies = setting.BaseCurrencies.Split(',');
                metaModel.SiteName = _siteNameList[SessionService.LicenseeSession.LicenseeId];

                metaModel.CurrencyListXml = GenerateCurrencyListXml(metaModel);

                var result = _licenseeWinLossDataAccess.GetData(metaModel);

                if (result.DataList.Any())
                {
                    var summaryTotalView = result.SummaryTotal.ToDictionary(d => d.Key, d => MapToGridView(d.Value));
                    return new
                    {
                        Data = result.DataList.Select(MapToGridView).ToList(),
                        Total = result.TotalDataList.Select(MapToGridView).ToList(),
                        SummaryTotal = summaryTotalView
                    };
                }
            }

            return new
            {
                Data = new List<LicenseeWinLossGridViewModel>(),
                Total = new List<LicenseeWinLossGridViewModel>(),
                SummaryTotal = new Dictionary<string, LicenseeWinLossGridViewModel>()
            };
        }

        private string GenerateCurrencyListXml(SpecialLicenseeWinLossMetaModel metaModel)
        {
            var currencyList = SessionService.UserSession.CurrencyList
               .Where(i => i.Key == metaModel.CurrencyId || metaModel.CurrencyId <= 0)
               .Select(i => $"<CR Id=\"{i.Key}\" Code =\"{i.Value}\" />").ToArray();

            return $"<ROOT>{string.Join("", currencyList)}</ROOT>";
        }

        protected internal virtual bool IsValidParams(SpecialLicenseeWinLossMetaModel metaModel, ReportSetting setting)
        {
            return IsValidDateRange(metaModel, setting) && IsValidProduct(metaModel) && IsValidCurrency(metaModel);
        }

        private bool IsValidCurrency(SpecialLicenseeWinLossMetaModel metaModel)
        {
            return metaModel.CurrencyId <= 0
                || SessionService.UserSession.CurrencyList.ContainsKey(metaModel.CurrencyId);
        }

        private bool IsValidProduct(SpecialLicenseeWinLossMetaModel metaModel)
        {
            if (metaModel.ProductCodeList == null)
            {
                return false;
            }

            var productAccess = this.ProductService.GetSingleWalletProductAccess(
               SessionService.LicenseeSession.LicenseeId,
               SessionService.UserSession.Language,
               FunctionNames.LicenseeWinLoss).ProductList.Select(x => x.ProductCode);

            return productAccess.Intersect(metaModel.ProductCodeList).Count() == metaModel.ProductCodeList.Count();
        }
    }
}