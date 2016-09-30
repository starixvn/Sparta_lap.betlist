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
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Interfaces;
    using Logging;
    using Model.BetList.ViewModel;

    public class ColossusBetBetListDataAccess : BaseBetListDataAccess, IColossusBetBetListDataAccess
    {
        private const string UspKey = "Lrf_BetList_CB_Detail";

        public ColossusBetDetailViewModel GetData(ColossusBetDetailMetaModel metaModel)
        {
            try
            {
                var ticketHelper = new TicketHelper();
                using (var db = this.GetObjectDb(UspKey))
                {
                    db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("custid", DbType.String, metaModel.CustId);
                    db.AddInParameter("refno", DbType.String, metaModel.RefNo);
                    using (var reader = db.ExecuteReader())
                    {
                        var winlossColumn = "winlost" + metaModel.BaseCurrency;
                        var commColumn = "comm" + metaModel.BaseCurrency;

                        var dataList = new List<ColossusBetDetail>();
                        decimal totalSoldPercent = 0;
                        while (reader.Read())
                        {
                            var bettypeId = reader.ToInt("BetType");
                            var sold = reader.ToInt("Sold");
                            totalSoldPercent += sold;
                            dataList.Add(new ColossusBetDetail()
                            {
                                TransDate = reader.ToDateTime("TransDate"),
                                TransactionType = BuilderExtension.BuildColossusBetsTransactionType(bettypeId, sold, ticketHelper),
                                WinLoss = BuilderExtension.BuildColossusBetsWinLoss(reader.ToDecimal(winlossColumn)),
                                Commission = reader.ToDecimal(commColumn)
                            });
                        }

                        return new ColossusBetDetailViewModel()
                        {
                            DataList = dataList,
                            TotalSoldPercent = totalSoldPercent
                        };
                    }
                }
            }
            catch (ObjectDbException exception)
            {
                LogService.Error(exception.Message + "\n UspName=" + UspKey, exception);
                throw new DataAccessException(exception.Message, UspKey, metaModel);
            }
        }
    }
}