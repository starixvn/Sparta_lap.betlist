namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using Interfaces;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public class CasinoTurnoverDataAccess : ICasinoTurnoverDataAccess
    {
        public ReportResultModel<CasinoTurnover> GetData(CasinoTurnoverMetaModel metaModel)
        {
            return new ReportResultModel<CasinoTurnover>
            {
                DataList = new List<CasinoTurnover>(),
                TotalData = new CasinoTurnover(),
                TotalRows = 1
            };
        }
    }
}