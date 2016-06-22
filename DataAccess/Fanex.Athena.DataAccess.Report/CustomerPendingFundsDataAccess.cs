// -----------------------------------------------------------------------
// <copyright file="CustomerPendingFundsDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;

    /// <summary>
    /// Customer pending funds.
    /// </summary>
    public class CustomerPendingFundsDataAccess : BaseDataAccess, ICustPendingFundsDataAccess
    {
        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="totalData">Total data value.</param>
        /// <param name="totalRows">Total rows value.</param>
        /// <returns>The result value.</returns>
        public SortableList<CustPendingFunds> GetDataList(CustomerPendingFundsMetaModel metaModel, ref CustPendingFunds totalData, ref int totalRows)
        {
            SortableList<CustPendingFunds> result = new SortableList<CustPendingFunds>();
            using (var db = new ObjectDb("Lrf_Rpt_Outstanding"))
            {
                db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                if (!string.IsNullOrEmpty(metaModel.LicenseeType))
                {
                    db.AddInParameter("licenseetype", DbType.String, metaModel.LicenseeType);
                }

                db.AddInParameter("pagesize", DbType.Int32, metaModel.PageSize);
                db.AddInParameter("pageindex", DbType.Int32, metaModel.PageIndex);
                db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
                foreach (var item in metaModel.ProductOptions)
                {
                    db.AddInParameter("show" + item.Code, DbType.Boolean, item.Status);
                }

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new CustPendingFunds
                        {
                            CustId = reader.ToInt("CustID"),
                            ExternalId = reader.ToString("nickname"),
                            SystemId = reader.ToString("username"),
                            BetCounts = reader.ToInt("BetCounts"),
                            PendingFunds = reader.ToDecimal("Outstanding" + metaModel.BaseCurrency)
                        });
                    }

                    if (result.Count > 0 && reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            totalData = new CustPendingFunds
                            {
                                BetCounts = reader.ToInt("SumBetCounts"),
                                PendingFunds = reader.ToDecimal("SumOutstanding" + metaModel.BaseCurrency)
                            };
                            totalRows = reader.ToInt("TotalRows");
                        }
                    }
                }
            }

            return result;
        }
    }
}