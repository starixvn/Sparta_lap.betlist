namespace Fanex.Athena.DataAccess.BetList
{
    using System;
    using System.Data;
    using System.Linq;
    using Common;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Core.Exception;
    using Data;
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Common;

    public class BettingTicketCenterDataAccess : BaseBetListDataAccess, IBettingTicketCenterDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public BettingTicketCenterDataAccess()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
        }

        public BettingTicketCenterDataAccess(IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel)
        {
            return new TicketResult();
        }

        public TicketResult GetBetListForSingleCustomer(SingleCustomersBetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_Mem";

            var ticketResult = new TicketResult();
            try
            {
                using (var db = GetObjectDb(UspKey))
                {
                    AddSingleCustomerBetListSpParameters(db, metaModel);

                    using (var reader = db.ExecuteReader())
                    {
                        ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader));
                    }

                    var outSqlParams = db.GetOuputParams();
                    ticketResult.SumStake = Converter.ToDecimal(outSqlParams["sumstake"]);
                    ticketResult.SumWinLoss = Converter.ToDecimal(outSqlParams["sumwinlost"]);
                    ticketResult.SumLicenseeWL = Converter.ToDecimal(outSqlParams["sumlicenseewl"]);
                    ticketResult.SumComm = Converter.ToDecimal(outSqlParams["sumcomm"]);
                    ticketResult.SumPlayerComm = Converter.ToDecimal(outSqlParams["sumplayercomm"]);
                    ticketResult.TotalRows = Converter.ToInt(outSqlParams["totalrows"]);
                }
            }
            catch (Exception sqlException)
            {
                throw new DataAccessException(sqlException, UspKey, metaModel);
            }

            return ticketResult;
        }

        public TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_RefNo_Mem";

            var ticketResult = new TicketResult();
            try
            {
                using (var db = GetObjectDb(UspKey))
                {
                    var balanceUp = _balanceUpDataAccess.GetBalanceUp();
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("UserName", DbType.String, metaModel.UserName);
                    db.AddInParameter("RefNo", DbType.Int64, metaModel.RefNo);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, balanceUp.MoveBetsDay);
                    db.AddInParameter("BalanceUpDay", DbType.DateTime, balanceUp.BalanceUpDay);
                    db.AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);

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
                    var productList = string.Join(",", productCodes.Except(new[] { Product.BA }));
                    var endOfToDate = metaModel.ToDate.AddDays(1).AddMilliseconds(-2);

                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("UserName", DbType.String, metaModel.UserName);
                    db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                    db.AddInParameter("ToDate", DbType.DateTime, endOfToDate);
                    db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                    db.AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
                    db.AddInParameter("ProductList", DbType.String, productList);
                    db.AddInParameter("showBA", DbType.Boolean, productCodes.Contains(Product.BA));
                    db.AddInParameter("PageSize", DbType.Int32, metaModel.IsExport ? int.MaxValue - 1 : metaModel.PageSize);
                    db.AddInParameter("PageIndex", DbType.Int32, metaModel.IsExport ? 1 : metaModel.PageIndex);
                    db.AddOutParameter("TotalRows", DbType.Int32, 4);
                    db.AddOutParameter("SumStake", DbType.Currency, 8);
                    AddSortParameter(db, metaModel.SortBy, metaModel.SortOrder);

                    using (var reader = db.ExecuteReader())
                    {
                        ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader));
                    }

                    var outSqlParams = db.GetOuputParams();
                    ticketResult.SumStake = Converter.ToDecimal(outSqlParams["sumstake"]);
                    ticketResult.TotalRows = Converter.ToInt(outSqlParams["totalrows"]);
                }
            }
            catch (Exception sqlException)
            {
                throw new DataAccessException(sqlException, UspKey, metaModel);
            }

            return ticketResult;
        }

        private static void AddSortParameter(IObjectDb db, string sortBy, string sortOrder)
        {
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                db.AddInParameter("Sort", DbType.String, sortBy + " " + sortOrder);
            }
        }

        private void AddSingleCustomerBetListSpParameters(IObjectDb db, SingleCustomersBetListMetaModel metaModel)
        {
            var productCodes = metaModel.ProductCodeList.Split(',');
            var productList = string.Join(",", productCodes.Except(new[] { Product.BA }));
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            db.AddInParameter("ShowBA", DbType.Boolean, productCodes.Contains(Product.BA));
            db.AddInParameter("ProductList", DbType.String, productList);
            db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("UserName", DbType.String, metaModel.UserName);
            db.AddInParameter("StatusIdList", DbType.String, metaModel.Status);
            db.AddInParameter("LiveIndicator", DbType.Byte, metaModel.LiveStatus);
            db.AddInParameter("Platform", DbType.Byte, metaModel.Platform);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, balanceUp.MoveBetsDay);
            db.AddInParameter("BalanceUpDay", DbType.DateTime, balanceUp.BalanceUpDay);
            db.AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.IsExport ? int.MaxValue - 1 : metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.IsExport ? 1 : metaModel.PageIndex);
            db.AddOutParameter("TotalRows", DbType.Int32, 4);
            db.AddOutParameter("SumStake", DbType.Currency, 8);
            db.AddOutParameter("SumWinLost", DbType.Currency, 8);
            db.AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
            db.AddOutParameter("SumComm", DbType.Currency, 8);
            db.AddOutParameter("SumPlayerComm", DbType.Currency, 8);
            AddSortParameter(db, metaModel.SortBy, metaModel.SortOrder);
        }
    }
}