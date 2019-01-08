namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common;
    using Common.Extensions;
    using Core;
    using Core.Configuration.Interfaces;
    using Core.Exception;
    using Data;
    using Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.Helper;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;

    public class CasinoTurnoverDataAccess : BaseDataAccess, ICasinoTurnoverDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public CasinoTurnoverDataAccess(IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public ReportResultModel<CasinoTurnover> GetData(CasinoTurnoverMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_CasinoTurnover";

            IEnumerable<CasinoTurnover> dataList = null;
            CasinoTurnover totalData = null;
            try
            {
                using (var db = GetObjectDb(UspName))
                {
                    AddParams(metaModel, db);
                    using (var reader = db.ExecuteReader())
                    {
                        dataList = ReadData(reader, metaModel.IsConvertAll);
                        if (dataList.Any())
                        {
                            reader.NextResult();
                            if (reader.Read())
                            {
                                totalData = ReadTotalData(reader);
                            }
                        }
                    }

                    var totalRows = Converter.ToInt(db.GetParameterValue("TotalRows"));
                    return new ReportResultModel<CasinoTurnover>
                    {
                        DataList = dataList,
                        TotalData = totalData,
                        TotalRows = totalRows
                    };
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        private static IEnumerable<CasinoTurnover> ReadData(IDataReader reader, bool isConvertAll)
        {
            var dataList = new List<CasinoTurnover>();
            var turnoverColumn = "Turnover" + (isConvertAll ? "Base" : string.Empty);
            var validBetAmountColumn = "ValidBetAmount" + (isConvertAll ? "Base" : string.Empty);
            var winlostColumn = "Winlost" + (isConvertAll ? "Base" : string.Empty);
            while (reader.Read())
            {
                var productCode = reader.ToString("ProductCode");
                dataList.Add(new CasinoTurnover
                {
                    Date = reader.ToDateTime("Date"),
                    SystemId = reader.ToString("UserName"),
                    ExternalId = reader.ToString("NickName"),
                    Product = ProductHelper.GetProductName(productCode),
                    BetCount = reader.ToInt("BetCount"),
                    Turnover = reader.ToDecimal(turnoverColumn),
                    ValidBetAmount = reader.ToDecimal(validBetAmountColumn),
                    CustomerWL = reader.ToDecimal(winlostColumn)
                });
            }

            AddRowSpan(dataList);
            return dataList;
        }

        private static CasinoTurnover ReadTotalData(IDataReader reader)
        {
            return new CasinoTurnover
            {
                BetCount = reader.ToInt("BetCount"),
                Turnover = reader.ToDecimal("Turnover"),
                ValidBetAmount = reader.ToDecimal("ValidBetAmount"),
                CustomerWL = reader.ToDecimal("Winlost")
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
                var key = item.Date.ToString("MM/dd/yyyy") + item.SystemId;

                item.RowSpan = dataList.Count(x => x.Date.ToString("MM/dd/yyyy") + x.SystemId == key);
                item.Index = index;

                index++;
                i += item.RowSpan;
            }
        }

        private void AddParams(CasinoTurnoverMetaModel metaModel, IObjectDb db)
        {
            db.AddInParameter("fromdate", DbType.Date, metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, metaModel.ToDate.Date);
            db.AddInParameter("basecurrency", DbType.String, metaModel.BaseCurrency);
            db.AddInParameter("UserName", DbType.String, metaModel.UserName);
            db.AddInParameter("StatusList", DbType.String, string.Join(",", metaModel.Status));
            db.AddInParameter("ProductCodes", DbType.String, string.Join(",", metaModel.ProductCode));
            db.AddInParameter("MoveBetsDay", DbType.Date, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);

            db.AddInParameter("PageSize", DbType.Int32, metaModel.IsExportExcel ? SystemConstants.MaxDataTotalRow : metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.IsExportExcel ? 1 : metaModel.PageIndex);
            db.AddOutParameter("TotalRows", DbType.Int32, 10);
        }
    }
}