// -----------------------------------------------------------------------
// <copyright file="ColossusBetBetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetList
{
    using System.Collections.Generic;
    using System.Data;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Interfaces;
    using Model.BetList.DTO;
    using Model.BetList.ViewModel;
    using Session.Interfaces;

    public class CashOutBetListDataAccess : BaseBetListDataAccess, ICashOutBetListDataAccess
    {
        public CashOutBetListDataAccess() : this(null, null)
        {
        }

        public CashOutBetListDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

        public CashOutBetListDetailViewModel GetDetail(CashOutBetListDetailMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_CashOut_Detail";

            try
            {
                using (var db = GetObjectDb(UspKey))
                {
                    var param = new
                    {
                        metaModel.TransId,
                        metaModel.RefNo,
                        metaModel.BetType,
                        OriginalTransId = metaModel.BetdaqId,
                        AgentList = metaModel.AgentListXml,
                        metaModel.BaseCurrency
                    };
                    using (var reader = db.ExecuteReader(param))
                    {
                        var viewModel = new CashOutBetListDetailViewModel
                        {
                            OriginalTicket = ReadBetListTicket(reader).GetFirstOrDefault()
                        };

                        if (reader.NextResult())
                        {
                            viewModel.CashOutTicketList = ReadCashOutTickets(reader);
                        }

                        return viewModel;
                    }
                }
            }
            catch (ObjectDbException exception)
            {
                throw new DataAccessException(exception, UspKey, metaModel);
            }
        }

        private static IEnumerable<CashOutTicket> ReadCashOutTickets(IDataReader reader)
        {
            var result = new SortableList<CashOutTicket>();

            while (reader.Read())
            {
                var item = new CashOutTicket
                {
                    BetType = reader.ToInt("bettype"),
                    TransId = reader.ToLong("transid"),
                    RefNo = reader.ToString("refno"),
                    Status = reader.ToString("status"),
                    Stake = reader.ToDecimal("stake"),
                    WinLoss = reader.ToDecimal("winlost"),
                    Price = reader.ToDecimal("price"),
                    WinLostDate = reader.ToDateTime("winlostdate"),
                    TransDate = reader.ToDateTime("transdate"),
                    Currency = reader.ToInt("currency"),
                    CustId = reader.ToInt("custid")
                };

                result.Add(item);
            }

            return result;
        }
    }
}