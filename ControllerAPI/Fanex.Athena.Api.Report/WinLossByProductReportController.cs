// --------------------------------------------------------------------------
// <copyright file="WinLossByProductReportController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Hosting;
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportWinLossByProduct)]
    public class WinLossByProductReportController : ApiReportBaseController
    {
        private readonly IWinLossByProductService _winlossByProductService;

        public WinLossByProductReportController()
        {
            _winlossByProductService = new WinLossByProductService();
        }

        public WinLossByProductReportController(
            ISessionService sessionService,
            IProductService productService,
            IWinLossByProductService winlossByProductService)
            : base(sessionService, productService)
        {
            _winlossByProductService = winlossByProductService;
        }

        [HttpPost]
        public dynamic Index(
           WinLossByProductMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0,
            int memberId = 0,
            string systemId = null,
            string currencyName = null)
        {
            return ProcessRequest(metaModel, fdate, tdate, level, currencyId, custId, memberId, systemId, currencyName);
        }

        [HttpPost]
        public dynamic Index([FromBody] WinLossByProductMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        [CLSCompliant(false)]
        protected internal virtual WinlossByProductExcel CreateExcel(
            WinLossByProductMetaModel metaModel,
            string systemId,
            string errorMessage,
            string templatePath,
            ShowingOption showingOption,
            ConvertingOption convertingOption,
            WinLossByProductViewModel viewModel)
        {
            return new WinlossByProductExcel(templatePath, convertingOption, showingOption, metaModel, viewModel, systemId, errorMessage)
            {
                Level = metaModel.Level,
                EnableReportByMonth = metaModel.ReportBy
            };
        }

        #region Private Utility

        /// <summary>
        /// Restructure data.
        /// </summary>
        /// <param name="dataList">The data collection.</param>
        /// <param name="level">The Level value.</param>
        /// <returns>Return the grid view data collection.</returns>
        private static IList<WinLossByProduct> WinLossByProductRestructureData(IList<WinLossByProduct> dataList, int level)
        {
            // Calculate rowspan
            return level == 0
                ? DoRestructureData(dataList.GroupBy(i => new { i.Date, i.CurrencyName }))
                : DoRestructureData(dataList.GroupBy(i => new { i.SystemId, i.CurrencyName }));
        }

        #endregion Private Utility

        private static dynamic GetEmptyResult(WinLossByProductMetaModel metaModel, int memberId, string systemId, string errorMessage)
        {
            return new
            {
                Data = new List<WinLossByProduct>(),
                Total = new List<WinLossByProduct>(),
                ProductOptions = metaModel.ProductOptions.ToJson(),
                MemberId = memberId,
                SystemId = systemId,
                ErrorMessage = errorMessage
            };
        }

        private static IList<WinLossByProduct> DoRestructureData(dynamic groupData)
        {
            var newDataList = new List<WinLossByProduct>();

            var index = 0;
            foreach (var item in groupData)
            {
                var rowSpanFlag = true;
                var temps = (IList<WinLossByProduct>)item;
                /*processing index value*/
                var numOfItem = temps.Count();

                foreach (var col in temps)
                {
                    if (rowSpanFlag && numOfItem > 1)
                    {
                        index++;
                        col.RowSpan = numOfItem;
                        rowSpanFlag = false;
                    }
                    else if (numOfItem > 1)
                    {
                        col.RowSpan = -1;
                    }
                    else
                    {
                        index++;
                        col.RowSpan = 0;
                    }

                    col.Index = index;
                    newDataList.Add(col);
                }
            }

            return newDataList;
        }

        private static string GetErrorMessage(int code)
        {
            switch (code)
            {
                case 100:
                    return ViewResources.NoCustomerFoundMsg;

                case 101:
                    return ViewResources.MoreThanOneCustomerFoundMsg;

                default:
                    return string.Empty;
            }
        }

        #region ProcessRequest

        private HttpResponseMessage ExportExcel(
            WinLossByProductMetaModel metaModel,
            IList<WinLossByProduct> dataList,
            IDictionary<string, IList<WinLossByProduct>> summaryTotal,
            string systemId,
            string errorMessage)
        {
            const string FileName = "WinLossByProduct.xls";
            var templatePath = HostingEnvironment.MapPath("~/App_Data/Template/Reports/WinLossByProducts.xls");
            var licenseeSession = SessionService.LicenseeSession;
            var userSession = SessionService.UserSession;
            var productAccess = ProductService.GetProductAccess(licenseeSession.LicenseeId, userSession.Language);

            metaModel.FromDate = metaModel.Level == 0 ? metaModel.FromDate : metaModel.DrillDownFromDate;
            metaModel.ToDate = metaModel.Level == 0 ? metaModel.ToDate : metaModel.DrillDownToDate;

            var showingOption = new ShowingOption
            {
                BAVisible = productAccess.IsAllowBA,
                RBVisible = productAccess.IsAllowRB,
                NGVisible = productAccess.IsAllowNG,
                LCVisible = productAccess.IsAllowLC,
                SBVisible = productAccess.IsAllowSB,
                VSVisible = productAccess.IsAllowVS,
                KNVisible = productAccess.IsAllowKN,
                CustomerCommVisible = licenseeSession.IsAllowedCustomerComm
            };

            var convertingOption = new ConvertingOption
            {
                IsConvert = false,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.Level == 0 ? metaModel.CurrencyName : metaModel.DrillDownCurrencyName
            };

            var viewModel = new WinLossByProductViewModel
            {
                MainResult = dataList,
                SummaryTotal = summaryTotal
            };

            WinlossByProductExcel winlossByProduct = CreateExcel(metaModel, systemId, errorMessage, templatePath, showingOption, convertingOption, viewModel);
            winlossByProduct.Export();

            return ExcelStream(winlossByProduct.Workbook, FileName);
        }

        private void GetData(
            WinLossByProductMetaModel metaModel,
            out IList<WinLossByProduct> dataList,
            out IDictionary<string, IList<WinLossByProduct>> summaryTotal,
            out IResultInfo resultInfo)
        {
            if (metaModel.Level == 0)
            {
                dataList = _winlossByProductService.GetWinlossByProducts(metaModel, out summaryTotal, out resultInfo);
            }
            else
            {
                resultInfo = new ResultInfo();
                dataList = SessionService.LicenseeSession.LicenseeId > 1 || metaModel.Level == 2
                               ? _winlossByProductService.GetWinlossByProductCustomers(metaModel, out summaryTotal)
                               : _winlossByProductService.GetWinlossByProductAgents(metaModel, out summaryTotal);
            }
        }

        [SuppressMessage(
           "StyleCop.CSharp.StylecopPlus.Rules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        private dynamic ProcessRequest(
            WinLossByProductMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0,
            int memberId = 0,
            string systemId = null,
            string currencyName = null)
        {
            if (!IsValidRequest(metaModel, currencyId))
            {
                return BadRequest(ViewResources.InvalidParams);
            }

            metaModel.AgentListXml = SessionService.GenerateAgentListXml(level == 0 ? metaModel.CurrencyId : currencyId, metaModel.IsExcludedTestData);
            metaModel.Username = !string.IsNullOrWhiteSpace(metaModel.Username)
                                && metaModel.Username.Equals(ViewResources.ExternalIdOrSystemId, StringComparison.OrdinalIgnoreCase)
                                ? string.Empty : metaModel.Username;

            metaModel.DrillDownCurrencyId = currencyId;
            metaModel.DrillDownCurrencyName = currencyName;
            metaModel.Level = level;
            metaModel.CustomerId = custId;
            if (metaModel.MemberId == 0)
            {
                metaModel.MemberId = memberId;
            }

            if (fdate.HasValue)
            {
                metaModel.DrillDownFromDate = fdate.Value;
            }

            if (tdate.HasValue)
            {
                metaModel.DrillDownToDate = tdate.Value;
            }

            IDictionary<string, IList<WinLossByProduct>> summaryTotal;
            IList<WinLossByProduct> dataList;
            IResultInfo resultInfo;

            GetData(metaModel, out dataList, out summaryTotal, out resultInfo);

            resultInfo.Message = GetErrorMessage(resultInfo.Code);

            if (memberId == 0 && !string.IsNullOrWhiteSpace(resultInfo.ExtendedData))
            {
                var extendedItems = resultInfo.ExtendedData.Split(';');
                memberId = string.IsNullOrWhiteSpace(extendedItems[0]) ? 0 : Convert.ToInt32(extendedItems[0]);
                systemId = extendedItems[1];
                metaModel.MemberId = memberId;
            }

            if (metaModel.IsExportExcel)
            {
                return ExportExcel(metaModel, dataList, summaryTotal, systemId, resultInfo.Message);
            }

            return dataList.Count > 0
                ? new
                {
                    Data = WinLossByProductRestructureData(dataList, level),
                    Total = _winlossByProductService.TotalCalc(dataList),
                    SummaryTotal = summaryTotal,
                    ProductOptions = metaModel.ProductOptions.ToJson(),
                    MemberId = memberId,
                    SystemId = systemId,
                    ErrorMessage = resultInfo.Message
                }
                : GetEmptyResult(metaModel, memberId, systemId, resultInfo.Message);
        }

        #endregion ProcessRequest
    }
}