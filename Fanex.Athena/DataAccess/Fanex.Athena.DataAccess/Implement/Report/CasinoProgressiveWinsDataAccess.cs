// -----------------------------------------------------------------------
// <copyright file="CasinoProgressiveWinsDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class CasinoProgressiveWinsDataAccess : BaseDataAccess, ICasinoProgressiveWinsDataAccess
    {
        public CasinoProgressiveWinsDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        #region ICasinoProgressiveWinsDataAccess Members

        public IEnumerable<CasinoProgressiveWins> GetDataList(CasinoProgressiveWinsMetaModel metaModel)
        {
            var uspName = "Lrf_Rpt_CasinoProgressiveWins";
            try
            {
                using (var db = new ObjectDb(uspName))
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
                    CurrencyId = reader.ToInt("Currency")
                };

                result.Add(item);
            }

            return result;
        }

        private void AddParams(ObjectDb db, CasinoProgressiveWinsMetaModel metaModel)
        {
            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
            db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("fromdate", DbType.Date, metaModel.FromDate.Date);
            db.AddInParameter("todate", DbType.Date, metaModel.ToDate.Date);
            db.AddInParameter("movebetsday", DbType.DateTime, this.BalanceUp.MoveBetsDay);
            db.AddInParameter("BalanceUpDay", DbType.DateTime, this.BalanceUp.BalanceUpDay);

            foreach (var product in metaModel.ProductOptions)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        #endregion Private
    }
}