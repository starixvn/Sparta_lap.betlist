// --------------------------------------------------------------------------
// <copyright file="ApiWeeklyLiveMatchReportController.cs" company="">
// Copyright (c). All rights reserved.
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
    using Excel.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Report.ResultModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class WeeklyLiveMatchReportController : ApiReportBaseController
    {
        private readonly IWeeklyLiveMatchService _weeklyLiveMatchService;

        public WeeklyLiveMatchReportController()
        {
            _weeklyLiveMatchService = new WeeklyLiveMatchService();
        }

        public WeeklyLiveMatchReportController(IWeeklyLiveMatchService service, ISessionService sessionService, IProductService productService)
            : base(sessionService, productService)
        {
            _weeklyLiveMatchService = service;
        }

        [HttpPost]
        public dynamic Index(WeeklyLiveMatchMetaModel metaModel)
        {
            if (IsValidRequest(metaModel))
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.UserId = SessionService.UserSession.UserId;
                var result = _weeklyLiveMatchService.GetData(metaModel);

                var weekHeaders = _weeklyLiveMatchService.GetWeekHeaders(metaModel.FromDate, metaModel.ToDate, metaModel.NumberOfWeeks);
                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, result, weekHeaders);
                }

                if (result.DataList.Any())
                {
                    return new
                    {
                        WeekHeaders = weekHeaders,
                        NumberOfWeeks = metaModel.NumberOfWeeks,
                        Data = result.DataList.Select(MapToGridView).ToArray(),
                        TotalData = result.TotalData
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

        [CLSCompliant(false)]
        protected internal virtual WeeklyLiveMatchExcel CreateExcel(
            string templatePath,
            WeeklyLiveMatchMetaModel metaModel,
            WeeklyLiveMatchResultModel resultModel,
            IEnumerable<string> weekHeaders)
        {
            return new WeeklyLiveMatchExcel(templatePath, metaModel, resultModel, weekHeaders);
        }

        private HttpResponseMessage ExportExcel(
            WeeklyLiveMatchMetaModel metaModel,
            WeeklyLiveMatchResultModel resultModel,
            IEnumerable<string> weekHeaders)
        {
            const string FileName = "WeeklyLiveMatches.xls";
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/WeeklyLiveMatch.xls");

            var excel = CreateExcel(templatePath, metaModel, resultModel, weekHeaders);

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }
    }
}