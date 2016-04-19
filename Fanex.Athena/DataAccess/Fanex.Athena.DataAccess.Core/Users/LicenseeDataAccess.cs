// -----------------------------------------------------------------------
// <copyright file="LicenseeDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Users
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Users.Interfaces;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Users;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    ///     The licensee data access.
    /// </summary>
    public class LicenseeDataAccess : BaseDataAccess, ILicenseeDataAccess
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The can update member ba status.
        /// </summary>
        /// <param name="request"> The request. </param>
        /// <param name="licenseeId"> The licensee id. </param>
        /// <param name="isAllow"> The is allow. </param>
        /// <returns> Result information. </returns>
        public IResultInfo IsAllowUpdateMemberBAStatus(Request request, int licenseeId, out bool isAllow)
        {
            const string UspName = "Lrf_BATimeCheck";
            try
            {
                isAllow = false;
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("langkey", DbType.String, request.Language);
                    db.AddInParameter("LicenseeID", DbType.Int32, licenseeId);
                    db.AddOutParameter("isallow", DbType.Boolean, sizeof(bool));

                    AddResultParam(db);
                    db.ExecuteNonQuery();

                    isAllow = Convert.ToBoolean(db.GetParameterValue("isallow"));
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
        ///     The get belonging status.
        /// </summary>
        /// <param name="licenseeId">
        ///     The licensee id.
        /// </param>
        /// <param name="custId">
        ///     The customer id.
        /// </param>
        /// <returns>
        ///     Integer result.
        /// </returns>
        public int GetBelongingStatus(int licenseeId, int custId)
        {
            const string UspName = "Lrf_CustomerSelCheckBelonging";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeID", DbType.Int32, licenseeId);
                    db.AddInParameter("custid", DbType.Int32, custId);
                    db.AddOutParameter("status", DbType.Byte, 4);
                    db.ExecuteNonQuery();
                    return Convert.ToInt32(db.GetParameterValue("status"));
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        ///     The get by user.
        /// </summary>
        /// <param name="userId">
        ///     The user id.
        /// </param>
        /// <returns>
        /// List licensee.
        /// </returns>
        public IList<Licensee> GetByUser(int userId)
        {
            /*
             *@Alex20120722
             *Re:Add more "IsAllowdCustomerComm" field
            */
            const string UspName = "Lrf_LicenseeSel";
            var uspParams = new { UserId = userId };
            try
            {
                var entityList = new List<Licensee>();
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        while (reader.Read())
                        {
                            var entity = new Licensee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("LicenseeID")),
                                Name = reader.GetString(reader.GetOrdinal("LicenseeName")),
                                IsAllowedCustomerComm = reader.GetBoolean(reader.GetOrdinal("IsAllowedMemberComm"))
                            };
                            entityList.Add(entity);
                        }
                    }
                }

                return entityList;
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        ///     The get currencies.
        /// </summary>
        /// <param name="licenseeId">
        ///     The licensee id.
        /// </param>
        /// <param name="userId">
        ///     The user id.
        /// </param>
        /// <returns>
        /// List currency.
        /// </returns>
        public IList<Currency> GetCurrencies(int licenseeId, int userId)
        {
            const string UspName = "Lrf_SelCurrency";
            var uspParams = new { LicenseeId = licenseeId, UserId = userId };
            try
            {
                var entityList = new List<Currency>();
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        while (reader.Read())
                        {
                            var currency = new Currency
                            {
                                CurrencyId = reader.GetInt32(reader.GetOrdinal("Currency")),
                                Name = reader.GetString(reader.GetOrdinal("CurrencyName"))
                            };

                            entityList.Add(currency);
                        }
                    }
                }

                return entityList;
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        ///     The get customer by licensee.
        /// </summary>
        /// <param name="licenseeId">
        ///     The licensee id.
        /// </param>
        /// <returns>
        /// List licensee customer.
        /// </returns>
        public IList<LicenseeCustomer> GetCustByLicensee(int licenseeId)
        {
            const string UspName = "Lrf_LicenseeCustSel";
            var uspParams = new { LicenseeId = licenseeId };
            try
            {
                var entityList = new List<LicenseeCustomer>();
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        while (reader.Read())
                        {
                            var entity = new LicenseeCustomer
                            {
                                Id = licenseeId,
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                CustId = reader.GetInt32(reader.GetOrdinal("CustId")),
                                Currency = reader.GetInt32(reader.GetOrdinal("Currency")),
                                RoleId = reader.GetByte(reader.GetOrdinal("RoleID")),
                                Description = reader["Description"].ToString(),
                                CurrencyName = reader.GetString(reader.GetOrdinal("CurrencyName"))
                            };
                            entityList.Add(entity);
                        }
                    }
                }

                return entityList;
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        ///     Get list of licenseeId base on resellerId.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="resellerId">Reseller ID.</param>
        /// <returns>List integer.</returns>
        public IEnumerable<int> GetByResellerId(int userId, int resellerId)
        {
            const string UspName = "Lrf_LicenseeSel";

            using (var db = new ObjectDb(UspName))
            {
                using (var reader = db.ExecuteReader(new { UserId = userId }))
                {
                    var resultList = new List<int>();

                    while (reader.Read())
                    {
                        if (reader.ToInt("ResellerID") == resellerId)
                        {
                            resultList.Add(reader.ToInt("LicenseeID"));
                        }
                    }

                    return resultList;
                }
            }
        }

        #endregion Public Methods and Operators

        public string GetNumberOfOnlineUser()
        {
            var uspName = "Lrf_GetNumberOfOnlineUsers";

            using (var db = new ObjectDb(uspName))
            {
                var result = db.ExecuteScalar();
                return result != null ? result.ToString() : string.Empty;
            }
        }
    }
}