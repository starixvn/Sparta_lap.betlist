// -----------------------------------------------------------------------
// <copyright file="CustomerLogDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Customer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core;
    using Fanex.Athena.DataAccess.Customer.Interfaces;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;

    /// <summary>
    /// Customer log.
    /// </summary>
    public class CustomerLogDataAccess : BaseDataAccess, ICustomerLogDataAccess
    {
        /// <summary>
        /// Get Customer Setting Log Data List.
        /// </summary>
        /// <param name="metaModel">Customer log MetaModel.</param>
        /// <returns>The list of customer log.</returns>
        public IList<CustomerLog> GetCustomerSettingList(CustomerLogMetaModel metaModel)
        {
            const string SpName = "Lrf_CustomerSettingLog_Sel";
            using (var db = this.GetObjectDb(SpName))
            {
                db.AddInParameter("UserName", DbType.String, metaModel.UserName);
                db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                db.AddInParameter("FilterBy", DbType.Int16, metaModel.ViewById);
                db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);

                return db.Query<CustomerLog>().ToList();
            }
        }

        /// <summary>
        /// Get Group Commission Log Data List.
        /// </summary>
        /// <param name="metaModel">Group commission log MeteModel.</param>
        /// <returns>A list of group commission log.</returns>
        public IList<CustomerLog> GetGroupCommList(CustomerLogMetaModel metaModel)
        {
            const string SpName = "Lrf_GroupCommLog_Sel";

            using (var db = this.GetObjectDb(SpName))
            {
                db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                return db.Query<CustomerLog>().ToList();
            }
        }
    }
}