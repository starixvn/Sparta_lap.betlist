// -----------------------------------------------------------------------
// <copyright file="LastBetsMonitoringBetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetList
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Data;
    using Interfaces;
    using Session.Interfaces;

    public class LastBetsMonitoringBetListDataAccess : BaseBetListDataAccess, ILastBetsMonitoringBetListDataAccess
    {
        private const string UspKey = "Lrf_Monitor_BetlistSel_New";

        public TicketResult GetLastBetsMonitoringData(LastBetsMonitoringMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            var ticketResult = new TicketResult();
            try
            {
                using (var db = this.GetObjectDb(UspKey))
                {
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("BetType", DbType.String, metaModel.BetType);
                    db.AddInParameter("Currency", DbType.String, metaModel.Currency);
                    db.AddInParameter("SpecialLevel", DbType.String, metaModel.Level);
                    db.AddInParameter("TransId", DbType.Int64, metaModel.TransId);
                    db.AddInParameter("LimitRowCount", DbType.Int32, metaModel.PageSize);
                    db.AddInParameter("SportId", DbType.Int32, metaModel.SportId);
                    db.AddInParameter("LeagueId", DbType.Int32, metaModel.LeagueId);
                    db.AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
                    db.AddInParameter("MinStake", DbType.Decimal, metaModel.StakeFrom);
                    db.AddInParameter("MaxStake", DbType.Decimal, metaModel.StakeTo);
                    db.AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveStatus);
                    db.AddInParameter("BaseCurrency", DbType.String, metaModel.BasedCurrency);
                    db.AddInParameter("BetFrom", DbType.Byte, metaModel.BetFrom);
                    using (var reader = db.ExecuteReader())
                    {
                        var mainTicket = ReadBetListTicket(reader, currencyList);
                        mainTicket.Sort("TransId", false);
                        ticketResult.MainTicketList.AddRange(mainTicket);
                    }

                    return ticketResult;
                }
            }
            catch (ObjectDbException exception)
            {
                throw new DataAccessException(exception, UspKey, metaModel);
            }
        }

        public IList<BetListMatch> GetMatchList(string uspKey)
        {
            using (var db = this.GetObjectDb(uspKey))
            {
                return db.Query<BetListMatch>().ToList();
            }
        }
    }
}