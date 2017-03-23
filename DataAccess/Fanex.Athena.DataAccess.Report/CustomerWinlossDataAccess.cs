// -----------------------------------------------------------------------
// <copyright file="CustomerWinlossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;

    public class CustomerWinlossDataAccess : BaseDataAccess, ICustWinlossDataAccess
    {
        public CustomerWinlossDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public SortableList<CustomerWinLoss> GetDataList(CustomerWinLossMetaModel metaModel, out CustomerWinLoss totalData, out int totalRows)
        {
            totalRows = 0;
            totalData = new CustomerWinLoss();
            using (var db = this.GetObjectDb("Lrf_Rpt_WinLoss"))
            {
                db.AddInParameter("drawfilter", DbType.Int32, metaModel.DrawFilter);
                db.AddInParameter("fromdate", DbType.DateTime, metaModel.FromDate);
                db.AddInParameter("todate", DbType.DateTime, metaModel.ToDate);
                db.AddInParameter("pagesize", DbType.Int32, metaModel.IsExportExcel ? metaModel.PageSizeExcel : metaModel.PageSize);
                db.AddInParameter("pageindex", DbType.Int32, metaModel.PageIndex);
                db.AddInParameter("movebetsday", DbType.DateTime, BalanceUp.MoveBetsDay);
                db.AddInParameter("isold", DbType.Boolean, metaModel.IsOld);
                db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
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