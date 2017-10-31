// -----------------------------------------------------------------------
// <copyright file="RacingCommissionDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Data;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.ResultModel;

    public class RacingCommissionDataAccess : BaseDataAccess, IRacingCommissionDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public RacingCommissionDataAccess()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
        }

        public RacingCommissionDataAccess(IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public SortableReportResultModel<RacingCommission> GetData(RacingCommissionMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_CommByBettype_HR";
            var uspParams = new
            {
                AgentList = metaModel.AgentListXml,
                metaModel.FromDate,
                metaModel.ToDate,
                _balanceUpDataAccess.GetBalanceUp().MoveBetsDay,
                metaModel.PageSize,
                metaModel.PageIndex
            };

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var baseCurrency = metaModel.BaseCurrency;

                        return ReadData(reader, baseCurrency);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, uspParams);
            }
        }

        private static SortableReportResultModel<RacingCommission> ReadData(IDataReader reader, string baseCurrency)
        {
            var dataList = new SortableList<RacingCommission>();
            var totalData = new RacingCommission();
            var toteWinColumn = "Tote_Win" + baseCurrency;
            var totePlaceColumn = "Tote_Place" + baseCurrency;
            var toteWinPlaceColumn = "Tote_WinPlace" + baseCurrency;
            var toteTotalColumn = "Tote_Total" + baseCurrency;
            var customerCommColumn = "Tote_Total_PlayerComm" + baseCurrency;
            var sumToteWinColumn = "SumTote_Win" + baseCurrency;
            var sumTotePlaceColumn = "SumTote_Place" + baseCurrency;
            var sumToteWinPlaceColumn = "SumTote_WinPlace" + baseCurrency;
            var sumToteTotalColumn = "SumTote_Total" + baseCurrency;
            var sumTcustomerCommColumn = "SumTote_Total_PlayerComm" + baseCurrency;

            while (reader.Read())
            {
                dataList.Add(new RacingCommission
                {
                    CustID = reader.ToInt("custID"),
                    ExternalID = reader.ToString("NickName"),
                    SystemID = reader.ToString("UserName"),
                    ToteWin = reader.ToDecimal(toteWinColumn),
                    TotePlace = reader.ToDecimal(totePlaceColumn),
                    ToteWinPlace = reader.ToDecimal(toteWinPlaceColumn),
                    ToteTotal = reader.ToDecimal(toteTotalColumn),
                    CustomerCommission = reader.ToDecimal(customerCommColumn)
                });
            }

            if (dataList.Count > 0 && reader.NextResult())
            {
                while (reader.Read())
                {
                    totalData.TotalRows = reader.ToInt("TotalRows");
                    totalData.ToteWin = reader.ToDecimal(sumToteWinColumn);
                    totalData.TotePlace = reader.ToDecimal(sumTotePlaceColumn);
                    totalData.ToteWinPlace = reader.ToDecimal(sumToteWinPlaceColumn);
                    totalData.ToteTotal = reader.ToDecimal(sumToteTotalColumn);
                    totalData.CustomerCommission = reader.ToDecimal(sumTcustomerCommColumn);
                }
            }

            return new SortableReportResultModel<RacingCommission>
            {
                DataList = dataList,
                TotalData = totalData
            };
        }
    }
}