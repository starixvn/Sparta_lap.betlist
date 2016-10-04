// -----------------------------------------------------------------------
// <copyright file="ColossusBetBetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetList
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.BetList.ColossusBets.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Interfaces;
    using Logging;
    using Model.BetList.ViewModel;

    public class ColossusBetBetListDataAccess : BaseBetListDataAccess, IColossusBetBetListDataAccess
    {
        private const int SoldBetType = 18002;

        private const string UspKey = "Lrf_BetList_CB_Detail";

        public ColossusBetDetailViewModel GetData(ColossusBetDetailMetaModel metaModel)
        {
            try
            {
                using (var db = this.GetObjectDb(UspKey))
                {
                    db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("custid", DbType.String, metaModel.CustId);
                    db.AddInParameter("refno", DbType.String, metaModel.RefNo);
                    using (var reader = db.ExecuteReader())
                    {
                        return ReadData(reader, metaModel.BaseCurrency);
                    }
                }
            }
            catch (ObjectDbException exception)
            {
                LogService.Error(exception.Message + "\n UspName=" + UspKey, exception);
                throw new DataAccessException(exception.Message, UspKey, metaModel);
            }
        }

        private static ColossusBetDetailViewModel ReadData(IDataReader reader, string baseCurrency)
        {
            var ticketHelper = new TicketHelper();
            var winlossColumn = "winlost" + baseCurrency;
            var commColumn = "comm" + baseCurrency;

            var dataList = new List<ColossusBetDetail>();
            decimal totalSoldPercent = 0;
            decimal totalWinLoss = 0;
            decimal totalComm = 0;
            decimal totalSoldWinLoss = 0;
            while (reader.Read())
            {
                var bettypeId = reader.ToInt("BetType");
                var sold = reader.ToInt("Sold");
                var winloss = reader.ToDecimal(winlossColumn);
                var comm = reader.ToDecimal(commColumn);
                totalSoldPercent += sold;
                totalWinLoss += winloss;
                totalComm += comm;
                totalSoldWinLoss += bettypeId == SoldBetType ? winloss : 0;
                dataList.Add(new ColossusBetDetail()
                {
                    TransDate = reader.ToDateTime("TransDate"),
                    TransactionType = BuilderExtension.BuildColossusBetsTransactionType(bettypeId, sold, ticketHelper),
                    WinLoss = BuilderExtension.BuildColossusBetsWinLoss(winloss),
                    Commission = comm
                });
            }

            return new ColossusBetDetailViewModel()
            {
                DataList = dataList,
                TotalSoldPercent = totalSoldPercent,
                TotalWinLoss = totalWinLoss,
                TotalComm = totalComm,
                TotalSoldWinLoss = totalSoldWinLoss
            };
        }
    }
}