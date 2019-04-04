namespace Fanex.Athena.DataAccess.Customer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
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

        public async Task<ReportResultModel<CustomerDTO>> GetActiveCustomers(CustomerListMetaModel metaModel)
        {
            const string UspName = "Lrf_CustomerSel";

            using (var db = this.GetObjectDb(UspName))
            {
                AddCustomerParam(db, metaModel);
                AddOddsGroupParam(db, metaModel);

                var dataList = await ReadCustomerList(db, metaModel).ConfigureAwait(false);
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

        public LogInfo GetCustomerLoginInfo(int custId, int licenseeId, string siteName)
        {
            const string UspName = "Lrf_CustomerLoginInfo";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("licenseeId", licenseeId);
                db.SetParameterValue("siteName", siteName);

                return db.QueryEntity<LogInfo>();
            }
        }

        #endregion LogInfo

        #region Update

        public IResultInfo UpdateOddsGroup(CustomerOddsGroupMetaModel metaModel)
        {
            using (var db = this.GetObjectDb("Lrf_CustomerUpdUgroup"))
            {
                db.SetParameterValue("UserId", metaModel.UserId);
                db.SetParameterValue("CustId", metaModel.CustId);
                db.SetParameterValue("OddsGroup", metaModel.OddsGroup);
                db.SetParameterValue("IP", metaModel.Ip);
                db.SetParameterValue("Langkey", metaModel.Language);

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
                db.SetParameterValue("LicenseeID", licenseeId);
                db.SetParameterValue("CustID", custId);
                db.SetParameterValue("ModifyBy", modifiedBy);
                db.SetParameterValue("IsBA", allowBA);
                db.SetParameterValue("langkey", request.Language);
                db.SetParameterValue("IP", request.Ip);
                db.SetParameterValue("OddsGroup", null);
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
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("userid", userId);
                db.SetParameterValue("speciallevel", level);
                db.SetParameterValue("ip", ip);
                db.SetParameterValue("langkey", language);
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
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("userid", userId);
                db.SetParameterValue("oddsgroup", group);
                db.SetParameterValue("ip", ip);
                db.SetParameterValue("langkey", language);
                AddResultParam(db);
                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        #endregion Update

        private static void AddCustomerParam(IObjectDb objectDb, CustomerListMetaModel metaModel)
        {
            objectDb.SetParameterValue("LicenseeID", metaModel.LicenseeId);
            objectDb.SetParameterValue("username", metaModel.Username);
            objectDb.SetParameterValue("PageSize", metaModel.PageSize);
            objectDb.SetParameterValue("PageIndex", metaModel.PageIndex);
            objectDb.SetParameterValue("BAFilter", metaModel.BAFilter);
            objectDb.SetParameterValue("CustStatus", metaModel.CustStatus ?? string.Empty);
            objectDb.SetParameterValue("SpecialLevel", metaModel.SpecialLevel);
            objectDb.SetParameterValue("GroupCommId", metaModel.GroupCommId);
            objectDb.SetParameterValue("AgentList", metaModel.AgentListXml);
            objectDb.SetParameterValue("TotalRows", null);
        }

        private static void AddOddsGroupParam(IObjectDb objectDb, CustomerListMetaModel metaModel)
        {
            var oddsGroups = metaModel.OddsGroups;
            if (oddsGroups != null)
            {
                var selectedOddsGroups = oddsGroups.Where(g => g.Checked).Select(g => g.Name).ToArray();
                var oddsGroupValues = string.Join(",", selectedOddsGroups);
                objectDb.SetParameterValue("OddsGroups", oddsGroupValues);
            }
        }

        private async Task<IList<CustomerDTO>> ReadCustomerList(IObjectDb objectDb, CustomerListMetaModel metaModel)
        {
            var dataList = new List<CustomerDTO>();
            using (var reader = await objectDb.ExecuteReaderAsync().ConfigureAwait(false))
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
                        NGComm = reader.ToDecimal("NGComm") * 100,
                        CSComm = reader.ToDecimal("RNGComm") * 100
                    };

                    var unClosedStatus = customer.IsSuspended ? ViewResources.Suspended : ViewResources.Open;
                    customer.Status = customer.IsClosed ? ViewResources.Closed : unClosedStatus;
 
                    dataList.Add(customer);
                }
            }

            return dataList;
        }

        private IResultInfo UpdateCustomerStatus(CustomerStatusMetaModel metaModel, string uspName, string statusParamName)
        {
            using (var db = GetObjectDb(uspName))
            {
                db.SetParameterValue("LicenseeId", metaModel.LicenseeId);
                db.SetParameterValue("UserId", metaModel.ModifyById);
                db.SetParameterValue("CustId", metaModel.CustId);
                db.SetParameterValue("IP", metaModel.Ip);
                db.SetParameterValue(statusParamName, metaModel.IsOn);

                AddResultParam(db);
                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }
    }
}