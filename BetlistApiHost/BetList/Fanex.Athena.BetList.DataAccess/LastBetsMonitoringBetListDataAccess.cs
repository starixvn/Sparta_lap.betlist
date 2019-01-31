// -----------------------------------------------------------------------
// <copyright file="LastBetsMonitoringBetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.DataAccess
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Interfaces;

    public class LastBetsMonitoringBetListDataAccess : BaseBetListDataAccess, ILastBetsMonitoringBetListDataAccess
    {
        private const string UspKey = "Lrf_Monitor_BetlistSel_New";

        public TicketResult GetLastBetsMonitoringData(LastBetsMonitoringMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            var ticketResult = new TicketResult();

            using (var db = this.GetObjectDb(UspKey))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("BetType", metaModel.BetType);
                db.SetParameterValue("Currency", metaModel.Currency);
                db.SetParameterValue("SpecialLevel", metaModel.Level);
                db.SetParameterValue("TransId", metaModel.TransId);
                db.SetParameterValue("LimitRowCount", metaModel.PageSize);
                db.SetParameterValue("SportId", metaModel.SportId);
                db.SetParameterValue("LeagueId", metaModel.LeagueId);
                db.SetParameterValue("MatchId", metaModel.MatchId);
                db.SetParameterValue("MinStake", metaModel.StakeFrom);
                db.SetParameterValue("MaxStake", metaModel.StakeTo);
                db.SetParameterValue("LiveIndicator", metaModel.LiveStatus);
                db.SetParameterValue("BaseCurrency", metaModel.BasedCurrency);
                db.SetParameterValue("BetFrom", metaModel.BetFrom);
                using (var reader = db.ExecuteReader())
                {
                    var mainTicket = ReadBetListTicket(reader, currencyList);
                    mainTicket.Sort("TransId", false);
                    ticketResult.MainTicketList.AddRange(mainTicket);
                }

                return ticketResult;
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