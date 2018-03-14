namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report;
    using DataAccess.Report.Interfaces;
    using Model.Core.DataDomain;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportLicenseeWinLossByTimeZone)]
    public class LicenseeWinLossByTimeZoneReportController : ApiReportBaseController
    {
        private readonly ILicenseeWinLossByTimeZoneDataAccess _licenseeWinLossByTimeZoneDataAccess;

        public LicenseeWinLossByTimeZoneReportController()
        {
            _licenseeWinLossByTimeZoneDataAccess = new LicenseeWinLossByTimeZoneDataAccess();
        }

        public LicenseeWinLossByTimeZoneReportController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            ILicenseeWinLossByTimeZoneDataAccess licenseeWinLossByTimeZoneDataAccess)
            : base(sessionService, productService, langService)
        {
            _licenseeWinLossByTimeZoneDataAccess = licenseeWinLossByTimeZoneDataAccess;
        }

        [HttpPost]
        public dynamic Index(LicenseeWinLossByTimeZoneMetaModel metaModel)
        {
            if (metaModel.IsValid)
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId, metaModel.IsExcludeTestData);

                var result = _licenseeWinLossByTimeZoneDataAccess.GetData(metaModel);

                return new
                {
                    Data = result.DataList,
                    Total = result.TotalData
                };
            }

            return new
            {
                Data = new List<LicenseeWinLossByTimeZone>()
            };
        }
    }
}