// -----------------------------------------------------------------------
// <copyright file="UserDataAccess.cs" company="">
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
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Common;
    using Fanex.Common.Extensions;

    public class UserDataAccess : BaseDataAccess, IUserDataAccess
    {
        public IResultInfo ChangePassword(User requestUser, string oldPassword, string newPassword, string salt)
        {
            const string UspName = "Licensee_UserPasswordUpd";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("UserName", requestUser.Username);
                db.SetParameterValue("OldPwd", oldPassword);
                db.SetParameterValue("NewPwd", newPassword);
                db.SetParameterValue("LoginIP", requestUser.Request.Ip);
                db.SetParameterValue("UserSalt", salt);
                AddResultParam(db);

                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        public IList<User> GetByLicensee(int licenseeId, bool isInternal)
        {
            const string UspName = "Lrf_UserAccessSel";
            var uspParams = new { LicenseeID = licenseeId, IsInternal = isInternal };

            var entityList = new List<User>();
            using (var db = this.GetObjectDb(UspName))
            {
                using (var reader = db.ExecuteReader(uspParams))
                {
                    while (reader.Read())
                    {
                        var entity = new User
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Username = reader.ToString("username"),
                            FirstName = reader.ToString("firstname"),
                            LastName = reader.ToString("lastname"),
                            Status = reader.ToString("status"),
                            IsManageAccount = Convert.ToBoolean(reader["ismanageaccount"]),
                            Licensee = new Licensee { Id = licenseeId },
                            IsInternal = Converter.ToBool(reader["isinternal"])
                        };

                        entityList.Add(entity);
                    }
                }
            }

            return entityList;
        }

        public User GetByUsername(string userName, bool isInternal, int licenseeId = 0)
        {
            var param = new
            {
                Username = userName,
                LicenseeId = licenseeId,
                isInternal
            };
            using (var objectDb = GetObjectDb("Lrf_UserInfoSelByUserName"))
            {
                return objectDb.Query<User>(param).FirstOrDefault();
            }
        }

        public User GetUserInfo(int licenseeId, int userId)
        {
            const string UspName = "Lrf_UserInfoSel";
            var uspParams = new { LicenseeId = licenseeId, UserId = userId };

            using (var db = this.GetObjectDb(UspName))
            {
                using (var reader = db.ExecuteReader(uspParams))
                {
                    var user = new User();
                    if (reader.Read())
                    {
                        user.UserID = reader.ToInt("userid");
                        user.Username = reader.ToString("username");
                        user.FirstName = reader.ToString("firstname");
                        user.LastName = reader.ToString("lastname");
                        user.Status = reader.ToString("status");
                        user.Licensee = new Licensee { Id = licenseeId };
                        user.IsInternal = Converter.ToBool(reader["isinternal"]);
                    }

                    return user;
                }
            }
        }

        public bool RecheckIn(int userId)
        {
            const string UspName = "Lrf_ReCheckIn";

            bool isKickedOut;
            using (var db = GetObjectDb(UspName, 30))
            {
                db.SetParameterValue("On_Userid", userId);
                db.SetParameterValue("IsKickedOut", null);
                db.ExecuteNonQuery();

                // get OnUserId
                isKickedOut = Convert.ToBoolean(db.GetParameterValue("IsKickedOut"));
            }

            return isKickedOut;
        }

        public IResultInfo ResetPassword(User user, string salt, int resetPwdBy)
        {
            const string UspName = "Lrf_UserPwdReset";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("LicenseeID", user.Licensee.Id);
                db.SetParameterValue("UserId", user.UserID);
                db.SetParameterValue("UpdateBy", resetPwdBy);
                db.SetParameterValue("Pwd", user.Password);
                db.SetParameterValue("UserSalt", salt);
                db.SetParameterValue("langkey", user.Request.Language);
                db.SetParameterValue("LoginIP", user.Request.Ip);

                AddResultParam(db);

                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        public IResultInfo UpdateInfo(User user, int updatedBy)
        {
            const string UspName = "Lrf_UserUpdAccount";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("LicenseeID", user.Licensee.Id);
                db.SetParameterValue("UserId", user.UserID);
                db.SetParameterValue("UpdateBy", updatedBy);
                db.SetParameterValue("FirstName", user.FirstName);
                db.SetParameterValue("LastName", user.LastName);
                db.SetParameterValue("Status", user.Status);
                db.SetParameterValue("langkey", user.Request.Language);
                db.SetParameterValue("LoginIP", user.Request.Ip);
                AddResultParam(db);

                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }
    }
}