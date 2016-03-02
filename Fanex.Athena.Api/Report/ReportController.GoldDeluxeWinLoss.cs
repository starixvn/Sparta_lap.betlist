// --------------------------------------------------------------------------
// <copyright file="ReportController.GoldDeluxeWinLoss.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Utility;
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
        public dynamic GoldDeluxeWinLoss(
            GoldDeluxeWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {
            return ProcessRequest(metaModel, fdate, tdate, level, currencyId, custId);
        }

        /// <summary>
        /// Licensee Win Loss - Level 0.
        /// </summary>
        /// <param name="metaModel">The level 0 filter.</param>
        /// <returns>Process Request.</returns>
        [HttpPost]
        public dynamic GoldDeluxeWinLoss([FromBody] GoldDeluxeWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        #endregion Action

        private static dynamic GetEmptyGoldDeluxeWinLossResult()
        {
            return new
            {
                Data = new List<GoldDeluxeWinLoss>(),
                Total = new List<GoldDeluxeWinLoss>(),
                SummaryTotal = new Dictionary<string, GoldDeluxeWinLoss>()
            };
        }

        #region Private Utility

        /// <summary>
        /// AGCasino win loss - Export excel.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Data list.</param>
        /// <param name="summaryTotal">Summary data.</param>
        /// <returns>Http Response Message.</returns>
        private HttpResponseMessage ExportExcel(
            GoldDeluxeWinLossMetaModel metaModel, 
            IList<GoldDeluxeWinLoss> dataList, 
            IDictionary<string, GoldDeluxeWinLoss> summaryTotal)
        {
            var userSession = SessionService.UserSession;
            const string FileName = "GoldDeluxeWinLoss.xls";
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/GoldDeluxeWinLoss.xls");

            var currencyList = userSession.CurrencyList;

            var currencyName = currencyList.SingleOrDefault(i => i.Value == (metaModel.DrillDownLevel == 0 ? metaModel.CurrencyId : metaModel.DrillDownCurrencyId));
            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = currencyName.Key
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

            var exccelModel = new GoldDeluxeWinLossViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal,
            };

            var excel = new GoldDeluxeWinLossExcel(templatePath, FileName, convertingOption, metaModel, exccelModel)
            {
                Level = metaModel.DrillDownLevel,
            };

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        /// <summary>
        /// Get data for Licensee Win Loss report.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Output parameter: Data List.</param>
        /// <param name="summaryTotal">Output parameter summary data.</param>
        private void GetData(
                GoldDeluxeWinLossMetaModel metaModel,
                out SortableList<GoldDeluxeWinLoss> dataList,
                out IDictionary<string, GoldDeluxeWinLoss> summaryTotal)
        {
            if (metaModel.DrillDownLevel == 0)
            {
                dataList = GolddeluxeWinLossService.GetSummaryWinloss(metaModel, out summaryTotal);
            }
            else
            {
                dataList = metaModel.DrillDownLevel == 2
                               ? GolddeluxeWinLossService.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                               : GolddeluxeWinLossService.GetAgentWinlossDetail(metaModel, out summaryTotal);
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
            GoldDeluxeWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {
            if (IsValidRequest(metaModel, currencyId))
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.UserId = SessionService.UserSession.UserId;
                metaModel.DrillDownCurrencyId = currencyId;
                metaModel.DrillDownLevel = level;
                metaModel.DrillDownCustomerId = custId;
                metaModel.DrillDownFromDate = fdate.HasValue ? fdate.Value : metaModel.DrillDownFromDate;
                metaModel.DrillDownToDate = tdate.HasValue ? tdate.Value : metaModel.DrillDownToDate;

                IDictionary<string, GoldDeluxeWinLoss> summaryTotal;
                SortableList<GoldDeluxeWinLoss> dataList;
                GetData(metaModel, out dataList, out summaryTotal);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, dataList, summaryTotal);
                }

                if (dataList != null && dataList.Count > 0)
                {
                    return new
                    {
                        Data = dataList,
                        Total = GolddeluxeWinLossService.TotalCalc(dataList),
                        SummaryTotal = summaryTotal
                    };
                }
            }

            return GetEmptyGoldDeluxeWinLossResult();
        }

        #endregion Private Utility
    }
}