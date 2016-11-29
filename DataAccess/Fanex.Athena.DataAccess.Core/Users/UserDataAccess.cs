// -----------------------------------------------------------------------
// <copyright file="UserDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
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
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    ///     The user data access.
    /// </summary>
    public class UserDataAccess : BaseDataAccess, IUserDataAccess
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The change password.
        /// </summary>
        /// <param name="requestUser"> The request user. </param>
        /// <param name="oldPassword"> The old password. </param>
        /// <param name="newPassword"> The new password. </param>
        /// <param name="salt"> The salt value. </param>
        /// <returns> Result information. </returns>
        public IResultInfo ChangePassword(User requestUser, string oldPassword, string newPassword, string salt)
        {
            const string UspName = "Licensee_UserPasswordUpd";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("UserName", DbType.String, requestUser.Username);
                    db.AddInParameter("OldPwd", DbType.String, oldPassword);
                    db.AddInParameter("NewPwd", DbType.String, newPassword);
                    db.AddInParameter("LoginIP", DbType.String, requestUser.Request.Ip);
                    db.AddInParameter("UserSalt", DbType.String, salt);
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
        ///     The get by licensee.
        /// </summary>
        /// <param name="licenseeId"> The licensee ID. </param>
        /// <param name="isInternal"> The is internal. </param>
        /// <returns>List user. </returns>
        public List<User> GetByLicensee(int licenseeId, bool isInternal)
        {
            const string UspName = "Lrf_UserAccessSel";
            var uspParams = new { LicenseeID = licenseeId, IsInternal = isInternal };
            try
            {
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
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        public User GetByUsername(string userName, int licenseeId = 0)
        {
            var param = new
            {
                Username = userName,
                LicenseeId = licenseeId
            };
            using (var objectDb = this.GetObjectDb("Lrf_UserInfoSelByUserName"))
            {
                return objectDb.Query<User>(param).FirstOrDefault();
            }
        }

        /// <summary>
        ///     The get user info.
        /// </summary>
        /// <param name="licenseeId"> The licensee id. </param>
        /// <param name="userId"> The user id. </param>
        /// <returns> User result.</returns>
        public User GetUserInfo(int licenseeId, int userId)
        {
            const string UspName = "Lrf_UserInfoSel";
            var uspParams = new { LicenseeId = licenseeId, UserId = userId };
            try
            {
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
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        ///     The get user salt.
        /// </summary>
        /// <param name="userName"> The user name. </param>
        /// <returns> Object result. </returns>
        public object GetUserSalt(string userName)
        {
            const string UspName = "Licensee_UserSaltSel";
            var uspParams = new { UserName = userName };
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    var salt = db.ExecuteScalar(uspParams);
                    return salt;
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        ///     The is username exited.
        /// </summary>
        /// <param name="userName"> The user name. </param>
        /// <returns> Boolean result. </returns>
        public bool IsUsernameExisted(string userName)
        {
            const string UspName = "Lrf_IsUserExisted";
            try
            {
                bool isUserExisted;

                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("Username", DbType.String, userName);
                    db.AddOutParameter("IsUserExisted", DbType.Boolean, 1);
                    db.ExecuteNonQuery();
                    isUserExisted = Convert.ToBoolean(db.GetParameterValue("IsUserExisted"));
                }

                return isUserExisted;
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        ///     The login on user.
        /// </summary>
        /// <param name="requestUser"> The request user. </param>
        /// <param name="netSessionId"> The net session id. </param>
        /// <param name="serverId"> The server id. </param>
        /// <returns> Integer result. </returns>
        public int LoginOnUser(User requestUser, string netSessionId, int serverId)
        {
            const string UspName = "Lrf_CheckIn";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("CustID", DbType.Int32, requestUser.UserID);
                    db.AddInParameter("NetSessionID", DbType.String, netSessionId);
                    db.AddInParameter("ServerID", DbType.Int32, serverId);
                    db.AddOutParameter("On_UserID", DbType.Int32, 4);
                    db.AddInParameter("IP", DbType.String, requestUser.Request.Ip);
                    db.ExecuteNonQuery();

                    return Convert.ToInt32(db.GetParameterValue("On_UserID"));
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        ///     The logout.
        /// </summary>
        /// <param name="userId"> The on user id. </param>
        public void Logout(int userId)
        {
            // return;
            const string UspName = "Lrf_UserLogout";
            var uspParams = new { On_Userid = userId };
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.ExecuteNonQuery(uspParams);
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        ///     The re check in.
        /// </summary>
        /// <param name="userId"> The on user id. </param>
        /// <returns> Boolean result. </returns>
        public bool RecheckIn(int userId)
        {
            const string UspName = "Lrf_ReCheckIn";
            try
            {
                bool isKickedOut;
                using (var db = new ObjectDb(UspName, 30))
                {
                    db.AddInParameter("On_Userid", DbType.Int32, userId);
                    db.AddOutParameter("IsKickedOut", DbType.Boolean, 0);
                    db.ExecuteNonQuery();

                    // get OnUserId
                    isKickedOut = Convert.ToBoolean(db.GetParameterValue("IsKickedOut"));
                }

                return isKickedOut;
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        ///     The reset password.
        /// </summary>
        /// <param name="user"> The user value. </param>
        /// <param name="salt"> The salt value. </param>
        /// <param name="resetPwdBy"> The reset password by. </param>
        /// <returns>Result information. </returns>
        public IResultInfo ResetPassword(User user, string salt, int resetPwdBy)
        {
            const string UspName = "Lrf_UserPwdReset";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeID", DbType.Int32, user.Licensee.Id);
                    db.AddInParameter("UserId", DbType.Int32, user.UserID);
                    db.AddInParameter("UpdateBy", DbType.Int32, resetPwdBy);
                    db.AddInParameter("Pwd", DbType.String, user.Password);
                    db.AddInParameter("UserSalt", DbType.String, salt);
                    db.AddInParameter("langkey", DbType.String, user.Request.Language);
                    db.AddInParameter("LoginIP", DbType.String, user.Request.Ip);

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

        public IResultInfo SyncUserInfo(string licenseeName, string userName)
        {
            const string UspName = "Lrf_SyncInternalUser";
            const string DefaultPass = "754A738BF99D986D8716FE00A81BC5CA32916BF6";//1234aa
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeName", DbType.String, licenseeName);
                    db.AddInParameter("Username", DbType.String, userName);
                    db.AddInParameter("EncryptedPassword", DbType.String, DefaultPass);
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
        ///     The update info.
        /// </summary>
        /// <param name="user"> The user value. </param>
        /// <param name="updatedBy"> The updated by. </param>
        /// <returns> Result information. </returns>
        public IResultInfo UpdateInfo(User user, int updatedBy)
        {
            const string UspName = "Lrf_UserUpdAccount";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("LicenseeID", DbType.Int32, user.Licensee.Id);
                    db.AddInParameter("UserId", DbType.Int32, user.UserID);
                    db.AddInParameter("UpdateBy", DbType.Int32, updatedBy);
                    db.AddInParameter("FirstName", DbType.String, user.FirstName);
                    db.AddInParameter("LastName", DbType.String, user.LastName);
                    db.AddInParameter("Status", DbType.String, user.Status);
                    db.AddInParameter("langkey", DbType.String, user.Request.Language);
                    db.AddInParameter("LoginIP", DbType.String, user.Request.Ip);
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
        ///     The validate internal user.
        /// </summary>
        /// <param name="requestUser"> The request user. </param>
        /// <param name="timeStamp"> The time stamp. </param>
        /// <param name="licenseeName"> The licensee name. </param>
        /// <param name="resultInfo"> The result. </param>
        /// <returns> User result. </returns>
        public User ValidateInternalUser(User requestUser, long timeStamp, string licenseeName, out IResultInfo resultInfo)
        {
            const string UspName = "Lrf_InternalLogin";
            try
            {
                var user = new User();
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("UserName", DbType.String, requestUser.Username);
                    db.AddInParameter("EncryptedPassword", DbType.String, requestUser.Password);
                    db.AddInParameter("LicenseeName", DbType.String, licenseeName);
                    db.AddInParameter("LoginTime", DbType.DateTime, new DateTime(timeStamp));
                    db.AddInParameter("LoginIP", DbType.String, requestUser.Request.Ip);
                    db.AddInParameter("LoginDomain", DbType.String, requestUser.Request.Domain);
                    db.AddInParameter("BrowserName", DbType.String, requestUser.Request.Browser);
                    db.AddInParameter("Platform", DbType.String, requestUser.Request.Platform);
                    db.AddInParameter("CountryCode", DbType.String, requestUser.Request.CountryCode);
                    db.AddInParameter("Language", DbType.String, requestUser.Request.Language);
                    AddResultParam(db);
                    db.AddOutParameter("ServerID", DbType.Int32, 4);
                    using (var reader = db.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                UserID = reader.ToInt("UserID"),
                                Username = reader.ToString("username"),
                                FirstName = reader.ToString("firstname"),
                                LastName = reader.ToString("lastname"),
                                Email = reader.ToString("email"),
                                Role = reader.ToString("userrole"),
                                IsInternal = Converter.ToBool(reader["isinternal"])
                            };
                            var licensee = new Licensee { Id = reader.GetInt32(reader.GetOrdinal("LicenseeID")) };
                            user.Licensee = licensee;
                        }
                    }

                    user.ServerId = Converter.ToInt(db.GetParameterValue("ServerId"));
                    resultInfo = GetResult(db);
                }

                return user;
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        ///     The validate user for LRF system.
        /// </summary>
        /// <param name="requestUser"> The request user. </param>
        /// <param name="resultInfo"> The result. </param>
        /// <returns>User result. </returns>
        public User ValidateUserForLrfSystem(User requestUser, out IResultInfo resultInfo)
        {
            const string UspName = "Lrf_UserLogin";
            try
            {
                var user = new User();
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("UserName", DbType.String, requestUser.Username);
                    db.AddInParameter("LoginIP", DbType.String, requestUser.Request.Ip);
                    db.AddInParameter("LoginDomain", DbType.String, requestUser.Request.Domain);
                    db.AddInParameter("BrowserName", DbType.String, requestUser.Request.Browser);
                    db.AddInParameter("Platform", DbType.String, requestUser.Request.Platform);
                    db.AddInParameter("CountryCode", DbType.String, requestUser.Request.CountryCode);
                    db.AddInParameter("langkey", DbType.String, requestUser.Request.Language);
                    AddResultParam(db);
                    db.AddOutParameter("ServerID", DbType.Int32, 4);

                    using (var reader = db.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                UserID = reader.ToInt("UserID"),
                                Username = reader.ToString("username"),
                                FirstName = reader.ToString("firstname"),
                                LastName = reader.ToString("lastname"),
                                Email = reader.ToString("email"),
                                Role = reader.ToString("userrole"),
                                IsInternal = Converter.ToBool(reader["isinternal"])
                            };

                            var licensee = new Licensee { Id = reader.GetInt32(reader.GetOrdinal("LicenseeID")) };
                            user.Licensee = licensee;
                        }
                    }

                    user.ServerId = Converter.ToInt(db.GetParameterValue("ServerId"));
                    resultInfo = GetResult(db);
                }

                return user;
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        #endregion Public Methods and Operators
    }
}