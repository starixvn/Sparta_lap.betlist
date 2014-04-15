// -----------------------------------------------------------------------
// <copyright file="TopCustomerDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Warehouse
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Extension;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Warehouse;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Warehouse;
    using Fanex.Athena.Models.MetaModel.Warehouse;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Top customer data access.
    /// </summary>
    public class TopCustomerDataAccess : BaseDataAccess, ITopCustomerDataAccess
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="TopCustomerDataAccess" /> class.
        /// </summary>
        /// <param name="balanceUp"> The balance up. </param>
        public TopCustomerDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        #region ITopCustomerDataAccess Members

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="metaModel"> The model. </param>
        /// <returns>List top customer. </returns>
        public IList<TopCustomer> GetDataList(TopCustomerMetaModel metaModel)
        {
            const string UspName = "Lrf_rpt_DWTopCustomerSel";
            var dataList = new List<TopCustomer>();
            var baseCurrency = metaModel.BaseCurrency == "eur" ? 2 : (metaModel.BaseCurrency == "usd") ? 1 : 0;
            var uspParams = new
                            {
                                FromDate = metaModel.FromDate,
                                ToDate = metaModel.ToDate,
                                currencyid = metaModel.CurrencyId,
                                sortingid = metaModel.SortingId,
                                customer = metaModel.ExternalId,
                                activemin = metaModel.ActiveMin,
                                basedcurrency = baseCurrency,
                                wlmtotalmin = metaModel.WinLossTotalMin,
                                betcountmin = metaModel.BetCountMin,
                                sportid = metaModel.SportId,
                                toprow = metaModel.TopRow,
                                UserID = metaModel.UserId
                            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.DbCommand.CommandTimeout = 180;
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        while (reader.Read())
                        {
                            var topcustomer = new TopCustomer();
                            if (reader.FieldExists("errorcode"))
                            {
                                topcustomer.ErrorCode = reader["errorcode"];
                                topcustomer.Message = reader["message"];
                            }
                            else
                            {
                                topcustomer.Member = reader.FieldExists("UserName")
                                                         ? reader.ToString("UserName")
                                                         : string.Empty;
                                topcustomer.Currency = reader.ToString("Currency");
                                topcustomer.MemberWinLoss = reader.ToDecimal("MemberWinlost");
                                topcustomer.MemberWinLossCom = reader.FieldExists("playercomm") ? reader.ToDecimal("playercomm") : 0;
                                topcustomer.BetCount = reader.ToInt("betcount");
                                topcustomer.Turnover = reader.ToDecimal("Turnover");
                                topcustomer.LastLoginDate = reader.FieldExists("LastActiveDate")
                                                                ? reader["LastActiveDate"]
                                                                : null;
                            }

                            dataList.Add(topcustomer);
                        }
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }

            return dataList;
        }

        /// <summary>
        ///   Get Data For Excel.
        /// </summary>
        /// <param name="metaModel"> The model. </param>
        /// <returns> Data table.</returns>
        public DataTable GetDataForExcel(TopCustomerMetaModel metaModel)
        {
            const string UspName = "Lrf_rpt_DWTopCustomerSel";
            var baseCurrency = metaModel.BaseCurrency == "eur" ? 2 : (metaModel.BaseCurrency == "usd") ? 1 : 0;
            var uspParams = new
                            {
                                FromDate = metaModel.FromDate,
                                ToDate = metaModel.ToDate,
                                currencyid = metaModel.CurrencyId,
                                sortingid = metaModel.SortingId,
                                customer = metaModel.ExternalId,
                                activemin = metaModel.ActiveMin,
                                basedcurrency = baseCurrency,
                                wlmtotalmin = metaModel.WinLossTotalMin,
                                betcountmin = metaModel.BetCountMin,
                                sportid = metaModel.SportId,
                                toprow = metaModel.TopRow,
                                UserID = metaModel.UserId
                            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.DbCommand.CommandTimeout = 180;
                    var dataset = db.ExecuteDataSet(uspParams);
                    return dataset.Tables.Count > 0 ? dataset.Tables[0] : new DataTable();
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        #endregion ITopCustomerDataAccess Members
    }
}