namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Linq;
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
            var dataList = new List<CasinoTurnover>(100);
            for (int i = 0; i < 100; i++)
            {
                dataList.Add(new CasinoTurnover
                {
                    BetCount = i,
                    CustomerWL = i * 100,
                    Date = new System.DateTime().AddDays(200-i),
                    ExternalId = "ExternalId " + i,
                    Product = i % 2 == 0 ? "AB" : "CS",
                    SystemId = "SystemId " + i,
                    Turnover = i * 10,
                    ValidBetAmount = i * 20
                });
            }
            
            return new ReportResultModel<CasinoTurnover>
            {
                DataList = dataList.Skip((metaModel.PageIndex - 1) * metaModel.PageSize).Take(metaModel.PageSize),
                TotalData = new CasinoTurnover(),
                TotalRows = 100
            };
        }
    }
}