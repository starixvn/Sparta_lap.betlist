// --------------------------------------------------------------------------
// <copyright file="LicenseeWinLossReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Service.Implement.Report;
    using Service.Interface.Report;
    using Web.ActionFilter;

    public class LicenseeWinLossReportController : ApiReportBaseController
    {
        private readonly ILicenseeWinLossService _licenseeWinLossService;

        public LicenseeWinLossReportController()
        {
            _licenseeWinLossService = new LicenseeWinLossService();
        }
        
        [HttpPost]
        [ApiFunctionAuthorization]
        public dynamic LicenseeWL(
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
        [ApiFunctionAuthorization]
        public dynamic LicenseeWL([FromBody] LicenseeWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        private static dynamic GetEmptyResult(LicenseeWinLossMetaModel metaModel)
        {
            return new
            {
                Data = new List<LicenseeWinLossGridViewModel>(),
                Total = new List<LicenseeWinLossGridViewModel>(),
                SummaryTotal = new Dictionary<string, LicenseeWinLossGridViewModel>(),
                ProductOptions = metaModel.ProductOptions.ToJson()
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
                    // The two last characters is product code.
                    var indexProductCode = sortBy.Length - 2;
                    var propertyName = sortBy.Substring(0, indexProductCode);
                    var productCode = sortBy.Substring(indexProductCode);

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
        }

        private HttpResponseMessage ExportExcel(LicenseeWinLossMetaModel metaModel, IList<LicenseeWinLoss> dataList, IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            const string FileName = "LicenseeWinLoss.xls";
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/LicenseeWinLoss.xls");
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);

            var showingOption = new ShowingOption
            {
                SBVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.SB) && productAccess.IsAllowSB,
                BTVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.BT) && productAccess.IsAllowBT,
                BAVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.BA) && productAccess.IsAllowBA,
                RBVisible = metaModel.ProductOptions.Any(a => a.Status && a.Code == Product.RB) && productAccess.IsAllowRB,
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

            var excelModel = new LicenseeWinLossViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal,
            };

            var licenseeWinLoss = new LicenseeWinLossExcel(templatePath, FileName, convertingOption, showingOption, metaModel, excelModel)
            {
                Level = metaModel.DrillDownLevel,
            };

            licenseeWinLoss.Export();

            return ExcelStream(licenseeWinLoss.Workbook, FileName);
        }

        private void GetData(
                LicenseeWinLossMetaModel metaModel,
                out SortableList<LicenseeWinLoss> dataList,
                out IDictionary<string, LicenseeWinLoss> summaryTotal)
        {
            if (metaModel.DrillDownLevel == 0)
            {
                dataList = _licenseeWinLossService.GetSummaryWinloss(metaModel, out summaryTotal);
            }
            else
            {
                dataList = metaModel.LicenseeId > 1 || metaModel.DrillDownLevel == 2
                               ? _licenseeWinLossService.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                               : _licenseeWinLossService.GetAgentWinlossDetail(metaModel, out summaryTotal);
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
                metaModel.UserId = SessionService.UserSession.UserId;
                metaModel.DrillDownCurrencyId = currencyId;
                metaModel.DrillDownCurrencyName = currencyName;
                metaModel.DrillDownLevel = level;
                metaModel.DrillDownCustomerId = custId;
                metaModel.DrillDownFromDate = fdate.HasValue ? fdate.Value : metaModel.DrillDownFromDate;
                metaModel.DrillDownToDate = tdate.HasValue ? tdate.Value : metaModel.DrillDownToDate;

                IDictionary<string, LicenseeWinLoss> summaryTotal;
                SortableList<LicenseeWinLoss> dataList;
                GetData(metaModel, out dataList, out summaryTotal);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, dataList, summaryTotal);
                }

                if (dataList != null && dataList.Count > 0)
                {
                    dataList = SortData(dataList, metaModel.SortBy, metaModel.SortOrder);

                    var summaryTotalView = summaryTotal.ToDictionary(d => d.Key, d => MapToGridView(d.Value));
                    return new
                    {
                        Data = dataList.Select(MapToGridView).ToList(),
                        Total = _licenseeWinLossService.TotalCalc(dataList).Select(MapToGridView).ToList(),
                        SummaryTotal = summaryTotalView,
                        ProductOptions = metaModel.ProductOptions.ToJson()
                    };
                }
            }

            return GetEmptyResult(metaModel);
        }

        #endregion Private Utility
    }
}