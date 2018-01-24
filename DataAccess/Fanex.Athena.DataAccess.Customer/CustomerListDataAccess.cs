namespace Fanex.Athena.DataAccess.Customer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Customer.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Customer.DTO;
    using Fanex.Athena.Model.Customer.MetaModel;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.ResultModel;
    using Resources;

    public class CustomerListDataAccess : BaseDataAccess, ICustomerListDataAccess
    {
        #region Read

        public IEnumerable<string> SearchCustomers(int userId, string userName, string agentListXml)
        {
            using (var db = this.GetObjectDb("Lrf_CustomerSelUserSearch"))
            {
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("username", DbType.String, userName);
                db.AddInParameter("AgentList", DbType.String, agentListXml);

                return db.Query<string>();
            }
        }

        public ReportResultModel<CustomerDTO> Search(CustomerListMetaModel metaModel)
        {
            const string UspName = "Lrf_CustomerSel";

            using (var db = this.GetObjectDb(UspName))
            {
                AddCustomerParam(db, metaModel);
                AddOddsGroupParam(db, metaModel);

                var dataList = ReadCustomerList(db, metaModel);          
                var totalRows = Converter.ToInt(db.GetParameterValue("TotalRows"));

                return new ReportResultModel<CustomerDTO>
                {
                    DataList = dataList,
                    TotalRows = totalRows
                };
            }
        }

        #endregion Read

        #region LogInfo

        public LogInfo GetCustomerLoginInfo(int custId, int licenseeId)
        {
            const string UspName = "Lrf_CustomerLoginInfo";

            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter(nameof(licenseeId), DbType.Int32, licenseeId);

                return db.QueryEntity<LogInfo>();
            }
        }

        #endregion LogInfo

        #region Update

        public IResultInfo UpdateOddsGroup(CustomerOddsGroupMetaModel metaModel)
        {
            using (var db = this.GetObjectDb("Lrf_CustomerUpdUgroup"))
            {
                db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                db.AddInParameter("CustId", DbType.Int32, metaModel.CustId);
                db.AddInParameter("OddsGroup", DbType.String, metaModel.OddsGroup);
                db.AddInParameter("IP", DbType.String, metaModel.Ip);
                db.AddInParameter("Langkey", DbType.String, metaModel.Language);

                AddResultParam(db);
                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        public IResultInfo UpdateCustClosed(CustomerStatusMetaModel metaModel)
        {
            return UpdateCustomerStatus(metaModel, "Lrf_CustomerUpdClosed", "Closed");
        }

        public IResultInfo UpdateCustSuspend(CustomerStatusMetaModel metaModel)
        {
            return UpdateCustomerStatus(metaModel, "Lrf_CustomerUpdSuspended", "Suspended");
        }

        public IResultInfo UpdateCustomerBAStatus(int licenseeId, int custId, int modifiedBy, bool allowBA, Request request)
        {
            const string UspName = "Lrf_CustomerUpdMemberBAStatus";

            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("LicenseeID", DbType.Int32, licenseeId);
                db.AddInParameter("CustID", DbType.String, custId);
                db.AddInParameter("ModifyBy", DbType.String, modifiedBy);
                db.AddInParameter("IsBA", DbType.Boolean, allowBA);
                db.AddInParameter("langkey", DbType.String, request.Language);
                db.AddInParameter("IP", DbType.String, request.Ip);
                db.AddOutParameter("OddsGroup", DbType.String, 10);
                AddResultParam(db);
                db.ExecuteNonQuery();

                var resultInfo = GetResult(db);
                resultInfo.ExtendedData = db.GetParameterValue("OddsGroup").ToString().ToUpper(CultureInfo.CurrentCulture);

                return resultInfo;
            }
        }

        public IResultInfo UpdateSpecialLevel(int userId, int custId, int level, string ip, string language)
        {
            const string UspName = "Lrf_CustomerUpdSpecialLevel";

            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("speciallevel", DbType.Int32, level);
                db.AddInParameter(nameof(ip), DbType.String, ip);
                db.AddInParameter("langkey", DbType.String, language);
                AddResultParam(db);
                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        public IResultInfo UpdateOddsGroup(int userId, int custId, string group, string ip, string language)
        {
            const string UspName = "Lrf_CustomerUpdOddsGroup";

            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("oddsgroup", DbType.String, group);
                db.AddInParameter(nameof(ip), DbType.String, ip);
                db.AddInParameter("langkey", DbType.String, language);
                AddResultParam(db);
                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        #endregion Update

        private static void AddCustomerParam(IObjectDb objectDb, CustomerListMetaModel metaModel)
        {
            objectDb.AddInParameter("LicenseeID", DbType.Int32, metaModel.LicenseeId);
            objectDb.AddInParameter("username", DbType.String, metaModel.Username);
            objectDb.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            objectDb.AddInParameter("PageIndex ", DbType.Int32, metaModel.PageIndex);
            objectDb.AddInParameter("BAFilter", DbType.Int32, metaModel.BAFilter);
            objectDb.AddInParameter("CustStatus", DbType.String, metaModel.CustStatus ?? string.Empty);
            objectDb.AddInParameter("SpecialLevel", DbType.Int32, metaModel.SpecialLevel);
            objectDb.AddInParameter("GroupCommId", DbType.Int32, metaModel.GroupCommId);
            objectDb.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            objectDb.AddOutParameter("TotalRows", DbType.Int32, 4);
        }

        private static void AddOddsGroupParam(IObjectDb objectDb, CustomerListMetaModel metaModel)
        {
            var oddsGroups = metaModel.OddsGroups;
            if (oddsGroups != null)
            {
                var selectedOddsGroups = oddsGroups.Where(g => g.Checked).Select(g => g.Name).ToArray();
                var oddsGroupValues = string.Join(",", selectedOddsGroups);
                objectDb.AddInParameter("OddsGroups", DbType.String, oddsGroupValues);
            }
        }

        private List<CustomerDTO> ReadCustomerList(IObjectDb objectDb, CustomerListMetaModel metaModel)
        {
            var dataList = new List<CustomerDTO>();
            using (var reader = objectDb.ExecuteReader())
            {
                var availableFundsCol = "TotalBalance" + metaModel.BaseCurrency;
                var pendingFundsCol = "OutStanding" + metaModel.BaseCurrency;

                var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
                while (reader.Read())
                {
                    var customer = new CustomerDTO
                    {
                        Index = startIndex++,
                        CustId = reader.ToInt("custid"),
                        SystemId = reader.ToString("username"),
                        ExternalId = reader.ToString("nickname"),
                        IsClosed = reader.ToBool("closed"),
                        IsSuspended = reader.ToBool("suspended"),
                        BAStatus = reader.ToBool("BAStatus"),
                        SpecialLevel = reader.ToInt("SpecialLevel"),
                        IsPowerBAUserTurnoff = reader.ToBool("IsPowerBAUserTurnoff"),
                        AvailableFunds = reader.ToDecimal(availableFundsCol),
                        PendingFunds = reader.ToDecimal(pendingFundsCol),
                        GroupCommId = reader.ToInt("GroupCommId"),
                        GroupColor = reader.ToString("GroupColor"),
                        Group = reader.ToString("GroupName"),
                        OddsGroup = reader.ToString("OddsGroup"),
                        SBComm = reader.ToDecimal("SBComm") * 100,
                        RBComm = reader.ToDecimal("RacingComm") * 100,
                        NGComm = reader.ToDecimal("NGComm") * 100,
                        LCComm = reader.ToDecimal("LCComm") * 100,
                        CSComm = reader.ToDecimal("RNGComm") * 100,
                        ABComm = reader.ToDecimal("AllbetComm") * 100
                    };

                    customer.Status = customer.IsClosed ? ViewResources.Closed : customer.IsSuspended ? ViewResources.Suspended : ViewResources.Open;
                    dataList.Add(customer);
                }
            }

            return dataList;
        }

        private IResultInfo UpdateCustomerStatus(CustomerStatusMetaModel metaModel, string uspName, string statusParamName)
        {
            using (var db = GetObjectDb(uspName))
            {
                db.AddInParameter("LicenseeId", DbType.String, metaModel.LicenseeId);
                db.AddInParameter("UserId", DbType.String, metaModel.ModifyById);
                db.AddInParameter("CustId", DbType.String, metaModel.CustId);
                db.AddInParameter("IP", DbType.String, metaModel.Ip);
                db.AddInParameter(statusParamName, DbType.String, metaModel.IsOn);

                AddResultParam(db);
                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }
    }
}