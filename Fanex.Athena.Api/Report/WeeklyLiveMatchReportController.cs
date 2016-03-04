// --------------------------------------------------------------------------
// <copyright file="ApiWeeklyLiveMatchReportController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using Excel.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Implement.Report;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    /// ReportController class.
    /// </summary>
    public class WeeklyLiveMatchReportController : ApiReportBaseController
    {
        private readonly IWeeklyLiveMatchService _weeklyLiveMatchService;

        public WeeklyLiveMatchReportController()
        {
            _weeklyLiveMatchService = new WeeklyLiveMatchService();
        }

        [HttpPost]
        public dynamic Index(WeeklyLiveMatchMetaModel metaModel)
        {
            if (IsValidRequest(metaModel))
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.UserId = SessionService.UserSession.UserId;
                int[] totalData;
                var dataList = _weeklyLiveMatchService.GetData(metaModel, out totalData);

                var weekHeaders = _weeklyLiveMatchService.GetWeekHeaders(metaModel.FromDate, metaModel.ToDate, metaModel.NumberOfWeeks);
                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, dataList, totalData, weekHeaders);
                }

                if (dataList.Any())
                {
                    return new
                    {
                        WeekHeaders = weekHeaders,
                        NumberOfWeeks = metaModel.NumberOfWeeks,
                        Data = dataList.Select(MapToGridView).ToArray(),
                        TotalData = totalData
                    };
                }
            }

            return new
            {
                WeekHeaders = new List<string>(),
                NumberOfWeeks = 0,
                Data = new object[0],
                TotalData = new int[0]
            };
        }

        private static object MapToGridView(WeeklyLiveMatch item)
        {
            var data = new List<object> { item.Index, item.SportName };
            data.AddRange(item.NumbersOfMatches.Cast<object>());
            return new
            {
                Cell = data
            };
        }

        private HttpResponseMessage ExportExcel(
            WeeklyLiveMatchMetaModel metaModel,
            IEnumerable<WeeklyLiveMatch> dataList,
            int[] totalData,
            IEnumerable<string> weekHeaders)
        {
            const string FileName = "WeeklyLiveMatches.xls";
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/WeeklyLiveMatch.xls");

            var excel = new WeeklyLiveMatchExcel(templatePath, FileName, metaModel, dataList, totalData, weekHeaders);

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }
    }
}