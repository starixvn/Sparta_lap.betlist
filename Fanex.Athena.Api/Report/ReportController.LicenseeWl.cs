// --------------------------------------------------------------------------
// <copyright file="ReportController.LicenseeWl.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;

    /// <summary>
    /// ReportController class.
    /// </summary>
    public partial class ReportController
    {
        #region Action

        /// <summary>
        /// Licensee win loss report for down line level.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <param name="fdate">From Date value.</param>
        /// <param name="tdate">To Date value.</param>
        /// <param name="level">Level value.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <param name="custId">Customer Id.</param>
        /// <returns>Call method ProcessRequest.</returns>
        [HttpPost]
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

        /// <summary>
        /// Licensee Win Loss - Level 0.
        /// </summary>
        /// <param name="metaModel">The level 0 filter.</param>
        /// <returns>Process Request.</returns>
        [HttpPost]
        public dynamic LicenseeWL([FromBody] LicenseeWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        #endregion Action

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

        /// <summary>
        /// Sort data list by property (column name).
        /// </summary>
        /// <param name="dataList">Data list to be sorted.</param>
        /// <param name="sortBy">Sort column name.</param>
        /// <param name="sortOrder">ASC or DESC.</param>
        /// <returns>Data list after being sorted.</returns>
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

        /// <summary>
        /// Convert to view model of grid.
        /// </summary>
        /// <param name="item">The data object.</param>
        /// <param name="index">Row index.</param>
        /// <returns>Licensee WinLoss Grid View Model.</returns>
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
                ActualStakeSB = item.ActualStakeSB,
                ActualStakeBA = item.ActualStakeBA,
                TotalActualStake = item.TotalActualStake
            };

            SetLicenseeWinLossProductData(licenseeWinLossGrid, item.LicenseeWL);
            SetTurnOverProductData(licenseeWinLossGrid, item.TurnOver);

            return licenseeWinLossGrid;
        }

        private static void SetLicenseeWinLossProductData(LicenseeWinLossGridViewModel licenseeWinLoss, IDictionary<string, decimal> licenseeWLProducts)
        {
            licenseeWinLoss.LicenseeWLBA = licenseeWLProducts.Keys.Contains(Product.BA) ? licenseeWLProducts[Product.BA] : 0;
            licenseeWinLoss.LicenseeWLSB = licenseeWLProducts.Keys.Contains(Product.SB) ? licenseeWLProducts[Product.SB] : 0;
            licenseeWinLoss.LicenseeWLRB = licenseeWLProducts.Keys.Contains(Product.RB) ? licenseeWLProducts[Product.RB] : 0;
            licenseeWinLoss.LicenseeWLNG = licenseeWLProducts.Keys.Contains(Product.NG) ? licenseeWLProducts[Product.NG] : 0;
            licenseeWinLoss.LicenseeWLLC = licenseeWLProducts.Keys.Contains(Product.LC) ? licenseeWLProducts[Product.LC] : 0;
            licenseeWinLoss.LicenseeWLVS = licenseeWLProducts.Keys.Contains(Product.VS) ? licenseeWLProducts[Product.VS] : 0;
            licenseeWinLoss.LicenseeWLBG = licenseeWLProducts.Keys.Contains(Product.BG) ? licenseeWLProducts[Product.BG] : 0;
            licenseeWinLoss.LicenseeWLCS = licenseeWLProducts.Keys.Contains(Product.CS) ? licenseeWLProducts[Product.CS] : 0;
            licenseeWinLoss.LicenseeWLKN = licenseeWLProducts.Keys.Contains(Product.KN) ? licenseeWLProducts[Product.KN] : 0;
            licenseeWinLoss.LicenseeWLBT = licenseeWLProducts.Keys.Contains(Product.BT) ? licenseeWLProducts[Product.BT] : 0;
            licenseeWinLoss.LicenseeWLAGC = licenseeWLProducts.Keys.Contains(Product.AGC) ? licenseeWLProducts[Product.AGC] : 0;
            licenseeWinLoss.LicenseeWLMGCLC = licenseeWLProducts.Keys.Contains(Product.MGCLC) ? licenseeWLProducts[Product.MGCLC] : 0;
            licenseeWinLoss.LicenseeWLMGCRNG = licenseeWLProducts.Keys.Contains(Product.MGCRNG) ? licenseeWLProducts[Product.MGCRNG] : 0;
        }

        private static void SetTurnOverProductData(LicenseeWinLossGridViewModel licenseeWinLoss, IDictionary<string, decimal> turnOverProducts)
        {
            licenseeWinLoss.TurnOverBA = turnOverProducts.Keys.Contains(Product.BA) ? turnOverProducts[Product.BA] : 0;
            licenseeWinLoss.TurnOverSB = turnOverProducts.Keys.Contains(Product.SB) ? turnOverProducts[Product.SB] : 0;
            licenseeWinLoss.TurnOverRB = turnOverProducts.Keys.Contains(Product.RB) ? turnOverProducts[Product.RB] : 0;
            licenseeWinLoss.TurnOverNG = turnOverProducts.Keys.Contains(Product.NG) ? turnOverProducts[Product.NG] : 0;
            licenseeWinLoss.TurnOverLC = turnOverProducts.Keys.Contains(Product.LC) ? turnOverProducts[Product.LC] : 0;
            licenseeWinLoss.TurnOverVS = turnOverProducts.Keys.Contains(Product.VS) ? turnOverProducts[Product.VS] : 0;
            licenseeWinLoss.TurnOverCS = turnOverProducts.Keys.Contains(Product.CS) ? turnOverProducts[Product.CS] : 0;
            licenseeWinLoss.TurnOverBG = turnOverProducts.Keys.Contains(Product.BG) ? turnOverProducts[Product.BG] : 0;
            licenseeWinLoss.TurnOverKN = turnOverProducts.Keys.Contains(Product.KN) ? turnOverProducts[Product.KN] : 0;
            licenseeWinLoss.TurnOverBT = turnOverProducts.Keys.Contains(Product.BT) ? turnOverProducts[Product.BT] : 0;
            licenseeWinLoss.TurnOverAGC = turnOverProducts.Keys.Contains(Product.AGC) ? turnOverProducts[Product.AGC] : 0;
            licenseeWinLoss.TurnOverMGCLC = turnOverProducts.Keys.Contains(Product.MGCLC) ? turnOverProducts[Product.MGCLC] : 0;
            licenseeWinLoss.TurnOverMGCRNG = turnOverProducts.Keys.Contains(Product.MGCRNG) ? turnOverProducts[Product.MGCRNG] : 0;
        }

        /// <summary>
        /// Licensee win loss - Export excel.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Data list.</param>
        /// <param name="summaryTotal">Summary data.</param>
        /// <returns>Http Response Message.</returns>
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

            // Update metalModel Value
            if (metaModel.DrillDownLevel > 0 && metaModel.CurrencyId > 0)
            {
                metaModel.CurrencyId = metaModel.DrillDownCurrencyId;
            }

            if (metaModel.DrillDownLevel > 0)
            {
                metaModel.FromDate = metaModel.DrillDownFromDate;
                metaModel.ToDate = metaModel.DrillDownToDate;
            }

            var exccelModel = new LicenseeWinLossViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal,
            };

            var licenseeWinLoss = new LicenseeWinLossExcel(templatePath, FileName, convertingOption, showingOption, metaModel, exccelModel)
            {
                Level = metaModel.DrillDownLevel,
            };

            licenseeWinLoss.Export();

            return ExcelStream(licenseeWinLoss.Workbook, FileName);
        }

        /// <summary>
        /// Get data for Licensee Win Loss report.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Output parameter: Data List.</param>
        /// <param name="summaryTotal">Output parameter summary data.</param>
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

        /// <summary>
        ///  Process request.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <param name="fdate">From Date value.</param>
        /// <param name="tdate">To Date value.</param>
        /// <param name="level">The Level value.</param>
        /// <param name="currencyId">Currency Id.</param>
        /// <param name="custId">Customer Id.</param>
        /// <returns>Dynamic object.</returns>
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
                metaModel.UserId = SessionService.UserSession.UserId; // focus here
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