namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Extensions;
    using Core;
    using Core.Configuration.Interfaces;
    using Core.Exception;
    using Data;
    using Extension.Utility;
    using Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.Helper;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Session.Interfaces;

    public class CasinoTurnoverDataAccess : BaseDataAccess, ICasinoTurnoverDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public CasinoTurnoverDataAccess(
            IAppSettings appSetting,
            ISessionStorage sessionStorage,
            IBalanceUpDataAccess balanceUpDataAccess)
            : base(appSetting, sessionStorage)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public ReportResultModel<CasinoTurnover> GetData(CasinoTurnoverMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_CasinoTurnover";

            try
            {
                using (var db = GetObjectDb(UspName))
                {
                    AddParams(metaModel, db);
                    using (var reader = db.ExecuteReader())
                    {
                        var dataList = new List<CasinoTurnover>();
                        var turnoverColumn = "Turnover" + (metaModel.IsConvertAll ? "Base" : string.Empty);
                        var validBetAmountColumn = "ValidBetAmount" + (metaModel.IsConvertAll ? "Base" : string.Empty);
                        var winlostColumn = "Winlost" + (metaModel.IsConvertAll ? "Base" : string.Empty);
                        while (reader.Read())
                        {
                            var productCode = reader.ToString("ProducCode");
                            dataList.Add(new CasinoTurnover
                            {
                                Date = reader.ToDateTime("Date"),
                                SystemId = reader.ToString("UserName"),
                                ExternalId = reader.ToString("NickName"),
                                Product = ProductHelper.GetProductName(productCode),
                                Turnover = reader.ToDecimal(turnoverColumn),
                                ValidBetAmount = reader.ToDecimal(validBetAmountColumn),
                                CustomerWL = reader.ToDecimal(winlostColumn)
                            });
                        }

                        AddRowSpan(dataList);

                        var totalRows = (int)db.GetParameterValue("TotalRows");

                        return new ReportResultModel<CasinoTurnover>
                        {
                            DataList = dataList,
                            TotalRows = totalRows,
                            TotalData = new CasinoTurnover
                            {
                                BetCount = 10000,
                                Turnover = 34343m,
                                ValidBetAmount = 45645m,
                                CustomerWL = 934534m
                            },
                        };
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        private void AddParams(CasinoTurnoverMetaModel metaModel, IObjectDb db)
        {
            db.AddInParameter("fromdate", DbType.Date, metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, metaModel.ToDate.Date);
            db.AddInParameter("basecurrency", DbType.String, metaModel.BaseCurrency);
            db.AddInParameter("UserName", DbType.String, metaModel.UserName);
            db.AddInParameter("Statuses", DbType.String, metaModel.UserName);
            db.AddInParameter("ProductCodes", DbType.String, string.Join(",", metaModel.ProductCode));
            db.AddInParameter("MoveBetsDay", DbType.Date, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);

            db.AddInParameter("PageSize", DbType.Int32, metaModel.IsExportExcel ? SystemConstants.MaxDataTotalRow : metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.IsExportExcel ? 1 : metaModel.PageIndex);
            db.AddOutParameter("TotalRows", DbType.Int32, 10);
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