// -----------------------------------------------------------------------
// <copyright file="ActiveCustomerSummaryDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Report.ResultModel;

    public class ActiveCustomerSummaryDataAccess : BaseDataAccess, IActiveCustomerSummaryDataAccess
    {
        public async Task<ActiveCustomerSummaryResultModel> GetActiveCustomers(ActiveCustomerMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_ActiveMember";

            using (var db = this.GetObjectDb(UspName))
            {
                UpdateUspParams(db, metaModel);
                db.SetParameterValue("TotalActive", null);
                SortableList<ActiveCustomer> data;
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    data = GetData(reader, metaModel);
                }

                var totalCustomer = Convert.ToInt32(db.GetParameterValue("TotalActive"));

                return new ActiveCustomerSummaryResultModel
                {
                    DataList = data,
                    TotalCustomer = totalCustomer
                };
            }
        }

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

        private static void UpdateUspParams(IObjectDb db, ActiveCustomerMetaModel metaModel)
        {
            var productCodes = metaModel.ProductOptions.Where(x => x.Status).Select(x => x.Code).ToArray();
            db.SetParameterValue("fromdate", metaModel.FromDate);
            db.SetParameterValue("todate", metaModel.ToDate);
            db.SetParameterValue("products", string.Join(",", productCodes));
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
        }
    }
}