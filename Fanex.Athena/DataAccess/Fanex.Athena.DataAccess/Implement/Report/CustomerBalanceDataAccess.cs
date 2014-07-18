// -----------------------------------------------------------------------
// <copyright file="CustomerBalanceDataAccess.cs" company="Nexcel Solution">
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
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;

    /// <summary>
    /// Customer balance.
    /// </summary>
    public class CustomerBalanceDataAccess : BaseDataAccess, ICustBalanceDataAccess
    {
        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="totalData">Total data value.</param>
        /// <param name="totalRow">Total rows value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IList<CustBalance> GetDataList(CustomerBalanceMetaModel metaModel, ref CustBalance totalData, ref int totalRow)
        {
            IList<CustBalance> result = new List<CustBalance>();
            using (var db = new ObjectDb("Lrf_Rpt_CustomerBalance"))
            {
                db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                db.AddInParameter("licenseetype", DbType.String, metaModel.LicenseeType);
                db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
                db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                db.AddInParameter("topcount", DbType.Int32, metaModel.TopCount);
                db.AddInParameter("orderitem", DbType.Byte, metaModel.OrderItem);
                db.AddInParameter("excludetest", DbType.Boolean, metaModel.IsExcludeTest);
                db.AddInParameter("pagesize", DbType.Int32, metaModel.PageSize);
                db.AddInParameter("pageindex", DbType.Int32, metaModel.PageIndex);

                foreach (var item in metaModel.ProductOptions)
                {
                    db.AddInParameter("show" + item.Code, DbType.Boolean, item.Status);
                }

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new CustBalance
                        {
                            ExternalId = reader.ToString("nickname"),
                            SystemId = reader.ToString("username"),
                            OpeningBalance = reader.ToDecimal("openingbalance"),
                            Deposit = reader.ToDecimal("deposit"),
                            Withdrawal = reader.ToDecimal("withdrawal"),
                            Bonus = reader.ToDecimal("bonus"),
                            Adjustment = reader.ToDecimal("adjustment"),
                            Turnover = reader.ToDecimal("turnover"),
                            DrawGame = reader.ToDecimal("drawgame"),
                            NetTurnover = reader.ToDecimal("turnover") - reader.ToDecimal("drawgame"),
                            PendingFunds = reader.ToDecimal("outstanding"),
                            WinLoss = reader.ToDecimal("winloss"),
                            PlayerComm = reader.ToDecimal("playercomm"),
                            TotalWl = reader.ToDecimal("winloss") + reader.ToDecimal("playercomm"),
                            AffiliateTransfer = reader.ToDecimal("affiliatetransfer"),
                            ClosingBalance = reader.ToDecimal("closingbalance"),
                            TurnoverBA = reader.ToDecimal("TurnOverBA"),
                            CommBA = reader.ToDecimal("CommBA"),
                            AmountDueToBA = reader.ToDecimal("AmountDueToBA")
                        });
                    }

                    if (result.Count > 0 && reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            totalData = new CustBalance
                            {
                                OpeningBalance = reader.ToDecimal("sumopeningbalance"),
                                Deposit = reader.ToDecimal("sumdeposit"),
                                Withdrawal = reader.ToDecimal("sumwithdrawal"),
                                Bonus = reader.ToDecimal("sumbonus"),
                                Adjustment = reader.ToDecimal("sumadjustment"),
                                Turnover = reader.ToDecimal("sumturnover"),
                                DrawGame = reader.ToDecimal("sumdrawgame"),
                                NetTurnover = reader.ToDecimal("sumturnover") - reader.ToDecimal("sumdrawgame"),
                                PendingFunds = reader.ToDecimal("sumoutstanding"),
                                WinLoss = reader.ToDecimal("sumwinloss"),
                                AffiliateTransfer = reader.ToDecimal("sumaffiliatetransfer"),
                                ClosingBalance = reader.ToDecimal("sumclosingbalance"),
                                TurnoverBA = reader.ToDecimal("sumturnoverba"),
                                CommBA = reader.ToDecimal("sumcommba"),
                                AmountDueToBA = reader.ToDecimal("sumamountduetoba"),
                                PlayerComm = reader.ToDecimal("sumplayercomm"),
                                TotalWl = reader.ToDecimal("sumwinloss") +
                                            reader.ToDecimal("sumplayercomm")
                            };

                            totalRow = reader.ToInt("TotalRows");
                        }
                    }
                }
            }

            return result;
        }
    }
}