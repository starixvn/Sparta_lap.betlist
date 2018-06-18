namespace Fanex.Athena.DataAccess.BetList
{
    using System;
    using System.Data;
    using System.Linq;
    using Common;
    using Core.Exception;
    using Data;
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Common;

    public class BettingTicketsCenterDataAccess : BaseBetListDataAccess, IBettingTicketsCenterDataAccess
    {
        public TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel)
        {
            return new TicketResult();
        }

        public TicketResult GetBetListForSingleCustomer(SingleCustomersBetListMetaModel metaModel)
        {
            return new TicketResult();
        }

        public TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RefNo_Mem";

            var ticketResult = new TicketResult();
            try
            {
                using (var db = GetObjectDb(UspKey))
                {
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
                    db.AddInParameter("RefNo", DbType.Int64, metaModel.RefNo);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
                    db.AddInParameter("BalanceUpDay", DbType.DateTime, metaModel.BalanceUpDay);
                    db.AddInParameter("BaseCurrency", DbType.String, metaModel.IsConvert ? metaModel.BaseCurrency : string.Empty);

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

        public TicketResult GetRunningBetList(BaseBettingTicketMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_Running";

            var ticketResult = new TicketResult();
            try
            {
                using (var db = GetObjectDb(UspKey))
                {
                    var productCodes = metaModel.ProductCodeList.Split(',');

                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
                    db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                    db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                    db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                    db.AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
                    db.AddInParameter("showSB", DbType.Boolean, productCodes.Contains(Product.SB));
                    db.AddInParameter("showBA", DbType.Boolean, productCodes.Contains(Product.BA));
                    db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                    db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
                    db.AddOutParameter("TotalRows", DbType.Int32, 4);
                    db.AddOutParameter("SumStake", DbType.Decimal, 8);

                    using (var reader = db.ExecuteReader())
                    {
                        ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader));
                    }

                    var outSqlParams = db.GetOuputParams();
                    ticketResult.SumStake = Converter.ToDecimal(outSqlParams["SumStake"]);
                    ticketResult.TotalRows = Converter.ToInt(outSqlParams["TotalRows"]);
                }
            }
            catch (Exception sqlException)
            {
                throw new DataAccessException(sqlException, UspKey, metaModel);
            }

            return ticketResult;
        }
    }
}