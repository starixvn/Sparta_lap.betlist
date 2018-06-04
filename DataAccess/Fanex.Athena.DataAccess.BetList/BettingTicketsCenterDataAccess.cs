namespace Fanex.Athena.DataAccess.BetList
{
    using System;
    using System.Data;
    using Core.Exception;
    using Data;
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;

    public class BettingTicketsCenterDataAccess : BaseBetListDataAccess, IBettingTicketsCenterDataAccess
    {
        public TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelWinlossDaily";

            var ticketResult = new TicketResult();
            try
            {
                using (var db = this.GetObjectDb(UspKey))
                {
                    AddParameters(db, metaModel);
                    using (var reader = db.ExecuteReader())
                    {
                        ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader));
                    }
                }
            }
            catch (Exception sqlException)
            {
                throw new DataAccessException(sqlException, UspKey, metaModel);
            }

            return ticketResult;
        }

        public TicketResult GetBetListForSingleCustomer()
        {
            return new TicketResult();
        }

        public TicketResult GetBetListByRefno()
        {
            return new TicketResult();
        }

        public TicketResult GetRunningBetList()
        {
            return new TicketResult();
        }

        private static void AddParameters(IObjectDb db, MultipleCustomersBetListMetaModel metaModel)
        {
            db.AddInParameter("date", DbType.DateTime, metaModel.Date);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("product", DbType.String, metaModel.ProductCode);
            db.AddInParameter("status", DbType.String, metaModel.Status);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("BalanceUpDay", DbType.DateTime, metaModel.BalanceUpDay);
            db.AddOutParameter("totalrows", DbType.Int32, 4);
        }
    }
}