using System.Threading.Tasks;

using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Common;

namespace Fanex.Athena.BetList.DataAccess
{
    public class BetListByProductDataAccess : BaseBetListDataAccess, IBetListByProductDataAccess

    {
        public async Task<TicketResult> QueryAsync(BetListByProductMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_Running";

            var ticketResult = new TicketResult();

            using (var db = GetObjectDb(UspKey))
            {
                db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
                db.SetParameterValue("PageSize", metaModel.PageSize);
                db.SetParameterValue("PageIndex", metaModel.PageIndex);
                db.SetParameterValue("CustId", metaModel.CustId);
                db.SetParameterValue("WinLossDate", metaModel.WinLossDate);
                db.SetParameterValue("TotalRows", null);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader, metaModel.UserCurrencyList));
                }

                var outSqlParams = db.GetOuputParams();
                ticketResult.TotalRows = Converter.ToInt(outSqlParams["totalrows"]);
            }

            return ticketResult;
        }
    }
}
