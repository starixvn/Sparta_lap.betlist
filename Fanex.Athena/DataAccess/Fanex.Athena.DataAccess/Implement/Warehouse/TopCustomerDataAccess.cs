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
        /// <param name="licenseeList"> The licensee list. </param>
        /// <returns>List top customer. </returns>
        public IList<TopCustomer> GetDataList(TopCustomerMetaModel metaModel, string licenseeList)
        {
            const string UspName = "Lrf_Get_Licensees_TopCustomer";
            var dataList = new List<TopCustomer>();
            var baseCurrency = metaModel.BaseCurrency == "eur" ? 2 : (metaModel.BaseCurrency == "usd") ? 1 : 0;
            var uspParams = new
                            {
                                datestart = metaModel.FromDate,
                                dateend = metaModel.ToDate,
                                currencyid = metaModel.CurrencyId == 0 ? -1 : metaModel.CurrencyId,
                                sortingid = metaModel.SortingId,
                                customer = metaModel.ExternalId,
                                activemin = metaModel.ActiveMin,
                                licenseelist = licenseeList,
                                basedcurrency = baseCurrency,
                                wlmtotalmin = metaModel.WinLossTotalMin,
                                betcountmin = metaModel.BetCountMin,
                                sportid = metaModel.SportId,
                                toprow = metaModel.TopRow
                            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
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
                                topcustomer.Member = reader.FieldExists("member")
                                                         ? reader.ToString("member")
                                                         : string.Empty;
                                topcustomer.Currency = reader.ToString("currency");
                                topcustomer.BaComm = reader.ToDecimal("commission");
                                topcustomer.MemberWinLoss = reader.ToDecimal("memberwl");
                                topcustomer.MemberWinLossCom = reader.ToDecimal("playercomm");
                                topcustomer.BetCount = reader.ToInt("betcount");
                                topcustomer.Turnover = reader.ToDecimal("turnover100");
                                topcustomer.LastLoginDate = reader.FieldExists("lastactivitydate")
                                                                ? reader["lastactivitydate"]
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
        /// <param name="licenseeList"> The licensee list. </param>
        /// <returns> Data table.</returns>
        public DataTable GetDataForExcel(TopCustomerMetaModel metaModel, string licenseeList)
        {
            const string UspName = "Lrf_Get_Licensees_TopCustomer";
            var baseCurrency = metaModel.BaseCurrency == "eur" ? 2 : (metaModel.BaseCurrency == "usd") ? 1 : 0;
            var uspParams = new
                            {
                                datestart = metaModel.FromDate,
                                dateend = metaModel.ToDate,
                                currencyid = metaModel.CurrencyId == 0 ? -1 : metaModel.CurrencyId,
                                sortingid = metaModel.SortingId,
                                customer = metaModel.ExternalId,
                                activemin = metaModel.ActiveMin,
                                licenseelist = licenseeList,
                                basedcurrency = baseCurrency,
                                wlmtotalmin = metaModel.WinLossTotalMin,
                                betcountmin = metaModel.BetCountMin,
                                sportid = metaModel.SportId,
                                toprow = metaModel.TopRow
                            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
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