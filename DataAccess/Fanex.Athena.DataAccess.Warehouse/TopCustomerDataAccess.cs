// -----------------------------------------------------------------------
// <copyright file="TopCustomerDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Warehouse
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Warehouse.Interfaces;
    using Fanex.Athena.Model.Warehouse.DTO;
    using Fanex.Athena.Model.Warehouse.MetaModel;
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
        public TopCustomerDataAccess()
        {
        }

        #region ITopCustomerDataAccess Members

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
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
            var uspParams = new
            {
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                currencyid = metaModel.CurrencyId,
                sortingid = metaModel.SortingId,
                customer = metaModel.ExternalId,
                activemin = metaModel.ActiveMin,
                basedcurrency = metaModel.BaseCurrencyId,
                wlmtotalmin = metaModel.WinLossTotalMin,
                betcountmin = metaModel.BetCountMin,
                sportid = metaModel.SportId,
                toprow = metaModel.TopRow,
                UserID = metaModel.UserId
            };

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        while (reader.Read())
                        {
                            var topcustomer = new TopCustomer();
                            if (reader.HasColumn("errorcode"))
                            {
                                topcustomer.ErrorCode = reader["errorcode"];
                                topcustomer.Message = reader["message"];
                            }
                            else
                            {
                                topcustomer.Member = reader.HasColumn("UserName") ? reader.ToString("UserName") : string.Empty;
                                topcustomer.Currency = reader.ToString("Currency");
                                topcustomer.MemberWinLoss = reader.ToDecimal("MemberWinlost");
                                topcustomer.BetCount = reader.ToInt("betcount");
                                topcustomer.Turnover = reader.ToDecimal("Turnover");
                                topcustomer.LastLoginDate = reader.HasColumn("LastActiveDate") ? reader["LastActiveDate"] : null;
                            }

                            dataList.Add(topcustomer);
                        }
                    }
                }
            }
            catch (ObjectDbException sqlException)
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
            var uspParams = new
            {
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                currencyid = metaModel.CurrencyId,
                sortingid = metaModel.SortingId,
                customer = metaModel.ExternalId,
                activemin = metaModel.ActiveMin,
                basedcurrency = metaModel.BaseCurrencyId,
                wlmtotalmin = metaModel.WinLossTotalMin,
                betcountmin = metaModel.BetCountMin,
                sportid = metaModel.SportId,
                toprow = metaModel.TopRow,
                UserID = metaModel.UserId
            };

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    var dataset = db.ExecuteDataSet(uspParams);
                    return dataset.Tables.Count > 0 ? dataset.Tables[0] : new DataTable();
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        #endregion ITopCustomerDataAccess Members
    }
}