namespace Fanex.Athena.DataAccess.Report
{
    using System;
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
                    Date = DateTime.Now.AddDays((i / 2)),
                    ExternalId = "ExternalId " + (i / 2),
                    Product = i % 2 == 0 ? "AB" : "CS",
                    SystemId = "SystemId " + (i / 2),
                    Turnover = i * 10,
                    ValidBetAmount = i * 20,
                    RowSpan = -1
                });
            }

            AddRowSpan(dataList);

            return new ReportResultModel<CasinoTurnover>
            {
                DataList = dataList,
                TotalData = new CasinoTurnover(),
                TotalRows = 100
            };
        }
        

        private static void AddRowSpan(IEnumerable<CasinoTurnover> dataList)
        {
            var index = 1;
            var i = 0;
            var total = dataList.Count();

            while (i < total)
            {
                var item = dataList.ElementAt(i);
                var key = item.Date.ToString("MM/dd/yyyy") + item.ExternalId;

                item.RowSpan = dataList.Count(x => x.Date.ToString("MM/dd/yyyy") + x.ExternalId == key);
                item.Index = index;

                index++;
                i += item.RowSpan;
            }
        }
    }
}