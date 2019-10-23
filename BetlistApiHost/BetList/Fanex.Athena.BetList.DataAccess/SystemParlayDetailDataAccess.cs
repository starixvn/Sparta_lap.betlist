using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.BetList.Core.Entities;
using Fanex.Common;
using Fanex.Data;

namespace Fanex.Athena.BetList.DataAccess
{
    public class SystemParlayDetailDataAccess : BaseBetListDataAccess, ISystemParlayDetailDataAccess
    {
        [CLSCompliant(false)]
        public async Task<SystemParlayData> GetData(SystemParlayDetailMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_SelCombinationParlayDetail";
            using (var db = this.GetObjectDb(UspName))
            {
                SetSystemParlayDetailParameter(db, metaModel);

                var dataSet = await db.ExecuteDataSetAsync().ConfigureAwait(false);
                var systemParlayData = new SystemParlayData
                {
                    TicketList = new List<ISystemParlayTicket>(),
                    SerialList = new List<ISystemParlaySerial>()
                };

                if (dataSet.Tables.Count > 0)
                {
                    using (var ticketData = dataSet.Tables[0])
                    {
                        foreach (DataRow row in ticketData.Rows)
                        {
                            systemParlayData.TicketList.Add(ReadSystemParlayTicket(row));
                        }
                    }

                    if (dataSet.Tables.Count > 1)
                    {
                        using (var serialData = dataSet.Tables[1])
                        {
                            foreach (DataRow row in serialData.Rows)
                            {
                                systemParlayData.SerialList.Add(ReadSystemParlaySerial(row));
                            }
                        }
                    }
                }

                systemParlayData.Outstanding = Converter.ToDecimal(db.GetParameterValue("outstanding"));

                return systemParlayData;
            }
        }

        private static void SetSystemParlayDetailParameter(IObjectDb db, SystemParlayDetailMetaModel metaModel)
        {
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("refno", metaModel.RefNo);
            db.SetParameterValue("movebetsday", metaModel.MoveBetsDay);
            db.SetParameterValue("balanceupday", metaModel.BalanceUpDay);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            db.SetParameterValue("outstanding", null);
        }

        private SystemParlayTicket ReadSystemParlayTicket(DataRow row)
        {
            return new SystemParlayTicket
            {
                Comm = Converter.ToDecimal(row["comm"]),
                MOdds = Converter.ToDecimal(row["modds"]),
                PlayerComm = Converter.ToDecimal(row["playercomm"]),
                Serial = Convert.ToInt64(row["serial"]),
                Stake = Converter.ToDecimal(row["stake"]),
                Status = Converter.ToString2(row["status"]),
                StatusId = Converter.ToInt(row["statusid"]),
                WinLost = Converter.ToDecimal(row["winlost"]),
                WinLostDate = Converter.ToDateTime(row["winlostdate"])
            };
        }

        private SystemParlaySerial ReadSystemParlaySerial(DataRow row)
        {
            return new SystemParlaySerial
            {
                Serial = Convert.ToInt64(row["serial"]),
                HomeId = Converter.ToInt(row["homeid"]),
                AwayId = Converter.ToInt(row["awayid"])
            };
        }
    }
}