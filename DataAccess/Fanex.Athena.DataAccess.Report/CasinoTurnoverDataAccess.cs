namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Common;
    using Common.Extensions;
    using Core;
    using Core.Configuration.Interfaces;
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

        public async Task<ReportResultModel<CasinoTurnover>> GetData(CasinoTurnoverMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_CasinoTurnover";

            IEnumerable<CasinoTurnover> dataList = null;
            CasinoTurnover totalData = null;

            using (var db = GetObjectDb(UspName))
            {
                await AddParams(metaModel, db).ConfigureAwait(false);
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

        private async Task AddParams(CasinoTurnoverMetaModel metaModel, IObjectDb db)
        {
            db.SetParameterValue("fromdate", metaModel.FromDate.Date);
            db.SetParameterValue("todate", metaModel.ToDate.Date);
            db.SetParameterValue("basecurrency", metaModel.BaseCurrency);
            db.SetParameterValue("UserName", metaModel.UserName);
            db.SetParameterValue("StatusList", string.Join(",", metaModel.Status));
            db.SetParameterValue("ProductCodes", string.Join(",", metaModel.ProductCode));
            db.SetParameterValue("MoveBetsDay", (await _balanceUpDataAccess.GetBalanceUp()).MoveBetsDay);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);

            db.SetParameterValue("PageSize", metaModel.IsExportExcel ? SystemConstants.MaxDataTotalRow : metaModel.PageSize);
            db.SetParameterValue("PageIndex", metaModel.IsExportExcel ? 1 : metaModel.PageIndex);
            db.SetParameterValue("TotalRows", null);
        }
    }
}