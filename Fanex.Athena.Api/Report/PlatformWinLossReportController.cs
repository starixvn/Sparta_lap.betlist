// --------------------------------------------------------------------------
// <copyright file="ReportController.PlatformWinLoss.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
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
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel.Report;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Resources;
    using Service.Implement.Report;
    using Service.Interface.Report;
    using Web.ActionFilter;

    public class PlatformWinLossReportController : ApiReportBaseController
    {
        private readonly IPlatformWinLossService _platformWinLossService;

        public PlatformWinLossReportController()
        {
            _platformWinLossService = new PlatformWinLossService();
        }

        [HttpPost]
        [ApiFunctionAuthorization]
        public dynamic PlatformWinloss(PlatformWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel);
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        private HttpResponseMessage ExportExcel(
            PlatformWinLossMetaModel metaModel,
            IList<PlatformWinLossMember> dataList,
            PlatformWinLossMember totalData)
        {
            const string FileName = "PlatformWinLossMember.xls";
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/PlatformWinLossMember.xls");
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language, FunctionNames.PlatformWinloss);

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

            var excelModel = new PlatformWinLossViewModel
            {
                MainResult = dataList,
                TotalResult = totalData,
                Param = metaModel,
                CurrencyList = userSession.CurrencyList,
                ProductAccess = productAccess,
                Colspan = 7 + (productAccess.IsAllowBA ? 2 : 0) + (productAccess.IsAllowRB ? 2 : 0),
                IsAllowedCustomerCommission = licenseeSession.IsAllowedCustomerComm
            };

            var mobileWinLossExcel = new PlatformWinLossMemberExcel(templatePath, FileName, excelModel, showingOption, convertingOption);
            mobileWinLossExcel.ProductOptions = metaModel.ProductOptions;
            mobileWinLossExcel.Export();

            return ExcelStream(mobileWinLossExcel.Workbook, FileName);
        }

        private HttpResponseMessage ExportExcelSummary(
            PlatformWinLossMetaModel metaModel,
            IList<PlatformWinLossSummary> dataList,
            PlatformWinLossSummary total)
        {
            const string FileName = "PlatformWinLossSummary.xls";
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/PlatformWinLossSummary.xls");

            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };

            var platformWinLossExcel = new PlatformWinLossSummaryExcel(templatePath, FileName, metaModel, dataList, total, convertingOption);
            platformWinLossExcel.Export();

            return ExcelStream(platformWinLossExcel.Workbook, FileName);
        }

        private dynamic ProcessRequest(PlatformWinLossMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ViewResources.InvalidParams));
            }

            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            if (metaModel.Level == 0)
            {
                PlatformWinLossSummary totalData;
                var dataList = _platformWinLossService.GetSummaryData(metaModel, out totalData);
                return metaModel.IsExportExcel ?
                    ExportExcelSummary(metaModel, dataList, totalData) :
                   (dynamic)new
                   {
                       Data = dataList,
                       Total = totalData,
                       ProductOptions = metaModel.ProductOptions.ToJson()
                   };
            }
            else
            {
                PlatformWinLossMember totalData;
                var dataList = _platformWinLossService.GetMemberData(metaModel, out totalData);

                return metaModel.IsExportExcel
                    ? ExportExcel(metaModel, dataList, totalData)
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