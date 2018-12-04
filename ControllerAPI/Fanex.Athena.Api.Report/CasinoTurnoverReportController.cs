namespace Fanex.Athena.Api.Report
{
    using System.Linq;
    using System.Web.Http;
    using Base.Attributes;
    using DataAccess.Report.Interfaces;
    using Model.Core.DataDomain;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ReportCasinoTurnover)]
    public class CasinoTurnoverReportController : ApiReportBaseController
    {
        private readonly ICasinoTurnoverDataAccess _casinoTurnoverDataAccess;

        public CasinoTurnoverReportController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            ICasinoTurnoverDataAccess dataAccess)
            : base(sessionService, productService, langService)
        {
            _casinoTurnoverDataAccess = dataAccess;
        }

        [HttpPost]
        public dynamic GetData(CasinoTurnoverMetaModel metaModel)
        {
            CheckValidProducts(FunctionNames.CasinoTurnover, metaModel.ProductCode);
            if (IsValidRequest(metaModel.CurrencyId))
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);
                var result = _casinoTurnoverDataAccess.GetData(metaModel);
                return new
                {
                    Data = result.DataList,
                    Total = result.TotalData
                };
            }

            return new
            {
                Data = Enumerable.Empty<CasinoTurnover>(),
                Total = new CasinoTurnover()
            };
        }
    }
}