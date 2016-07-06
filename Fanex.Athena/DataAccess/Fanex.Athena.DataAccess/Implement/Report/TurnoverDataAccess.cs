// -----------------------------------------------------------------------
// <copyright file="TurnoverDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;

    /// <summary>
    /// Turnover data access.
    /// </summary>
    public class TurnoverDataAccess : BaseDataAccess, ITurnoverDataAccess
    {
        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="metaModel">Meta model.</param>
        /// <returns>Result value.</returns>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IList<TurnoverDTO> GetDataList(TurnoverMetaModel metaModel)
        {
            IList<TurnoverDTO> result = new List<TurnoverDTO>();
            using (var db = new ObjectDb("Lrf_Rpt_TurnOver"))
            {
                db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                foreach (var item in metaModel.ProductOptions)
                {
                    db.AddInParameter("show" + item.Code, DbType.Boolean, item.Status);
                }

                db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId < 0 ? 0 : metaModel.CurrencyId);
                db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                db.AddInParameter("excludetest", DbType.Boolean, metaModel.IsExcludeTest);
                db.AddInParameter("username", DbType.String, metaModel.Username);
                if (!string.IsNullOrEmpty(metaModel.LicenseeType))
                {
                    db.AddInParameter("licenseetype", DbType.String, metaModel.LicenseeType);
                }

                using (var reader = db.ExecuteReader())
                {
                    var index = 1;
                    while (reader.Read())
                    {
                        result.Add(new TurnoverDTO
                        {
                            Index = index++,
                            Date = reader.ToDateTime("winlostdate").ToString("MM/dd/yyyy"),
                            OpeningBalance = reader.ToDecimal("OpeningBalance" + metaModel.BaseCurrency),
                            Deposit = reader.ToDecimal("Deposit" + metaModel.BaseCurrency),
                            Withdrawal = reader.ToDecimal("Withdrawal" + metaModel.BaseCurrency),
                            AffiliateTransfer = reader.ToDecimal("AffiliateTransfer" + metaModel.BaseCurrency),
                            Bonus = reader.ToDecimal("Bonus" + metaModel.BaseCurrency),
                            Adjustment = reader.ToDecimal("Adjustment" + metaModel.BaseCurrency),
                            Turnover = reader.ToDecimal("Turnover" + metaModel.BaseCurrency),
                            PendingFunds = reader.ToDecimal("PendingFund" + metaModel.BaseCurrency),
                            WinLoss = reader.ToDecimal("WinLoss" + metaModel.BaseCurrency),
                            ClosingBalance = reader.ToDecimal("ClosingBalance" + metaModel.BaseCurrency),
                            Transfer3rd = reader.ToDecimal("Transfer3rd" + metaModel.BaseCurrency),
                            DrawGame = reader.ToDecimal("DrawGame" + metaModel.BaseCurrency),
                            TurnoverBA = reader.ToDecimal("TurnoverBA" + metaModel.BaseCurrency),
                            CommBA = reader.ToDecimal("CommBA" + metaModel.BaseCurrency),
                            AmountDueToBA = reader.ToDecimal("AmountDueToBA" + metaModel.BaseCurrency),
                            NetTurnover = reader.ToDecimal("Turnover" + metaModel.BaseCurrency) - reader.ToDecimal("DrawGame" + metaModel.BaseCurrency),
                            PlayerComm = reader.ToDecimal("PlayerComm" + metaModel.BaseCurrency),
                        });
                    }
                }
            }

            return result;
        }
    }
}