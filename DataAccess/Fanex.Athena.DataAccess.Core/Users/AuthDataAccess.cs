using System;
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

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("CustID", requestUser.UserID);
                db.SetParameterValue("NetSessionID", netSessionId);
                db.SetParameterValue("ServerID", serverId);
                db.SetParameterValue("On_UserID", null);
                db.SetParameterValue("IP", requestUser.Request.Ip);
                db.ExecuteNonQuery();

                return Convert.ToInt32(db.GetParameterValue("On_UserID"));
            }
        }

        public void Logout(int userId)
        {
            const string UspName = "Lrf_UserLogout";
            var uspParams = new { On_Userid = userId };

            using (var db = this.GetObjectDb(UspName))
            {
                db.ExecuteNonQuery(uspParams);
            }
        }

        public IResultInfo<User> ValidateUserForLrf(User requestUser)
        {
            const string UspName = "Lrf_UserLogin";

            using (var db = this.GetObjectDb(UspName))
            {
                AddCommonParameters(requestUser, db);
                db.SetParameterValue("langkey", requestUser.Request.Language);

                var user = ReadUser(db);
                var resultInfo = GetResult<User>(db);
                resultInfo.ExtendedData = user;
                return resultInfo;
            }
        }

        private static void AddCommonParameters(User requestUser, IObjectDb db)
        {
            db.SetParameterValue("UserName", requestUser.Username);
            db.SetParameterValue("LoginIP", requestUser.Request.Ip);
            db.SetParameterValue("LoginDomain", requestUser.Request.Domain);
            db.SetParameterValue("BrowserName", requestUser.Request.Browser);
            db.SetParameterValue("Platform", requestUser.Request.Platform);
            db.SetParameterValue("CountryCode", requestUser.Request.CountryCode);
            AddResultParam(db);
            db.SetParameterValue("ServerID", null);
        }

        private static User ReadUser(IObjectDb db)
        {
            var user = new User();
            using (var reader = db.ExecuteReader())
            {
                if (reader.Read())
                {
                    user.UserID = reader.ToInt("UserID");
                    user.Username = reader.ToString("username");
                    user.FirstName = reader.ToString("firstname");
                    user.LastName = reader.ToString("lastname");
                    user.Email = reader.ToString("email");
                    user.Role = reader.ToString("userrole");
                    user.IsInternal = Converter.ToBool(reader["isinternal"]);
                    user.Licensee = new Licensee { Id = reader.GetInt32(reader.GetOrdinal("LicenseeID")) };
                }
            }

            user.ServerId = Converter.ToInt(db.GetParameterValue("ServerId"));
            return user;
        }
    }
}