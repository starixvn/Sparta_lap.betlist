using System;
using System.Data;
using Fanex.Athena.DataAccess.Core.Exception;
using Fanex.Athena.DataAccess.Core.Users.Interfaces;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.DTO.Users;
using Fanex.Common;
using Fanex.Common.Extensions;
using Fanex.Data;

namespace Fanex.Athena.DataAccess.Core.Users
{
    public class AuthDataAccess : BaseDataAccess, IAuthDataAccess
    {
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
                throw new DataAccessException(sqlException, UspName);
            }
        }

        public void Logout(int userId)
        {
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
                throw new DataAccessException(sqlException, UspName, uspParams);
            }
        }

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
                throw new DataAccessException(sqlException, UspName);
            }
        }

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
                throw new DataAccessException(sqlException, UspName);
            }
        }
    }
}