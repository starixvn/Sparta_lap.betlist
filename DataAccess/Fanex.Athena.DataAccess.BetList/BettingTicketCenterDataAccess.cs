namespace Fanex.Athena.DataAccess.BetList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web.Hosting;
    using Common;
    using Core.Configuration.Interfaces;
    using Data;
    using Fanex.Athena.Extension.Utility.Interfaces;
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Common;
    using Newtonsoft.Json;

    public class BettingTicketCenterDataAccess : BaseBetListDataAccess, IBettingTicketCenterDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;
        private readonly ITempFileService _tempFileService;

        public BettingTicketCenterDataAccess(IBalanceUpDataAccess balanceUpDataAccess, ITempFileService tempFileService)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _tempFileService = tempFileService;
        }

        public GroupTicketResult GetBetListForMultipleCustomers(
            MultipleCustomersBetListMetaModel metaModel,
            int rowPerGroup,
            IDictionary<int, string> currencyList)
        {
            const string UspKey = "Lrf_BetList_Licensee";
            var groupData = new GroupTicketResult
            {
                RowPerGroup = rowPerGroup
            };
            
            using (var db = GetObjectDb(UspKey))
            {
                AddMultiCustomerParameters(db, metaModel);

                using (var reader = db.ExecuteReader())
                {
                    var count = 0;
                    var ticket = new TicketResult();

                    var columns = GetColumns();
                    var safeReader = this.GetSafeDataReader(reader, columns.Select(x => x.Value).ToList());

                    while (reader.Read())
                    {
                        if (count == rowPerGroup)
                        {
                            SaveDataToFile(groupData, ticket);
                            count = 0;
                            ticket = new TicketResult();
                        }

                        ticket.MainTicketList.Add(MapTicket(safeReader, columns, currencyList));
                        count++;
                    }

                    if (ticket.MainTicketList.Any())
                    {
                        SaveDataToFile(groupData, ticket);
                    }
                }

                var outSqlParams = db.GetOuputParams();
                ReadOutputParams(groupData, outSqlParams);

                return groupData;
            }
        }

        public TicketResult GetBetListForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            const string UspKey = "Lrf_BetList_Licensee";

            var ticketResult = new TicketResult();

            using (var db = GetObjectDb(UspKey))
            {
                AddMultiCustomerParameters(db, metaModel);

                using (var reader = db.ExecuteReader())
                {
                    ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader, currencyList));
                }

                var outSqlParams = db.GetOuputParams();
                ticketResult.SumStake = Converter.ToDecimal(outSqlParams["sumstake"]);
                ticketResult.SumWinLoss = Converter.ToDecimal(outSqlParams["sumwinlost"]);
                ticketResult.SumLicenseeWL = Converter.ToDecimal(outSqlParams["sumlicenseewl"]);
                ticketResult.SumComm = Converter.ToDecimal(outSqlParams["sumcomm"]);
                ticketResult.SumPlayerComm = Converter.ToDecimal(outSqlParams["sumplayercomm"]);
                ticketResult.TotalRows = Converter.ToInt(outSqlParams["totalrows"]);
            }

            return ticketResult;
        }

        public int GetTotalRowForMultipleCustomers(MultipleCustomersBetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_Licensee_TotalRows";

            using (var db = GetObjectDb(UspKey))
            {
                var productCodes = metaModel.ProductCodeList.Split(',');
                var productList = string.Join(",", productCodes.Except(new[] { Product.BA }));
                var balanceUp = _balanceUpDataAccess.GetBalanceUp();
                db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                db.AddInParameter("MoveBetsDay", DbType.DateTime, balanceUp.MoveBetsDay);
                db.AddInParameter("SportType", DbType.Int32, metaModel.SportId);
                db.AddInParameter("BetType", DbType.Int32, metaModel.BetTypeId);
                db.AddInParameter("LeagueId", DbType.Int32, metaModel.LeagueId);
                db.AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
                db.AddInParameter("StatusIdList", DbType.String, metaModel.StatusList);
                db.AddInParameter("LiveIndicator", DbType.Byte, metaModel.LiveStatus);
                db.AddInParameter("ShowBA", DbType.Boolean, productCodes.Contains(Product.BA));
                db.AddInParameter("ProductList", DbType.String, productList);

                db.AddOutParameter("TotalRows", DbType.Int32, 4);

                db.ExecuteNonQuery();

                var outSqlParams = db.GetOuputParams();
                return Converter.ToInt(outSqlParams["totalrows"]);
            }
        }

        public TicketResult GetBetListForSingleCustomer(
            SingleCustomersBetListMetaModel metaModel,
            IDictionary<int, string> currencyList)
        {
            const string UspKey = "Lrf_BetList_Mem";

            var ticketResult = new TicketResult();

            using (var db = GetObjectDb(UspKey))
            {
                AddSingleCustomerParameters(db, metaModel);

                using (var reader = db.ExecuteReader())
                {
                    ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader, currencyList));
                }

                var outSqlParams = db.GetOuputParams();
                ticketResult.SumStake = Converter.ToDecimal(outSqlParams["sumstake"]);
                ticketResult.SumWinLoss = Converter.ToDecimal(outSqlParams["sumwinlost"]);
                ticketResult.SumLicenseeWL = Converter.ToDecimal(outSqlParams["sumlicenseewl"]);
                ticketResult.SumComm = Converter.ToDecimal(outSqlParams["sumcomm"]);
                ticketResult.SumPlayerComm = Converter.ToDecimal(outSqlParams["sumplayercomm"]);
                ticketResult.TotalRows = Converter.ToInt(outSqlParams["totalrows"]);
            }

            return ticketResult;
        }

        public TicketResult GetBetListByRefno(RefNoBetListMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            const string UspKey = "Lrf_BetList_RefNo_Mem";

            var ticketResult = new TicketResult();

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
                    ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader, currencyList));
                }
            }

            return ticketResult;
        }

        public TicketResult GetRunningBetList(BaseBettingTicketMetaModel metaModel, IDictionary<int, string> currencyList)
        {
            const string UspKey = "Lrf_BetList_Running";

            var ticketResult = new TicketResult();

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
                    ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader, currencyList));
                }

                var outSqlParams = db.GetOuputParams();
                ticketResult.SumStake = Converter.ToDecimal(outSqlParams["sumstake"]);
                ticketResult.TotalRows = Converter.ToInt(outSqlParams["totalrows"]);
            }

            return ticketResult;
        }

        private static void ReadOutputParams(GroupTicketResult groupData, Hashtable outSqlParams)
        {
            groupData.SumStake = Converter.ToDecimal(outSqlParams["sumstake"]);
            groupData.SumWinLoss = Converter.ToDecimal(outSqlParams["sumwinlost"]);
            groupData.SumLicenseeWL = Converter.ToDecimal(outSqlParams["sumlicenseewl"]);
            groupData.SumComm = Converter.ToDecimal(outSqlParams["sumcomm"]);
            groupData.SumPlayerComm = Converter.ToDecimal(outSqlParams["sumplayercomm"]);
            groupData.TotalRows = Converter.ToInt(outSqlParams["totalrows"]);
        }

        private static void AddSortParameter(IObjectDb db, string sortBy, string sortOrder)
        {
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                db.AddInParameter("Sort", DbType.String, sortBy + " " + sortOrder);
            }
        }

        private void SaveDataToFile(GroupTicketResult groupData, TicketResult ticket)
        {
            var data = JsonConvert.SerializeObject(ticket);
            var dataFileName = $"BTC\\{Guid.NewGuid().ToString()}.json";
            _tempFileService.Save(dataFileName, data);
            groupData.DataPath.Add(dataFileName);
            ticket.MainTicketList.Clear();
            ticket.MainTicketList.TrimExcess();
        }

        private void AddSingleCustomerParameters(IObjectDb db, SingleCustomersBetListMetaModel metaModel)
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

        private void AddMultiCustomerParameters(IObjectDb db, MultipleCustomersBetListMetaModel metaModel)
        {
            var productCodes = metaModel.ProductCodeList.Split(',');
            var productList = string.Join(",", productCodes.Except(new[] { Product.BA }));
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, balanceUp.MoveBetsDay);
            db.AddInParameter("SportType", DbType.Int32, metaModel.SportId);
            db.AddInParameter("BetType", DbType.Int32, metaModel.BetTypeId);
            db.AddInParameter("LeagueId", DbType.Int32, metaModel.LeagueId);
            db.AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
            db.AddInParameter("BaseCurrency", DbType.String, metaModel.BaseCurrency);
            db.AddInParameter("StatusIdList", DbType.String, metaModel.StatusList);
            db.AddInParameter("LiveIndicator", DbType.Byte, metaModel.LiveStatus);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.IsExport ? int.MaxValue - 1 : metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.IsExport ? 1 : metaModel.PageIndex);
            db.AddInParameter("ShowBA", DbType.Boolean, productCodes.Contains(Product.BA));
            db.AddInParameter("ProductList", DbType.String, productList);

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