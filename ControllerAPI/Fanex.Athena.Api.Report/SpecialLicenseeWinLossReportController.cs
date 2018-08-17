namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Hosting;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Excel.Report.Models;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.MetaModel.Product;
    using Model.Core.ResultModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportLicenseeWinLoss)]
    public class SpecialLicenseeWinLossReportController : ApiReportBaseController
    {
        private readonly IReportSettingService _reportSettingService;

        private readonly ILicenseeWinLossDataAccess _licenseeWinLossDataAccess;

        public SpecialLicenseeWinLossReportController()
        {
            _reportSettingService = new ReportSettingService();
            _licenseeWinLossDataAccess = new LicenseeWinLossDataAccess();
        }

        public SpecialLicenseeWinLossReportController(
            ISessionService sessionService,
            IProductService productService,
            IReportSettingService reportSettingService,
            ILanguageService langService,
            ILicenseeWinLossDataAccess licenseeWinLossDataAccess)
            : base(sessionService, productService, langService)
        {
            _reportSettingService = reportSettingService;
            _licenseeWinLossDataAccess = licenseeWinLossDataAccess;
        }

        [HttpPost]
        public dynamic Index(LicenseeWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel);
        }

        private static dynamic GetEmptyResult(IEnumerable<ProductOption> productOptions)
        {
            return new
            {
                Data = new List<LicenseeWinLossGridViewModel>(),
                Total = new List<LicenseeWinLossGridViewModel>(),
                SummaryTotal = new Dictionary<string, LicenseeWinLossGridViewModel>(),
                ProductOptions = productOptions.ToJson()
            };
        }

        private static LicenseeWinLossGridViewModel MapToGridView(LicenseeWinLoss item)
        {
            var licenseeWinLossGrid = new LicenseeWinLossGridViewModel
            {
                Index = item.Index,
                AmtDueToBA = item.AmtDueToBA,
                AmtDueToRacing = item.AmtDueToRacing,
                AmtDueToBetTrade = item.AmtDueToBetTrade,
                CashOutCommission = item.CashOutCommission,
                BACommission = item.BACommission,
                BetTradeCommission = item.BetTradeCommission,
                CurrencyId = item.CurrencyId,
                SystemId = item.SystemId,
                CurrencyName = item.CurrencyName,
                CustomerComm = item.CustomerComm,
                CustomerWinLoss = item.CustomerWinLoss,
                TotalTransaction = item.TotalTransaction,
                RacingRevenue = item.RacingRevenue,
                CustomerId = item.CustomerId,
                Date = item.Date,
                ExternalId = item.ExternalId,
                BuyBackAmountSB = item.BuyBackAmountSB,
                BuyBackAmountBA = item.BuyBackAmountBA,
                TotalBuyBackAmount = item.TotalBuyBackAmount,
                ActualStake = item.ActualStake
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

        private SortableReportResultModel<LicenseeWinLoss> GetData(LicenseeWinLossMetaModel metaModel)
        {
            metaModel.ProductOptions = new List<ProductOption>
            {
                new ProductOption { Code = "SB", Status = true },
                new ProductOption { Code = "NG", Status = true }
            };

            if (metaModel.IsValid)
            {
                var result = metaModel.DrillDownLevel == 0 ? _licenseeWinLossDataAccess.GetSummaryWinloss(metaModel) :
                                metaModel.LicenseeId != LicenseeIdentity.OneTwoBet || metaModel.DrillDownLevel == 2
                               ? _licenseeWinLossDataAccess.GetCustomerWinlossDetail(metaModel)
                               : _licenseeWinLossDataAccess.GetAgentWinlossDetail(metaModel);

                return result;
            }

            return new SortableReportResultModel<LicenseeWinLoss>
            {
                TotalDataList = new List<LicenseeWinLoss>(),
                SummaryTotal = new Dictionary<string, LicenseeWinLoss>(),
                DataList = new SortableList<LicenseeWinLoss>()
            };
        }

        private dynamic ProcessRequest(LicenseeWinLossMetaModel metaModel)
        {
            var currencyId = metaModel.DrillDownLevel == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId;

            if (IsValidParams(metaModel) && IsValidRequest(metaModel, currencyId))
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(currencyId, metaModel.IsExcludedTestData);
                var result = GetData(metaModel);

                if (result.DataList.Any())
                {
                    var summaryTotalView = result.SummaryTotal.ToDictionary(d => d.Key, d => MapToGridView(d.Value));
                    return new
                    {
                        Data = result.DataList.Select(MapToGridView).ToList(),
                        Total = result.TotalDataList.Select(MapToGridView).ToList(),
                        SummaryTotal = summaryTotalView,
                        ProductOptions = metaModel.ProductOptions.ToJson()
                    };
                }
            }

            return GetEmptyResult(metaModel.ProductOptions);
        }

        protected internal virtual bool IsValidParams(LicenseeWinLossMetaModel metaModel)
        {
            var setting = _reportSettingService.GetSetting(PermissionKeys.ReportLicenseeWinLoss);
            if (setting == null)
            {
                return true;
            }

            if (metaModel.DrillDownLevel > 0 && setting.IsDisableBetList)
            {
                return false;
            }

            var fromDate = metaModel.DrillDownLevel == 0 ? metaModel.FromDate : metaModel.DrillDownFromDate;
            var toDate = metaModel.DrillDownLevel == 0 ? metaModel.ToDate : metaModel.DrillDownToDate;
            return DateTimeHelper.IsBetween(fromDate, setting.MinDate, setting.MaxDate) &&
                DateTimeHelper.IsBetween(toDate, setting.MinDate, setting.MaxDate);
        }
    }
}