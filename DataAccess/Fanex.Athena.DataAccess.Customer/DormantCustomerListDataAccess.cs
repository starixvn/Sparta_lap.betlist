using System.Collections.Generic;
using System.Data;
using Fanex.Athena.DataAccess.Core;
using Fanex.Athena.DataAccess.Customer.Interfaces;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Model.Core.ResultModel;
using Fanex.Athena.Model.Customer.DTO;
using Fanex.Athena.Model.Customer.MetaModel;
using Fanex.Athena.Session.Interfaces;
using Fanex.Common;
using Fanex.Common.Extensions;
using Fanex.Data;

namespace Fanex.Athena.DataAccess.Customer
{
    public class DormantCustomerListDataAccess : BaseDataAccess, IDormantCustomerListDataAccess
    {

        public ReportResultModel<DormantCustomer> GetDormantCustomers(CustomerListMetaModel metaModel)
        {
            const string UspName = "Lrf_DormantCustomerSel";

            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("username", DbType.String, metaModel.Username);
                db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                db.AddInParameter("PageIndex ", DbType.Int32, metaModel.PageIndex);
                db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                db.AddOutParameter("TotalRows", DbType.Int32, 4);

                var dataList = ReadDormantCustomerList(db, metaModel);
                var totalRows = Converter.ToInt(db.GetParameterValue("TotalRows"));

                return new ReportResultModel<DormantCustomer>
                {
                    DataList = dataList,
                    TotalRows = totalRows
                };
            }
        }

        private IList<DormantCustomer> ReadDormantCustomerList(IObjectDb objectDb, CustomerListMetaModel metaModel)
        {
            var dataList = new List<DormantCustomer>();
            using (var reader = objectDb.ExecuteReader())
            {
                var balanceColumn = "Balance" + metaModel.BaseCurrency;
                var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
                while (reader.Read())
                {
                    dataList.Add(new DormantCustomer
                    {
                        Index = startIndex++,
                        CustId = reader.ToInt("custid"),
                        SystemId = reader.ToString("username"),
                        ExternalId = reader.ToString("nickname"),
                        Balance = reader.ToDecimal(balanceColumn),
                        ArchivedDate = reader.ToDateTime("ArchivedDate")
                    });
                }
            }

            return dataList;
        }
    }
}