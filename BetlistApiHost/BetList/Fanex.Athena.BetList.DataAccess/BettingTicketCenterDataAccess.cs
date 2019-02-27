namespace Fanex.Athena.BetList.DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using BetList.Model.DTO;
    using BetList.Model.MetaModel;
    using Common.Constant;
    using Common.DataAccess.Interfaces;
    using Common.Utility.Services.Interfaces;
    using Data;
    using Fanex.Common;
    using Interfaces;
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
                var productList = string.Join(",", productCodes.Except(new[] { ProductCodes.BA }));
                var balanceUp = _balanceUpDataAccess.GetBalanceUp();
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("FromDate", metaModel.FromDate);
                db.SetParameterValue("ToDate", metaModel.ToDate);
                db.SetParameterValue("MoveBetsDay", balanceUp.MoveBetsDay);
                db.SetParameterValue("SportType", metaModel.SportId);
                db.SetParameterValue("BetType", metaModel.BetTypeId);
                db.SetParameterValue("LeagueId", metaModel.LeagueId);
                db.SetParameterValue("MatchId", metaModel.MatchId);
                db.SetParameterValue("StatusIdList", metaModel.StatusList);
                db.SetParameterValue("LiveIndicator", metaModel.LiveStatus);
                db.SetParameterValue("ShowBA", productCodes.Contains(ProductCodes.BA));
                db.SetParameterValue("ProductList", productList);

                db.SetParameterValue("TotalRows", null);

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
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("UserName", metaModel.UserName);
                db.SetParameterValue("RefNo", metaModel.RefNo);
                db.SetParameterValue("MoveBetsDay", balanceUp.MoveBetsDay);
                db.SetParameterValue("BalanceUpDay", balanceUp.BalanceUpDay);
                db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);

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
                var productList = string.Join(",", productCodes.Except(new[] { ProductCodes.BA }));

                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("UserName", metaModel.UserName);
                db.SetParameterValue("CurrencyId", metaModel.CurrencyId);
                db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
                db.SetParameterValue("ProductList", productList);
                db.SetParameterValue("showBA", productCodes.Contains(ProductCodes.BA));
                db.SetParameterValue("PageSize", metaModel.IsExport ? int.MaxValue - 1 : metaModel.PageSize);
                db.SetParameterValue("PageIndex", metaModel.IsExport ? 1 : metaModel.PageIndex);
                db.SetParameterValue("TotalRows", null);
                db.SetParameterValue("SumStake", null);
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
                db.SetParameterValue("Sort", sortBy + " " + sortOrder);
            }
        }

        private void SaveDataToFile(GroupTicketResult groupData, TicketResult ticket)
        {
            var data = JsonConvert.SerializeObject(ticket);
            var dataFileName = $"{Guid.NewGuid().ToString()}.json";
            _tempFileService.Save(dataFileName, data);
            groupData.DataPath.Add(dataFileName);
            ticket.MainTicketList.Clear();
            ticket.MainTicketList.TrimExcess();
        }

        private void AddSingleCustomerParameters(IObjectDb db, SingleCustomersBetListMetaModel metaModel)
        {
            var productCodes = metaModel.ProductCodeList.Split(',');
            var productList = string.Join(",", productCodes.Except(new[] { ProductCodes.BA }));
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            db.SetParameterValue("ShowBA", productCodes.Contains(ProductCodes.BA));
            db.SetParameterValue("ProductList", productList);
            db.SetParameterValue("FromDate", metaModel.FromDate);
            db.SetParameterValue("ToDate", metaModel.ToDate);
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("UserName", metaModel.UserName);
            db.SetParameterValue("StatusIdList", metaModel.Status);
            db.SetParameterValue("LiveIndicator", metaModel.LiveStatus);
            db.SetParameterValue("Platform", metaModel.Platform);
            db.SetParameterValue("MoveBetsDay", balanceUp.MoveBetsDay);
            db.SetParameterValue("BalanceUpDay", balanceUp.BalanceUpDay);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            db.SetParameterValue("PageSize", metaModel.IsExport ? int.MaxValue - 1 : metaModel.PageSize);
            db.SetParameterValue("PageIndex", metaModel.IsExport ? 1 : metaModel.PageIndex);

            db.SetParameterValue("TotalRows", null);
            db.SetParameterValue("SumStake", null);
            db.SetParameterValue("SumWinLost", null);
            db.SetParameterValue("SumLicenseeWL", null);
            db.SetParameterValue("SumComm", null);
            db.SetParameterValue("SumPlayerComm", null);
            AddSortParameter(db, metaModel.SortBy, metaModel.SortOrder);
        }

        private void AddMultiCustomerParameters(IObjectDb db, MultipleCustomersBetListMetaModel metaModel)
        {
            var productCodes = metaModel.ProductCodeList.Split(',');
            var productList = string.Join(",", productCodes.Except(new[] { ProductCodes.BA }));
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("FromDate", metaModel.FromDate);
            db.SetParameterValue("ToDate", metaModel.ToDate);
            db.SetParameterValue("MoveBetsDay", balanceUp.MoveBetsDay);
            db.SetParameterValue("SportType", metaModel.SportId);
            db.SetParameterValue("BetType", metaModel.BetTypeId);
            db.SetParameterValue("LeagueId", metaModel.LeagueId);
            db.SetParameterValue("MatchId", metaModel.MatchId);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            db.SetParameterValue("StatusIdList", metaModel.StatusList);
            db.SetParameterValue("LiveIndicator", metaModel.LiveStatus);
            db.SetParameterValue("PageSize", metaModel.IsExport ? int.MaxValue - 1 : metaModel.PageSize);
            db.SetParameterValue("PageIndex", metaModel.IsExport ? 1 : metaModel.PageIndex);
            db.SetParameterValue("ShowBA", productCodes.Contains(ProductCodes.BA));
            db.SetParameterValue("ProductList", productList);

            db.SetParameterValue("TotalRows", null);
            db.SetParameterValue("SumStake", null);
            db.SetParameterValue("SumWinLost", null);
            db.SetParameterValue("SumLicenseeWL", null);
            db.SetParameterValue("SumComm", null);
            db.SetParameterValue("SumPlayerComm", null);
            AddSortParameter(db, metaModel.SortBy, metaModel.SortOrder);
        }
    }
}