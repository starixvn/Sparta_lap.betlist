// -----------------------------------------------------------------------
// <copyright file="BetListDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.BetList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Extension;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.BetList;
    using Fanex.Athena.Extension;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.BetList;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Customer;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BetListDataAccess : BaseDataAccess, IBetListDataAccess
    {
        private const string UspExceptionName = "\n UspName=";

        /// <summary>
        /// Get Mix Parlay Detail data.
        /// </summary>
        /// <param name="transId">Transaction ID value.</param>
        /// <returns>The result value.</returns>
        public TicketResult GetMixParlayDetailTicket(long transId)
        {
            const string UspName = "Lrf_BetList_SelParlayDetail";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("TransId", DbType.Int64, transId);
                    using (var reader = db.ExecuteReader())
                    {
                        var result = new TicketResult();
                        result.MainTicketList.AddRange(ReadLrfTicket(reader, string.Empty));
                        return result;
                    }
                }
            }
            catch (System.Exception sqlException)
            {
                LogService.Error(sqlException.Message + UspExceptionName + UspName, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        /// Get System Parlay Detail data.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [System.CLSCompliant(false)]
        public SystemParlayData GetSystemParlayDetail(SystemParlayDetailMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_SelCombinationParlayDetail";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("transid", DbType.Int64, metaModel.TransId);
                    db.AddInParameter("refno", DbType.Int64, metaModel.RefNo);
                    db.AddInParameter("betid", DbType.Int64, metaModel.BetId);
                    db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
                    db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("movebetsday", DbType.DateTime, metaModel.MoveBetsDay);
                    db.AddInParameter("balanceupday", DbType.DateTime, metaModel.BalanceUpDay);
                    db.AddOutParameter("outstanding", DbType.Currency, 8);
                    db.AddOutParameter("outstandingeur", DbType.Currency, 8);
                    db.AddOutParameter("outstandingusd", DbType.Currency, 8);
                    var dataSet = db.ExecuteDataSet();
                    var ticketData = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    var serialData = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();

                    var systemParlayData = new SystemParlayData();

                    systemParlayData.TicketList = new List<ISystemParlayTicket>();
                    foreach (DataRow row in ticketData.Rows)
                    {
                        ISystemParlayTicket ticket = new SystemParlayTicket();
                        ticket.Comm = Converter.ToDecimal(row["comm" + metaModel.BaseCurrency]);
                        ticket.MOdds = Converter.ToDecimal(row["modds"]);
                        ticket.PlayerComm = Converter.ToDecimal(row["playercomm" + metaModel.BaseCurrency]);
                        ticket.Serial = Convert.ToInt64(row["serial"]);
                        ticket.Stake = Converter.ToDecimal(row["stake" + metaModel.BaseCurrency]);
                        ticket.Status = Converter.ToString2(row["status"]);
                        ticket.StatusId = Converter.ToInt(row["statusid"]);
                        ticket.TransId = Convert.ToInt64(row["transid"]);
                        ticket.WinLost = Converter.ToDecimal(row["winlost" + metaModel.BaseCurrency]);
                        ticket.WinLostDate = Converter.ToDateTime(row["winlostdate"]);

                        systemParlayData.TicketList.Add(ticket);
                    }

                    systemParlayData.SerialList = new List<ISystemParlaySerial>();
                    foreach (DataRow row in serialData.Rows)
                    {
                        ISystemParlaySerial serial = new SystemParlaySerial();
                        serial.Serial = Convert.ToInt64(row["serial"]);
                        serial.HomeId = Converter.ToInt(row["homeid"]);
                        serial.AwayId = Converter.ToInt(row["awayid"]);

                        systemParlayData.SerialList.Add(serial);
                    }

                    systemParlayData.Outstanding = Converter.ToDecimal(db.GetParameterValue("outstanding" + metaModel.BaseCurrency));

                    return systemParlayData;
                }
            }
            catch (System.Exception sqlException)
            {
                LogService.Error(sqlException.Message + UspExceptionName + UspName, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, metaModel);
            }
        }

        /// <summary>
        /// Get Bet List Data.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="uspKey">USP key value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public TicketResult GetBetListData(BetListMetaModel metaModel, string uspKey)
        {
            var outPutParams = new Hashtable
            {
                    { "totalrows", DBNull.Value },
                    { "sumwinlost", DBNull.Value },
                    { "sumwinlostusd", DBNull.Value },
                    { "sumwinlosteur", DBNull.Value },
                    { "sumbeteffective", DBNull.Value },
                    { "sumbeteffectiveusd", DBNull.Value },
                    { "sumbeteffectiveeur", DBNull.Value },
                    { "sumlicenseewl", DBNull.Value },
                    { "sumlicenseewlusd", DBNull.Value },
                    { "sumlicenseewleur", DBNull.Value },
                    { "sumcomm", DBNull.Value },
                    { "sumcommusd", DBNull.Value },
                    { "sumcommeur", DBNull.Value },
                    { "sumplayercomm", DBNull.Value },
                    { "sumplayercommusd", DBNull.Value },
                    { "sumplayercommeur", DBNull.Value },
                    { "sumstake", DBNull.Value },
                    { "sumstakeusd", DBNull.Value },
                    { "sumstakeeur", DBNull.Value }
            };
            var baseCurrency = metaModel.BaseCurrency;
            var ticketResult = new TicketResult();
            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    CreateBetListParameters(db, uspKey, metaModel);
                    using (var reader = db.ExecuteReader())
                    {
                        ticketResult.MainTicketList.AddRange(ReadLrfTicket(reader, baseCurrency));
                        if (reader.NextResult())
                        {
                            ticketResult.TicketDataList.AddRange(ReadLrfReferneceTicket(reader, baseCurrency).ToList());
                        }

                        if (!metaModel.IsServerPaging && !string.IsNullOrWhiteSpace(metaModel.SortBy))
                        {
                            // Because Winloss and Commission are shared the same column (in grid)
                            // so we need to sort by the total of two values which is stored in WinlossComm property.
                            if (metaModel.SortBy.Equals("WinLoss", StringComparison.OrdinalIgnoreCase))
                            {
                                metaModel.SortBy = "WinLossComm";
                            }

                            ticketResult.MainTicketList.Sort(metaModel.SortBy, metaModel.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));
                        }
                    }

                    var outSqlParams = db.GetOuputParams();

                    if (outSqlParams != null)
                    {
                        outPutParams.UpdateWith(outSqlParams);
                    }

                    ticketResult.TotalRows = Converter.ToInt(outPutParams["totalrows"]);
                    ticketResult.SumWinLoss =
                        outPutParams["sumwinlost" + baseCurrency] != DBNull.Value ?
                        Converter.ToDecimal(outPutParams["sumwinlost" + baseCurrency])
                    : ticketResult.MainTicketList.Sum(x => x.Winlost);
                    ticketResult.SumBetEffective =
                        outPutParams["sumbeteffective" + baseCurrency] != DBNull.Value ?
                        Converter.ToDecimal(outPutParams["sumbeteffective" + baseCurrency])
                    : ticketResult.MainTicketList.Sum(x => x.BetEffective);
                    ticketResult.SumLicenseeWL = outPutParams["sumlicenseewl" + baseCurrency] != DBNull.Value
                        ? Converter.ToDecimal(outPutParams["sumlicenseewl" + baseCurrency])
                        : ticketResult.MainTicketList.Sum(x => x.LicenseeWL);
                    ticketResult.SumComm = outPutParams["sumcomm" + baseCurrency] != DBNull.Value
                        ? Converter.ToDecimal(outPutParams["sumcomm" + baseCurrency])
                        : ticketResult.MainTicketList.Sum(x => x.Comm);
                    ticketResult.SumPlayerComm = outPutParams["sumplayercomm" + baseCurrency] != DBNull.Value
                        ? Converter.ToDecimal(outPutParams["sumplayercomm" + baseCurrency]) 
                        : ticketResult.MainTicketList.Sum(x => x.PlayerComm);

                    return ticketResult;
                }
            }
            catch (System.Exception sqlException)
            {
                LogService.Error(sqlException.Message + UspExceptionName + uspKey, sqlException);
                throw new DataAccessException(sqlException.Message, uspKey, metaModel);
            }
        }

        /// <summary>
        /// Get last bets monitoring data.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public TicketResult GetLastBetsMonitoringData(LastBetsMonitoringMetaModel metaModel)
        {
            const string UspKey = "Lrf_Monitor_BetlistSel_New";

            var ticketResult = new TicketResult();
            try
            {
                using (var db = new ObjectDb(UspKey))
                {
                    db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
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
                        var mainTicket = ReadLrfTicket(reader, metaModel.BasedCurrency);
                        mainTicket.Sort("TransId", false);
                        ticketResult.MainTicketList.AddRange(mainTicket);
                    }

                    return ticketResult;
                }
            }
            catch (System.Exception sqlException)
            {
                LogService.Error(sqlException.Message + UspExceptionName + UspKey, sqlException);
                throw new DataAccessException(sqlException.Message, UspKey, metaModel);
            }
        }

        /// <summary>
        /// Get Daily Statement Data of Customer.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="allowCustComm">Allow customer commission value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IList<DailyStatement> GetDailyStatementData(BetListMetaModel metaModel, bool allowCustComm)
        {
            const string FormatRemark = "<span class='bl-underdog'>{0} <br/>- {1}</span>";
            const string UspKey = "Lrf_Statement_Mem";
            var transTypeHasBetList = new[] { "get", "com", "post", "bet" };
            var transTypeIdMap = new Dictionary<string, int> { { "betting statement", 1 }, { "void/cancelled bet", 2 }, { "outstanding bet", 3 } };
            var index = 1;
            var result = new List<DailyStatement>();
            var commissionColumn = "commission" + metaModel.BaseCurrency;
            var totalAmtColumn = "totalamt" + metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + metaModel.BaseCurrency;

            try
            {
                using (var db = new ObjectDb(UspKey))
                {
                    db.AddInParameter("CustID", DbType.Int32, metaModel.CustId);
                    CreateBetListDateRangeParameters(db, metaModel);
                    CreateBetlistBUParameters(db, metaModel);
                    CreateBetListPagingParameters(db, metaModel);
                    decimal balance = 0;

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var winlostDate = reader.ToDateTime("winlostdate");
                            var transType = reader.ToString("transtype").ToLower();
                            var transDesc = reader.ToString("transdesc");
                            var transTypeId = transTypeIdMap.ContainsKey(transDesc.ToLower()) ? transTypeIdMap[transDesc.ToLower()] : 0;
                            var credit = transDesc.ToLower() == "outstanding bet" || transType == "bal" ? 0 : reader.ToDecimal(totalAmtColumn);
                            var comm = reader.ToDecimal(commissionColumn);

                            // Add commission.
                            balance = transType == "bal" ? reader.ToDecimal(totalAmtColumn) : balance + credit + (allowCustComm ? comm : 0);

                            result.Add(new DailyStatement
                            {
                                // Add data to CustBalance Item List
                                TransType = transTypeId.ToString(),
                                CustId = metaModel.CustId,
                                ExternalId = metaModel.ExternalId == null ? "0" : metaModel.ExternalId,
                                SystemId = metaModel.SystemId == null ? "0" : metaModel.SystemId,
                                FullStatementDate = string.Format("{0:dddd} <br/> {1}", winlostDate, winlostDate),
                                TransTypeHasBetList = transTypeHasBetList.Contains(transType),
                                Index = index++,
                                Date = winlostDate.ToShortDateString(),
                                Remark = string.Format(FormatRemark, transType.ToUpper(), transDesc),
                                Turnover = NumberReport(reader.ToDecimal(turnoverColumn)),
                                CreditDebit = NumberReport(credit),
                                Commission = NumberReport(comm),
                                Balance = NumberReport(balance)
                            });
                        }
                    }
                }

                return result;
            }
            catch (System.Exception sqlException)
            {
                LogService.Error(sqlException.Message + UspExceptionName + UspKey, sqlException);
                throw new DataAccessException(sqlException.Message, UspKey);
            }
        }

        /// <summary>
        /// Get Cancelled Bets of Customer.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IList<CustomerCancelBetItem> GetCancelBetCustList(BetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_SelDeleted_UserList";
            try
            {
                using (var db = new ObjectDb(UspKey))
                {
                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("LicenseeType", DbType.String, string.IsNullOrWhiteSpace(metaModel.LicenseeType) ? null : metaModel.LicenseeType);
                    db.AddInParameter("CurrencyId", DbType.String, metaModel.CurrencyId.ToString());
                    return db.Query<CustomerCancelBetItem>().ToList();
                }
            }
            catch (System.Exception sqlException)
            {
                LogService.Error(sqlException.Message + UspExceptionName + UspKey, sqlException);
                throw new DataAccessException(sqlException.Message, UspKey);
            }
        }

        /// <summary>
        /// Get Match List of system.
        /// </summary>
        /// <param name="uspKey">USP key value.</param>
        /// <returns>The result value.</returns>
        public List<BetListMatch> GetMatchList(string uspKey)
        {
            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    return db.Query<BetListMatch>().ToList();
                }
            }
            catch (System.Exception sqlException)
            {
                LogService.Error(sqlException.Message + UspExceptionName + uspKey, sqlException);
                throw new DataAccessException(sqlException.Message, uspKey, "No params");
            }
        }

        #region Utility

        #region CreateComonBetlistParameter

        /// <summary>
        /// Create parameters for win loss by league.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateWinlossByLeagueParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            var showSb = metaModel.ProductOptions.Any(i => i.Code == Product.SB && i.Status == true);
            var showBa = metaModel.ProductOptions.Any(i => i.Code == Product.BA && i.Status == true);

            db.AddInParameter("CustID", DbType.Int32, metaModel.CustId);
            db.AddInParameter("LeagueID", DbType.Int32, metaModel.LeagueId);
            db.AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("Odds", DbType.Decimal, metaModel.Odds);
            db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("BalanceUpDay", DbType.DateTime, metaModel.BalanceUpDay);
            db.AddInParameter("ShowWin", DbType.Boolean, metaModel.ShowWin);
            db.AddInParameter("ShowLoss", DbType.Boolean, metaModel.ShowLoss);
            db.AddInParameter("ShowDraw", DbType.Boolean, metaModel.ShowDraw);
            db.AddInParameter("ShowSB", DbType.Boolean, showSb);
            db.AddInParameter("ShowBA", DbType.Boolean, showBa);
        }

        /// <summary>
        /// Create parameters for fore cast bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateRunningBetListByForeCastMatchParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("LicenseeType", DbType.String, string.IsNullOrWhiteSpace(metaModel.LicenseeType) ? null : metaModel.LicenseeType);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            CreateBetListPagingParameters(db, metaModel);
            db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.AddInParameter("betteam", DbType.String, metaModel.BetTeam);
            db.AddInParameter("BetChoice", DbType.String, metaModel.BetChoice);
            db.AddInParameter("Handicap", DbType.Decimal, metaModel.Handicap);
        }

        private static void CreateBetListSiteParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
        }

        /// <summary>
        /// Create parameters for running bet list by bet type.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateMatchRunningBetLitByBettypeParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("LicenseeType", DbType.String, string.IsNullOrEmpty(metaModel.LicenseeType) ? null : metaModel.LicenseeType);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            CreateBetListPagingParameters(db, metaModel);
            db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.AddInParameter("betteam", DbType.String, metaModel.BetTeam);
        }

        /// <summary>
        /// Create parameters for racing running bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateMatchRunningBetListOfHRParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("LicenseeType", DbType.String, string.IsNullOrEmpty(metaModel.LicenseeType) ? null : metaModel.LicenseeType);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
        }

        /// <summary>
        /// Create parameters for running bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateRunningBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistProductOptionParameters(db, metaModel);
        }

        /// <summary>
        /// Create parameters for daily statement.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateDailyStatementBetListOldParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
        }

        /// <summary>
        /// Create parameters for daily statement.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateDailyStatementBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("transtype", DbType.Int32, metaModel.TransType);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            CreateBetListErrorParameters(db);
        }

        /// <summary>
        /// Create parameters for completed outstanding bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateOutStandingCompletedBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            CreateBetlistProductOptionParameters(db, metaModel);
        }

        /// <summary>
        /// Create parameters for Mobile win loss bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateMobileWLBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetlistProductOptionParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            db.AddInParameter("drawfilter", DbType.Int16, metaModel.DrawFilter);
            db.AddInParameter("SiteType", DbType.Byte, metaModel.SiteType);
        }

        /// <summary>
        /// Create parameters for Match parlay bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateMatchWLParlayBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            CreateBetListSortingParameters(db, metaModel);

            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("LicenseeType", DbType.String, string.IsNullOrEmpty(metaModel.LicenseeType) ? null : metaModel.LicenseeType);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            var suportProducts = new string[5] { Product.SB, Product.NG, Product.BA, Product.LC, Product.VS };
            var products = metaModel.ProductOptions.Where(i => suportProducts.Contains(i.Code));
            foreach (var product in products)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        /// <summary>
        /// Create parameters for match out right bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateMatchWLOutRightBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("LicenseeType", DbType.String, string.IsNullOrEmpty(metaModel.LicenseeType) ? null : metaModel.LicenseeType);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            var suportProducts = new string[2] { Product.SB, Product.BA };
            var products = metaModel.ProductOptions.Where(i => suportProducts.Contains(i.Code));
            foreach (var product in products)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        /// <summary>
        /// Create parameters for match bet list by bet type.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateMatchWLBettypeBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("LicenseeType", DbType.String, string.IsNullOrEmpty(metaModel.LicenseeType) ? null : metaModel.LicenseeType);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            db.AddInParameter("Date", DbType.DateTime, metaModel.Date);

            var suportProducts = new string[5] { Product.SB, Product.NG, Product.BA, Product.LC, Product.VS };
            var products = metaModel.ProductOptions.Where(i => suportProducts.Contains(i.Code));
            foreach (var product in products)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        /// <summary>
        /// Create parameters for racing bet list by match.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateMatchWLRacingBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("LicenseeType", DbType.String, string.IsNullOrEmpty(metaModel.LicenseeType) ? null : metaModel.LicenseeType);
            db.AddInParameter("Bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("LeagueId", DbType.Int32, metaModel.LeagueId);
            db.AddInParameter("RaceNo", DbType.String, metaModel.RaceNo);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
        }

        /// <summary>
        /// Create parameters for racing bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateLiveReportBetListRacingParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(db, metaModel);
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            db.AddInParameter("SportType", DbType.Int32, metaModel.SportType);
            db.AddOutParameter("SUMWL", DbType.Currency, 8);
            db.AddOutParameter("SUMLicenseeWL", DbType.Currency, 8);
            db.AddOutParameter("SUMWLUSD", DbType.Currency, 8);
            db.AddOutParameter("SUMLicenseeWLUSD", DbType.Currency, 8);
            db.AddOutParameter("SUMWLEUR", DbType.Currency, 8);
            db.AddOutParameter("SUMLicenseeWLEUR", DbType.Currency, 8);
        }

        /// <summary>
        /// Create parameters for parlay bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateLiveReportParlayBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(db, metaModel);
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            var suportProducts = new string[2] { Product.SB, Product.BA };
            var products = metaModel.ProductOptions.Where(i => suportProducts.Contains(i.Code));

            foreach (var product in products)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }

            db.AddInParameter("Bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
        }

        /// <summary>
        /// Create parameters for number game bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreaeteLiveReportNumberGameBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(db, metaModel);
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            db.AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
        }

        /// <summary>
        /// Create parameters for bet list by match.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateLiveReportBetListByMatchParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            db.AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveIndicator);
            var suportProducts = new string[4] { Product.SB, Product.BA, Product.LC, Product.VS };
            var products = metaModel.ProductOptions.Where(i => suportProducts.Contains(i.Code));
            foreach (var product in products)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        /// <summary>
        /// Create parameters for out right bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateLiveReportOutRightBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            CreateBetListDateRangeParameters(db, metaModel);
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            var suportProducts = new string[2] { Product.SB, Product.BA };
            var products = metaModel.ProductOptions.Where(i => suportProducts.Contains(i.Code));
            foreach (var product in products)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        /// <summary>
        /// Create parameters for licensee win-loss bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateLicenseeWLBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
            db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetlistProductOptionParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            db.AddInParameter("drawfilter", DbType.Int16, metaModel.DrawFilter);
        }

        /// <summary>
        /// Create parameters for daily win-loss bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateDailyWLBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSortingParameters(db, metaModel);

            db.AddInParameter("date", DbType.DateTime, metaModel.Date);
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("product", DbType.String, metaModel.ProductCode);
            db.AddInParameter("status", DbType.String, metaModel.DailyBetListStatus);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
        }

        /// <summary>
        /// Create parameters for daily racing bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        /// <param name="metaModel">The metal model.</param>
        private static void CreateDailyRacingWLBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("date", DbType.DateTime, metaModel.Date);
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("LicenseeType", DbType.String, string.IsNullOrEmpty(metaModel.LicenseeType) ? null : metaModel.LicenseeType);
            db.AddInParameter("leagueid", DbType.Int32, metaModel.LeagueId);
            db.AddInParameter("raceNo", DbType.String, metaModel.RaceNo);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            CreateBetListSummaryStakeParameters(db);
        }

        private static void CreateBetListSummaryStakeParameters(IObjectDb db)
        {
            db.AddOutParameter("SumStake", DbType.Currency, 8);
            db.AddOutParameter("SumStakeUSD", DbType.Currency, 8);
            db.AddOutParameter("SumStakeEUR", DbType.Currency, 8);
        }

        private static void CreateCorectScoreBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            CreateBetListPagingParameters(db, metaModel);
            db.AddInParameter("LicenseeType", DbType.String, string.IsNullOrEmpty(metaModel.LicenseeType) ? null : metaModel.LicenseeType);
        }

        private static void CreateCustomerWlBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CustID", DbType.Int32, metaModel.CustId);
            db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateBetlistSummaryParameters(db);
            CreateBetlistProductOptionParameters(db, metaModel);
        }

        private static void CreateCancelBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CustID", DbType.Int32, metaModel.CustId);
            db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateRunningBetListByBettypePaprameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("bettype", DbType.Int32, metaModel.BetType);
            db.AddInParameter("LicenseeType", DbType.String, string.IsNullOrWhiteSpace(metaModel.LicenseeType) ? null : metaModel.LicenseeType);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateRunningBetListByMultipleBettypePaprameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("bettype", DbType.String, metaModel.MultipleBettype);
            db.AddInParameter("LicenseeType", DbType.String, string.IsNullOrWhiteSpace(metaModel.LicenseeType) ? null : metaModel.LicenseeType);
            CreateBetListPagingParameters(db, metaModel);
        }
        
        #region CommonParam

        private static void CreateBetListSortingParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            if (string.IsNullOrWhiteSpace(metaModel.SortBy) == false)
            {
                db.AddInParameter("Sort", DbType.String, metaModel.SortBy + metaModel.BaseCurrency + " " + metaModel.SortOrder);
            }
        }

        private static void CreateBetListErrorParameters(IObjectDb db)
        {
            db.AddOutParameter("ErrCode", DbType.Int32, 4);
            db.AddOutParameter("ErrMsg", DbType.String, 8000);
        }

        private static void CreateBetListPagingParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            db.AddInParameter("PageIndex", DbType.Int32, metaModel.PageIndex);
            db.AddOutParameter("totalrows", DbType.Int32, 4);
        }

        private static void CreateBetlistBUParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("MoveBetsDay", DbType.DateTime, metaModel.MoveBetsDay);
            db.AddInParameter("BalanceUpDay", DbType.DateTime, metaModel.BalanceUpDay);
        }

        /// <summary>
        /// Create summary parameters for  bet list.
        /// </summary>
        /// <param name="db">The data object.</param>
        private static void CreateBetlistSummaryParameters(IObjectDb db)
        {
            db.AddOutParameter("SumWinlost", DbType.Currency, 8);
            db.AddOutParameter("SumLicenseeWL", DbType.Currency, 8);
            db.AddOutParameter("SumComm", DbType.Currency, 8);
            db.AddOutParameter("SumPlayerComm", DbType.Currency, 8);
            db.AddOutParameter("SumWinlostUSD", DbType.Currency, 8);
            db.AddOutParameter("SumLicenseeWLUSD", DbType.Currency, 8);
            db.AddOutParameter("SumCommUSD", DbType.Currency, 8);
            db.AddOutParameter("SumPlayerCommUSD", DbType.Currency, 8);
            db.AddOutParameter("SumWinlostEUR", DbType.Currency, 8);
            db.AddOutParameter("SumLicenseeWLEUR", DbType.Currency, 8);
            db.AddOutParameter("SumCommEUR", DbType.Currency, 8);
            db.AddOutParameter("SumPlayerCommEUR", DbType.Currency, 8);
        }

        private static void CreateBetlistProductOptionParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            foreach (var productOption in metaModel.ProductOptions)
            {
                db.AddInParameter("show" + productOption.Code, DbType.Boolean, productOption.Status);
            }
        }

        private static void CreateBetListDateRangeParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
        }

        #endregion CommonParam

        #endregion CreateComonBetlistParameter

        /// <summary>
        /// Numbers the report.
        /// </summary>
        /// <param name="value">The decimal number. value.</param>
        /// <returns>The result value.</returns>
        private static string NumberReport(object value)
        {
            decimal number = Convert.ToDecimal(value);
            return (number >= 0)
                       ? "<span class='bl-underdog'>" + string.Format("{0:#,##0.00}", number) + "</span>"
                       : "<span class='bl-favorite'>" + string.Format("{0:#,##0.00}", number) + "</span>";
        }

        /// <summary>
        /// Read boolean value.
        /// </summary>
        /// <param name="value">Convert value.</param>
        /// <returns>The result value.</returns>
        private static bool ReadBoolean(object value)
        {
            if (value == null || value == Convert.DBNull)
            {
                return false;
            }

            if (value.GetType() == typeof(bool))
            {
                return Convert.ToBoolean(value);
            }

            if (value.GetType() == typeof(int))
            {
                return Convert.ToInt32(value) > 0;
            }

            return false;
        }

        /// <summary>
        /// Do not use reflection because of bad performance.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static SortableList<LrfTicket> ReadLrfTicket(IDataReader reader, string baseCurrency)
        {
            var playerCommCol = "playercomm" + baseCurrency;
            var commCol = "comm" + baseCurrency;
            var licenseeWlCol = "licenseewl" + baseCurrency;
            var winlostCol = "winlost" + baseCurrency;
            var beteffectiveCol = "beteffective" + baseCurrency;
            var stakeCol = "stake" + baseCurrency;
            var actualStakeCol = "actual_stake" + baseCurrency;

            // string
            var ordinalUsername = reader.TryGetOrdinal("username");
            var ordinalRefNo = reader.TryGetOrdinal("refno");
            var ordinalMatchCode = reader.TryGetOrdinal("matchcode");
            var ordinalStatus = reader.TryGetOrdinal("status");
            var ordinalBetTeam = reader.TryGetOrdinal("betteam");
            var ordinalBetStatus = reader.TryGetOrdinal("betstatus");
            var ordinalShowTime = reader.TryGetOrdinal("showtime");
            var ordinalOddsType = reader.TryGetOrdinal("oddstype");
            var ordinalExternalId = reader.TryGetOrdinal("nickname");
            var ordinalRace = reader.TryGetOrdinal("race");
            var ordinalBetFrom = reader.TryGetOrdinal("betfrom");

            // Boolean
            var ordinalLiveIndicator = reader.TryGetOrdinal("liveindicator");
            var ordinalNeutral = reader.TryGetOrdinal("isneutral");

            // Int
            var ordinalSportType = reader.TryGetOrdinal("sporttype");
            var ordinalBetType = reader.TryGetOrdinal("bettype");
            var ordinalLeagueId = reader.TryGetOrdinal("leagueid");
            var ordinalHomeId = reader.TryGetOrdinal("homeid");
            var ordinalAwayId = reader.TryGetOrdinal("awayid");
            var ordinalMatchId = reader.TryGetOrdinal("matchid");
            var ordinalCustId = reader.TryGetOrdinal("custid");
            var ordinalCurrencyId = reader.TryGetOrdinal("currencyid");
            var ordinalLiveHomeScore = reader.TryGetOrdinal("livehomescore");
            var ordinalLiveAwayScore = reader.TryGetOrdinal("liveawayscore");
            var ordinalSpecialLevel = reader.TryGetOrdinal("SpecialLevel");
            var ordinalStatusId = reader.TryGetOrdinal("StatusId");

            // Long
            var ordinalTransId = reader.TryGetOrdinal("transid");
            var ordinalBetId = reader.TryGetOrdinal("betid");

            // DateTime
            var ordinalWinlostDate = reader.TryGetOrdinal("winlostdate");
            var ordinalTransDate = reader.TryGetOrdinal("transdate");

            // Money
            var ordinalPlayerComm = reader.TryGetOrdinal(playerCommCol);
            var ordinalComm = reader.TryGetOrdinal(commCol);
            var ordinalLicenseeWl = reader.TryGetOrdinal(licenseeWlCol);
            var ordinalWinlost = reader.TryGetOrdinal(winlostCol);
            var ordinalBetEffective = reader.TryGetOrdinal(beteffectiveCol);
            var ordinalStake = reader.TryGetOrdinal(stakeCol);
            var ordinalActualStake = reader.TryGetOrdinal(actualStakeCol);
            var ordinalOdds = reader.TryGetOrdinal("odds");
            var ordinalMasterOdds = reader.TryGetOrdinal("modds");
            var ordinalHandicap1 = reader.TryGetOrdinal("hdp1");
            var ordinalHandicap2 = reader.TryGetOrdinal("hdp2");

            var tickets = new SortableList<LrfTicket>();
            while (reader.Read())
            {
                var ticket = new LrfTicket();

                // string
                ticket.UserName = ordinalUsername >= 0 ? reader.ToString("username") : string.Empty;
                ticket.RefNo = ordinalRefNo >= 0 ? reader.ToString("refno") : string.Empty;
                ticket.MatchCode = ordinalMatchCode >= 0 ? reader.ToString("matchcode") : string.Empty;
                ticket.Status = ordinalStatus >= 0 ? reader.ToString("status") : string.Empty;
                ticket.BetTeam = ordinalBetTeam >= 0 ? reader.ToString("betteam") : string.Empty;
                ticket.EventStatus = ordinalBetStatus >= 0 ? reader.ToString("betstatus") : string.Empty;
                ticket.ShowTime = ordinalShowTime >= 0 ? reader.ToString("showtime") : string.Empty;
                ticket.OddsType = ordinalOddsType >= 0 ? reader.ToString("oddstype") : "0";
                ticket.ExternalId = ordinalExternalId >= 0 ? reader.ToString("nickname") : string.Empty;
                ticket.Race = ordinalRace >= 0 ? reader.ToString("race") : string.Empty;
                ticket.BetFrom = ordinalBetFrom >= 0 ? reader.ToString("betfrom") : string.Empty;

                // Boolean
                ticket.IsLive = ordinalLiveIndicator >= 0 ? ReadBoolean(reader["liveindicator"]) : false;
                ticket.IsNeutral = ordinalNeutral >= 0 ? ReadBoolean(reader["isneutral"]) : false;

                // Int
                ticket.SportTypeId = ordinalSportType >= 0 ? reader.ToInt("sporttype") : 0;
                ticket.BetTypeId = ordinalBetType >= 0 ? reader.ToInt("bettype") : 0;
                ticket.LeagueId = ordinalLeagueId >= 0 ? reader.ToInt("leagueid") : 0;
                ticket.HomeId = ordinalHomeId >= 0 ? reader.ToInt("homeid") : 0;
                ticket.AwayId = ordinalAwayId >= 0 ? reader.ToInt("awayid") : 0;
                ticket.MatchId = ordinalMatchId >= 0 ? reader.ToInt("matchid") : 0;
                ticket.CustId = ordinalCustId >= 0 ? reader.ToInt("custid") : 0;
                ticket.CurrencyId = ordinalCurrencyId >= 0 ? reader.ToInt("currencyid") : 0;
                ticket.LiveHomeScore = ordinalLiveHomeScore >= 0 ? reader.ToInt("livehomescore") : 0;
                ticket.LiveAwayScore = ordinalLiveAwayScore >= 0 ? reader.ToInt("liveawayscore") : 0;
                ticket.SpecialLevel = ordinalSpecialLevel >= 0 ? reader.ToInt("SpecialLevel") : 0;
                ticket.StatusId = ordinalStatusId >= 0 ? reader.ToInt("StatusId") : 0;

                // Long
                ticket.TransId = ordinalTransId >= 0 ? reader.To<long>("transid") : 0;
                ticket.BetId = ordinalBetId >= 0 && !Convert.IsDBNull(reader["betid"]) ? new long?(reader.To<long>("betid")) : null;

                // DateTime
                ticket.WinlostDate = ordinalWinlostDate >= 0 ? reader.ToDateTime("winlostdate") : DateTime.Now;
                ticket.TransDate = ordinalTransDate >= 0 ? reader.ToDateTime("transdate") : DateTime.Now;

                // Money
                ticket.PlayerComm = ordinalPlayerComm >= 0 ? reader.ToDecimal(playerCommCol) : 0;
                ticket.Comm = ordinalComm >= 0 ? reader.ToDecimal(commCol) : 0;
                ticket.LicenseeWL = ordinalLicenseeWl >= 0 ? reader.ToDecimal(licenseeWlCol) : 0;
                ticket.Winlost = ordinalWinlost >= 0 ? reader.ToDecimal(winlostCol) : 0;
                ticket.BetEffective = ordinalBetEffective >= 0 ? reader.ToDecimal(beteffectiveCol) : 0;
                ticket.Stake = ordinalStake >= 0 ? reader.ToDecimal(stakeCol) : 0;
                ticket.ActualStake = ordinalActualStake >= 0 ? reader.ToDecimal(actualStakeCol) : 0;
                ticket.Odds = ordinalOdds >= 0 ? reader.ToDecimal("odds") : 0;
                ticket.MasterOdds = ordinalMasterOdds >= 0 ? reader.ToDecimal("modds") : 0;
                ticket.Handicap1 = ordinalHandicap1 >= 0 ? reader.ToDecimal("hdp1") : 0;
                ticket.Handicap2 = ordinalHandicap2 >= 0 ? reader.ToDecimal("hdp2") : 0;

                tickets.Add(ticket);
            }

            return tickets;
        }

        /// <summary>
        /// Do not use reflection because of bad performance.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>The result value.</returns>
        private static IEnumerable<ITicketData> ReadLrfReferneceTicket(IDataReader reader, string baseCurrency)
        {
            var stakeColumn = "stake" + baseCurrency;
            var ordinalBetTeam = reader.TryGetOrdinal("betteam");
            var ordinalStake = reader.TryGetOrdinal(stakeColumn);
            while (reader.Read())
            {
                var ticket = new TicketData();

                // string
                ticket.BetTeam = ordinalBetTeam >= 0 ? reader.ToString("betteam") : string.Empty;
                ticket.RefNo = reader.ToString("refno");
                ticket.Status = reader.ToString("status");

                // Int
                ticket.BetTypeId = reader.ToInt("bettype");

                // Money
                ticket.Stake = ordinalStake >= 0 ? reader.ToDecimal(stakeColumn) : 0;

                // Decimal
                ticket.Odds = reader.ToDecimal("odds");

                yield return ticket;
            }
        }

        /// <summary>
        /// Create a hash table of parameter.
        /// </summary>
        /// <param name="db">The Data Object.</param>
        /// <param name="uspKey">Store procedure name.</param>
        /// <param name="metaModel">Meta model value.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static void CreateBetListParameters(IObjectDb db, string uspKey, BetListMetaModel metaModel)
        {
            switch (uspKey)
            {
                case "Lrf_BetList_WinlostByLeague_Mem":
                    CreateWinlossByLeagueParameters(db, metaModel);
                    break;

                case "Lrf_BetList_SelDeleted":
                    CreateCancelBetListParameters(db, metaModel);
                    break;

                case "Lrf_BetList_CorrectScore":
                    CreateCorectScoreBetlistParameters(db, metaModel);
                    break;

                case "Lrf_BetList_SelWinLossByDate_Mem":
                    CreateCustomerWlBetlistParameters(db, metaModel);
                    break;

                case "Lrf_BetList_SelWinlossDaily":
                    CreateDailyWLBetlistParameters(db, metaModel);
                    break;

                case "Lrf_BetList_SelWinlostRacing":
                    CreateDailyRacingWLBetlistParameters(db, metaModel);
                    break;

                case "Lrf_BetList_SelWinloss_Mem":
                    CreateLicenseeWLBetlistParameters(db, metaModel);
                    break;

                case "Lrf_BetList_SelRunningByBettype":
                    CreateRunningBetListByBettypePaprameters(db, metaModel);
                    break;
                case "Lrf_BetList_SelRunningByBettypeList":
                    CreateRunningBetListByMultipleBettypePaprameters(db, metaModel);
                    break;

                case "Lrf_BetList_LiveReport_SelWinlossByMatch":
                    CreateLiveReportBetListByMatchParameters(db, metaModel);
                    break;

                case "Lrf_BetList_LiveReport_SelWinlossOutright":
                    CreateLiveReportOutRightBetlistParameters(db, metaModel);
                    break;

                case "Lrf_BetList_LiveReport_SelWinlossNumberGame":
                    CreaeteLiveReportNumberGameBetlistParameters(db, metaModel);

                    break;

                case "Lrf_BetList_LiveReport_SelWinlossParlay":
                    CreateLiveReportParlayBetlistParameters(db, metaModel);
                    break;

                case "Lrf_BetList_LiveReport_SelWinlossRacing":
                case "Lrf_BetList_LiveReport_SelWinlossVHRacing":
                    CreateLiveReportBetListRacingParameters(db, metaModel);
                    break;

                case "Lrf_BetList_MatchWL_Racing":
                    CreateMatchWLRacingBetlistParameters(db, metaModel);
                    break;

                case "Lrf_BetList_MatchWL_Bettype":
                    CreateMatchWLBettypeBetlistParameters(db, metaModel);
                    break;

                case "Lrf_BetList_MatchWL_Outright":
                    CreateMatchWLOutRightBetListParameters(db, metaModel);
                    break;

                case "Lrf_BetList_MatchWL_Parlay":
                    CreateMatchWLParlayBetListParameters(db, metaModel);
                    break;

                case "Lrf_BetList_MobileReport_SelWinloss_Mem":
                    CreateMobileWLBetListParameters(db, metaModel);
                    break;

                case "Lrf_BetList_SelAllByDate_Mem":
                    CreateOutStandingCompletedBetListParameters(db, metaModel);
                    break;

                case "Lrf_BetList_SelByDayAll_Mem":
                    CreateDailyStatementBetListParameters(db, metaModel);

                    break;

                case "Lrf_BetList_SelByDayAll_Mem_Old":
                    CreateDailyStatementBetListOldParameters(db, metaModel);
                    break;

                case "Lrf_BetList_SelRunningByDate_Mem":
                    CreateRunningBetListParameters(db, metaModel);
                    break;

                case "Lrf_BetList_SelAllByDateHR":
                    CreateMatchRunningBetListOfHRParameters(db, metaModel);
                    break;

                case "Lrf_BetList_SelRunningByBettypeMatch":
                    CreateMatchRunningBetLitByBettypeParameters(db, metaModel);
                    break;

                case "Lrf_BetList_SelRunningByMultipleBettypeMatch":
                    CreateMatchRunningBetLitByMultipleBettypeOfMatchParameters(db, metaModel);
                    break;

                case "Lrf_BetList_Forecast_SelByMatch":
                    CreateRunningBetListByForeCastMatchParameters(db, metaModel);
                    break;
            }
        }

        private static void CreateMatchRunningBetLitByMultipleBettypeOfMatchParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            CreateBetListSiteParameters(db, metaModel);
            db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("LicenseeType", DbType.String, string.IsNullOrEmpty(metaModel.LicenseeType) ? null : metaModel.LicenseeType);
            db.AddInParameter("bettype", DbType.String, !string.IsNullOrEmpty(metaModel.MultipleBettype) ? metaModel.MultipleBettype : string.Empty);
            db.AddInParameter("MatchID", DbType.Int32, metaModel.MatchId);
            CreateBetListPagingParameters(db, metaModel);
            db.AddInParameter("liveindicator", DbType.Int32, metaModel.LiveIndicator.HasValue ? metaModel.LiveIndicator.Value : -1);
            db.AddInParameter("betteam", DbType.String, metaModel.BetTeam);
        }

        #endregion Utility
    }
}