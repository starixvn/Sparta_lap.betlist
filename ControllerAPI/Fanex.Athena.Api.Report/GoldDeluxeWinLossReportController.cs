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
    using Base.Attributes;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportGoldDeluxeWinloss)]
    public class GoldDeluxeWinLossReportController : ApiReportBaseController
    {
        private readonly IGoldDeluxeWinLossService _golddeluxeWinLossService;

        public GoldDeluxeWinLossReportController()
        {
            _golddeluxeWinLossService = new GoldDeluxeWinLossService();
        }

        [HttpPost]
        public dynamic Index(
            GoldDeluxeWinLossMetaModel metaModel,
            DateTime? fdate,
            DateTime? tdate,
            int level = 0,
            int currencyId = 0,
            int custId = 0)
        {
            return ProcessRequest(metaModel, fdate, tdate, level, currencyId, custId);
        }

        [HttpPost]
        public dynamic Index([FromBody] GoldDeluxeWinLossMetaModel metaModel)
        {
            return ProcessRequest(metaModel, null, null);
        }

        private static dynamic GetEmptyGoldDeluxeWinLossResult()
        {
            return new
            {
                Data = new List<GoldDeluxeWinLoss>(),
                Total = new List<GoldDeluxeWinLoss>(),
                SummaryTotal = new Dictionary<string, GoldDeluxeWinLoss>()
            };
        }

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

        private void GetData(
                GoldDeluxeWinLossMetaModel metaModel,
                out SortableList<GoldDeluxeWinLoss> dataList,
                out IDictionary<string, GoldDeluxeWinLoss> summaryTotal)
        {
            if (metaModel.DrillDownLevel == 0)
            {
                dataList = _golddeluxeWinLossService.GetSummaryWinloss(metaModel, out summaryTotal);
            }
            else
            {
                dataList = metaModel.DrillDownLevel == 2
                               ? _golddeluxeWinLossService.GetCustomerWinlossDetail(metaModel, out summaryTotal)
                               : _golddeluxeWinLossService.GetAgentWinlossDetail(metaModel, out summaryTotal);
            }
        }

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
                        Total = _golddeluxeWinLossService.TotalCalc(dataList),
                        SummaryTotal = summaryTotal
                    };
                }
            }

            return GetEmptyGoldDeluxeWinLossResult();
        }
    }
}