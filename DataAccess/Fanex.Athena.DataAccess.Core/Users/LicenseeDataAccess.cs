// -----------------------------------------------------------------------
// <copyright file="LicenseeDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Users.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;

    public class LicenseeDataAccess : BaseDataAccess, ILicenseeDataAccess
    {
        public IResultInfo<bool> IsAllowUpdateMemberBAStatus(Request request, int licenseeId)
        {
            const string UspName = "Lrf_BATimeCheck";

            var isAllow = false;
            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("langkey", request.Language);
                db.SetParameterValue("LicenseeID", licenseeId);
                db.SetParameterValue("isallow", null);

                AddResultParam(db);
                db.ExecuteNonQuery();

                isAllow = Convert.ToBoolean(db.GetParameterValue("isallow"));
                var result = GetResult(db);
                return new ResultInfo<bool>
                {
                    Code = result.Code,
                    Message = result.Message,
                    ExtendedData = isAllow
                };
            }
        }

        public bool IsLicenseeBelongToUser(int licenseeId, int userId)
        {
            var licenseeList = GetByUser(userId);
            return licenseeList.Any(l => l.Id == licenseeId);
        }

        public IList<Licensee> GetByUser(int userId)
        {
            const string UspName = "Lrf_LicenseeSel";
            var uspParams = new { UserId = userId };

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

        public Licensee GetLicenseeByName(string licenseeName)
        {
            const string UspName = "Lrf_LicenseeSelBySiteName";
            var uspParams = new { siteName = licenseeName };

            using (var db = this.GetObjectDb(UspName))
            {
                return db.QueryEntity<Licensee>(uspParams);
            }
        }
    }
}