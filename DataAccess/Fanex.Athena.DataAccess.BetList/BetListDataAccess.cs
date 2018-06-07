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
    using System.Linq;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.BetList.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Util;

    public class BetListDataAccess : BaseBetListDataAccess, IBetListDataAccess
    {
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
                throw new DataAccessException(sqlException, UspName);
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
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }

        public TicketResult GetBetListData(BetListMetaModel metaModel, string uspKey)
        {
            var ticketResult = new TicketResult();
            try
            {
                using (var db = this.GetObjectDb(uspKey))
                {
                    var dataInfo = BetListParameterBuilder.CreateBetListParameters(db, uspKey, metaModel);
                    var baseCurrency = dataInfo.BaseCurrencies != null && dataInfo.BaseCurrencies.Contains(metaModel.BaseCurrency, StringComparer.InvariantCultureIgnoreCase)
                        ? metaModel.BaseCurrency
                        : string.Empty;

                    using (var reader = db.ExecuteReader())
                    {
                        ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader, baseCurrency, metaModel.CustId));
                        if (reader.NextResult())
                        {
                            ticketResult.TicketDataList.AddRange(ReadLrfReferneceTicket(reader, baseCurrency).ToList());
                        }

                        SortData(metaModel, ticketResult.MainTicketList);
                    }

                    var outSqlParams = db.GetOuputParams();

                    if (outSqlParams == null)
                    {
                        outSqlParams = new Hashtable();
                    }

                    UpdateSummaryData(ticketResult, outSqlParams, baseCurrency);

                    return ticketResult;
                }
            }
            catch (Exception sqlException)
            {
                throw new DataAccessException(sqlException, uspKey, metaModel);
            }
        }

        private static void UpdateSummaryData(TicketResult ticketResult, Hashtable outPutParams, string baseCurrency)
        {
            ticketResult.TotalRows = Converter.ToInt(outPutParams["totalrows"]);
            ticketResult.SumWinLoss = outPutParams.ContainsKey("sumwinlost" + baseCurrency)
                ? Converter.ToDecimal(outPutParams["sumwinlost" + baseCurrency])
                : ticketResult.MainTicketList.Sum(x => x.Winlost);
            ticketResult.SumBetEffective = outPutParams.ContainsKey("sumbeteffective" + baseCurrency)
                ? Converter.ToDecimal(outPutParams["sumbeteffective" + baseCurrency])
                : ticketResult.MainTicketList.Sum(x => x.BetEffective);
            ticketResult.SumLicenseeWL = outPutParams.ContainsKey("sumlicenseewl" + baseCurrency)
                ? Converter.ToDecimal(outPutParams["sumlicenseewl" + baseCurrency])
                : ticketResult.MainTicketList.Sum(x => x.LicenseeWL);
            ticketResult.SumComm = outPutParams.ContainsKey("sumcomm" + baseCurrency)
                ? Converter.ToDecimal(outPutParams["sumcomm" + baseCurrency])
                : ticketResult.MainTicketList.Sum(x => x.Comm);
            ticketResult.SumPlayerComm = outPutParams.ContainsKey("sumplayercomm" + baseCurrency)
                ? Converter.ToDecimal(outPutParams["sumplayercomm" + baseCurrency])
                : ticketResult.MainTicketList.Sum(x => x.PlayerComm);
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

        private static void SortData(BetListMetaModel metaModel, SortableList<LrfTicket> ticketList)
        {
            if (!metaModel.IsServerPaging && !string.IsNullOrWhiteSpace(metaModel.SortBy))
            {
                // Because Winloss and Commission are shared the same column (in grid)
                // so we need to sort by the total of two values which is stored in WinlossComm property.
                if (metaModel.SortBy.Equals("WinLoss", StringComparison.OrdinalIgnoreCase))
                {
                    metaModel.SortBy = "WinLossComm";
                }

                if (metaModel.SortBy.Equals("LicenseeWL", StringComparison.OrdinalIgnoreCase))
                {
                    metaModel.SortBy = "LicenseeWLComm";
                }

                if (metaModel.SortBy.Equals("nickname", StringComparison.OrdinalIgnoreCase))
                {
                    metaModel.SortBy = "ExternalId";
                }

                ticketList.Sort(metaModel.SortBy, metaModel.SortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}