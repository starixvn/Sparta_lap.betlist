// -----------------------------------------------------------------------
// <copyright file="CustomerDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Customer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Customer.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Models.DTO.Customer;
    using Fanex.Athena.Models.MetaModel.Customer;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;
    using Resources;

    /// <summary>
    /// Class CustomerDataAccess contains all method relates to customer.
    /// </summary>
    public class CustomerDataAccess : BaseDataAccess, ICustomerDataAccess
    {
        #region Read

        public IEnumerable<string> SearchCustomers(int userId, string userName, int licenseeId, string licenseeType, int currencyId)
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

                    return db.Query<string>();
                }
            }
            catch (ObjectDbException sqlException)
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
                    db.AddInParameter("BAFilter", DbType.Int32, metaModel.BAFilter);
                    db.AddInParameter("CustStatus", DbType.String, metaModel.CustStatus ?? string.Empty);
                    db.AddInParameter("SpecialLevel", DbType.Int32, metaModel.SpecialLevel);
                    db.AddInParameter("GroupCommId", DbType.Int32, metaModel.GroupCommId);
                    db.AddOutParameter("TotalRows", DbType.Int32, 4);

                    AddOddsGroupParam(db, metaModel);

                    var dataList = new List<CustomerDTO>();

                    using (var reader = db.ExecuteReader())
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
                                CSComm = reader.ToDecimal("RNGComm") * 100
                            };

                            customer.Status = customer.IsClosed ? ViewResources.Closed : customer.IsSuspended ? ViewResources.Suspended : ViewResources.Open;
                            dataList.Add(customer);
                        }
                    }

                    totalRows = Converter.ToInt(db.GetParameterValue("TotalRows"));
                    return dataList;
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        private void AddOddsGroupParam(ObjectDb objectDb, CustomerListMetaModel metaModel)
        {
            var oddsGroups = metaModel.OddsGroups;
            if (oddsGroups != null)
            {
                var selectedOddsGroups = oddsGroups.Where(g => g.Checked).Select(g => g.Name).ToArray();
                var oddsGroupValues = string.Join(",", selectedOddsGroups);
                objectDb.AddInParameter("OddsGroups", DbType.String, oddsGroupValues);
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

                    var resultList = db.Query<CustomerInfo>();

                    return resultList.Any() ? resultList.First() : new CustomerInfo();
                }
            }
            catch (ObjectDbException sqlException)
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
            catch (ObjectDbException sqlException)
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
            catch (ObjectDbException sqlException)
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
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        #endregion Group commission

        #region Update

        /// <summary>
        /// Update customer odds group.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateOddsGroup(CustomerOddsGroupMetaModel metaModel)
        {
            try
            {
                using (var db = new ObjectDb("Lrf_CustomerUpdUgroup"))
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
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, "Lrf_CustomerUpdUgroup");
            }
        }

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
            catch (ObjectDbException sqlException)
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
            catch (ObjectDbException sqlException)
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
        /// <param name="allowBA">Allow BA value.</param>
        /// <param name="request">Request value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateCustomerBAStatus(int licenseeId, int custId, int modifiedBy, bool allowBA, Request request)
        {
            const string UspName = "Lrf_CustomerUpdMemberBAStatus";
            try
            {
                using (var db = new ObjectDb(UspName))
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
                    resultInfo.ExtendedData = db.GetParameterValue("OddsGroup").ToString().ToUpper();

                    return resultInfo;
                }
            }
            catch (ObjectDbException sqlException)
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
            catch (ObjectDbException sqlException)
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
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        /// Update customer odds group.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="group">Level value.</param>
        /// <param name="ip">IP parameter value.</param>
        /// <param name="language">Language value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateOddsGroup(int userId, int custId, string group, string ip, string language)
        {
            const string UspName = "Lrf_CustomerUpdOddsGroup";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("custid", DbType.Int32, custId);
                    db.AddInParameter("userid", DbType.Int32, userId);
                    db.AddInParameter("oddsgroup", DbType.String, group);
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