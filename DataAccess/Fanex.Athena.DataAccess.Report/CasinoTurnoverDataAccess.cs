namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using Core;
    using Extension.Utility;
    using Interfaces;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Session.Interfaces;

    public class CasinoTurnoverDataAccess : BaseDataAccess, ICasinoTurnoverDataAccess
    {
        public CasinoTurnoverDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

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