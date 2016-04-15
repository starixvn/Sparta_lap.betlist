// -----------------------------------------------------------------------
// <copyright file="ActiveCustomerSummaryDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Licensee win loss data access.
    /// </summary>
    public class ActiveCustomerSummaryDataAccess : IActiveCustomerSummaryDataAccess
    {
        public ActiveCustomerSummaryDataAccess()
        {
        }

        #region IActiveCustomerDataAccess Members

        /// <summary>
        /// Get summary win loss.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="totalCustomer">Total customer value.</param>
        /// <returns>The result value.</returns>
        public SortableList<ActiveCustomer> GetActiveCustomers(ActiveCustomerMetaModel metaModel, out int totalCustomer)
        {
            var uspName = "Lrf_Rpt_ActiveMember";
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    UpdateUspParams(db, metaModel);
                    db.AddOutParameter("TotalActive", DbType.Int32, 10);
                    SortableList<ActiveCustomer> data;
                    using (var reader = db.ExecuteReader())
                    {
                         data = GetData(reader, metaModel);
                    }

                    totalCustomer = Convert.ToInt32(db.GetParameterValue("TotalActive"));

                    return data;
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        #endregion IActiveCustomerDataAccess Members

        #region Private

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static SortableList<ActiveCustomer> GetData(IDataReader reader, ActiveCustomerMetaModel metaModel)
        {
            var result = new SortableList<ActiveCustomer>();

            while (reader.Read())
            {
                var customerCountPerProduct = new Dictionary<string, int>();
                foreach (var product in metaModel.ProductOptions)
                {
                    if (reader.HasColumn(product.Code))
                    {
                        customerCountPerProduct.Add(product.Code, reader.ToInt(product.Code));
                    }
                }

                var item = new ActiveCustomer
                {
                    TransactionDate = reader.ToDateTime("TransDate"),
                    CustomerCount = customerCountPerProduct,
                    TotalCustomer = reader.ToInt("MemCountPerMonth")
                };

                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Update USP parameter.
        /// </summary>
        /// <param name="db">Database value.</param>
        /// <param name="metaModel">Meta model value.</param>
        private void UpdateUspParams(ObjectDb db, ActiveCustomerMetaModel metaModel)
        {
            var productCodes = metaModel.ProductOptions.Where(x => x.Status).Select(x => x.Code).ToArray();
            db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
            db.AddInParameter("currencyid", DbType.Int32, metaModel.CurrencyId);
            db.AddInParameter("fromdate", DbType.Date, metaModel.FromDate);
            db.AddInParameter("todate", DbType.Date, metaModel.ToDate);
            db.AddInParameter("products", DbType.String, string.Join(",", productCodes));
            db.AddInParameter("excludetest", DbType.Boolean, metaModel.IsExcludeTest);
        }

        #endregion Private
    }
}