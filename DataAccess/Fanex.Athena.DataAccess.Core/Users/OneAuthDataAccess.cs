// -----------------------------------------------------------------------
// <copyright file="UserDataAccess.cs" company="">
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
    using Fanex.Athena.Model.Core.DTO.Users;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Model.Core.DTO.OneAuth;

    public class OneAuthDataAccess : BaseDataAccess, IOneAuthDataAccess
    {
        public OneAuthUserInfo GetUserInfo(int userSEQ)
        {
            const string UspName = "Lrf_GetUserLevelInfo";
            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("UserSEQ", DbType.Int32, userSEQ);
                db.AddOutParameter("UserLevel", DbType.Byte, 0);
                db.AddOutParameter("HasSubsidiary", DbType.Boolean, 0);
                db.AddOutParameter("OperatorID", DbType.String, 50);

                var result = new OneAuthUserInfo();

                using (var reader = db.ExecuteReader())
                {
                    var currencyList = new List<CurrencyUser>();
                    while (reader.Read())
                    {
                        currencyList.Add(new CurrencyUser
                        {
                            AgentId = reader.ToInt("AgentId"),
                            CurrencyId = reader.ToInt("Currency")
                        });
                    }

                    result.CurrencyUserList = currencyList.GroupBy(x => x.CurrencyId)
                        .Select(x => new CurrencyUser
                        {
                            CurrencyId = x.Key,
                            AgentIdList = x.Select(y => y.AgentId).ToList(),
                            IsActive = true
                        }).ToList();
                }

                result.UserLevel = Convert.ToByte(db.GetParameterValue("UserLevel"));
                result.HasSubsidiary = Convert.ToBoolean(db.GetParameterValue("HasSubsidiary"));
                result.OperatorID = Convert.ToString(db.GetParameterValue("OperatorID"));
                return result;
            }
        }
    }
}