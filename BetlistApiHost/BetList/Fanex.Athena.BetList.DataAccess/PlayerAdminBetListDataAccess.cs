namespace Fanex.Athena.BetList.DataAccess
{
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Data;

    public class PlayerAdminBetListDataAccess : BaseBetListDataAccess, IPlayerAdminBetListDataAccess
    {
        const string UspKey = "Lrf_BetList_Running_Mem";

        public async Task<TicketResult> GetRunningBetListAsync(PlayerAdminBetListMetaModel metaModel)
        {
            var ticketResult = new TicketResult();

            using (var db = GetObjectDb(UspKey))
            {
                SetParameter(db, metaModel);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    ticketResult.MainTicketList = ReadBetListTicket(reader, metaModel.CurrencyList);

                    if (reader.NextResult())
                    {
                        ticketResult.TicketDataList.AddRange(ReadReferneceTicket(reader));
                    }
                }

                ticketResult.SumStake = metaModel.SumStake;
                ticketResult.TotalRows = ticketResult.MainTicketList.Count;
            }

            return ticketResult;
        }

        private static void SetParameter(IObjectDb db, PlayerAdminBetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("CustId", metaModel.CustId);
            db.SetParameterValue("SumStake", metaModel.SumStake);

            if (!string.IsNullOrEmpty(metaModel.SortBy) && !string.IsNullOrEmpty(metaModel.SortOrder))
            {
                db.SetParameterValue("Sort", $"{metaModel.SortBy} {metaModel.SortOrder}");
            }
        }
    }
}
