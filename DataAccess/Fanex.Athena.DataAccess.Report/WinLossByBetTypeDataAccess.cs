// -----------------------------------------------------------------------
// <copyright file="WinLossByBetTypeDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class WinLossByBetTypeDataAccess : BaseDataAccess, IWinLossByBetTypeDataAccess
    {
        public WinLossByBetTypeDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        public SortableList<WinLossByBetType> GetDataList(WinLossByBetTypeMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_WinLossByBettype";
            var uspParams = new
            {
                AgentList = metaModel.AgentListXml,
                metaModel.FromDate,
                metaModel.ToDate,
                BalanceUp.MoveBetsDay,
                BalanceUp.BalanceUpDay
            };

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        return ReadData(reader, metaModel.BaseCurrency);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        private static SortableList<WinLossByBetType> ReadData(IDataReader reader, string baseCurrency)
        {
            var dataList = new SortableList<WinLossByBetType>();
            var turnoverColumn = "Turnover" + baseCurrency;
            var winlossColumn = "MemberWinLost" + baseCurrency;
            var custCommColumn = "PlayerComm" + baseCurrency;
            var licenseeWlColumn = "LicenseeWL" + baseCurrency;
            var commBaColumn = "CommBA" + baseCurrency;
            var commRbColumn = "CommRB" + baseCurrency;
            var racebookRevenue = "WinLossRB" + baseCurrency;
            var amtDueToBaComumn = "AmtDueToBA" + baseCurrency;
            var amtDueToRbColumn = "AmtDueToRB" + baseCurrency;

            while (reader.Read())
            {
                var item = new WinLossByBetType();

                item.LiveIndicator = reader.ToBool("liveindicator");
                item.BetType = reader.ToInt("bettype");
                item.Turnover = reader.ToDecimal(turnoverColumn);
                item.CustomerWinLoss = reader.ToDecimal(winlossColumn);
                item.CustomerComm = reader.ToDecimal(custCommColumn);
                item.LicenseeWL = reader.ToDecimal(licenseeWlColumn);
                item.BACommission = reader.ToDecimal(commBaColumn);
                item.RBComm = reader.ToDecimal(commRbColumn);
                item.RacingRevenue = reader.ToDecimal(racebookRevenue);
                item.AmtDueToBA = reader.ToDecimal(amtDueToBaComumn);
                item.AmtDueToRacing = reader.ToDecimal(amtDueToRbColumn);

                dataList.Add(item);
            }

            return dataList;
        }
    }
}