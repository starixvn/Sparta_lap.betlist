// --------------------------------------------------------------------------
// <copyright file="PlatformWinLossReportController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Hosting;
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportPlatformWinLoss)]
    public class PlatformWinLossReportController : ApiReportBaseController
    {
        private readonly IPlatformWinLossService _platformWinLossService;

        public PlatformWinLossReportController()
        {
            _platformWinLossService = new PlatformWinLossService();
        }

        public PlatformWinLossReportController(
            ISessionService sessionService,
            IProductService productService,
            IPlatformWinLossService platformWinLossService)
            : base(sessionService, productService)
        {
            _platformWinLossService = platformWinLossService;
        }

        [HttpPost]
        public dynamic Index(PlatformWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel);
        }

        [CLSCompliant(false)]
        protected internal virtual PlatformWinLossSummaryExcel CreateExcelSummary(
            string templatePath,
            PlatformWinLossMetaModel metaModel,
            IList<PlatformWinLossSummary> dataList,
            PlatformWinLossSummary total,
            ConvertingOption convertingOption)
        {
            return new PlatformWinLossSummaryExcel(templatePath, metaModel, dataList, total, convertingOption);
        }

        [CLSCompliant(false)]
        protected internal virtual PlatformWinLossMemberExcel CreateExcelMember(
            string templatePath,
            PlatformWinLossMetaModel metaModel,
            PlatformWinLossViewModel excelModel,
            ShowingOption showingOption,
            ConvertingOption convertingOption)
        {
            return new PlatformWinLossMemberExcel(templatePath, excelModel, showingOption, convertingOption)
            {
                ProductOptions = metaModel.ProductOptions
            };
        }

        private HttpResponseMessage ExportExcelSummary(
            PlatformWinLossMetaModel metaModel,
            IList<PlatformWinLossSummary> dataList,
            PlatformWinLossSummary total)
        {
            const string FileName = "PlatformWinLossSummary.xls";
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/PlatformWinLossSummary.xls");

            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };

            var excel = CreateExcelSummary(templatePath, metaModel, dataList, total, convertingOption);
            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        private HttpResponseMessage ExportExcelMember(
            PlatformWinLossMetaModel metaModel,
            IList<PlatformWinLossMember> dataList,
            PlatformWinLossMember totalData)
        {
            const string FileName = "PlatformWinLossMember.xls";
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/PlatformWinLossMember.xls");
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

            var platformWinLossExcel = CreateExcelMember(templatePath, metaModel, excelModel, showingOption, convertingOption);
            platformWinLossExcel.Export();

            return ExcelStream(platformWinLossExcel.Workbook, FileName);
        }

        private dynamic ProcessRequest(PlatformWinLossMetaModel metaModel)
        {
            if (!IsValidRequest(metaModel))
            {
                return BadRequest(ViewResources.InvalidParams);
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludeTest);

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
                    ? ExportExcelMember(metaModel, dataList, totalData)
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