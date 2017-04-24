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
    using Logging;
    using Model.BetList.DTO;
    using Model.BetList.ViewModel;

    public class CashOutBetListDataAccess : BaseBetListDataAccess, ICashOutBetListDataAccess
    {
        public CashOutBetListDetailViewModel GetDetail(CashOutBetListDetailMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_CashOut_Detail";

            try
            {
                using (var db = this.GetObjectDb(UspKey))
                {
                    var param = new
                    {
                        metaModel.TransId,
                        metaModel.RefNo,
                        metaModel.BetType,
                        OriginalTransId = metaModel.BetdaqId,
                        AgentList = metaModel.AgentListXml
                    };
                    using (var reader = db.ExecuteReader(param))
                    {
                        var viewModel = new CashOutBetListDetailViewModel();
                        viewModel.OriginalTicket = ReadBetListTicket(reader, metaModel.BaseCurrency).GetFirstOrDefault() ?? new LrfTicket();

                        if (reader.NextResult())
                        {
                            viewModel.CashOutTicketList = ReadCashOutTickets(reader, metaModel.BaseCurrency);
                        }

                        return viewModel;
                    }
                }
            }
            catch (ObjectDbException exception)
            {
                LogService.Error(exception.Message + "\n UspName=" + UspKey, exception);
                throw new DataAccessException(exception.Message, UspKey, metaModel);
            }
        }

        private IEnumerable<CashOutTicket> ReadCashOutTickets(IDataReader reader, string baseCurrency)
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
                    Stake = reader.ToDecimal("stake" + baseCurrency),
                    WinLoss = reader.ToDecimal("winlost" + baseCurrency),
                    Price = reader.ToDecimal("price" + baseCurrency),
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