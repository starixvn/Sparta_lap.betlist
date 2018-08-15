namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Hosting;
    using System.Web.Http;
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

    public class SpecialLicenseeWinLossReportController : ApiReportBaseController
    {
        private readonly IReportSettingService _reportSettingService;

        private readonly IAppSettings _appSettings;

        private readonly ILicenseeWinLossDataAccess _licenseeWinLossDataAccess;

        public SpecialLicenseeWinLossReportController()
        {
            _reportSettingService = new ReportSettingService();
            _appSettings = new AppSettings();
            _licenseeWinLossDataAccess = new LicenseeWinLossDataAccess();
        }

        public SpecialLicenseeWinLossReportController(
            ISessionService sessionService,
            IProductService productService,
            IReportSettingService reportSettingService,
            ILanguageService langService,
            IAppSettings appSettings,
            ILicenseeWinLossDataAccess licenseeWinLossDataAccess)
            : base(sessionService, productService, langService)
        {
            _reportSettingService = reportSettingService;
            _appSettings = appSettings;
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

        private static void SortData(SortableList<LicenseeWinLoss> dataList, string sortBy, string sortOrder = "asc")
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return;
            }

            var propertyName = sortBy;

            // Sort for grouping columns, get the real column name
            if (sortBy.EndsWith("Single", StringComparison.OrdinalIgnoreCase))
            {
                propertyName = sortBy.Remove(sortBy.LastIndexOf("Single", StringComparison.OrdinalIgnoreCase));
            }

            var property = typeof(LicenseeWinLoss).GetProperty(propertyName);

            if (property == null)
            {
                string productCode;

                if (propertyName.Contains(nameof(LicenseeWinLoss.LicenseeWL)))
                {
                    var licenseeWLColumn = nameof(LicenseeWinLoss.LicenseeWL);

                    productCode = propertyName.Substring(licenseeWLColumn.Length);
                    propertyName = licenseeWLColumn;
                }
                else
                {
                    var turnOverColumn = nameof(LicenseeWinLoss.TurnOver);

                    productCode = propertyName.Substring(turnOverColumn.Length);
                    propertyName = turnOverColumn;
                }

                dataList.SortByDictionary(productCode, propertyName, sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase), nameof(LicenseeWinLoss.Index), true);
            }
            else
            {
                dataList.Sort(propertyName, sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase), nameof(LicenseeWinLoss.Index), true);
            }
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

        private static bool IsProductVisible(IEnumerable<ProductOption> productOptions, ProductAccess productAccess, string productCode)
        {
            return productOptions.Any(x => x.Status && x.Code == productCode) && productAccess.IsAllow(productCode);
        }

        private HttpResponseMessage ExportExcel(
            LicenseeWinLossMetaModel metaModel,
            SortableReportResultModel<LicenseeWinLoss> data)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            const string FileName = "LicenseeWinLoss.xls";
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/LicenseeWinLoss.xls");
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);

            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.DrillDownLevel == 0 ? metaModel.CurrencyName : metaModel.DrillDownCurrencyName
            };

            if (metaModel.DrillDownLevel > 0)
            {
                metaModel.FromDate = metaModel.DrillDownFromDate;
                metaModel.ToDate = metaModel.DrillDownToDate;
                metaModel.CurrencyId = metaModel.CurrencyId > 0 ? metaModel.DrillDownCurrencyId : metaModel.CurrencyId;
            }

            if (data.DataList.Any())
            {
                SortData(data.DataList, metaModel.SortBy, metaModel.SortOrder);
            }

            var excelModel = new LicenseeWinLossExcelModel
            {
                ConvertingOption = convertingOption,
                DataList = data.DataList,
                MetaModel = metaModel,
                SummaryTotal = data.SummaryTotal,
                TotalData = data.TotalDataList,
                SBVisible = IsProductVisible(metaModel.ProductOptions, productAccess, Product.SB),
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm,
                SystemIdVisible = !_appSettings.FakeLicenseeList.Contains(licenseeSession.LicenseeId)
            };

            var excel = CreateExcel(templatePath, excelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        protected internal virtual LicenseeWinLossExcel CreateExcel(string templatePath, LicenseeWinLossExcelModel excelModel)
        {
            return new LicenseeWinLossExcel(templatePath, excelModel);
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

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, result);
                }

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
            var setting = _reportSettingService.GetSetting(PermissionKeys.ReportSpecialLicenseeWinLoss);
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