// -----------------------------------------------------------------------
// <copyright file="CustomerWinlossDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------
namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;

    /// <summary>
    /// Customer win loss data access.
    /// </summary>
    public class CustomerWinlossDataAccess : BaseDataAccess, ICustWinlossDataAccess
    {
        public CustomerWinlossDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="totalData">Total data value.</param>
        /// <param name="totalRows">Total rows value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public SortableList<CustomerWinLoss> GetDataList(CustomerWinLossMetaModel metaModel, out CustomerWinLoss totalData, out int totalRows)
        {
            totalRows = 0;
            totalData = new CustomerWinLoss();
            using (var db = new ObjectDb("Lrf_Rpt_WinLoss"))
            {
                db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                db.AddInParameter("drawfilter", DbType.Int32, metaModel.DrawFilter);
                db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                db.AddInParameter("pagesize", DbType.Int32, metaModel.IsExportExcel ? metaModel.PageSizeExcel : metaModel.PageSize);
                db.AddInParameter("pageindex", DbType.Int32, metaModel.PageIndex);
                db.AddInParameter("movebetsday", DbType.DateTime, BalanceUp.MoveBetsDay);
                db.AddInParameter("isold", DbType.Boolean, metaModel.IsOld);
                db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);

                foreach (var item in metaModel.ProductOptions)
                {
                    db.AddInParameter("show" + item.Code, DbType.Boolean, item.Status);
                }

                using (var reader = db.ExecuteReader())
                {
                    var result = new SortableList<CustomerWinLoss>();
                    while (reader.Read())
                    {
                        result.Add(new CustomerWinLoss
                        {
                            // Add data to CustBalance Item List
                            CustId = reader.ToInt("CustID"),
                            ExternalId = reader.ToString("nickname"),
                            SystemId = reader.ToString("username"),
                            Turnover = reader.ToDecimal("turnover" + metaModel.BaseCurrency),
                            TotalTransaction = reader.ToInt("betcount"),
                            MemberWL = reader.ToDecimal("memberwinlost" + metaModel.BaseCurrency),
                            BetEffective = reader.ToDecimal("beteffective" + metaModel.BaseCurrency),
                            PlayerComm = reader.ToDecimal("playercomm" + metaModel.BaseCurrency),
                            MemberTotalWL = reader.ToDecimal("memberwinlost" + metaModel.BaseCurrency) + reader.ToDecimal("playercomm" + metaModel.BaseCurrency),
                            LicenseeWL = reader.ToDecimal("licenseewl" + metaModel.BaseCurrency),
                            BAComm = reader.ToDecimal("commba" + metaModel.BaseCurrency),
                            RacingRevenue = reader.ToDecimal("winlossrb" + metaModel.BaseCurrency)
                                + reader.ToDecimal("commrb" + metaModel.BaseCurrency),
                            AmtDueToBA = reader.ToDecimal("amtduetoba" + metaModel.BaseCurrency),
                            AmtDueToRacing = reader.ToDecimal("amtduetorb" + metaModel.BaseCurrency)
                        });
                    }

                    if (result.Count > 0 && reader.NextResult())
                    {
                        if (reader.Read())
                        {
                            totalData.Turnover = reader.ToDecimal("sumturnover" + metaModel.BaseCurrency);
                            totalData.TotalTransaction = reader.ToInt("sumbetcount");
                            totalData.MemberWL = reader.ToDecimal("summemberwinlost" + metaModel.BaseCurrency);
                            totalData.PlayerComm = reader.ToDecimal("sumplayercomm" + metaModel.BaseCurrency);
                            totalData.LicenseeWL = reader.ToDecimal("sumlicenseewl" + metaModel.BaseCurrency);
                            totalData.BAComm = reader.ToDecimal("sumcommba" + metaModel.BaseCurrency);
                            totalData.RacingRevenue = reader.ToDecimal("sumwinlossrb" + metaModel.BaseCurrency)
                                + reader.ToDecimal("sumcommrb" + metaModel.BaseCurrency);
                            totalData.AmtDueToBA = reader.ToDecimal("sumamtduetoba" + metaModel.BaseCurrency);
                            totalData.AmtDueToRacing = reader.ToDecimal("sumamtduetorb" + metaModel.BaseCurrency);
                            totalData.BetEffective = reader.ToDecimal("sumbeteffective" + metaModel.BaseCurrency);

                            totalRows = reader.ToInt("TotalRows");
                        }
                    }

                    return result;
                }
            }
        }
    }
}