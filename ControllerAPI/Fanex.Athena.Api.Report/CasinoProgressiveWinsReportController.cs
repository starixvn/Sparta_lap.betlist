// --------------------------------------------------------------------------
// <copyright file="ReportController.LicenseeWl.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Web.Hosting;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Model.Report.ViewModel;
    using Model.Core.DataDomain;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using VendorAdapter;
    using VendorAdapter.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportCasinoProgressiveWins)]
    public class CasinoProgressiveWinsReportController : ApiReportBaseController
    {
        private readonly ICasinoProgressiveWinsDataAccess _casinoProgressiveWinsDataAccess;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public CasinoProgressiveWinsReportController()
        {
            _casinoProgressiveWinsDataAccess = new CasinoProgressiveWinsDataAccess();
            _cacheRefAdapter = new CacheRefAdapter();
        }

        public CasinoProgressiveWinsReportController(
            ICasinoProgressiveWinsDataAccess dataAccess,
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            ICacheRefAdapter cacheRefAdapter)
            : base(sessionService, productService, langService)
        {
            _casinoProgressiveWinsDataAccess = dataAccess;
            _cacheRefAdapter = cacheRefAdapter;
        }

        [HttpPost]
        public dynamic Index(BaseReportMetaModel metaModel)
        {
            return ProcessRequest(metaModel);
        }

        private CasinoProgressiveWinsGridViewModel MapToGridView(CasinoProgressiveWins item)
        {
            var gameName = _cacheRefAdapter.GetMicroGamingGameName(item.TransDesc);
            var betTypeName = _cacheRefAdapter.GetBettype(item.BetType);
            var sportTypeName = _cacheRefAdapter.GetSportName(item.SportType);

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

        [CLSCompliant(false)]
        protected internal virtual CasinoProgressiveWinsExcel CreateExcel(
            string templatePath,
            ConvertingOption convertingOption,
            CasinoProgressiveWinsViewModel excelModel)
        {
            return new CasinoProgressiveWinsExcel(templatePath, convertingOption, excelModel);
        }

        private HttpResponseMessage ExportExcel(BaseReportMetaModel metaModel, IEnumerable<CasinoProgressiveWins> dataList)
        {
            const string FileName = "CasinoProgressiveWin_Microgaming.xls";
            var templatePath = HostingEnvironment.MapPath(@"~/App_Data/Template/Reports/CasinoProgressiveWins.xls");

            var convertingOption = new ConvertingOption
            {
                IsConvert = metaModel.IsConvert,
                BaseCurrency = metaModel.BaseCurrency,
                CurrencyName = metaModel.CurrencyName
            };

            var excelModel = new CasinoProgressiveWinsViewModel
            {
                DataList = dataList,
                Param = metaModel
            };

            var excel = CreateExcel(templatePath, convertingOption, excelModel);

            excel.Export();

            return ExcelStream(excel.Workbook, FileName);
        }

        private dynamic ProcessRequest(BaseReportMetaModel metaModel)
        {
            if (IsValidRequest(metaModel))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                var dataList = metaModel.IsValid
                       ? this._casinoProgressiveWinsDataAccess.GetDataList(metaModel)
                       : new List<CasinoProgressiveWins>();

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