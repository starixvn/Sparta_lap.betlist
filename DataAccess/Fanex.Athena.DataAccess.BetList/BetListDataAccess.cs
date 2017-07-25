// -----------------------------------------------------------------------
// <copyright file="BetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using Fanex.Athena.DataAccess.BetList.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.Extension;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;
    using Util;

    public class BetListDataAccess : BaseBetListDataAccess, IBetListDataAccess
    {
        private const string UspExceptionName = "\n UspName=";
        private static readonly string[] TransTypeHasBetList = { "get", "com", "post", "bet", "gp" };

        private static readonly Dictionary<string, int> TransTypeIdMap = new Dictionary<string, int>
            {
                { "betting statement", 1 }, { "void/cancelled bet", 2 },
                { "outstanding bet", 3 }, { "grand prize", 4 }
            };

        public TicketResult GetMixParlayDetailTicket(long transId)
        {
            const string UspName = "Lrf_BetList_SelParlayDetail";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("TransId", DbType.Int64, transId);
                    using (var reader = db.ExecuteReader())
                    {
                        var result = new TicketResult();
                        result.MainTicketList.AddRange(ReadBetListTicket(reader, string.Empty));
                        return result;
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message + UspExceptionName + UspName, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        [System.CLSCompliant(false)]
        public SystemParlayData GetSystemParlayDetail(SystemParlayDetailMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_SelCombinationParlayDetail";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("refno", DbType.Int64, metaModel.RefNo);
                    db.AddInParameter("movebetsday", DbType.DateTime, metaModel.MoveBetsDay);
                    db.AddInParameter("balanceupday", DbType.DateTime, metaModel.BalanceUpDay);
                    db.AddOutParameter("outstanding", DbType.Currency, 8);
                    db.AddOutParameter("outstandingeur", DbType.Currency, 8);
                    db.AddOutParameter("outstandingusd", DbType.Currency, 8);
                    db.AddOutParameter("outstandinggbp", DbType.Currency, 8);
                    var dataSet = db.ExecuteDataSet();
                    var systemParlayData = new SystemParlayData
                    {
                        TicketList = new List<ISystemParlayTicket>(),
                        SerialList = new List<ISystemParlaySerial>()
                    };
                    using (var ticketData = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable())
                    {
                        foreach (DataRow row in ticketData.Rows)
                        {
                            ISystemParlayTicket ticket = new SystemParlayTicket
                            {
                                Comm = Converter.ToDecimal(row["comm" + metaModel.BaseCurrency]),
                                MOdds = Converter.ToDecimal(row["modds"]),
                                PlayerComm = Converter.ToDecimal(row["playercomm" + metaModel.BaseCurrency]),
                                Serial = Convert.ToInt64(row["serial"]),
                                Stake = Converter.ToDecimal(row["stake" + metaModel.BaseCurrency]),
                                Status = Converter.ToString2(row["status"]),
                                StatusId = Converter.ToInt(row["statusid"]),
                                TransId = Convert.ToInt64(row["transid"]),
                                WinLost = Converter.ToDecimal(row["winlost" + metaModel.BaseCurrency]),
                                WinLostDate = Converter.ToDateTime(row["winlostdate"])
                            };

                            systemParlayData.TicketList.Add(ticket);
                        }
                    }

                    using (var serialData = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable())
                    {
                        foreach (DataRow row in serialData.Rows)
                        {
                            ISystemParlaySerial serial = new SystemParlaySerial
                            {
                                Serial = Convert.ToInt64(row["serial"]),
                                HomeId = Converter.ToInt(row["homeid"]),
                                AwayId = Converter.ToInt(row["awayid"])
                            };

                            systemParlayData.SerialList.Add(serial);
                        }
                    }

                    systemParlayData.Outstanding = Converter.ToDecimal(db.GetParameterValue("outstanding" + metaModel.BaseCurrency));

                    return systemParlayData;
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message + UspExceptionName + UspName, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, metaModel);
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public TicketResult GetBetListData(BetListMetaModel metaModel, string uspKey)
        {
            var outPutParams = new Hashtable
            {
                    { "totalrows", DBNull.Value },
                    { "sumwinlost", DBNull.Value },
                    { "sumwinlostusd", DBNull.Value },
                    { "sumwinlosteur", DBNull.Value },
                    { "sumwinlostgbp", DBNull.Value },
                    { "sumbeteffective", DBNull.Value },
                    { "sumbeteffectiveusd", DBNull.Value },
                    { "sumbeteffectiveeur", DBNull.Value },
                    { "sumbeteffectivegbp", DBNull.Value },
                    { "sumlicenseewl", DBNull.Value },
                    { "sumlicenseewlusd", DBNull.Value },
                    { "sumlicenseewleur", DBNull.Value },
                    { "sumlicenseewlgbp", DBNull.Value },
                    { "sumcomm", DBNull.Value },
                    { "sumcommusd", DBNull.Value },
                    { "sumcommeur", DBNull.Value },
                    { "sumcommgbp", DBNull.Value },
                    { "sumplayercomm", DBNull.Value },
                    { "sumplayercommusd", DBNull.Value },
                    { "sumplayercommeur", DBNull.Value },
                    { "sumplayercommgbp", DBNull.Value },
                    { "sumstake", DBNull.Value },
                    { "sumstakeusd", DBNull.Value },
                    { "sumstakeeur", DBNull.Value },
                    { "sumstakegbp", DBNull.Value }
            };
            var baseCurrency = metaModel.BaseCurrency;
            var ticketResult = new TicketResult();
            try
            {
                using (var db = this.GetObjectDb(uspKey))
                {
                    BetListParameterBuilder.CreateBetListParameters(db, uspKey, metaModel);
                    using (var reader = db.ExecuteReader())
                    {
                        ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader, baseCurrency, metaModel.CustId));
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

                            if (metaModel.SortBy.Equals("nickname", StringComparison.OrdinalIgnoreCase))
                            {
                                metaModel.SortBy = "ExternalId";
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

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IList<DailyStatement> GetDailyStatementData(BetListMetaModel metaModel, bool allowCustComm)
        {
            const string FormatRemark = "<span class='bl-underdog'>{0} <br/>- {1}</span>";
            const string UspKey = "Lrf_Statement_Mem";
            var index = 1;
            var result = new List<DailyStatement>();
            var commissionColumn = "commission" + metaModel.BaseCurrency;
            var totalAmtColumn = "totalamt" + metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + metaModel.BaseCurrency;

            try
            {
                using (var db = this.GetObjectDb(UspKey))
                {
                    db.AddInParameter("CustID", DbType.Int32, metaModel.CustId);
                    BetListParameterBuilder.CreateBetListDateRangeParameters(db, metaModel);
                    BetListParameterBuilder.CreateBetlistBUParameters(db, metaModel);
                    BetListParameterBuilder.CreateBetListPagingParameters(db, metaModel);
                    decimal balance = 0;
                    var currentCulture = CultureInfo.CurrentCulture;
                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var winlostDate = reader.ToDateTime("winlostdate");
                            var transType = reader.ToString("transtype").ToLower(currentCulture);
                            var transDesc = reader.ToString("transdesc");
                            var transTypeId = TransTypeIdMap.ContainsKey(transDesc.ToLower(currentCulture)) ? TransTypeIdMap[transDesc.ToLower(currentCulture)] : 0;
                            var credit = transDesc.ToLower(currentCulture) == "outstanding bet" || transType == "bal" ? 0 : reader.ToDecimal(totalAmtColumn);
                            var comm = reader.ToDecimal(commissionColumn);

                            // Add commission.
                            if (!transType.Equals("gp"))
                            {
                                balance = transType == "bal" ? reader.ToDecimal(totalAmtColumn) : balance + credit + (allowCustComm ? comm : 0);
                            }

                            result.Add(new DailyStatement
                            {
                                // Add data to CustBalance Item List
                                TransType = transTypeId.ToString(),
                                CustId = metaModel.CustId,
                                ExternalId = metaModel.ExternalId == null ? "0" : metaModel.ExternalId,
                                SystemId = metaModel.SystemId == null ? "0" : metaModel.SystemId,
                                FullStatementDate = $"{winlostDate:dddd} <br/> {winlostDate}",
                                TransTypeHasBetList = TransTypeHasBetList.Contains(transType),
                                Index = index++,
                                Date = winlostDate.ToShortDateString(),
                                Remark = string.Format(FormatRemark, transType.ToUpper(currentCulture), transDesc),
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
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message + UspExceptionName + UspKey, sqlException);
                throw new DataAccessException(sqlException.Message, UspKey);
            }
        }

        private static IEnumerable<ITicketData> ReadLrfReferneceTicket(IDataReader reader, string baseCurrency)
        {
            var stakeColumn = "stake" + baseCurrency;
            var hasOrdinalBetTeam = reader.HasColumn("betteam");
            var hasOrdinalStake = reader.HasColumn(stakeColumn);
            while (reader.Read())
            {
                var ticket = new TicketData
                {
                    BetTeam = hasOrdinalBetTeam ? reader.ToString("betteam") : string.Empty,
                    RefNo = reader.ToString("refno"),
                    Status = reader.ToString("status"),
                    BetTypeId = reader.ToInt("bettype"),
                    Stake = hasOrdinalStake ? reader.ToDecimal(stakeColumn) : 0,
                    Odds = reader.ToDecimal("odds")
                };

                yield return ticket;
            }
        }
    }
}