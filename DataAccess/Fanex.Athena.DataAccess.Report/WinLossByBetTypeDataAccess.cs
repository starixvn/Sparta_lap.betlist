// -----------------------------------------------------------------------
// <copyright file="WinLossByBetTypeDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Data;
    using Constant.DataDomain;
    using Constant.Helper;
    using Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Resources;
    using VendorAdapter.Interfaces;

    public class WinLossByBetTypeDataAccess : BaseDataAccess, IWinLossByBetTypeDataAccess
    {
        private readonly ICacheRefAdapter _cacheRefAdapter;
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public WinLossByBetTypeDataAccess(
            ICacheRefAdapter cacheRefAdapter,
            IBalanceUpDataAccess balanceUpDataAccess)
        {
            _cacheRefAdapter = cacheRefAdapter;
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public SortableList<WinLossByBetType> GetDataList(WinLossByBetTypeMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_WinLossByBettype";
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            var uspParams = new
            {
                AgentList = metaModel.AgentListXml,
                metaModel.FromDate,
                metaModel.ToDate,
                balanceUp.MoveBetsDay,
                balanceUp.BalanceUpDay
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
                throw new DataAccessException(sqlException, UspName, uspParams);
            }
        }

        private static string GetSportName(int bettype)
        {
            if (BetTypeHelper.IsVirtualSports(bettype))
            {
                return " (" + ViewResources.ProductVS + ")";
            }

            if (BetTypeHelper.IsVirtualSports2(bettype))
            {
                return " (" + ViewResources.ProductVS2 + ")";
            }

            if (bettype == (int)BetType.NumberGameHappy5OddEven)
            {
                return " (" + ViewResources.Happy5 + ")";
            }

            return string.Empty;
        }

        private SortableList<WinLossByBetType> ReadData(IDataReader reader, string baseCurrency)
        {
            var dataList = new SortableList<WinLossByBetType>();
            var turnoverColumn = "Turnover" + baseCurrency;
            var winlossColumn = "MemberWinLost" + baseCurrency;
            var custCommColumn = "PlayerComm" + baseCurrency;
            var licenseeWlColumn = "LicenseeWL" + baseCurrency;
            var commBaColumn = "CommBA" + baseCurrency;
            var amtDueToBaComumn = "AmtDueToBA" + baseCurrency;

            while (reader.Read())
            {
                var bettype = reader.ToInt("bettype");
                var liveIndicator = reader.ToBool("liveindicator");
                var item = new WinLossByBetType
                {
                    LiveIndicator = liveIndicator,
                    BetType = bettype,
                    BetTypeName = _cacheRefAdapter.GetBettype(bettype, liveIndicator) + GetSportName(bettype),
                    Turnover = reader.ToDecimal(turnoverColumn),
                    CustomerWinLoss = reader.ToDecimal(winlossColumn),
                    CustomerComm = reader.ToDecimal(custCommColumn),
                    LicenseeWL = reader.ToDecimal(licenseeWlColumn),
                    BACommission = reader.ToDecimal(commBaColumn),
                    AmtDueToBA = reader.ToDecimal(amtDueToBaComumn)
                };

                dataList.Add(item);
            }

            return dataList;
        }
    }
}