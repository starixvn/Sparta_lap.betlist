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
    using System.Threading.Tasks;
    using Core;
    using Fanex.Athena.DataAccess.Customer.Interfaces;
    using Fanex.Athena.Model.Customer.DTO.CustomerLog;
    using Fanex.Athena.Model.Customer.MetaModel.CustomerLog;

    public class CustomerLogDataAccess : BaseDataAccess, ICustomerLogDataAccess
    {
        public async Task<IList<CustomerLog>> GetCustomerSettingList(CustomerLogMetaModel metaModel)
        {
            const string SpName = "Lrf_CustomerSettingLog_Sel";
            using (var db = this.GetObjectDb(SpName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("UserName", metaModel.UserName);
                db.SetParameterValue("FromDate", metaModel.FromDate);
                db.SetParameterValue("ToDate", metaModel.ToDate);
                db.SetParameterValue("FilterBy", metaModel.ViewById);
                db.SetParameterValue("LicenseeId", metaModel.LicenseeId);
                db.SetParameterValue("siteName", metaModel.SiteName);

                return (await db.QueryAsync<CustomerLog>().ConfigureAwait(false)).ToList();
            }
        }

        /// <summary>
        /// Get Group Commission Log Data List.
        /// </summary>
        /// <param name="metaModel">Group commission log MeteModel.</param>
        /// <returns>A list of group commission log.</returns>
        public async Task<IList<CustomerLog>> GetGroupCommList(CustomerLogMetaModel metaModel)
        {
            const string SpName = "Lrf_GroupCommLog_Sel";

            using (var db = this.GetObjectDb(SpName))
            {
                db.SetParameterValue("FromDate", metaModel.FromDate);
                db.SetParameterValue("ToDate", metaModel.ToDate);
                db.SetParameterValue("LicenseeId", metaModel.LicenseeId);
                db.SetParameterValue("siteName", metaModel.SiteName);
                return (await db.QueryAsync<CustomerLog>().ConfigureAwait(false)).ToList();
            }
        }
    }
}