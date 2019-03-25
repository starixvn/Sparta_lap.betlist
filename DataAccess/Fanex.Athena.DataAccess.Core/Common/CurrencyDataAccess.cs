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
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Data;
    using Model.Core.MetaModel.Account;

    public class CurrencyDataAccess : BaseDataAccess, ICurrencyDataAccess
    {
        /// <summary>
        /// Assign currencies by user.
        /// </summary>
        /// <param name="request">Request value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo AssignCurrenciesByUser(AssignCurrencyMetaModel request)
        {
            const string UspName = "Lrf_CurrencyCustIns";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("UserID", request.UserId);
                db.SetParameterValue("LicenseeID", request.LicenseeId);
                db.SetParameterValue("CurrencyXML", request.CurrencyList);
                db.SetParameterValue("ModifyBy", request.ModifyBy);
                db.SetParameterValue("LangKey", request.LangKey);
                db.SetParameterValue("IP", request.IP);

                AddResultParam(db);
                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        public IList<CurrencyUser> GetCurrenciesByUser(int licenseeId, int userId)
        {
            const string UspName = "Lrf_SelCurrencyByLicenseeUser";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("UserID", userId);
                db.SetParameterValue("LicenseeID", licenseeId);

                return BindCurrencyUser(db);
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