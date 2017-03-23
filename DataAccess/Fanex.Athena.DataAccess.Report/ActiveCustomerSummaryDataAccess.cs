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
    using Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class ActiveCustomerSummaryDataAccess : BaseDataAccess, IActiveCustomerSummaryDataAccess
    {
        public ActiveCustomerSummaryDataAccess()
        {
        }

        public SortableList<ActiveCustomer> GetActiveCustomers(ActiveCustomerMetaModel metaModel, out int totalCustomer)
        {
            var uspName = "Lrf_Rpt_ActiveMember";
            try
            {
                using (var db = this.GetObjectDb(uspName))
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

        private void UpdateUspParams(IObjectDb db, ActiveCustomerMetaModel metaModel)
        {
            var productCodes = metaModel.ProductOptions.Where(x => x.Status).Select(x => x.Code).ToArray();
            db.AddInParameter("fromdate", DbType.Date, metaModel.FromDate);
            db.AddInParameter("todate", DbType.Date, metaModel.ToDate);
            db.AddInParameter("products", DbType.String, string.Join(",", productCodes));
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
        }
    }
}