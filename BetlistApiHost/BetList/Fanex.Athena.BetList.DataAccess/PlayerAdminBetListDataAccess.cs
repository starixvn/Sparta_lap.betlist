namespace Fanex.Athena.BetList.DataAccess
{
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Common;
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

                var outSqlParams = db.GetOuputParams();
                ticketResult.SumStake = Converter.ToDecimal(outSqlParams["SumStake"]);
                ticketResult.TotalRows = Converter.ToInt(outSqlParams["TotalRows"]);
            }

            return ticketResult;
        }

        private static void SetParameter(IObjectDb db, PlayerAdminBetListMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("CustId", metaModel.CustId);
            db.SetParameterValue("PageIndex", metaModel.PageIndex);
            db.SetParameterValue("PageSize", metaModel.PageSize);
            db.SetParameterValue("SumStake", 0);
            db.SetParameterValue("TotalRows", 0);

            if (!string.IsNullOrEmpty(metaModel.SortBy) && !string.IsNullOrEmpty(metaModel.SortOrder))
            {
                db.SetParameterValue("Sort", $"{metaModel.SortBy} {metaModel.SortOrder}");
            }
        }
    }
}
