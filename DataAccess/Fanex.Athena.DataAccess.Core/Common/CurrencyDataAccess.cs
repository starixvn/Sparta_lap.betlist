// -----------------------------------------------------------------------
// <copyright file="CurrencyDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Data;
    using Fanex.Logging;
    using Model.Core.MetaModel.Account;

    public class CurrencyDataAccess : BaseDataAccess, ICurrencyDataAccess
    {
        public IList<CurrencyUser> GetCurrencies(int licenseeId)
        {
            const string UspName = "Lrf_SelCurrencyByLicensee";
            var uspParams = new { LicenseeId = licenseeId };
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    return BindCurrencyUser(db);
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Assign currencies by user.
        /// </summary>
        /// <param name="request">Request value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo AssignCurrenciesByUser(AssignCurrencyMetaModel request)
        {
            const string UspName = "Lrf_CurrencyCustIns";

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("UserID", DbType.Int32, request.UserId);
                    db.AddInParameter("LicenseeID", DbType.Int32, request.LicenseeId);
                    db.AddInParameter("CurrencyXML", DbType.String, request.CurrencyList);
                    db.AddInParameter("ModifyBy", DbType.Int32, request.ModifyBy);
                    db.AddInParameter("LangKey", DbType.String, request.LangKey);
                    db.AddInParameter("IP", DbType.String, request.IP);

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

        public IList<CurrencyUser> GetCurrenciesByUser(int licenseeId, int userId)
        {
            const string UspName = "Lrf_SelCurrencyByLicenseeUser";

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("UserID", DbType.Int32, userId);
                    db.AddInParameter("LicenseeID", DbType.Int32, licenseeId);

                    return BindCurrencyUser(db);
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        private static IList<CurrencyUser> BindCurrencyUser(IObjectDb db)
        {
            var currencyList = db.Query<CurrencyUser>();
            return currencyList.GroupBy(x => x.CurrencyId).Select(x => new CurrencyUser
            {
                CurrencyId = x.Key,
                CurrencyName = x.First().CurrencyName,
                IsActive = x.First().IsActive,
                AgentIdList = x.Select(y => y.AgentId).ToList()
            }).ToList();
        }
    }
}