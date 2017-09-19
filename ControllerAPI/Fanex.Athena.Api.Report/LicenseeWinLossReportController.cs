// --------------------------------------------------------------------------
// <copyright file="LicenseeWinLossReportController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
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

    [ApiFunctionAuthorization(PermissionKeys.ReportLicenseeWinLoss)]
    public class LicenseeWinLossReportController : ApiReportBaseController
    {
        private readonly ILicenseeWinLossDataAccess _licenseeWinLossDataAccess;

        public LicenseeWinLossReportController()
        {
            _licenseeWinLossDataAccess = new LicenseeWinLossDataAccess();
        }

        [HttpPost]
        public dynamic Index(
            LicenseeWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0,
            string currencyName = null)
        {
            return ProcessRequest(metaModel, fdate, tdate, level, currencyId, custId, currencyName);
        }

        [HttpPost]
        public dynamic Index([FromBody] LicenseeWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
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

        #region ProcessRequest

        private static SortableList<LicenseeWinLoss> SortData(SortableList<LicenseeWinLoss> dataList, string sortBy = null, string sortOrder = "asc")
        {
            if (sortBy != null)
            {
                // Sort for grouping columns, get the real column name
                if (sortBy.EndsWith("Single", StringComparison.OrdinalIgnoreCase))
                {
                    sortBy = sortBy.Remove(sortBy.LastIndexOf("Single", StringComparison.OrdinalIgnoreCase));
                }

                var property = typeof(LicenseeWinLoss).GetProperty(sortBy);

                if (property == null)
                {
                    string propertyName, productCode;

                    if (sortBy.Contains(nameof(LicenseeWinLoss.LicenseeWL)))
                    {
                        propertyName = nameof(LicenseeWinLoss.LicenseeWL);
                        productCode = sortBy.Substring(propertyName.Length);
                    }
                    else
                    {
                        propertyName = nameof(LicenseeWinLoss.TurnOver);
                        productCode = sortBy.Substring(propertyName.Length);
                    }

                    dataList.SortByDictionary<string, decimal>(productCode, propertyName, sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    dataList.Sort(sortBy, sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));
                }
            }

            return dataList;
        }

        #endregion ProcessRequest

        #region Private Utility

        private static LicenseeWinLossGridViewModel MapToGridView(LicenseeWinLoss item, int index = 0)
        {
            var licenseeWinLossGrid = new LicenseeWinLossGridViewModel
            {
                Index = index + 1,
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
            licenseeWinLoss.LicenseeWLBA = licenseeWLProducts.GetValueOrDefault(Product.BA, 0);
            licenseeWinLoss.LicenseeWLSB = licenseeWLProducts.GetValueOrDefault(Product.SB, 0);
            licenseeWinLoss.LicenseeWLRB = licenseeWLProducts.GetValueOrDefault(Product.RB, 0);
            licenseeWinLoss.LicenseeWLNG = licenseeWLProducts.GetValueOrDefault(Product.NG, 0);
            licenseeWinLoss.LicenseeWLLC = licenseeWLProducts.GetValueOrDefault(Product.LC, 0);
            licenseeWinLoss.LicenseeWLVS = licenseeWLProducts.GetValueOrDefault(Product.VS, 0);
            licenseeWinLoss.LicenseeWLBG = licenseeWLProducts.GetValueOrDefault(Product.BG, 0);
            licenseeWinLoss.LicenseeWLCS = licenseeWLProducts.GetValueOrDefault(Product.CS, 0);
            licenseeWinLoss.LicenseeWLKN = licenseeWLProducts.GetValueOrDefault(Product.KN, 0);
            licenseeWinLoss.LicenseeWLBT = licenseeWLProducts.GetValueOrDefault(Product.BT, 0);
            licenseeWinLoss.LicenseeWLAGC = licenseeWLProducts.GetValueOrDefault(Product.AGC, 0);
            licenseeWinLoss.LicenseeWLMGCLC = licenseeWLProducts.GetValueOrDefault(Product.MGCLC, 0);
            licenseeWinLoss.LicenseeWLMGCRNG = licenseeWLProducts.GetValueOrDefault(Product.MGCRNG, 0);
            licenseeWinLoss.LicenseeWLVB = licenseeWLProducts.GetValueOrDefault(Product.VB, 0);
            licenseeWinLoss.LicenseeWLAB = licenseeWLProducts.GetValueOrDefault(Product.AB, 0);
            licenseeWinLoss.LicenseeWLFW = licenseeWLProducts.GetValueOrDefault(Product.FW, 0);
            licenseeWinLoss.LicenseeWLVS2 = licenseeWLProducts.GetValueOrDefault(Product.VS2, 0);
            licenseeWinLoss.LicenseeWLLT = licenseeWLProducts.GetValueOrDefault(Product.LT, 0);
        }

        private static void SetTurnOverProductData(LicenseeWinLossGridViewModel licenseeWinLoss, IDictionary<string, decimal> turnOverProducts)
        {
            licenseeWinLoss.TurnOverBA = turnOverProducts.GetValueOrDefault(Product.BA, 0);
            licenseeWinLoss.TurnOverSB = turnOverProducts.GetValueOrDefault(Product.SB, 0);
            licenseeWinLoss.TurnOverRB = turnOverProducts.GetValueOrDefault(Product.RB, 0);
            licenseeWinLoss.TurnOverNG = turnOverProducts.GetValueOrDefault(Product.NG, 0);
            licenseeWinLoss.TurnOverLC = turnOverProducts.GetValueOrDefault(Product.LC, 0);
            licenseeWinLoss.TurnOverVS = turnOverProducts.GetValueOrDefault(Product.VS, 0);
            licenseeWinLoss.TurnOverCS = turnOverProducts.GetValueOrDefault(Product.CS, 0);
            licenseeWinLoss.TurnOverBG = turnOverProducts.GetValueOrDefault(Product.BG, 0);
            licenseeWinLoss.TurnOverKN = turnOverProducts.GetValueOrDefault(Product.KN, 0);
            licenseeWinLoss.TurnOverBT = turnOverProducts.GetValueOrDefault(Product.BT, 0);
            licenseeWinLoss.TurnOverAGC = turnOverProducts.GetValueOrDefault(Product.AGC, 0);
            licenseeWinLoss.TurnOverMGCLC = turnOverProducts.GetValueOrDefault(Product.MGCLC, 0);
            licenseeWinLoss.TurnOverMGCRNG = turnOverProducts.GetValueOrDefault(Product.MGCRNG, 0);
            licenseeWinLoss.TurnOverVB = turnOverProducts.GetValueOrDefault(Product.VB, 0);
            licenseeWinLoss.TurnOverAB = turnOverProducts.GetValueOrDefault(Product.AB, 0);
            licenseeWinLoss.TurnOverFW = turnOverProducts.GetValueOrDefault(Product.FW, 0);
            licenseeWinLoss.TurnOverVS2 = turnOverProducts.GetValueOrDefault(Product.VS2, 0);
            licenseeWinLoss.TurnOverLT = turnOverProducts.GetValueOrDefault(Product.LT, 0);
        }

        private HttpResponseMessage ExportExcel(
            LicenseeWinLossMetaModel metaModel,
            IEnumerable<LicenseeWinLoss> dataList,
            IEnumerable<LicenseeWinLoss> totalData,
            IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            const string FileName = "LicenseeWinLoss.xls";
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/LicenseeWinLoss.xls");
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);

            var showingOption = new ShowingOption
            {
                SBVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.SB) && productAccess.IsAllow(ProductTypes.Sportsbook),
                BTVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.BT) && productAccess.IsAllow(ProductTypes.BetTrade),
                COVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.CO) && productAccess.IsAllow(ProductTypes.CashOut),
                BAVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.BA) && productAccess.IsAllow(ProductTypes.BA),
                RBVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.RB) && productAccess.IsAllow(ProductTypes.Racing),
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };
            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.DrillDownLevel == 0 ? metaModel.CurrencyName : metaModel.DrillDownCurrencyName
            };

            if (metaModel.DrillDownLevel > 0 && metaModel.CurrencyId > 0)
            {
                metaModel.CurrencyId = metaModel.DrillDownCurrencyId;
            }

            if (metaModel.DrillDownLevel > 0)
            {
                metaModel.FromDate = metaModel.DrillDownFromDate;
                metaModel.ToDate = metaModel.DrillDownToDate;
            }

            var licenseeWinLoss = new LicenseeWinLossExcel(
                templatePath,
                convertingOption,
                showingOption,
                metaModel,
                dataList,
                totalData,
                summaryTotal);

            licenseeWinLoss.Export();

            return ExcelStream(licenseeWinLoss.Workbook, FileName);
        }

        private void GetData(
                LicenseeWinLossMetaModel metaModel,
                out SortableList<LicenseeWinLoss> dataList,
                out IEnumerable<LicenseeWinLoss> totalData,
                out IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            if (metaModel.IsValid)
            {
                dataList = metaModel.DrillDownLevel == 0 ? _licenseeWinLossDataAccess.GetSummaryWinloss(metaModel, out totalData, out summaryTotal) :
                                metaModel.LicenseeId != LicenseeIdentity.OneTwoBet || metaModel.DrillDownLevel == 2
                               ? _licenseeWinLossDataAccess.GetCustomerWinlossDetail(metaModel, out totalData, out summaryTotal)
                               : _licenseeWinLossDataAccess.GetAgentWinlossDetail(metaModel, out totalData, out summaryTotal);
            }
            else
            {
                totalData = new List<LicenseeWinLoss>();
                summaryTotal = new Dictionary<string, LicenseeWinLoss>();
                dataList = new SortableList<LicenseeWinLoss>();
            }
        }

        private dynamic ProcessRequest(
            LicenseeWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0,
            string currencyName = null)
        {
            if (IsValidRequest(metaModel, currencyId))
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.DrillDownCurrencyId = currencyId;
                metaModel.DrillDownCurrencyName = currencyName;
                metaModel.DrillDownLevel = level;
                metaModel.DrillDownCustomerId = custId;
                metaModel.DrillDownFromDate = fdate.HasValue ? fdate.Value : metaModel.DrillDownFromDate;
                metaModel.DrillDownToDate = tdate.HasValue ? tdate.Value : metaModel.DrillDownToDate;
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(level == 0 ? metaModel.CurrencyId : currencyId, metaModel.IsExcludedTestData);
                IDictionary<string, LicenseeWinLoss> summaryTotal;
                SortableList<LicenseeWinLoss> dataList;
                IEnumerable<LicenseeWinLoss> totalData;
                GetData(metaModel, out dataList, out totalData, out summaryTotal);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, dataList, totalData, summaryTotal);
                }

                if (dataList != null && dataList.Any())
                {
                    dataList = SortData(dataList, metaModel.SortBy, metaModel.SortOrder);

                    var summaryTotalView = summaryTotal.ToDictionary(d => d.Key, d => MapToGridView(d.Value));
                    return new
                    {
                        Data = dataList.Select(MapToGridView).ToList(),
                        Total = totalData.Select(MapToGridView).ToList(),
                        SummaryTotal = summaryTotalView,
                        ProductOptions = metaModel.ProductOptions.ToJson()
                    };
                }
            }

            return GetEmptyResult(metaModel.ProductOptions);
        }

        #endregion Private Utility
    }
}