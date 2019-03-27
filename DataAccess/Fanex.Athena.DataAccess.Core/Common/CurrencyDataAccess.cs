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
    using System.Threading.Tasks;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Data;
    using Model.Core.MetaModel.Account;

    public class CurrencyDataAccess : BaseDataAccess, ICurrencyDataAccess
    {
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
                db.ExecuteNonQueryAsync();

                return GetResult(db);
            }
        }

        public async Task<IList<CurrencyUser>> GetCurrenciesByUser(int licenseeId, int userId)
        {
            const string UspName = "Lrf_SelCurrencyByLicenseeUser";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("UserID", userId);
                db.SetParameterValue("LicenseeID", licenseeId);

                return await BindCurrencyUser(db).ConfigureAwait(false);
            }
        }

        private static async Task<IList<CurrencyUser>> BindCurrencyUser(IObjectDb db)
        {
            var currencyList = await db.QueryAsync<CurrencyUser>().ConfigureAwait(false);
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