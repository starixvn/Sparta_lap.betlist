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
    using System.Linq;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Users.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Data;
    using Fanex.Logging;
    using Model.Core.DTO.Users;

    public class LicenseeDataAccess : BaseDataAccess, ILicenseeDataAccess
    {
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
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        public bool IsLicenseeBelongToUser(int licenseeId, int userId)
        {
            var licenseeList = GetByUser(userId);
            return licenseeList.Any(l => l.Id == licenseeId);
        }

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
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

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
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        public IEnumerable<LicenseeSourcedSetting> GetLicenseeSourcedSettings(int licenseeId)
        {
            const string UspName = "LRF_LicenseeSourcedSel";
            var uspParams = new { LicenseeId = licenseeId };
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    return db.Query<LicenseeSourcedSetting>(uspParams);
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }
    }
}