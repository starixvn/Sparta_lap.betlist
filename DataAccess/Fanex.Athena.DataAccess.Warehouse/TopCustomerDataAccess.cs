// -----------------------------------------------------------------------
// <copyright file="TopCustomerDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Warehouse
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Warehouse.Interfaces;
    using Fanex.Athena.Model.Warehouse.DTO;
    using Fanex.Athena.Model.Warehouse.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class TopCustomerDataAccess : BaseDataAccess, ITopCustomerDataAccess
    {
        private const string UspName = "Lrf_rpt_DWTopCustomerSel";

        public TopCustomerDataAccess()
        {
        }

        public IList<TopCustomer> GetDataList(TopCustomerMetaModel metaModel)
        {
            var dataList = new List<TopCustomer>();
            var param = CreateParams(metaModel);
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(param))
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
                throw new DataAccessException(sqlException.Message, UspName, param);
            }

            return dataList;
        }

        public DataTable GetDataForExcel(TopCustomerMetaModel metaModel)
        {
            var param = CreateParams(metaModel);
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    var dataset = db.ExecuteDataSet(param);
                    return dataset.Tables.Count > 0 ? dataset.Tables[0] : new DataTable();
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, param);
            }
        }

        private static object CreateParams(TopCustomerMetaModel metaModel)
        {
            return new
            {
                AgentList = metaModel.AgentListXml,
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                sortingid = metaModel.SortingId,
                customer = metaModel.ExternalId,
                activemin = metaModel.ActiveMin,
                basedcurrency = metaModel.BaseCurrencyId,
                wlmtotalmin = metaModel.WinLossTotalMin,
                betcountmin = metaModel.BetCountMin,
                sportid = metaModel.SportId,
                toprow = metaModel.TopRow
            };
        }
    }
}