// --------------------------------------------------------------------------
// <copyright file="ReportController.LicenseeWl.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Web;
    using System.Web.Http;
    using Base.Attributes;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Report;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DataDomain;

    [ApiFunctionAuthorization(PermissionKeys.ReportCasinoProgressiveWins)]
    public class CasinoProgressiveWinsReportController : ApiReportBaseController
    {
        private readonly ICasinoProgressiveWinsService _casinoProgressiveWinsService;

        public CasinoProgressiveWinsReportController()
        {
            _casinoProgressiveWinsService = new CasinoProgressiveWinsService();
        }

        [HttpPost]
        public dynamic Index(CasinoProgressiveWinsMetaModel metaModel)
        {
            return ProcessRequest(metaModel);
        }

        private static CasinoProgressiveWinsGridViewModel MapToGridView(CasinoProgressiveWins item)
        {
            var gameName = CacheRef.GetMicroGamingGameName(item.TransDesc);
            var betTypeName = CacheRef.GetBettype(item.BetType);
            var sportTypeName = CacheRef.GetSportName(item.SportType);

            var choice = new StringBuilder();
            choice.AppendFormat("<strong>{0}</strong></br>", gameName);
            choice.AppendFormat("<strong class='sport'>{0}</strong>&nbsp;", sportTypeName);
            choice.AppendFormat("<strong class='bettype'>{0}</strong>", betTypeName);

            return new CasinoProgressiveWinsGridViewModel
            {
                Index = item.Index,
                SystemId = item.SystemId,
                ExternalId = item.ExternalId,
                RefNo = item.RefNo,
                WinlostDate = item.WinlostDate,
                Choice = choice.ToString(),
                WinAmount = item.WinAmount,
                CustId = item.CustId,
                BetType = item.BetType,
                GameId = item.GameId
            };
        }

        private HttpResponseMessage ExportExcel(CasinoProgressiveWinsMetaModel metaModel, IEnumerable<CasinoProgressiveWins> dataList)
        {
            const string FileName = "CasinoProgressiveWin_Microgaming.xls";
            var templatePath = HttpContext.Current.Server.MapPath(@"~/App_Data/Template/Reports/CasinoProgressiveWins.xls");

            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };

            var excelModel = new CasinoProgressiveWinsViewModel
            {
                DataList = dataList
            };

            var excel = new CasinoProgressiveWinsExcel(templatePath, FileName, convertingOption, metaModel, excelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        private dynamic ProcessRequest(CasinoProgressiveWinsMetaModel metaModel)
        {
            if (IsValidRequest(metaModel))
            {
                metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
                metaModel.UserId = SessionService.UserSession.UserId;

                var dataList = _casinoProgressiveWinsService.GetDataList(metaModel);

                if (metaModel.IsExportExcel)
                {
                    return ExportExcel(metaModel, dataList);
                }

                if (dataList.Any())
                {
                    return new
                    {
                        Data = dataList.Select(MapToGridView).ToList()
                    };
                }
            }

            return new
            {
                Data = new List<CasinoProgressiveWinsGridViewModel>()
            };
        }
    }
}