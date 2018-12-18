using System;
using System.Data;
using Fanex.Athena.DataAccess.Core.Exception;
using Fanex.Athena.DataAccess.Core.Users.Interfaces;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Model.Core.DTO.Users;
using Fanex.Athena.Session.Interfaces;
using Fanex.Common;
using Fanex.Common.Extensions;
using Fanex.Data;

namespace Fanex.Athena.DataAccess.Core.Users
{
    public class AuthDataAccess : BaseDataAccess, IAuthDataAccess
    {
        public AuthDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

        public int LoginOnUser(User requestUser, string netSessionId, int serverId)
        {
            const string UspName = "Lrf_CheckIn";

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

        public IResultInfo<User> ValidateUserForLrfSystem(User requestUser)
        {
            const string UspName = "Lrf_UserLogin";

            using (var db = this.GetObjectDb(UspName))
            {
                AddCommonParameters(requestUser, db);
                db.AddInParameter("langkey", DbType.String, requestUser.Request.Language);

                var user = ReadUser(db);
                var resultInfo = GetResult<User>(db);
                resultInfo.ExtendedData = user;
                return resultInfo;
            }
        }

        private static void AddCommonParameters(User requestUser, IObjectDb db)
        {
            db.AddInParameter("UserName", DbType.String, requestUser.Username);
            db.AddInParameter("LoginIP", DbType.String, requestUser.Request.Ip);
            db.AddInParameter("LoginDomain", DbType.String, requestUser.Request.Domain);
            db.AddInParameter("BrowserName", DbType.String, requestUser.Request.Browser);
            db.AddInParameter("Platform", DbType.String, requestUser.Request.Platform);
            db.AddInParameter("CountryCode", DbType.String, requestUser.Request.CountryCode);
            AddResultParam(db);
            db.AddOutParameter("ServerID", DbType.Int32, 4);
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