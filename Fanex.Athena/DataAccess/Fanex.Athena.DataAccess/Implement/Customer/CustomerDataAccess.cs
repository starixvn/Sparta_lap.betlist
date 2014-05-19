// -----------------------------------------------------------------------
// <copyright file="CustomerDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Customer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Customer;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Customer;
    using Fanex.Athena.Models.MetaModel.Customer;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Class CustomerDataAccess contains all method relates to customer.
    /// </summary>
    public class CustomerDataAccess : BaseDataAccess, ICustomerDataAccess
    {
        #region Read

        /// <summary>
        /// Quick search by userName.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="userName">Username value.</param>
        /// <param name="licenseeId">Licensee ID value.</param>
        /// <param name="licenseeType">Licensee type value.</param>
        /// <param name="currencyId">Currency ID value.</param>
        /// <returns>The result value.</returns>
        public DataTable QuickSearch(int userId, string userName, int licenseeId, string licenseeType, int currencyId)
        {
            try
            {
                using (var db = new ObjectDb("Lrf_CustomerSelUserSearch"))
                {
                    db.AddInParameter("licenseeid", DbType.Int32, licenseeId);
                    db.AddInParameter("userid", DbType.Int32, userId);
                    db.AddInParameter("licenseetype", DbType.String, licenseeType);
                    db.AddInParameter("currencyid", DbType.Int32, currencyId);
                    db.AddInParameter("username", DbType.String, userName);

                    var result = db.ExecuteDataSet();

                    return result.Tables.Count > 0 ? result.Tables[0] : new DataTable();
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, "Lrf_CustomerSelUserSearch");
            }
        }

        /// <summary>
        /// Advance search customer.
        /// </summary>
        /// <param name="metaModel">Meta model contains the filter such as: BA type, special level type, ... value.</param>
        /// <param name="totalRows">Total rows value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
                    Justification = "Reviewed. Suppression is OK here.")]
        public IList<CustomerDTO> Search(CustomerListMetaModel metaModel, ref int totalRows)
        {
            const string UspName = "Lrf_CustomerSel";

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeID", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("UserID", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                    db.AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
                    db.AddInParameter("username", DbType.String, metaModel.Username);
                    db.AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                    db.AddInParameter("PageIndex ", DbType.Int32, metaModel.PageIndex);
                    db.AddInParameter("BAFilter", DbType.Int32, metaModel.BaFilter);
                    db.AddInParameter("CustStatus", DbType.String, metaModel.CustStatus ?? string.Empty);
                    db.AddInParameter("SpecialLevel", DbType.Int32, metaModel.SpecialLevel);
                    db.AddInParameter("GroupCommId", DbType.Int32, metaModel.GroupCommId);
                    db.AddOutParameter("TotalRows", DbType.Int32, 4);

                    var dataTable = db.ExecuteDataSet().Tables[0];

                    var baseCurrency = metaModel.BaseCurrency;
                    var dataList = new List<CustomerDTO>();

                    var availableFundsCol = "TotalBalance" + baseCurrency;
                    var pendingFundsCol = "OutStanding" + baseCurrency;

                    var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        var cust = new CustomerDTO
                        {
                            Index = startIndex++,
                            CustId = Converter.ToInt(dr["custid"]),
                            SystemId = dr["username"].ToString(),
                            ExternalId = dr["nickname"].ToString(),
                            IsClosed = Converter.ToBool(dr["closed"]),
                            IsSuspended = Converter.ToBool(dr["suspended"]),
                            BaStatus = Converter.ToBool(dr["BAStatus"]),
                            SpecialLevel = Converter.ToInt(dr["SpecialLevel"]),
                            IsPowerBAUserTurnoff = Converter.ToBool(dr["IsPowerBAUserTurnoff"]),
                            AvailableFunds = Converter.ToDecimal(dr[availableFundsCol]),
                            PendingFunds = Converter.ToDecimal(dr[pendingFundsCol]),
                            GroupCommId = Converter.ToInt(dr["GroupCommId"]),
                            GroupColor = dr["GroupColor"].ToString(),
                            Group = dr["GroupName"].ToString(),
                            SbComm = Convert.ToDecimal(dr["SBComm"]) * 100,
                            RbComm = Convert.ToDecimal(dr["RacingComm"]) * 100,
                            NgComm = Convert.ToDecimal(dr["NGComm"]) * 100,
                            LcComm = Convert.ToDecimal(dr["LCComm"]) * 100,
                            CsComm = Convert.ToDecimal(dr["RNGComm"]) * 100
                        };

                        cust.Status = cust.IsClosed ? ViewResources.Closed : cust.IsSuspended ? ViewResources.Suspended : ViewResources.Open;
                        dataList.Add(cust);
                    }

                    totalRows = Converter.ToInt(db.GetParameterValue("TotalRows"));
                    return dataList;
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        /// Get customer Information such as: SyncCasino, DisableBingo, ...
        /// </summary>
        /// <param name="custId">Customer ID value.</param>
        /// <returns>The result value.</returns>
        public CustomerInfo GetCustomerInfo(int custId)
        {
            const string UspName = "Lrf_CustomerSelStatus";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("custid", DbType.Int32, custId);

                    var resultList = db.Query<CustomerInfo>().ToList();

                    return resultList.Count > 0 ? resultList[0] : new CustomerInfo();
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        #endregion Read

        #region LogInfo

        /// <summary>
        /// Get customer login information, last transfer time, ...
        /// </summary>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="licenseeId">Licensee ID value.</param>
        /// <returns>The result value.</returns>
        public LogInfo GetCustomerLoginInfo(int custId, int licenseeId)
        {
            const string UspName = "Lrf_CustomerLoginInfo";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("custid", DbType.Int32, custId);
                    db.AddInParameter("licenseeId", DbType.Int32, licenseeId);

                    return db.Query<LogInfo>().FirstOrDefault();
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        #endregion LogInfo

        #region Group commission

        /// <summary>
        ///     Move customers to new group including 2 following steps:
        ///     1: Remove all customers commission in group.
        ///     2: Update new customers commission in this group.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo MoveCustomersToGroup(MoveToGroupComMetaModel metaModel)
        {
            const string UspName = "Lrf_CustomerMoveGroupComm";

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);                    
                    db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("GroupCommId", DbType.Int32, metaModel.GroupComId);
                    db.AddInParameter("IsUpdateNG", DbType.Boolean, metaModel.IsAllowedNG);
                    db.AddInParameter("IsUpdateRacing", DbType.Boolean, metaModel.IsAllowedRC);
                    db.AddInParameter("IsUpdateLC", DbType.Boolean, metaModel.IsAllowedLC);
                    db.AddInParameter("IsUpdateRNG", DbType.Boolean, metaModel.IsAllowedRNG);
                    db.AddInParameter("Ip", DbType.String, metaModel.Ip);
                    db.AddInParameter("langkey", DbType.String, metaModel.Language);
                    db.AddInParameter("strCustIDOriginal", DbType.String, metaModel.CustomerToSQLParam);

                    db.AddOutParameter("strCustID", DbType.String, int.MaxValue);

                    AddResultParam(db);
                    db.ExecuteNonQuery();
                    var result = GetResult(db);
                    result.ExtendedData = Convert.ToString(db.GetParameterValue("strCustId"));
                    return result;
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        /// Remove customer from commission group.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo RemoveCustomersFromGroup(RemoveFromGroupComMetaModel metaModel)
        {
            const string UspName = "Lrf_CustomerRemoveGroupComm";

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);                    
                    db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("Ip", DbType.String, metaModel.Ip);
                    db.AddInParameter("IsRemovePlayercomm", DbType.Boolean, metaModel.IsRemovePlayercomm);
                    db.AddInParameter("langkey", DbType.String, metaModel.Language);
                    db.AddInParameter("strCustIDOriginal", DbType.String, metaModel.CustomerToSQLParam);

                    db.AddOutParameter("strCustID", DbType.String, int.MaxValue);

                    AddResultParam(db);
                    db.ExecuteNonQuery();
                    var result = GetResult(db);
                    result.ExtendedData = Convert.ToString(db.GetParameterValue("strCustId"));
                    return result;
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        #endregion Group commission

        #region Update

        /// <summary>
        /// Update customer closed status.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateCustClosed(CustomerStatusMetaModel metaModel)
        {
            try
            {
                using (var db = new ObjectDb("Lrf_CustomerUpdClosed"))
                {
                    db.AddInParameter("LicenseeId", DbType.String, metaModel.LicenseeId);
                    db.AddInParameter("UserId", DbType.String, metaModel.ModifyById);
                    db.AddInParameter("CustId", DbType.String, metaModel.CustId);
                    db.AddInParameter("Closed", DbType.String, metaModel.IsOn);
                    db.AddInParameter("IP", DbType.String, metaModel.Ip);

                    AddResultParam(db);
                    db.ExecuteNonQuery();

                    return GetResult(db);
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, "Lrf_CustomerUpdClosed");
            }
        }

        /// <summary>
        /// Update customer suspend status.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateCustSuspend(CustomerStatusMetaModel metaModel)
        {
            try
            {
                using (var db = new ObjectDb("Lrf_CustomerUpdSuspended"))
                {
                    db.AddInParameter("LicenseeId", DbType.String, metaModel.LicenseeId);
                    db.AddInParameter("UserId", DbType.String, metaModel.ModifyById);
                    db.AddInParameter("CustId", DbType.String, metaModel.CustId);
                    db.AddInParameter("Suspended", DbType.String, metaModel.IsOn);
                    db.AddInParameter("IP", DbType.String, metaModel.Ip);

                    AddResultParam(db);
                    db.ExecuteNonQuery();

                    return GetResult(db);
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, "Lrf_CustomerUpdSuspended");
            }
        }

        /// <summary>
        /// Update customer ba status.
        /// </summary>
        /// <param name="licenseeId">Licensee ID value.</param>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="modifiedBy">Modify by value.</param>
        /// <param name="allowBa">Allow BA value.</param>
        /// <param name="request">Request value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateCustomerBaStatus(int licenseeId, int custId, int modifiedBy, bool allowBa, Request request)
        {
            const string UspName = "Lrf_CustomerUpdMemberBAStatus";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeID", DbType.Int32, licenseeId);
                    db.AddInParameter("CustID", DbType.String, custId);
                    db.AddInParameter("ModifyBy", DbType.String, modifiedBy);
                    db.AddInParameter("IsBA", DbType.Boolean, allowBa);
                    db.AddInParameter("langkey", DbType.String, request.Language);
                    db.AddInParameter("IP", DbType.String, request.Ip);
                    AddResultParam(db);
                    db.ExecuteNonQuery();

                    return GetResult(db);
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        ///  Update customer commission setting.
        /// </summary>
        /// <param name="metaModel">Item value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateCustomersCommission(UpdatedCustomerComMetaModel metaModel)
        {
            const string UspName = "Lrf_LicenseeCustCommUpd";

            try
            {
                using (var db = new ObjectDb(UspName))
                {                                        
                    db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);                    
                    db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("SBComm", DbType.Decimal, metaModel.SBComm);
                    db.AddInParameter("NGComm", DbType.Decimal, metaModel.NGComm);
                    db.AddInParameter("RacingComm", DbType.Decimal, metaModel.RCComm);
                    db.AddInParameter("LCComm", DbType.Decimal, metaModel.LCComm);
                    db.AddInParameter("RNGComm", DbType.Decimal, metaModel.CSComm);
                    db.AddInParameter("IsUpdateNG", DbType.Boolean, metaModel.IsAllowedNG);
                    db.AddInParameter("IsUpdateRacing", DbType.Boolean, metaModel.IsAllowedRC);
                    db.AddInParameter("IsUpdateLC", DbType.Boolean, metaModel.IsAllowedLC);
                    db.AddInParameter("IsUpdateRNG", DbType.Boolean, metaModel.IsAllowedCS);
                    db.AddInParameter("Ip", DbType.String, metaModel.Ip);
                    db.AddInParameter("langkey", DbType.String, metaModel.Language);
                    db.AddInParameter("strCustIDOriginal", DbType.String, metaModel.CustomerToSQLParam);

                    db.AddOutParameter("strCustID", DbType.String, int.MaxValue);
                    AddResultParam(db);

                    db.ExecuteNonQuery();

                    var result = GetResult(db);
                    result.ExtendedData = Convert.ToString(db.GetParameterValue("strCustId"));
                    return result;
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        /// Update customer special level.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="level">Level value.</param>
        /// <param name="ip">IP parameter value.</param>
        /// <param name="language">Language value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateSpecialLevel(int userId, int custId, int level, string ip, string language)
        {
            const string UspName = "Lrf_CustomerUpdSpecialLevel";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("custid", DbType.Int32, custId);
                    db.AddInParameter("userid", DbType.Int32, userId);
                    db.AddInParameter("speciallevel", DbType.Int32, level);
                    db.AddInParameter("ip", DbType.String, ip);
                    db.AddInParameter("langkey", DbType.String, language);
                    AddResultParam(db);
                    db.ExecuteNonQuery();

                    return GetResult(db);
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        #endregion Update
    }
}