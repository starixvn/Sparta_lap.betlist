// --------------------------------------------------------------------------
// <copyright file="ReportController.MobileWl.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Fanex.Athena.Excel.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Athena.Models.ViewModel.Report;

    /// <summary>
    /// MobileWinLossController class.
    /// </summary>
    public partial class ReportController
    {
        private const string MobileWinlossName = "MobileWinloss";

        [HttpPost]
        public dynamic MobileWinloss([FromBody] MobileWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null);
        }

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        private HttpResponseMessage ExportExcel(
            MobileWinLossMetaModel metaModel,
            IList<MobileWinLossMember> dataList,
            MobileWinLossMember totalData,
            DateTime? date)
        {
            const string FileName = "PlatformWinLoss.xls";
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/MobileWinLoss.xls");
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, MobileWinlossName);

            var showingOption = new ShowingOption
            {
                BAVisible = metaModel.ProductOptions.Any(p => p.Code == Product.BA && p.Status),
                RBVisible = metaModel.ProductOptions.Any(p => p.Code == Product.RB && p.Status),
                NGVisible = metaModel.ProductOptions.Any(p => p.Code == Product.NG && p.Status),
                LCVisible = metaModel.ProductOptions.Any(p => p.Code == Product.LC && p.Status),
                SBVisible = metaModel.ProductOptions.Any(p => p.Code == Product.SB && p.Status),
                VSVisible = metaModel.ProductOptions.Any(p => p.Code == Product.VS && p.Status),
                CSVisible = metaModel.ProductOptions.Any(p => p.Code == Product.CS && p.Status),
                KNVisible = metaModel.ProductOptions.Any(p => p.Code == Product.KN && p.Status),
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

            var excelModel = new MobileWinLossViewModel
            {
                MainResult = dataList,
                TotalResult = totalData,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = productAccess,
                Colspan = 7 + (productAccess.IsAllowBA ? 2 : 0) + (productAccess.IsAllowRB ? 2 : 0),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            var mobileWinLossExcel = new MobileWinLossExcel(templatePath, FileName, excelModel, showingOption, convertingOption);
            mobileWinLossExcel.ProductOptions = metaModel.ProductOptions;
            mobileWinLossExcel.Export();

            return ExcelStream(mobileWinLossExcel.Workbook, FileName);
        }

        private HttpResponseMessage ExportExcelForPlatformWinLoss(
            MobileWinLossMetaModel metaModel,
            IList<MobileWinLossSummary> dataList,
            MobileWinLossSummary total)
        {
            const string FileName = "PlatformWinLoss.xls";
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/PlatformWinLoss.xls");

            var mobileWinLossExcel = new PlatformWinLossExcel(templatePath, FileName, metaModel, dataList, total);
            mobileWinLossExcel.Export();

            return ExcelStream(mobileWinLossExcel.Workbook, FileName);
        }

        private dynamic ProcessRequest(MobileWinLossMetaModel metaModel, DateTime? date)
        {
            if (!IsValidRequest(metaModel))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ViewResources.InvalidParams));
            }

            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            if (metaModel.Level == 0)
            {
                MobileWinLossSummary totalData;
                var dataList = _mobileWinLossService.GetSummaryData(metaModel, out totalData);
                return metaModel.IsExportExcel ?
                    ExportExcelForPlatformWinLoss(metaModel, dataList, totalData) :
                   (dynamic)new
                   {
                       Data = dataList,
                       Total = totalData,
                       ProductOptions = metaModel.ProductOptions.ToJson()
                   };
            }
            else
            {
                MobileWinLossMember totalData;
                var dataList = _mobileWinLossService.GetMemberData(metaModel, out totalData);

                return metaModel.IsExportExcel
                    ? ExportExcel(metaModel, dataList, totalData, date)
                    : (dynamic)new
                    {
                        Data = dataList,
                        Total = totalData,
                        ProductOptions = metaModel.ProductOptions.ToJson()
                    };
            }
        }
    }
}