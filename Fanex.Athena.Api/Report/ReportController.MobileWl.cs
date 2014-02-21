// --------------------------------------------------------------------------
// <copyright file="ReportController.MobileWl.cs" company="Nexcel Solutions Vietnam">
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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;

    /// <summary>
    /// MobileWinLossController class.
    /// </summary>
    public partial class ReportController
    {
        //// POST api/<controller>

        #region Action

        /// <summary>
        /// Mobile Win Loss report - Level 0.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <returns>Call method ProcessRequest.</returns>
        [HttpPost]
        public dynamic MobileWinloss([FromBody] MobileWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null);
        }

        #endregion Action

        #region ProceseeRequest

        /// <summary>
        /// Mobile Win Loss - Export excel.
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Report data list.</param>
        /// <param name="summaryTotal">Summary data.</param>
        /// <param name="date">Report date value.</param>
        /// <returns>Http Response Message.</returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        [NonAction]
        private HttpResponseMessage ExportExcel(
            MobileWinLossMetaModel metaModel,
            IList<MobileWinLoss> dataList,
            Dictionary<string, MobileWinLoss> summaryTotal,
            DateTime? date)
        {
            const string FileName = "MobileWinLoss.xls";
            var licenseeSession = SessionService.LicenseeSession;
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/MobileWinLoss.xls");
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, SessionService.UserSession.Language, false);

            var showingOption = new ShowingOption
                {
                    BaVisible = metaModel.ProductOptions.Any(p => p.Code == Product.Ba && p.Status),
                    RbVisible = metaModel.ProductOptions.Any(p => p.Code == Product.Rb && p.Status),
                    NgVisible = metaModel.ProductOptions.Any(p => p.Code == Product.Ng && p.Status),
                    LcVisible = metaModel.ProductOptions.Any(p => p.Code == Product.Lc && p.Status),
                    SbVisible = metaModel.ProductOptions.Any(p => p.Code == Product.Sb && p.Status),
                    VsVisible = metaModel.ProductOptions.Any(p => p.Code == Product.Vs && p.Status),
                    CsVisible = metaModel.ProductOptions.Any(p => p.Code == Product.Cs && p.Status),
                    CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
                };
            var convertingOption = new ConvertingOption
                {
                    IsConvert = metaModel.IsConvert,
                    BaseCurrency = metaModel.BaseCurrency,
                    CurrencyName = metaModel.CurrencyName
                };

            if (date.HasValue)
            {
                metaModel.FromDay = date.Value.Day;
                metaModel.ToDay = date.Value.Day;
                metaModel.Month = date.Value.Month;
                metaModel.Year = date.Value.Year;
            }

            var exccelModel = new MobileWinLossViewModel
                {
                    MainResult = dataList,
                    SummaryTotal = summaryTotal,
                    TotalResult = _mobileWinLossService.TotalCalc(dataList),
                    Param = metaModel,
                    CurrencyList = SessionService.UserSession.CurrencyList,
                    ProductAccess = productAccess,
                    Colspan = 7 + (productAccess.IsAllowBa ? 2 : 0) + (productAccess.IsAllowRb ? 2 : 0),
                    IsAllowedCustomerCommission = SessionService.LicenseeSession.IsAllowedCustomerComm
                };

            var mobileWinLossExcel = new MobileWinLossExcel(templatePath, FileName, exccelModel, showingOption, convertingOption);
            mobileWinLossExcel.ProductOptions = metaModel.ProductOptions;
            mobileWinLossExcel.Export();

            return ExcelStream(mobileWinLossExcel.Workbook, FileName);
        }

        /// <summary>
        /// Get report data .
        /// </summary>
        /// <param name="metaModel">MetaModel object.</param>
        /// <param name="dataList">Output parameter - report data list.</param>
        /// <param name="summaryTotal">Output parameter - summary data.</param>
        [NonAction]
        private void GetData(
                    MobileWinLossMetaModel metaModel,
                    out IList<MobileWinLoss> dataList,
                    out Dictionary<string, MobileWinLoss> summaryTotal)
        {
            var licenseeSession = SessionService.LicenseeSession;
            var licenseeId = licenseeSession.LicenseeId;
            metaModel.LicenseeId = licenseeId;

            dataList = _mobileWinLossService.GetMasterData(metaModel, out summaryTotal);
        }

        #region Private Utility

        /// <summary>
        /// Process request.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <param name="date">Report date value.</param>
        /// <returns>Dynamic object.</returns>
        [NonAction]
        private dynamic ProcessRequest(MobileWinLossMetaModel metaModel, DateTime? date)
        {
            if (!IsValidRequest(metaModel))
            {
                RedirectToErrorPage();
            }

            var licenseeSession = SessionService.LicenseeSession;
            metaModel.LicenseeId = licenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            Dictionary<string, MobileWinLoss> summaryTotal;
            IList<MobileWinLoss> dataList;

            GetData(metaModel, out dataList, out summaryTotal);

            return metaModel.IsExportExcel
                ? ExportExcel(metaModel, dataList, summaryTotal, date)
                : (dynamic)new
                  {
                      Data = dataList,
                      Total = _mobileWinLossService.TotalCalc(dataList),
                      SummaryTotal = summaryTotal,
                      ProductOptions = metaModel.ProductOptions.ToJson()
                  };
        }

        #endregion Private Utility

        #endregion ProceseeRequest
    }
}