// -----------------------------------------------------------------------
// <copyright file="LicenseeDataAccess.cs" company="">
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
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
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
                using (var db = this.GetObjectDb(UspName))
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
                using (var db = this.GetObjectDb(UspName))
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
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        while (reader.Read())
                        {
                            var entity = new Licensee
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("LicenseeID")),
                                Name = reader.GetString(reader.GetOrdinal("LicenseeName")),
                                IsAllowedCustomerComm = reader.GetBoolean(reader.GetOrdinal("IsAllowedMemberComm")),
                                IsEnableBA = reader.GetBoolean(reader.GetOrdinal("EnableBA"))
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

        public IList<LicenseeCustomer> GetCustByLicensee(int licenseeId)
        {
            const string UspName = "Lrf_LicenseeCustSel";
            var uspParams = new { LicenseeId = licenseeId };
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var entityList = new List<LicenseeCustomer>();
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

                        return entityList;
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        #endregion Public Methods and Operators

        public Licensee GetLicenseeByName(string licenseeName)
        {
            const string UspName = "Lrf_LicenseeSelBySiteName";
            var uspParams = new { siteName = licenseeName };
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    return db.QueryEntity<Licensee>(uspParams);
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }
    }
}