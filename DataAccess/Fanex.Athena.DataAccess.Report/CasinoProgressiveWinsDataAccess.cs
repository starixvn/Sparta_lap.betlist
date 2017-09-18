// -----------------------------------------------------------------------
// <copyright file="CasinoProgressiveWinsDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class CasinoProgressiveWinsDataAccess : BaseDataAccess, ICasinoProgressiveWinsDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public CasinoProgressiveWinsDataAccess()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
        }

        public CasinoProgressiveWinsDataAccess(IBalanceUpDataAccess balanceUpDataAccess)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        #region ICasinoProgressiveWinsDataAccess Members

        public IEnumerable<CasinoProgressiveWins> GetDataList(CasinoProgressiveWinsMetaModel metaModel)
        {
            var uspName = "Lrf_Rpt_CasinoProgressiveWins";
            try
            {
                using (var db = this.GetObjectDb(uspName))
                {
                    AddParams(db, metaModel);

                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel.BaseCurrency);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        #endregion ICasinoProgressiveWinsDataAccess Members

        #region Private

        private static IEnumerable<CasinoProgressiveWins> GetData(
            IDataReader reader, string baseCurrency)
        {
            var result = new List<CasinoProgressiveWins>();

            var index = 1;
            var winAmountColumn = "WinAmount" + baseCurrency;

            while (reader.Read())
            {
                var item = new CasinoProgressiveWins
                {
                    Index = index++,
                    WinlostDate = reader.ToDateTime("WinlostDate"),
                    SystemId = reader.ToString("SystemId"),
                    ExternalId = reader.ToString("ExternalId"),
                    RefNo = reader.ToLong("RefNo"),
                    WinAmount = reader.ToDecimal(winAmountColumn),
                    BetType = reader.ToInt("BetType"),
                    SportType = reader.ToInt("SportType"),
                    TransDesc = reader.ToString("TransDesc"),
                    CurrencyId = reader.ToInt("Currency"),
                    CustId = reader.ToInt("CustId"),
                    GameId = reader.ToString("TranId3rd")
                };

                result.Add(item);
            }

            return result;
        }

        private void AddParams(IObjectDb db, CasinoProgressiveWinsMetaModel metaModel)
        {
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            db.AddInParameter("fromdate", DbType.Date, metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, metaModel.ToDate.Date);
            db.AddInParameter("movebetsday", DbType.DateTime, balanceUp.MoveBetsDay);
            db.AddInParameter("BalanceUpDay", DbType.DateTime, balanceUp.BalanceUpDay);

            foreach (var product in metaModel.ProductOptions)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        #endregion Private
    }
}